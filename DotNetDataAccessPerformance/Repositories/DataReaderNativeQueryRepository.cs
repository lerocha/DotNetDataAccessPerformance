using System.Collections.Generic;
using System.Data.SqlClient;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.Helpers;

namespace DotNetDataAccessPerformance.Repositories
{
	public class DataReaderNativeQueryRepository : IRepository
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
			string query = "SELECT ArtistId, Name FROM Artist WHERE Artist.ArtistId=" + id;
			using (var connection = ConnectionFactory.OpenConnection())
			{
				var command = new SqlCommand(query, connection);
				using (var reader = command.ExecuteReader())
				{
					if(reader.Read())
					{
						return new Artist 
						       	{
						       		ArtistId = (int) reader[0],
									Name = (string) reader[1],
						       	};
					}
				}
			}
			return null;
		}

		public IEnumerable<Song> GetSongsByArtist(string name)
		{
			string query = @"SELECT Album.Title as AlbumName, Track.Name as SongName, Artist.Name as ArtistName
							FROM Artist
							INNER JOIN Album ON Album.ArtistId = Artist.ArtistId
							INNER JOIN Track ON Track.AlbumId = Album.AlbumId
							WHERE Artist.Name='" + name + "'";

			using (var connection = ConnectionFactory.OpenConnection())
			{
				var command = new SqlCommand(query, connection);

				var songs = new List<Song>();
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						songs.Add(new Song
						          	{
						          		AlbumName = reader[0] as string,
						          		SongName = reader[1] as string,
						          		ArtistName = reader[2] as string
						          	});
					}
				}
				return songs;
			}
		}
	}
}