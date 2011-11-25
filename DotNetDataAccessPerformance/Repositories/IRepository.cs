using System.Collections.Generic;
using DotNetDataAccessPerformance.Domain;

namespace DotNetDataAccessPerformance.Repositories
{
	public interface IRepository
	{
		IEnumerable<Song> GetSongsByArtist(string name);
	}
}
