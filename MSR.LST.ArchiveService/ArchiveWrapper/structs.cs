using System;
using System.Globalization; 


namespace MSR.LST.ConferenceXP.ArchiveService
{
    [Serializable]
    public class Conference
    {
        public Conference () {}

        public Conference (int conferenceID, string description, string venueIdentifier, DateTime start, DateTime end)
        {
            this.ConferenceID = conferenceID;
            this.Description = description;
            this.VenueIdentifier = venueIdentifier;
            this.Start = start;
            this.End = end;
        }

        public int      ConferenceID;
        public string   Description;
        public string   VenueIdentifier;
        public DateTime Start;
        public DateTime End;

        public override string ToString()
        {
            return  "ID: " + ConferenceID +
                    "\nDescription: " + Description + 
                    "\nVenueIdentifier: " + VenueIdentifier + 
                    "\nStart time: " + Start.ToLongTimeString() +
                    "\nEnd time: " + End.ToLongTimeString() +
                    "\nDuration: " + DisplayTimeSpan((End-Start));
        }

        private string DisplayTimeSpan (TimeSpan timeToShow)
        {
            if (timeToShow < TimeSpan.Zero)
                return Strings.NegativeDuration;

            string output = String.Empty;
            if (timeToShow.Days > 0)
            {
                output += timeToShow.Days+':';
            }
            
            output += timeToShow.Hours.ToString("00", CultureInfo.InvariantCulture)+ ':' +
                timeToShow.Minutes.ToString("00", CultureInfo.InvariantCulture) + ':' + 
                timeToShow.Seconds.ToString("00", CultureInfo.InvariantCulture);
            return output;
        }

    }

    [Serializable]
    public class Participant
    {
        public Participant(int participant, string cname, string name )
        {
            this.ParticipantID = participant;
            this.CName = cname;
            this.Name = name;
        }

        public Participant() {}
        
        public int      ParticipantID;
        public string   CName;
        public string   Name;

        public override string ToString()
        {
            return "ID: " + ParticipantID +
                    "\nCName: " + CName +
                    "\nName: " + Name;
        }
    }

    [Serializable]
    public class Stream
    {
        public Stream( int streamID, string name, string payload, int frames, long seconds, int bytes)
        {
            this.StreamID = streamID;
            this.Name = name;
            this.Payload = payload;
            this.Frames = frames;
            this.Seconds = seconds;
            this.Bytes = bytes;
        }
        public Stream() {}
        public int      StreamID;
        public string   Name;
        public string   Payload;
        public int      Frames;
        public long     Seconds;
        public int      Bytes;

        public override string ToString()
        {
            return "ID: " + StreamID +
                    "\nName: " + Name +
                    "\nPayloadType: " + Payload +
                    "\nFrames: " + Frames +
                    "\nSeconds: " + Seconds +
                    "\nBytes: " + Bytes;
        }
    }

    /// <summary>
    /// Represents an index into our buffer of frames
    /// </summary>
    public struct Index
    {
        public int start;                   // start offest
        public int  end;                    // end offset
        public long timestamp;              // time in ticks
    }

}
