using System.Collections.Generic;
using System.Linq;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.Helpers;

namespace DotNetDataAccessPerformance.Repositories
{
	public class NHibernateNativeQueryRepository : IRepository
	{
		public IEnumerable<Song> GetSongsByArtist(string name)
		{
			const string query = @"SELECT Album.Title as AlbumName, Track.Name as SongName, Artist.Name as ArtistName
									   FROM Artist
									   INNER JOIN Album ON Album.ArtistId = Artist.ArtistId
									   INNER JOIN Track ON Track.AlbumId = Album.AlbumId
									   WHERE Artist.Name='Pearl Jam'";

			using (var session = NHibernateHelper.OpenSession())
			{
				var sqlQuery = session.CreateSQLQuery(query);

				var songs = (from object[] item in sqlQuery.List()
				             select new Song
				                    	{
				                    		AlbumName = (string)item[0],
				                    		SongName = (string)item[1],
				                    		ArtistName = (string)item[2]
				                    	}).ToList();

				return songs;
			}
		}
	}
}