using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.Helpers;

namespace DotNetDataAccessPerformance.Repositories
{
	public class DataReaderStoredProcedureRepository : IRepository
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
				var command = new SqlCommand("spGetArtistById", connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.Add(new SqlParameter("@id", 10));
				using (var reader = command.ExecuteReader())
				{
					if (reader.Read())
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
			using (var connection = ConnectionFactory.OpenConnection())
			{
				var command = new SqlCommand("spGetSongsByArtist", connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.Add(new SqlParameter("@name", name));

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