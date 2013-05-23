----------------------------------------------------------------------------------------------------
----------------------- Stored procedures for use in the persistence system ------------------------
----------------------------------------------------------------------------------------------------

use ArchiveService
GO

-- NOTE: I can't seem to cause the script to just return due to an error finding the database
--  I've tried putting a check for @@ERROR both here and before the GO statement, and neither worked
--  Without it, this file will run fine even if the database isn't present, and thus we can't
--  pop up an error message in the installer.  (pfb, 14-Apr-05)

----------------------------------------------------------------------------------------------------
-- Stored procedures to support the web-service calls 
----------------------------------------------------------------------------------------------------
IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'GetConferences' )
	DROP PROCEDURE GetConferences
GO

CREATE PROCEDURE GetConferences
AS
SELECT conference_id, description, venue_identifier, start_dttm, end_dttm
FROM Conference
ORDER BY start_dttm ASC
GO


----------------------------------------------------------------------------------------------------
IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'GetParticipants' )
	DROP PROCEDURE GetParticipants
GO

CREATE PROCEDURE GetParticipants
				@conference_id int 
AS
SELECT participant_id, conference_id, cname, [name]
FROM participant
WHERE conference_id = @conference_id
ORDER BY cname ASC
GO


----------------------------------------------------------------------------------------------------
IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'GetStreams' )
	DROP PROCEDURE GetStreams
GO

/* GetStreams gets all the streams in a given participant, EXCEPT those that have no data (only 1 byte or less).
   This is assuming that no calling methods would want to open those streams.
   HOWEVER, they need to be cleaned up on occassion, *without* interfering with any ongoing recording. */

CREATE PROCEDURE GetStreams
				@participant_id int 
AS
SELECT	s.stream_id, s.name, s.payload_type, 
		( SELECT COUNT( frame_id) FROM frame WHERE stream_id = s.stream_id ) as frames,
		( SELECT (MAX( frame_time ) - MIN(frame_time))/10000000 FROM frame WHERE stream_id = s.stream_id ) as seconds,
		( SELECT datalength(data) FROM rawStream WHERE stream_id =  s.stream_id) as bytes
FROM stream as s
WHERE s.participant_id = @participant_id AND 
  (SELECT datalength(data) FROM rawStream WHERE stream_id =  s.stream_id) > 1
ORDER BY [name] ASC
GO


----------------------------------------------------------------------------------------------------
-- Stored procedures to support conference creation
----------------------------------------------------------------------------------------------------
--create the actual conference entry. called by the web-service
IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'CreateConference' )
	DROP PROCEDURE CreateConference
GO

CREATE PROCEDURE CreateConference	
					@description	varchar(255),
					@venue_identifier varchar(255),
					@start_dttm		datetime
					
									
AS
DECLARE @conference_id int

INSERT Conference ( description, venue_identifier, start_dttm ) 
		VALUES ( @description, @venue_identifier, @start_dttm)
SELECT @conference_id =@@identity
RETURN @conference_id
GO


----------------------------------------------------------------------------------------------------
-- create a new participant
IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'CreateParticipant' )
	DROP PROCEDURE CreateParticipant
GO

CREATE PROCEDURE CreateParticipant			
								@cname varchar(255),
								@conference_id int,
								@name	varchar(255)
								
AS
DECLARE @participant_id int
BEGIN TRANSACTION


INSERT Participant ( conference_id, cname, [name] ) VALUES 
	( @conference_id, @cname, @name )
SELECT @participant_id = @@identity

COMMIT TRANSACTION
RETURN @participant_id
GO


----------------------------------------------------------------------------------------------------
-- create a new stream
IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'CreateStream' )
	DROP PROCEDURE CreateStream
GO

CREATE PROCEDURE CreateStream	@participant_id	int ,		
								@name varchar(255),
								@payload_type	varchar(255),
								@priexns image
AS

DECLARE @stream_id int

BEGIN TRANSACTION
--todo. can we get round this by using isnull perhaps in the rawstream sp...i don't like it...
INSERT Stream ( participant_id, [name], payload_type, privextns ) VALUES ( @participant_id, @name, @payload_type, @priexns)
SELECT @stream_id = @@identity 

INSERT RawStream(stream_id, data) values (@stream_id, 0x0)

COMMIT TRANSACTION
RETURN @stream_id
GO


----------------------------------------------------------------------------------------------------
-- Stored procedures to support conference recording
----------------------------------------------------------------------------------------------------
-- append a chunk of data onto the raw stream for this stream
IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'AppendBuffer' )
	DROP PROCEDURE AppendBuffer
GO

CREATE PROCEDURE AppendBuffer	@stream_id	int,
				                @chunk		image
AS

DECLARE @oldLength int
DECLARE @dataPtr binary(16)
DECLARE @spaceAvailable int
DECLARE @chunkLength int

BEGIN TRANSACTION
--lock as we're updating
SELECT @oldLength = DATALENGTH(data), @dataPtr= TEXTPTR(data)  FROM RawStream WITH (UPDLOCK) WHERE stream_id = @stream_id

-- make sure we don't overflow
SET @spaceAvailable = 2147483647 - @oldLength -- to avoid overflowing int in our calculations, we do it this way
SET @chunkLength = DATALENGTH(@chunk)

IF (@chunkLength > @spaceAvailable)
BEGIN
    RAISERROR('Overflowed int type in AppendBuffer', 16, 1) -- make sure we don't write the data
    ROLLBACK TRANSACTION
END
ELSE
BEGIN
	-- WARNING: due to a "bug" in Shiloh (SQL 8.0), if you write beyond the end of the row, the length of the row overflows,
	--  and the data becomes rather irretrievablly lost :(  Note, this is fixed in Yukon (SQL 9.0).
    UPDATETEXT RawStream.data @dataPtr null null @chunk
    COMMIT TRANSACTION
END

--and return the buffer end so we can record the index
RETURN @oldLength
GO


----------------------------------------------------------------------------------------------------
-- create a new participant
IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'RecordConferenceEndTime' )
	DROP PROCEDURE RecordConferenceEndTime
GO

CREATE PROCEDURE RecordConferenceEndTime
								@conference_id int,
								@end_time datetime
AS

UPDATE Conference
SET end_dttm = @end_time
WHERE conference_id = @conference_id

GO

	
----------------------------------------------------------------------------------------------------
-- Stored procedures to support conference playback initialization
----------------------------------------------------------------------------------------------------
-- Gets the stream, plus the participant details to playback the stream
IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'GetStreamAndParticipantDetails' )
    DROP PROCEDURE GetStreamAndParticipantDetails
GO

CREATE PROCEDURE GetStreamAndParticipantDetails			
								@stream_id int
AS

SELECT  Stream.name, 
        Stream.payload_type, 
        Participant.cname, 
        Stream.privExtns
FROM Stream 
    INNER JOIN Participant ON Stream.participant_id = Participant.participant_id
WHERE Stream.stream_id = @stream_id

GO


----------------------------------------------------------------------------------------------------
--return the details of a stream, the raw stream associated with it, and the time it starts at
IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'GetStreamStatistics' )
	DROP PROCEDURE GetStreamStatistics
GO

CREATE PROCEDURE GetStreamStatistics	@stream_id int,
                            @interval int,
							@starting_tick bigint OUTPUT,
							@max_frame_size int OUTPUT,
							@max_buffer_size int OUTPUT,
							@max_frame_count int OUTPUT
							

AS
--find the start of the stream. not critical path and indexed so ok for efficiency
SELECT TOP 1 @starting_tick = frame.frame_time
FROM stream
INNER JOIN frame
	ON frame.stream_id = stream.stream_id
WHERE stream.stream_id = @stream_id
ORDER BY frame.frame_time ASC

--find out the max frame size, max frame count and max buffer size for this stream
--basically we walk the stream in 30 second intervals and record the max frame count and buffer size we find
DECLARE @mfs int				-- max frame size
DECLARE @mbs int				-- max buffer size
DECLARE @max_frames int			-- max no of frames
DECLARE @start_tick bigint		-- first tick in this batch
DECLARE @last_tick bigint    	-- have we finished

DECLARE @cur_frames int			-- current frames
DECLARE @cur_buffer_size int	-- current buffer size

SELECT @max_frames = 0			-- no frames initially
SELECT @mbs = 0					-- 0 length buffer

-- find the first tick for this stream
SELECT TOP 1 @start_tick = frame_time FROM frame WHERE stream_id = @stream_id ORDER BY frame_time ASC

-- find the last tick for this stream
SELECT TOP 1 @last_tick = frame_time FROM frame WHERE stream_id = @stream_id ORDER BY frame_time DESC

--find the maximum frame size for the stream
SELECT @mfs = MAX( raw_end - raw_start ) FROM frame WHERE stream_id = @stream_id

-- walk the stream in <interval> milisecond chunks looking for the period with the greatest # of frames, etc.
WHILE ( @start_tick <= @last_tick)
BEGIN
	-- how many frames in the next <interval> seconds?
	SELECT @cur_frames = COUNT( frame_id), @cur_buffer_size = 1 + MAX(raw_end) - MIN(raw_start)
	FROM frame
	WHERE stream_id = @stream_id
	AND (frame_time BETWEEN @start_tick AND @start_tick + @interval) --BETWEEN is inclusive, btw
	
	--update maxima if necessary
	IF ( @cur_frames > @max_frames )
	BEGIN
		SELECT @max_frames = @cur_frames
	END
	
	IF ( @cur_buffer_size > @mbs )
	BEGIN
		SELECT @mbs = @cur_buffer_size
	END
	
	-- walk to the next 30 seconds
	SELECT @start_tick = @start_tick + @interval + 1
END

-- TODO - Figure out why null values are sometimes seen. This also seems to be transient?!
if (@mfs is null or @mbs is null or @max_frames is null)
    raiserror ('Bad data - null value in GetParticipantDetails stored procedure', 16, 1)

SELECT @max_buffer_size = @mbs, @max_frame_count = @max_frames, @max_frame_size = @mfs
GO


----------------------------------------------------------------------------------------------------
-- Stored procedures to support conference playback
----------------------------------------------------------------------------------------------------
--Bring back a block of indexes for a given stream
IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'LoadIndices' )
	DROP PROCEDURE LoadIndices
GO


CREATE PROCEDURE LoadIndices
							@stream_id int,
							@starting_tick bigint,
							@count int
AS

-- For multithreading reasons, we can't set this back to '0' (OFF) without doing some locking.
--  The code needs to be analyzed to make sure setting the ROWCOUNT in this "global" manner won't
--  cause any problems...

SET ROWCOUNT @count

SELECT  frame.raw_start, frame.raw_end, frame.frame_time
FROM frame WITH (NOLOCK)
WHERE frame.stream_id = @stream_id
AND frame.frame_time >= @starting_tick
ORDER BY frame.frame_time ASC

GO



----------------------------------------------------------------------------------------------------
--read a chunk of data from the buffer
IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'ReadBuffer' )
	DROP PROCEDURE ReadBuffer
GO

CREATE PROCEDURE ReadBuffer	@stream_id	int,
							@start int,
							@length int
AS

DECLARE @ptrval binary(16)
--nolock cos these are write once
SELECT @ptrval = TEXTPTR(data) 
   FROM RawStream WITH (NOLOCK) WHERE stream_id = @stream_id
      
READTEXT rawStream.data @ptrval @start @length

GO


----------------------------------------------------------------------------------------------------
/* These are helper methods for the "database cleanup" feature in the admin service.
        The cleanup feature should:
            - Delete Conferences without child Participants
            - Delete Participants without child Streams
            - Delete Streams without more than 1 byte of raw data
            - Delete Streams without child Frames (this and the last qualification should be the same)
            - Check for Conferences without end times (unless they're handled elsewhere) */
----------------------------------------------------------------------------------------------------
IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'GetConferenceLastFrameSentTime' )
	DROP PROCEDURE GetConferenceLastFrameSentTime
GO

CREATE PROCEDURE GetConferenceLastFrameSentTime
								@conference_id int

AS
SELECT MAX(f.frame_time)
FROM Frame as f
    INNER JOIN Stream s ON (s.stream_id = f.stream_id)
    INNER JOIN Participant p ON (p.participant_id = s.participant_id)
    INNER JOIN Conference c ON (c.conference_id = p.conference_id AND c.conference_id = @conference_id)

GO


----------------------------------------------------------------------------------------------------
/*	Scripting of the user name and permissions for LOCAL SERVICE account
*/

DECLARE @serveraccount varchar(58)
select @serveraccount = CONVERT(varchar(50), SERVERPROPERTY('machinename'))
select @serveraccount = N'NT AUTHORITY\LOCAL SERVICE'

--create it if the login does not exist...
IF NOT EXISTS( SELECT * FROM master.dbo.syslogins WHERE loginname = @serveraccount)
	EXEC sp_grantlogin @serveraccount

EXEC sp_defaultdb @serveraccount, N'ArchiveService'

IF NOT EXISTS (SELECT * FROM dbo.sysusers WHERE [name] = @serveraccount AND uid < 16382)
	EXEC sp_grantdbaccess @serveraccount

EXEC sp_addrolemember N'db_datareader', @serveraccount
EXEC sp_addrolemember N'db_datawriter', @serveraccount

GRANT EXECUTE ON AppendBuffer to [NT AUTHORITY\LOCAL SERVICE]
GRANT EXECUTE ON CreateConference to [NT AUTHORITY\LOCAL SERVICE]
GRANT EXECUTE ON CreateParticipant to [NT AUTHORITY\LOCAL SERVICE]
GRANT EXECUTE ON CreateStream to [NT AUTHORITY\LOCAL SERVICE]
GRANT EXECUTE ON GetConferences to [NT AUTHORITY\LOCAL SERVICE]
GRANT EXECUTE ON GetParticipants to [NT AUTHORITY\LOCAL SERVICE]
GRANT EXECUTE ON GetStreamAndParticipantDetails to [NT AUTHORITY\LOCAL SERVICE]
GRANT EXECUTE ON GetStreams to [NT AUTHORITY\LOCAL SERVICE]
GRANT EXECUTE ON GetStreamStatistics to [NT AUTHORITY\LOCAL SERVICE]
GRANT EXECUTE ON LoadIndices to [NT AUTHORITY\LOCAL SERVICE]
GRANT EXECUTE ON ReadBuffer to [NT AUTHORITY\LOCAL SERVICE]
GRANT EXECUTE ON RecordConferenceEndTime to [NT AUTHORITY\LOCAL SERVICE]
GRANT EXECUTE ON GetConferenceLastFrameSentTime to [NT AUTHORITY\LOCAL SERVICE]

GO