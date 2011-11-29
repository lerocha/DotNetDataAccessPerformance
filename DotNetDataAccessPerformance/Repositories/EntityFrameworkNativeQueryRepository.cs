using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.EntityFramework;
using Artist = DotNetDataAccessPerformance.Domain.Artist;

namespace DotNetDataAccessPerformance.Repositories
{
	public class EntityFrameworkNativeQueryRepository : IRepository
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
			using (var context = new ChinookEntities())
			{
				var result = context.ExecuteStoreQuery<Artist>("SELECT ArtistId, Name FROM Artist WHERE Artist.ArtistId=@id",
				                                               new SqlParameter
				                                               	{
				                                               		ParameterName = "@id",
				                                               		Value = id
				                                               	});
				return result.FirstOrDefault();
			}
		}

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
				                                                    		Value = name
				                                                    	});
				return result.ToList();
			}
		}
	}
}