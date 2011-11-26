using System.Collections.Generic;
using System.Linq;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.EntityFramework;
using Artist = DotNetDataAccessPerformance.Domain.Artist;

namespace DotNetDataAccessPerformance.Repositories
{
	public class EntityFrameworkStoredProcedureRepository : IRepository
	{
		public Artist GetArtistById(int id)
		{
			using (var context = new ChinookEntities())
			{
				var result = context.GetArtistById(id);
				var items = from item in result
				            select new Artist
				                   	{
				                   		ArtistId = item.ArtistId,
				                   		Name = item.Name
				                   	};
				return items.FirstOrDefault();
			}
		}

		public IEnumerable<Song> GetSongsByArtist(string name)
		{
			using (var context = new ChinookEntities())
			{
				var result = context.GetSongsByArtist(name);
				var songs = (from item in result.ToList()
				             select new Song
				                    	{
				                    		SongName = item.SongName,
				                    		AlbumName = item.AlbumName,
				                    		ArtistName = item.ArtistName
				                    	}).ToList();
				return songs;
			}
		}
	}
}