using System.Collections.Generic;
using System.Data;
using Dapper;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.Helpers;

namespace DotNetDataAccessPerformance.Repositories
{
	public class DrapperStoredProcedureRepository : IRepository
	{
		public IEnumerable<Song> GetSongsByArtist(string name)
		{
			using (var connection = ConnectionFactory.OpenConnection())
			{
				var songs = connection.Query<Song>("spGetSongsByArtist",
				                                   new { name = "Pearl Jam" },
				                                   commandType: CommandType.StoredProcedure);
				return songs;
			}
		}
	}
}