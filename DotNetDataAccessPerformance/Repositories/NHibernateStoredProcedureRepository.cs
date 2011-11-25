using System.Collections.Generic;
using System.Linq;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.Helpers;

namespace DotNetDataAccessPerformance.Repositories
{
	public class NHibernateStoredProcedureRepository : IRepository
	{
		public IEnumerable<Song> GetSongsByArtist(string name)
		{
			using (var session = NHibernateHelper.OpenSession())
			{
				var query = session.GetNamedQuery("spGetSongsByArtist")
					.SetString("name", "Pearl Jam");
				var songs = (from object[] item in query.List()
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