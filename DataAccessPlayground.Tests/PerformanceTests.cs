using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Dapper;
using DataAccessPlayground.EntityFramework;
using Xunit;
using Xunit.Extensions;

namespace DataAccessPlayground.Tests
{
	public class Song
	{
		public string SongName { get; set; }
		public string AlbumName { get; set; }
		public string ArtistName { get; set; }
	}

	public class PerformanceTests
	{
		private const string ConnectionString = "data source=localhost;initial catalog=Chinook;integrated security=True;multipleactiveresultsets=True;";

		private const string SqlQuery = @"SELECT Album.Title as AlbumName, Track.Name as SongName, Artist.Name as ArtistName 
										FROM Track LEFT JOIN Album ON Track.AlbumId = Album.AlbumId
										LEFT JOIN Artist ON Album.ArtistId = Artist.ArtistId
										WHERE Artist.Name = @name";
		
		private readonly Stopwatch _timer = new Stopwatch();

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void DataReaderQueryTest(int total)
		{
			_timer.Reset();
			_timer.Start();

			const string query = @"SELECT Album.Title as AlbumName, Track.Name as SongName, Artist.Name as ArtistName 
								   FROM Track LEFT JOIN Album ON Track.AlbumId = Album.AlbumId
								   LEFT JOIN Artist ON Album.ArtistId = Artist.ArtistId
								   WHERE Artist.Name = 'Pearl Jam'";

			for (int i = 0; i < total; i++)
			{
				using (var connection = new SqlConnection(ConnectionString))
				{
					connection.Open();
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
			_timer.Stop();
			Console.WriteLine("test=DataReaderQueryTest; total={0}; time={1};", total, _timer.ElapsedMilliseconds);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void DataReaderStoredProcedureTest(int total)
		{
			_timer.Reset();
			_timer.Start();

			for (int i = 0; i < total; i++)
			{
				using (var connection = new SqlConnection(ConnectionString))
				{
					connection.Open();
					var command = new SqlCommand("spGetSongsByArtist", connection)
					              	{
					              		CommandType = CommandType.StoredProcedure
					              	};
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
			_timer.Stop();
			Console.WriteLine("test=DataReaderStoredProcedureTest; total={0}; time={1};", total, _timer.ElapsedMilliseconds);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void DapperQueryTest(int total)
		{
			_timer.Reset();
			_timer.Start();

			for (int i = 0; i < total; i++)
			{
				using (var connection = new SqlConnection(ConnectionString))
				{
					connection.Open();
					var songs = connection.Query<Song>(SqlQuery, new {name = "Pearl Jam"});
					Assert.True(songs.Count() > 0);
				}
			}
			_timer.Stop();
			Console.WriteLine("test=DapperQueryTest; total={0}; time={1};", total, _timer.ElapsedMilliseconds);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void DapperStoredProcedureTest(int total)
		{
			_timer.Reset();
			_timer.Start();

			for (int i = 0; i < total; i++)
			{
				using (var connection = new SqlConnection(ConnectionString))
				{
					connection.Open();
					var songs = connection.Query<Song>("spGetSongsByArtist", 
														new { name = "Pearl Jam" },
														commandType: CommandType.StoredProcedure);
					Assert.True(songs.Count() > 0);
				}
			}
			_timer.Stop();
			Console.WriteLine("test=DapperStoredProcedureTest; total={0}; time={1};", total, _timer.ElapsedMilliseconds);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void EntityFrameworkLinqToEntitiesTest(int total)
		{
			_timer.Reset();
			_timer.Start();

			for (int i = 0; i < total; i++)
			{
				using (var entities = new ChinookEntities())
				{
					var query = from track in entities.Tracks
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

			_timer.Stop();
			Console.WriteLine("test=EntityFrameworkLinqToEntitiesTest; total={0}; time={1};", total, _timer.ElapsedMilliseconds);
		}

		private static Func<ChinookEntities, string, IQueryable<Song>> _query;

		Func<ChinookEntities, string, IQueryable<Song>> Query
		{
			get
			{
				return _query ?? (_query = CompiledQuery.Compile<ChinookEntities, string, IQueryable<Song>>(
							(entities, name) => from track in entities.Tracks
												where track.Album.Artist.Name == name
												select new Song
					                           			{
					                           				AlbumName = track.Album.Title,
					                           				ArtistName = track.Album.Artist.Name,
					                           				SongName = track.Name
					                           			}));
			}
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void EntityFrameworkPreCompiledSelectTest(int total)
		{
			_timer.Reset();
			_timer.Start();

			for (int i = 0; i < total; i++)
			{
				using (var entities = new ChinookEntities())
				{
					var songs = Query.Invoke(entities, "Pearl Jam");
					Assert.True(songs.ToList().Count > 0);
				}
			}

			_timer.Stop();
			Console.WriteLine("test=EntityFrameworkPreCompiledSelectTest; total={0}; time={1};", total, _timer.ElapsedMilliseconds);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void EntityFrameworkQueryTest(int total)
		{
			_timer.Reset();
			_timer.Start();

			for (int i = 0; i < total; i++)
			{
				using (var entities = new ChinookEntities())
				{
					var result = entities.ExecuteStoreQuery<Song>(SqlQuery, new SqlParameter
					{
						ParameterName = "@name",
						Value = "Pearl Jam"
					});
					var songs = result.ToList();
					Assert.True(songs.Count > 0);
				}
			}

			_timer.Stop();
			Console.WriteLine("test=EntityFrameworkQueryTest; total={0}; time={1};", total, _timer.ElapsedMilliseconds);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void EntityFrameworkStoredProcedureTest(int total)
		{
			_timer.Reset();
			_timer.Start();

			for (int i = 0; i < total; i++)
			{
				using (var entities = new ChinookEntities())
				{
					var result = entities.GetSongsByArtist("Pearl Jam");
					var songs = result.ToList();
					Assert.True(songs.Count > 0);
				}
			}

			_timer.Stop();
			Console.WriteLine("test=EntityFrameworkStoredProcedureTest; total={0}; time={1};", total, _timer.ElapsedMilliseconds);
		}
	}
}
