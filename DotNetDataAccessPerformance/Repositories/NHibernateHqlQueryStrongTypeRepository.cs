using System.Collections.Generic;
using System.Linq;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.Helpers;

namespace DotNetDataAccessPerformance.Repositories
{
	public class NHibernateHqlQueryStrongTypeRepository : IRepository
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
				var query = session.CreateQuery("from Artist artist where artist.ArtistId=" + id);
				return query.List<Artist>().FirstOrDefault();
			}
		}

		public IEnumerable<Song> GetSongsByArtist(string name)
		{
			using (var session = NHibernateHelper.OpenSession())
			{
				var query = session.CreateQuery(@"select track
												from Track track 
												join track.Album as album
												join album.Artist as artist
												where artist.Name='" + name + "'");

				var songs = (from track in query.List<Track>()
				             select new Song
				                    	{
				                    		AlbumName = track.Album.Title,
				                    		SongName = track.Name,
				                    		ArtistName = track.Album.Artist.Name
				                    	}).ToList();

				return songs;
			}
		}
	}
}