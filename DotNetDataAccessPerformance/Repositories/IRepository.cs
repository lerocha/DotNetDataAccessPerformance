using System.Collections.Generic;
using DotNetDataAccessPerformance.Domain;

namespace DotNetDataAccessPerformance.Repositories
{
	public interface IRepository
	{
		Artist GetArtistById(int id);
		IEnumerable<Song> GetSongsByArtist(string name);
	}
}
