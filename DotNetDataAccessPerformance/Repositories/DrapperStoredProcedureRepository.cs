using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.Helpers;

namespace DotNetDataAccessPerformance.Repositories
{
	public class DrapperStoredProcedureRepository : IRepository
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
			using (var connection = ConnectionFactory.OpenConnection())
			{
				return connection.Query<Artist>("spGetArtistById", new { id }, 
												commandType: CommandType.StoredProcedure)
								 .FirstOrDefault();
			}
		}

		public IEnumerable<Song> GetSongsByArtist(string name)
		{
			using (var connection = ConnectionFactory.OpenConnection())
			{
				return connection.Query<Song>("spGetSongsByArtist",
				                                new { name },
				                                commandType: CommandType.StoredProcedure);
			}
		}
	}
}