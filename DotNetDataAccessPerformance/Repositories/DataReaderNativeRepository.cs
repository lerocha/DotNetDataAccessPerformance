using System.Collections.Generic;
using System.Data.SqlClient;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.Helpers;

namespace DotNetDataAccessPerformance.Repositories
{
	public class DataReaderNativeRepository : IRepository
	{
		public IEnumerable<Song> GetSongsByArtist(string name)
		{
			const string query = @"SELECT Album.Title as AlbumName, Track.Name as SongName, Artist.Name as ArtistName
									FROM Artist
									INNER JOIN Album ON Album.ArtistId = Artist.ArtistId
									INNER JOIN Track ON Track.AlbumId = Album.AlbumId
									WHERE Artist.Name='Pearl Jam'";

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