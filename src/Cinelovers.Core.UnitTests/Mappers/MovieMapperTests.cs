using Cinelovers.Core.Mappers;
using Cinelovers.Core.Rest.Dtos;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Cinelovers.Core.UnitTests.Mappers
{
    [TestFixture]
    public class MovieMapperTests
    {
        [Test]
        public void ToMovie_HasMovieResultWithNoAvailableGenres_ReturnsMovieWithNoGenres()
        {
            var smallPosterBaseUrl = "https://image.tmdb.org/t/p/w185";
            var largePosterBaseUrl = "https://image.tmdb.org/t/p/w500";

            var source = new MovieResult()
            {
                Adult = true,
                GenreIds = new int[] { 1, 2 },
                Id = 10,
                Overview = "Some overview text",
                Popularity = 8.52D,
                PosterPath = "/another.poster.path",
                ReleaseDate = "2010-01-01",
                Title = "Movie title",
                VoteAverage = 7.43D,
                VoteCount = 54
            };
            var genreInfo = new GenreInfo();

            var target = new MovieMapper();
            var actual = target.ToMovie(source, genreInfo);

            Assert.AreEqual($"{smallPosterBaseUrl}{source.PosterPath}", actual.SmallPosterUri.ToString());
            Assert.AreEqual($"{largePosterBaseUrl}{source.PosterPath}", actual.LargePosterUri.ToString());
            Assert.AreEqual(source.Id, actual.Id);
            Assert.AreEqual(source.Overview, actual.Overview);
            Assert.AreEqual(source.Popularity, actual.Popularity);
            Assert.AreEqual(source.Title, actual.Title);
            Assert.AreEqual(source.VoteAverage, actual.VoteAverage);
            Assert.AreEqual(source.VoteCount, actual.VoteCount);
            Assert.IsEmpty(actual.Genres);
            Assert.AreEqual(DateTime.Parse(
                source.ReleaseDate, 
                new CultureInfo("en-US"), 
                DateTimeStyles.AssumeUniversal), actual.ReleaseDate);
        }

        [Test]
        public void ToMovie_HasMovieResultWithSomeGenres_ReturnsMovieWithCorrectGenres()
        {
            var source = new MovieResult()
            {
                Adult = true,
                GenreIds = new int[] { 1, 2 },
                Id = 10,
                Overview = "Some overview text",
                Popularity = 8.52D,
                PosterPath = "another.poster.path",
                ReleaseDate = "2010/01/01",
                Title = "Movie title",
                VoteAverage = 7.43D,
                VoteCount = 54
            };
            var genreInfo = new GenreInfo()
            {
                Genres = new List<GenreResult>()
                {
                    new GenreResult() { Id = 2, Name = "Genre 2"}
                }
            };

            var target = new MovieMapper();
            var actual = target.ToMovie(source, genreInfo);

            Assert.AreEqual(1, actual.Genres.Count);
            Assert.AreEqual(genreInfo.Genres[0].Id, actual.Genres[0].Id);
        }

        [Test]
        public void ToMovie_MovieResultIsNull_ThrowsArgumentNullException()
        {
            var target = new MovieMapper();

            Assert.Throws<ArgumentNullException>(() => target.ToMovie(null, new GenreInfo()));
        }

        [Test]
        public void ToMovie_GenreInfoIsNull_ThrowsArgumentNullException()
        {
            var target = new MovieMapper();

            Assert.Throws<ArgumentNullException>(() => target.ToMovie(new MovieResult(), null));
        }
    }
}
