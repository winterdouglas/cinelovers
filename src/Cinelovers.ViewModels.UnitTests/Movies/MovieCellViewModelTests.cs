using Cinelovers.Core.Services.Models;
using Cinelovers.ViewModels.Movies;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Cinelovers.ViewModels.UnitTests.Movies
{
    [TestFixture]
    public class MovieCellViewModelTests
    {
        [Test]
        public void MovieCellViewModel_MovieIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new MovieCellViewModel(null));
        }

        [Test]
        public void GetProperties_HasMovie_ReturnsCorrectProperties()
        {
            var genres = new List<Genre>()
            {
                new Genre() { Id = 1, Name = "Genre 1" },
                new Genre() { Id = 2, Name = "Genre 2" }
            };
            var movie = new Movie()
            {
                Id = 2,
                LargePosterUri = new Uri("https://large.poster.path", UriKind.Absolute),
                SmallPosterUri = new Uri("https://small.poster.path", UriKind.Absolute),
                Overview = "Just a test overview for this movie",
                Popularity = 8D,
                ReleaseDate = DateTime.Parse(
                    "2010-10-12", 
                    new CultureInfo("en-US"), 
                    DateTimeStyles.AssumeUniversal),
                Title = "Movie Title",
                VoteAverage = 8.2D,
                VoteCount = 120,
                Genres = genres
            };
            var actual = new MovieCellViewModel(movie);

            Assert.AreEqual(movie.Id, actual.Id);
            Assert.AreEqual(movie.LargePosterUri, actual.LargePosterUri);
            Assert.AreEqual(movie.SmallPosterUri, actual.SmallPosterUri);
            Assert.AreEqual(movie.Overview, actual.Overview);
            Assert.AreEqual(movie.Popularity, actual.Popularity);
            Assert.AreEqual(movie.ReleaseDate, actual.ReleaseDate);
            Assert.AreEqual(movie.Title, actual.Title);
            Assert.AreEqual(movie.VoteAverage, actual.VoteAverage);
            Assert.AreEqual(movie.VoteCount, actual.VoteCount);
            Assert.AreEqual(movie.Genres.Count, actual.Genres.Count);
            Assert.AreEqual(movie.Genres[0].Name, actual.Genres[0]);
            Assert.AreEqual(movie.Genres[1].Name, actual.Genres[1]);
            Assert.AreEqual(string.Join(", ", movie.Genres.Select(g => g.Name)), actual.GenresText);
            Assert.AreEqual($"Released in {movie.ReleaseDate:yyyy-MM-dd}", actual.ReleasedIn);
        }
    }
}
