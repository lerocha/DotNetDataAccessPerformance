using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DotNetDataAccessPerformance.Domain;
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
		[InlineData(typeof(DapperNativeQueryRepository))]
		[InlineData(typeof(DapperStoredProcedureRepository))]
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
			var repository = RepositoryFactory.Create(type);

			foreach (var total in _totals)
			{
				using (new TimeIt(total, "QueryUsingJoins", type))
				{
					for (int i = 0; i < total; i++)
					{
						var songs = repository.GetSongsByArtist("Pearl Jam");
						Assert.True(songs.Count() > 0);
					}
				}
			}
		}

		[Theory]
		[InlineData(typeof(DataReaderNativeQueryRepository))]
		[InlineData(typeof(DataReaderStoredProcedureRepository))]
		[InlineData(typeof(DapperNativeQueryRepository))]
		[InlineData(typeof(DapperStoredProcedureRepository))]
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
			var repository = RepositoryFactory.Create(type);

			foreach (var total in _totals)
			{
				using (new TimeIt(total, "QueryByPrimaryKey", type))
				{
					for (int i = 0; i < total; i++)
					{
						var artist = repository.GetArtistById(10);
						Assert.NotNull(artist);
					}
				}
			}
		}

		[Theory(Skip = "Not implemented.")]
		[InlineData(typeof(DataReaderNativeQueryRepository))]
		[InlineData(typeof(DataReaderStoredProcedureRepository))]
		[InlineData(typeof(DapperNativeQueryRepository))]
		[InlineData(typeof(DapperStoredProcedureRepository))]
		[InlineData(typeof(EntityFrameworkCompiledLinqQueryRepository))]
		[InlineData(typeof(EntityFrameworkLinqToEntitiesRepository))]
		[InlineData(typeof(EntityFrameworkNativeQueryRepository))]
		[InlineData(typeof(EntityFrameworkStoredProcedureRepository))]
		[InlineData(typeof(NHibernateNativeQueryRepository))]
		[InlineData(typeof(NHibernateHqlQueryRepository))]
		[InlineData(typeof(NHibernateHqlQueryStrongTypeRepository))]
		[InlineData(typeof(NHibernateStoredProcedureRepository))]
		public void AddUpdateDeleteTest(Type type)
		{
			var repository = RepositoryFactory.Create(type);

			foreach (var total in _totals)
			{
				// Build a list of objects to be added, updated, and deleted.
				var artists = new List<Artist>();
				for (int i = 0; i < total; i++)
				{
					artists.Add(new Artist {Name = "Dummy" + i});
				}

				using (new TimeIt(total, "Add", type))
				{
					foreach (var artist in artists)
					{
						repository.AddArtist(artist);
					}
				}

				using (new TimeIt(total, "Update", type))
				{
					foreach (var artist in artists)
					{
						artist.Name += "_changed";
						repository.UpdateArtist(artist);
					}
				}

				using (new TimeIt(total, "Delete", type))
				{
					foreach (var artist in artists)
					{
						repository.DeleteArtist(artist);
					}
				}
			}
		}
	}
}
