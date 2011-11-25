using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.EntityFramework;

namespace DotNetDataAccessPerformance.Repositories
{
	public class EntityFrameworkCompiledLinqQueryRepository : IRepository
	{
		private static Func<ChinookEntities, string, IQueryable<Song>> _compiledLinqQuery;

		Func<ChinookEntities, string, IQueryable<Song>> CompiledLinqCompiledLinqQuery
		{
			get
			{
				if (_compiledLinqQuery == null)
				{
					_compiledLinqQuery = CompiledQuery.Compile<ChinookEntities, string, IQueryable<Song>>(
						(context, name) => from track in context.Tracks
						                   where track.Album.Artist.Name == name
						                   select new Song
						                          	{
						                          		AlbumName = track.Album.Title,
						                          		ArtistName = track.Album.Artist.Name,
						                          		SongName = track.Name
						                          	});
				}

				return _compiledLinqQuery;
			}
		}

		public IEnumerable<Song> GetSongsByArtist(string name)
		{
			using (var context = new ChinookEntities())
			{
				return CompiledLinqCompiledLinqQuery.Invoke(context, "Pearl Jam").ToList();
			}
		}
	}
}