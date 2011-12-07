using System.Collections.Generic;
using Dapper;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.Helpers;
using System.Linq;

namespace DotNetDataAccessPerformance.Repositories
{
	public class DapperNativeQueryRepository : IRepository
	{
		public void AddArtist(Artist artist)
		{
			throw new System.NotImplementedException();
		}

		public void UpdateArtist(Artist artist)
		{
			throw new System.NotImplementedException();
		}

		public void DeleteArtist(Artist artist)
		{
			throw new System.NotImplementedException();
		}

		public Artist GetArtistById(int id)
		{
			using (var connection = ConnectionFactory.OpenConnection())
			{
				return connection.Query<Artist>("SELECT ArtistId, Name FROM Artist WHERE Artist.ArtistId=@id", new { id })
								 .FirstOrDefault();
			}
		}

		public IEnumerable<Song> GetSongsByArtist(string name)
		{
			const string query = @"SELECT Album.Title as AlbumName, Track.Name as SongName, Artist.Name as ArtistName
									FROM Artist
									INNER JOIN Album ON Album.ArtistId = Artist.ArtistId
									INNER JOIN Track ON Track.AlbumId = Album.AlbumId
									WHERE Artist.Name=@name";

			using (var connection = ConnectionFactory.OpenConnection())
			{
				return connection.Query<Song>(query, new { name });
			}
		}
	}
}