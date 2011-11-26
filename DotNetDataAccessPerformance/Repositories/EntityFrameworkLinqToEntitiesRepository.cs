using System.Collections.Generic;
using System.Linq;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.EntityFramework;
using Artist = DotNetDataAccessPerformance.Domain.Artist;

namespace DotNetDataAccessPerformance.Repositories
{
	public class EntityFrameworkLinqToEntitiesRepository : IRepository
	{
		public Artist GetArtistById(int id)
		{
			using (var context = new ChinookEntities())
			{
				var query = from artist in context.Artists
				            where artist.ArtistId == id
				            select new Artist
				                   	{
				                   		ArtistId = artist.ArtistId,
				                   		Name = artist.Name
				                   	};
				return query.FirstOrDefault();
			}
		}

		public IEnumerable<Song> GetSongsByArtist(string name)
		{
			using (var context = new ChinookEntities())
			{
				var query = from track in context.Tracks
				            where track.Album.Artist.Name == name
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