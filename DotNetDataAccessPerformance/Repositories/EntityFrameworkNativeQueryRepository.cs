using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.EntityFramework;

namespace DotNetDataAccessPerformance.Repositories
{
	public class EntityFrameworkNativeQueryRepository : IRepository
	{
		public IEnumerable<Song> GetSongsByArtist(string name)
		{
			const string query = @"SELECT Album.Title as AlbumName, Track.Name as SongName, Artist.Name as ArtistName
									   FROM Artist
									   INNER JOIN Album ON Album.ArtistId = Artist.ArtistId
									   INNER JOIN Track ON Track.AlbumId = Album.AlbumId
									   WHERE Artist.Name=@name";

			using (var context = new ChinookEntities())
			{
				var result = context.ExecuteStoreQuery<Song>(query, new SqlParameter
				                                                    	{
				                                                    		ParameterName = "@name",
				                                                    		Value = "Pearl Jam"
				                                                    	});
				return result.ToList();
			}
		}
	}
}