using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.EntityFramework;
using Artist = DotNetDataAccessPerformance.Domain.Artist;

namespace DotNetDataAccessPerformance.Repositories
{
	public class EntityFrameworkCompiledLinqQueryRepository : IRepository
	{
		public void AddArtist(Artist artist)
		{
			throw new NotImplementedException();
		}

		public void UpdateArtist(Artist artist)
		{
			throw new NotImplementedException();
		}

		public void DeleteArtist(Artist artist)
		{
			throw new NotImplementedException();
		}

		private static Func<ChinookEntities, int, IQueryable<Artist>> _getArtistByIdQuery;
		Func<ChinookEntities, int, IQueryable<Artist>> GetArtistByIdQuery
		{
			get
			{
				if (_getArtistByIdQuery == null)
				{
					_getArtistByIdQuery = CompiledQuery.Compile<ChinookEntities, int, IQueryable<Artist>>(
							(context, id) => from artist in context.Artists 
											 where artist.ArtistId == id
											 select new Artist 
											 {
												 ArtistId = artist.ArtistId,
												 Name = artist.Name
											 });

				}
				return _getArtistByIdQuery;
			}
		}

		public Artist GetArtistById(int id)
		{
			using (var context = new ChinookEntities())
			{
				context.Artists.MergeOption = MergeOption.NoTracking;
				return GetArtistByIdQuery.Invoke(context, id).FirstOrDefault();
			}
		}

		private static Func<ChinookEntities, string, IQueryable<Song>> _getSongsByArtistQuery;
		Func<ChinookEntities, string, IQueryable<Song>> GetSongsByArtistQuery
		{
			get
			{
				if (_getSongsByArtistQuery == null)
				{
					_getSongsByArtistQuery = CompiledQuery.Compile<ChinookEntities, string, IQueryable<Song>>(
						(context, name) => from track in context.Tracks
						                   where track.Album.Artist.Name == name
						                   select new Song
						                          	{
						                          		AlbumName = track.Album.Title,
						                          		ArtistName = track.Album.Artist.Name,
						                          		SongName = track.Name
						                          	});
				}

				return _getSongsByArtistQuery;
			}
		}

		public IEnumerable<Song> GetSongsByArtist(string name)
		{
			using (var context = new ChinookEntities())
			{
				context.Tracks.MergeOption = MergeOption.NoTracking;
				return GetSongsByArtistQuery.Invoke(context, name).ToList();
			}
		}
	}
}