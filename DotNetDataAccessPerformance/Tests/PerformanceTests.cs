using System;
using System.Diagnostics;
using System.Linq;
using DotNetDataAccessPerformance.Helpers;
using DotNetDataAccessPerformance.Repositories;
using Xunit;
using Xunit.Extensions;

namespace DotNetDataAccessPerformance.Tests
{
	public class PerformanceTests
	{
		private readonly int[] _totals = { 1, 1, 10, 100, 500, 1000 };

		private static DelimitedListTraceListener _listener;

		public PerformanceTests()
		{
			if (_listener == null)
			{
				_listener = new DelimitedListTraceListener(@"PerformanceTests.csv");
			}

			Trace.Listeners.Clear();
			Trace.Listeners.Add(_listener);
			Trace.AutoFlush = true;
		}

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
		[InlineData(typeof(NHibernateHqlQueryStrongTypeRepository))]
		[InlineData(typeof(NHibernateStoredProcedureRepository))]
		public void QueryUsingJoinsTest(Type type)
		{
			foreach (var total in _totals)
			{
				using (new TimeIt(total, "QueryUsingJoins", type))
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
		[InlineData(typeof(NHibernateHqlQueryStrongTypeRepository))]
		[InlineData(typeof(NHibernateStoredProcedureRepository))]
		public void QueryByPrimaryKeyTest(Type type)
		{
			foreach (var total in _totals)
			{
				using (new TimeIt(total, "QueryByPrimaryKey", type))
				{
					for (int i = 0; i < total; i++)
					{
						var repository = RepositoryFactory.Create(type);
						var artist = repository.GetArtistById(10);
						Assert.NotNull(artist);
					}
				}
			}
		}
	}
}
