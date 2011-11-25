using System.Collections.Generic;
using Dapper;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.Helpers;

namespace DotNetDataAccessPerformance.Repositories
{
	public class DrapperNativeRepository : IRepository
	{
		public IEnumerable<Song> GetSongsByArtist(string name)
		{
			const string query = @"SELECT Album.Title as AlbumName, Track.Name as SongName, Artist.Name as ArtistName
									   FROM Artist
									   INNER JOIN Album ON Album.ArtistId = Artist.ArtistId
									   INNER JOIN Track ON Track.AlbumId = Album.AlbumId
									   WHERE Artist.Name=@name";

			using (var connection = ConnectionFactory.OpenConnection())
			{
				return connection.Query<Song>(query, new { name = "Pearl Jam" });
			}
		}
	}
}