using System.Collections.Generic;
using System.Linq;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.EntityFramework;

namespace DotNetDataAccessPerformance.Repositories
{
	public class EntityFrameworkStoredProcedureRepository : IRepository
	{
		public IEnumerable<Song> GetSongsByArtist(string name)
		{
			using (var context = new ChinookEntities())
			{
				var result = context.GetSongsByArtist("Pearl Jam");
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