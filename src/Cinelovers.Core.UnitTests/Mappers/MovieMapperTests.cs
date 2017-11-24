using Cinelovers.Core.Mappers;
using Cinelovers.Core.Rest.Dtos;
using NUnit.Framework;
using System;

namespace Cinelovers.Core.UnitTests.Mappers
{
    [TestFixture]
    public class MovieMapperTests
    {
        [Test]
        public void ToMovie_HasMovieResult_ReturnsMovie()
        {
            var source = new MovieResult()
            {
                Adult = true,
                BackdropPath = "any.path",
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
            var target = new MovieMapper();
            var actual = target.ToMovie(source);

            Assert.AreEqual(source.BackdropPath, actual.BackdropUrl);
            Assert.AreEqual(source.GenreIds.Count, actual.Genres.Count);
            Assert.AreEqual(source.Id, actual.Id);
            Assert.AreEqual(source.Overview, actual.Overview);
            Assert.AreEqual(source.Popularity, actual.Popularity);
            Assert.AreEqual(source.PosterPath, actual.PosterUrl);
            Assert.AreEqual(source.ReleaseDate, actual.ReleaseDate);
            Assert.AreEqual(source.Title, actual.Title);
            Assert.AreEqual(source.VoteAverage, actual.VoteAverage);
            Assert.AreEqual(source.VoteCount, actual.VoteCount);
        }

        [Test]
        public void ToMovie_MovieResultIsNull_ThrowsArgumentNullException()
        {
            var target = new MovieMapper();

            Assert.Throws<ArgumentNullException>(() => target.ToMovie(null));
        }
    }
}
