using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using DotNetDataAccessPerformance.Domain;
using DotNetDataAccessPerformance.EntityFramework;
using DotNetDataAccessPerformance.Helpers;
using Xunit;
using Xunit.Extensions;

namespace DotNetDataAccessPerformance.Tests
{
	public class PerformanceTests
	{
		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void DataReaderNativeQueryTest(int total)
		{
			using (new TimeIt(total))
			for (int i = 0; i < total; i++)
			{
				const string query = @"SELECT Album.Title as AlbumName, Track.Name as SongName, Artist.Name as ArtistName 
									   FROM Track LEFT JOIN Album ON Track.AlbumId = Album.AlbumId
									   LEFT JOIN Artist ON Album.ArtistId = Artist.ArtistId
									   WHERE Artist.Name = 'Pearl Jam'";

				using (var connection = ConnectionFactory.OpenConnection())
				{
					var command = new SqlCommand(query, connection);

					var songs = new List<Song>();
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							songs.Add(new Song
								        {
								          	AlbumName = reader[0] as string,
								          	SongName = reader[1] as string,
								          	ArtistName = reader[2] as string
								        });
						}
					}

					Assert.True(songs.Count() > 0);
				}
			}
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void DataReaderStoredProcedureTest(int total)
		{
			using(new TimeIt(total))
			for (int i = 0; i < total; i++)
			{
				using (var connection = ConnectionFactory.OpenConnection())
				{
					var command = new SqlCommand("spGetSongsByArtist", connection);
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@name", "Pearl Jam"));

					var songs = new List<Song>();
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							songs.Add(new Song
							{
								AlbumName = reader[0] as string,
								SongName = reader[1] as string,
								ArtistName = reader[2] as string
							});
						}
					}

					Assert.True(songs.Count() > 0);
				}
			}
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void DapperNativeQueryTest(int total)
		{
			using(new TimeIt(total))
			for (int i = 0; i < total; i++)
			{
				const string query = @"SELECT Album.Title as AlbumName, Track.Name as SongName, Artist.Name as ArtistName 
									   FROM Track LEFT JOIN Album ON Track.AlbumId = Album.AlbumId
									   LEFT JOIN Artist ON Album.ArtistId = Artist.ArtistId
									   WHERE Artist.Name = @name";

				using (var connection = ConnectionFactory.OpenConnection())
				{
					var songs = connection.Query<Song>(query, new {name = "Pearl Jam"});
					Assert.True(songs.Count() > 0);
				}
			}
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void DapperStoredProcedureTest(int total)
		{
			using (new TimeIt(total))
			for (int i = 0; i < total; i++)
			{
				using (var connection = ConnectionFactory.OpenConnection())
				{
					var songs = connection.Query<Song>("spGetSongsByArtist", 
														new { name = "Pearl Jam" },
														commandType: CommandType.StoredProcedure);
					Assert.True(songs.Count() > 0);
				}
			}
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void EntityFrameworkLinqToEntitiesTest(int total)
		{
			using (new TimeIt(total))
			for (int i = 0; i < total; i++)
			{
				using (var context = new ChinookEntities())
				{
					var query = from track in context.Tracks
					            where track.Album.Artist.Name == "Pearl Jam"
					            select new Song
					                   	{
					                   		AlbumName = track.Album.Title,
					                   		ArtistName = track.Album.Artist.Name,
					                   		SongName = track.Name
					                   	};

					var songs = query.ToList();
					Assert.True(songs.Count > 0);
				}
			}
		}

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

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void EntityFrameworkCompiledLinqQueryTest(int total)
		{
			using (new TimeIt(total))
			for (int i = 0; i < total; i++)
			{
				using (var context = new ChinookEntities())
				{
					var songs = CompiledLinqCompiledLinqQuery.Invoke(context, "Pearl Jam");
					Assert.True(songs.ToList().Count > 0);
				}
			}
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void EntityFrameworkNativeQueryTest(int total)
		{
			using (new TimeIt(total))
			for (int i = 0; i < total; i++)
			{
				const string query = @"SELECT Album.Title as AlbumName, Track.Name as SongName, Artist.Name as ArtistName 
									   FROM Track LEFT JOIN Album ON Track.AlbumId = Album.AlbumId
									   LEFT JOIN Artist ON Album.ArtistId = Artist.ArtistId
									   WHERE Artist.Name = @name";

				using (var context = new ChinookEntities())
				{
					var result = context.ExecuteStoreQuery<Song>(query, new SqlParameter
					{
						ParameterName = "@name",
						Value = "Pearl Jam"
					});
					var songs = result.ToList();
					Assert.True(songs.Count > 0);
				}
			}
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void EntityFrameworkStoredProcedureTest(int total)
		{
			using (new TimeIt(total))
			for (int i = 0; i < total; i++)
			{
				using (var context = new ChinookEntities())
				{
					var result = context.GetSongsByArtist("Pearl Jam");
					var songs = result.ToList();
					Assert.True(songs.Count > 0);
				}
			}
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void NHibernateNativeQueryTest(int total)
		{
			using (new TimeIt(total))
				for (int i = 0; i < total; i++)
				{
					const string query = @"SELECT Album.Title as AlbumName, Track.Name as SongName, Artist.Name as ArtistName 
									   FROM Track LEFT JOIN Album ON Track.AlbumId = Album.AlbumId
									   LEFT JOIN Artist ON Album.ArtistId = Artist.ArtistId
									   WHERE Artist.Name = 'Pearl Jam'";

					using (var session = NHibernateHelper.OpenSession())
					{
						var sqlQuery = session.CreateSQLQuery(query);

						var songs = (from object[] item in sqlQuery.List() 
									 select new Song
									        	{
									        		AlbumName = (string)item[0],
													SongName = (string) item[1],
													ArtistName = (string) item[2]
									        	}).ToList();

						Assert.True(songs.Count() > 0);
					}
				}
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void NHibernateStoredProcedureTest(int total)
		{
			using (new TimeIt(total))
			for (int i = 0; i < total; i++)
			{
				using (var session = NHibernateHelper.OpenSession())
				{
					var query = session.GetNamedQuery("spGetSongsByArtist")
									   .SetString("name", "Pearl Jam");
					var songs = (from object[] item in query.List()
								 select new Song
								 {
									 AlbumName = (string)item[0],
									 SongName = (string)item[1],
									 ArtistName = (string)item[2]
								 }).ToList();
					Assert.True(songs.Count > 0);
				}
			}
		}
	}
}
