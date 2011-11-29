using System.Collections.Generic;
using DotNetDataAccessPerformance.Domain;

namespace DotNetDataAccessPerformance.Repositories
{
	public interface IRepository
	{
		void AddArtist(Artist artist);
		void UpdateArtist(Artist artist);
		void DeleteArtist(Artist artist);
		Artist GetArtistById(int id);
		IEnumerable<Song> GetSongsByArtist(string name);
	}
}
