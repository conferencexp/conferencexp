-- Drop any existing ArchiveService database
IF DB_ID (N'ArchiveService') IS NOT NULL
DROP DATABASE ArchiveService;
GO
