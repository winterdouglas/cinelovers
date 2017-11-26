using Cinelovers.Core.Services.Models;
using Cinelovers.ViewModels.Movies;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;

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
            var target = new MovieCellViewModel(movie);

            Assert.AreEqual(movie.Id, target.Id);
            Assert.AreEqual(movie.LargePosterUri, target.LargePosterUri);
            Assert.AreEqual(movie.SmallPosterUri, target.SmallPosterUri);
            Assert.AreEqual(movie.Overview, target.Overview);
            Assert.AreEqual(movie.Popularity, target.Popularity);
            Assert.AreEqual(movie.ReleaseDate, target.ReleaseDate);
            Assert.AreEqual(movie.Title, target.Title);
            Assert.AreEqual(movie.VoteAverage, target.VoteAverage);
            Assert.AreEqual(movie.VoteCount, target.VoteCount);
            Assert.AreEqual(movie.Genres.Count, target.Genres.Count);
            Assert.AreEqual(movie.Genres[0].Name, target.Genres[0]);
            Assert.AreEqual(movie.Genres[1].Name, target.Genres[1]);
        }
    }
}
