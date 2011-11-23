using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using DataAccessPlayground.EntityFramework;
using DataAccessPlayground.Tests.Helpers;
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
		
		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void DataReaderQueryTest(int total)
		{
			using (new TimeIt(total))
			{
				const string query =@"SELECT Album.Title as AlbumName, Track.Name as SongName, Artist.Name as ArtistName 
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
				using (var connection = new SqlConnection(ConnectionString))
				{
					connection.Open();
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
		public void DapperQueryTest(int total)
		{
			using(new TimeIt(total))
			for (int i = 0; i < total; i++)
			{
				using (var connection = new SqlConnection(ConnectionString))
				{
					connection.Open();
					var songs = connection.Query<Song>(SqlQuery, new {name = "Pearl Jam"});
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
				using (var connection = new SqlConnection(ConnectionString))
				{
					connection.Open();
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

		private static Func<ChinookEntities, string, IQueryable<Song>> _query;

		Func<ChinookEntities, string, IQueryable<Song>> Query
		{
			get
			{
				return _query ?? (_query = CompiledQuery.Compile<ChinookEntities, string, IQueryable<Song>>(
							(context, name) => from track in context.Tracks
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
			using (new TimeIt(total))
			for (int i = 0; i < total; i++)
			{
				using (var context = new ChinookEntities())
				{
					var songs = Query.Invoke(context, "Pearl Jam");
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
		public void EntityFrameworkQueryTest(int total)
		{
			using (new TimeIt(total))
			for (int i = 0; i < total; i++)
			{
				using (var context = new ChinookEntities())
				{
					var result = context.ExecuteStoreQuery<Song>(SqlQuery, new SqlParameter
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
	}
}
