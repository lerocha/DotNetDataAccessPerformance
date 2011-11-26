USE Chinook;
GO
IF OBJECT_ID ('dbo.spGetArtistById', 'P') IS NOT NULL
	DROP PROCEDURE dbo.spGetArtistById
GO

CREATE PROCEDURE dbo.spGetArtistById
	@id INT
AS
	SELECT ArtistId, Name
	FROM Artist
	WHERE Artist.ArtistId=@id
GO