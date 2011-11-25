using System.Collections.Generic;
using System.Linq;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.EntityFramework;

namespace DotNetDataAccessPerformance.Repositories
{
	public class EntityFrameworkLinqToEntitiesRepository : IRepository
	{
		public IEnumerable<Song> GetSongsByArtist(string name)
		{
			using (var context = new ChinookEntities())
			{
				var query = from track in context.Tracks
				            where track.Album.Artist.Name == "Pearl Jam"
				            select new Song
				                   	{
				                   		AlbumName = track.Album.Title,
				                   		ArtistName = track.Album.Artist.Name,
				                   		SongName = track.Name
				                   	};

				return query.ToList();
			}
		}
	}
}