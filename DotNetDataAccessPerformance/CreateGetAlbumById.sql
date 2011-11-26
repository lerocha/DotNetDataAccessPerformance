USE Chinook;
GO
IF OBJECT_ID ('dbo.spGetAlbumById', 'P') IS NOT NULL
	DROP PROCEDURE dbo.spGetAlbumById
GO

CREATE PROCEDURE dbo.spGetAlbumById
	@id INT
AS
	SELECT AlbumId, Title, ArtistId
	FROM Album
	WHERE Album.AlbumId=@id
GO