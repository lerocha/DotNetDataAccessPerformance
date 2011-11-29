using System.Collections.Generic;
using System.Linq;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.Helpers;

namespace DotNetDataAccessPerformance.Repositories
{
	public class NHibernateStoredProcedureRepository : IRepository
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
			using (var session = NHibernateHelper.OpenSession())
			{
				var query = session.GetNamedQuery("spGetArtistById").SetInt32("id", id);
				return query.List<Artist>().FirstOrDefault();
			}
		}

		public IEnumerable<Song> GetSongsByArtist(string name)
		{
			using (var session = NHibernateHelper.OpenSession())
			{
				var query = session.GetNamedQuery("spGetSongsByArtist").SetString("name", name);
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