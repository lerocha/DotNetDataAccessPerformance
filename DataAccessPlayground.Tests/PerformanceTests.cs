using System;
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
		private readonly Stopwatch _timer = new Stopwatch();

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void DapperSelectTest(int total)
		{
			_timer.Reset();
			_timer.Start();

			const string query = @"SELECT Album.Title as AlbumName, Track.Name as SongName, Artist.Name as ArtistName 
								   FROM Track LEFT JOIN Album ON Track.AlbumId = Album.AlbumId
								   LEFT JOIN Artist ON Album.ArtistId = Artist.ArtistId
								   WHERE Artist.Name = @name";

			for (int i = 0; i < total; i++)
			{
				using (IDbConnection connection = new SqlConnection(ConnectionString))
				{
					connection.Open();
					var songs = connection.Query<Song>(query, new {name = "Pearl Jam"});
					Assert.True(songs.Count() > 0);
				}
			}
			_timer.Stop();
			Console.WriteLine("test=DapperSelectTest; total={0}; time={1};", total, _timer.ElapsedMilliseconds);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(500)]
		[InlineData(1000)]
		public void EntityFrameworkSelectTest(int total)
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
			Console.WriteLine("test=EntityFrameworkSelectTest; total={0}; time={1};", total, _timer.ElapsedMilliseconds);
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
	}
}
