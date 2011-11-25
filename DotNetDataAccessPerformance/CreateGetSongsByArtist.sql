USE Chinook;
GO
IF OBJECT_ID ('dbo.spGetSongsByArtist', 'P') IS NOT NULL
	DROP PROCEDURE dbo.spGetSongsByArtist
GO

CREATE PROCEDURE dbo.spGetSongsByArtist
	@name NVARCHAR(120)
AS
	SELECT Album.Title as AlbumName, Track.Name as SongName, Artist.Name as ArtistName
	FROM Artist
	INNER JOIN Album ON Album.ArtistId = Artist.ArtistId
	INNER JOIN Track ON Track.AlbumId = Album.AlbumId
	WHERE Artist.Name=@name
GO