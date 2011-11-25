using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.Helpers;

namespace DotNetDataAccessPerformance.Repositories
{
	public class DataReaderStoredProcedureRepository : IRepository
	{
		public IEnumerable<Song> GetSongsByArtist(string name)
		{
			using (var connection = ConnectionFactory.OpenConnection())
			{
				var command = new SqlCommand("spGetSongsByArtist", connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.Add(new SqlParameter("@name", "Pearl Jam"));

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