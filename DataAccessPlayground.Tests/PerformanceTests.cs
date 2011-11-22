using System;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Dapper;
using DataAccessPlayground.EntityFramework;
using Xunit;

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
		private const string ConnectionString = "data source=DEV-TEMPLATE;initial catalog=Chinook;integrated security=True;multipleactiveresultsets=True;";
		Stopwatch _timer = new Stopwatch();


		[Fact]
		public void DapperSelectTest()
		{
			_timer.Reset();
			_timer.Start();

			var query = @"SELECT Album.Title as AlbumName, Track.Name as SongName, Artist.Name as ArtistName 
						FROM Track LEFT JOIN Album ON Track.AlbumId = Album.AlbumId
						LEFT JOIN Artist ON Album.ArtistId = Artist.ArtistId
						WHERE Artist.Name = @name";

			using (IDbConnection connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				var songs = connection.Query<Song>(query, new {name = "Pearl Jam"});
				Assert.True(songs.Count() > 0);
			}

			_timer.Stop();
			Console.WriteLine(_timer.ElapsedMilliseconds);
		}

		[Fact]
		public void EntityFrameworkSelectTest()
		{
			_timer.Reset();
			_timer.Start();

			using (var entities = new ChinookEntities())
			{
				var songs = from track in entities.Tracks
							where track.Album.Artist.Name == "Pearl Jam"
							select new Song
							{
								AlbumName = track.Album.Title,
								ArtistName = track.Album.Artist.Name,
								SongName = track.Name
							};

				Assert.True(songs.ToList().Count > 0);
			}

			_timer.Stop();
			Console.WriteLine(_timer.ElapsedMilliseconds);
		}

		private static readonly Func<ChinookEntities, string, IQueryable<Song>> Query =
			CompiledQuery.Compile<ChinookEntities, string, IQueryable<Song>>(
				(entities, name) => from track in entities.Tracks
				                    where track.Album.Artist.Name == name
				                    select new Song
				                           	{
				                           		AlbumName = track.Album.Title,
				                           		ArtistName = track.Album.Artist.Name,
				                           		SongName = track.Name
				                           	});

		[Fact]
		public void EntityFrameworkPreCompiledSelectTest()
		{
			_timer.Reset();
			_timer.Start();

			using (var entities = new ChinookEntities())
			{
				var songs = Query.Invoke(entities, "Pearl Jam");
				Assert.True(songs.ToList().Count > 0);
			}

			_timer.Stop();
			Console.WriteLine(_timer.ElapsedMilliseconds);
		}
	}
}
