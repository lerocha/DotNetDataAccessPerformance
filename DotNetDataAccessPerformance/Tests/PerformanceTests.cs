using System;
using System.Linq;
using DotNetDataAccessPerformance.Helpers;
using DotNetDataAccessPerformance.Repositories;
using Xunit;
using Xunit.Extensions;

namespace DotNetDataAccessPerformance.Tests
{
	public class PerformanceTests
	{
		private readonly int[] _totals = {1, 1, 10, 100, 500, 1000};

		[Theory]
		[InlineData(typeof(DataReaderNativeQueryRepository))]
		[InlineData(typeof(DataReaderStoredProcedureRepository))]
		[InlineData(typeof(DrapperNativeQueryRepository))]
		[InlineData(typeof(DrapperStoredProcedureRepository))]
		[InlineData(typeof(EntityFrameworkCompiledLinqQueryRepository))]
		[InlineData(typeof(EntityFrameworkLinqToEntitiesRepository))]
		[InlineData(typeof(EntityFrameworkNativeQueryRepository))]
		[InlineData(typeof(EntityFrameworkStoredProcedureRepository))]
		[InlineData(typeof(NHibernateNativeQueryRepository))]
		[InlineData(typeof(NHibernateHqlQueryRepository))]
		[InlineData(typeof(NHibernateQueryStrongTypeRepository))]
		[InlineData(typeof(NHibernateStoredProcedureRepository))]
		public void GetSongsByArtistTest(Type type)
		{
			foreach (var total in _totals)
			{
				using (new TimeIt(total, type.Name))
				{
					for (int i = 0; i < total; i++)
					{
						var repository = RepositoryFactory.Create(type);
						var songs = repository.GetSongsByArtist("Pearl Jam");
						Assert.True(songs.Count() > 0);
					}
				}
			}
		}
	}
}
