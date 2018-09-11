using Cinelovers.Core.Caching;
using Cinelovers.Core.Rest;
using Cinelovers.Core.Rest.Dtos;
using Cinelovers.Core.Services;
using Cinelovers.Core.Services.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace Cinelovers.Core.UnitTests.Services
{
    [TestFixture]
    public class MovieServiceTests
    {
        [Test]
        public void GetUpcomingMovies_PagingInfoIsNull_ReturnsEmpty()
        {
            int expectedPage = 2;
            MovieResponse pagingInfo = null;
            GenreResponse genreInfo = null;

            var apiClientMock = new Mock<ITmdbApiClient>();
            apiClientMock
                .Setup(x => x.GetUpcomingMovies(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .Returns(() => Observable.Return(pagingInfo));
            apiClientMock
                .Setup(x => x.GetGenres(
                    It.IsAny<string>()))
                .Returns(() => Observable.Return(genreInfo));

            var apiServiceMock = new Mock<IApiService>();
            apiServiceMock
                .Setup(x => x.GetClient())
                .Returns(apiClientMock.Object);

            var cacheMock = new Mock<ICache>();
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<MovieResponse>>>()))
                .Returns<string, Func<IObservable<MovieResponse>>>((arg, targetMethod) => targetMethod());
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<GenreResponse>>>()))
                .Returns<string, Func<IObservable<GenreResponse>>>((arg, targetMethod) => targetMethod());

            var target = new MovieService(
                apiServiceMock.Object,
                cacheMock.Object);

            IEnumerable<Movie> resultData = null;
            Assert.DoesNotThrow(
                () => target
                    .GetUpcomingMovies(expectedPage)
                    .Subscribe(results => resultData = results));

            Assert.IsEmpty(resultData);
        }

        [Test]
        public void GetUpcomingMovies_SpecificPage_InvokeServiceWithCorrectParameters()
        {
            int expectedPage = 2;
            var expectedLanguage = "en-US";
            var pagingInfo = new MovieResponse();
            GenreResponse genreInfo = null;

            var apiClientMock = new Mock<ITmdbApiClient>();
            apiClientMock
                .Setup(x => x.GetUpcomingMovies(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .Returns(() => Observable.Return(pagingInfo));
            apiClientMock
               .Setup(x => x.GetGenres(
                   It.IsAny<string>()))
               .Returns(() => Observable.Return(genreInfo));

            var apiServiceMock = new Mock<IApiService>();
            apiServiceMock
                .Setup(x => x.GetClient())
                .Returns(apiClientMock.Object);

            var cacheMock = new Mock<ICache>();
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<MovieResponse>>>()))
                .Returns<string, Func<IObservable<MovieResponse>>>((arg, targetMethod) => targetMethod());
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<GenreResponse>>>()))
                .Returns<string, Func<IObservable<GenreResponse>>>((arg, targetMethod) => targetMethod());

            var target = new MovieService(
                apiServiceMock.Object,
                cacheMock.Object);

            target.GetUpcomingMovies(expectedPage).Subscribe();

            apiServiceMock.Verify(x => x.GetClient(), Times.Once);
            apiClientMock.Verify(
                x => x.GetUpcomingMovies(
                    It.Is<int>(page => page == expectedPage),
                    It.Is<string>(language => language == expectedLanguage)),
                Times.Once);
            apiClientMock.Verify(
                x => x.GetGenres(
                    It.Is<string>(language => language == expectedLanguage)),
                Times.Once);
        }

        [Test]
        public void GetUpcomingMovies_WhenInvoked_UsesSpecifiedCacheKey()
        {
            int expectedPage = 2;
            var pagingInfo = new MovieResponse();
            GenreResponse genreInfo = null;

            var apiClientMock = new Mock<ITmdbApiClient>();
            apiClientMock
                .Setup(x => x.GetUpcomingMovies(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .Returns(() => Observable.Return(pagingInfo));
            apiClientMock
                .Setup(x => x.GetGenres(
                    It.IsAny<string>()))
                .Returns(() => Observable.Return(genreInfo));

            var apiServiceMock = new Mock<IApiService>();
            apiServiceMock
                .Setup(x => x.GetClient())
                .Returns(apiClientMock.Object);

            var cacheMock = new Mock<ICache>();
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<MovieResponse>>>()))
                .Returns<string, Func<IObservable<MovieResponse>>>((arg, targetMethod) => targetMethod());
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<GenreResponse>>>()))
                .Returns<string, Func<IObservable<GenreResponse>>>((arg, targetMethod) => targetMethod());

            var target = new MovieService(
                apiServiceMock.Object,
                cacheMock.Object);

            target.GetUpcomingMovies(expectedPage).Subscribe();

            cacheMock.Verify(
                x => x.GetAndFetchLatest(
                    It.Is<string>(cacheKey => cacheKey == $"upcoming_movies_{expectedPage}"),
                    It.IsAny<Func<IObservable<MovieResponse>>>()),
                Times.Once);
        }

        [Test]
        public void GetUpcomingMovies_MovieHasGenres_MovieWithGenres()
        {
            int expectedPage = 2;
            var pagingInfo = new MovieResponse()
            {
                Results = new List<MovieResult>()
                {
                    new MovieResult() { Id = 1, Title = "Movie 1", GenreIds = new List<int>() { 1, 2, 3 }, ReleaseDate = "2004-01-02" },
                    new MovieResult() { Id = 2, Title = "Movie 2", GenreIds = new List<int>() { 1, 3 }, ReleaseDate = "2003-10-10" }
                }
            };
            var genreInfo = new GenreResponse()
            {
                Genres = new List<GenreResult>()
                {
                    new GenreResult() { Id = 1, Name = "Genre 1" },
                    new GenreResult() { Id = 2, Name = "Genre 2" },
                    new GenreResult() { Id = 3, Name = "Genre 3" }
                }
            };

            var apiClientMock = new Mock<ITmdbApiClient>();
            apiClientMock
                .Setup(x => x.GetUpcomingMovies(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .Returns(() => Observable.Return(pagingInfo));
            apiClientMock
               .Setup(x => x.GetGenres(
                   It.IsAny<string>()))
               .Returns(() => Observable.Return(genreInfo));

            var apiServiceMock = new Mock<IApiService>();
            apiServiceMock
                .Setup(x => x.GetClient())
                .Returns(apiClientMock.Object);

            var cacheMock = new Mock<ICache>();
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<MovieResponse>>>()))
                .Returns<string, Func<IObservable<MovieResponse>>>((arg, targetMethod) => targetMethod());
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<GenreResponse>>>()))
                .Returns<string, Func<IObservable<GenreResponse>>>((arg, targetMethod) => targetMethod());

            IList<Movie> results = null;

            var target = new MovieService(
                apiServiceMock.Object,
                cacheMock.Object);

            target
                .GetUpcomingMovies(expectedPage)
                .Subscribe(movies => results = movies.ToList());

            Assert.AreEqual(pagingInfo.Results.Count, results.Count());
            Assert.AreEqual(pagingInfo.Results[0].GenreIds.Count, results[0].Genres.Count());
            Assert.AreEqual(pagingInfo.Results[1].GenreIds.Count, results[1].Genres.Count());
        }

        [Test]
        public void GetMovies_PagingInfoIsNull_ReturnsEmpty()
        {
            string expectedQuery = "a query value";
            int expectedPage = 2;

            MovieResponse pagingInfo = null;
            GenreResponse genreInfo = null;

            var apiClientMock = new Mock<ITmdbApiClient>();
            apiClientMock
                .Setup(x => x.GetMovies(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .Returns(() => Observable.Return(pagingInfo));
            apiClientMock
                .Setup(x => x.GetGenres(
                    It.IsAny<string>()))
                .Returns(() => Observable.Return(genreInfo));

            var apiServiceMock = new Mock<IApiService>();
            apiServiceMock
                .Setup(x => x.GetClient())
                .Returns(apiClientMock.Object);

            var cacheMock = new Mock<ICache>();
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<MovieResponse>>>()))
                .Returns<string, Func<IObservable<MovieResponse>>>((arg, targetMethod) => targetMethod());
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<GenreResponse>>>()))
                .Returns<string, Func<IObservable<GenreResponse>>>((arg, targetMethod) => targetMethod());

            var target = new MovieService(
                apiServiceMock.Object,
                cacheMock.Object);

            IEnumerable<Movie> resultData = null;
            Assert.DoesNotThrow(
                () => target
                    .GetMovies(expectedQuery, expectedPage)
                    .Subscribe(results => resultData = results));

            Assert.IsEmpty(resultData);
        }

        [Test]
        public void GetMovies_SpecificPage_InvokeServiceWithCorrectParameters()
        {
            string expectedQuery = "a query value";
            int expectedPage = 2;
            var expectedLanguage = "en-US";

            var pagingInfo = new MovieResponse();
            GenreResponse genreInfo = null;

            var apiClientMock = new Mock<ITmdbApiClient>();
            apiClientMock
                .Setup(x => x.GetMovies(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .Returns(() => Observable.Return(pagingInfo));
            apiClientMock
               .Setup(x => x.GetGenres(
                   It.IsAny<string>()))
               .Returns(() => Observable.Return(genreInfo));

            var apiServiceMock = new Mock<IApiService>();
            apiServiceMock
                .Setup(x => x.GetClient())
                .Returns(apiClientMock.Object);

            var cacheMock = new Mock<ICache>();
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<MovieResponse>>>()))
                .Returns<string, Func<IObservable<MovieResponse>>>((arg, targetMethod) => targetMethod());
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<GenreResponse>>>()))
                .Returns<string, Func<IObservable<GenreResponse>>>((arg, targetMethod) => targetMethod());

            var target = new MovieService(
                apiServiceMock.Object,
                cacheMock.Object);

            target
                .GetMovies(expectedQuery, expectedPage)
                .Subscribe();

            apiServiceMock.Verify(x => x.GetClient(), Times.Once);
            apiClientMock.Verify(
                x => x.GetMovies(
                    It.Is<string>(query => query == expectedQuery),
                    It.Is<int>(page => page == expectedPage),
                    It.Is<string>(language => language == expectedLanguage)),
                Times.Once);
            apiClientMock.Verify(
                x => x.GetGenres(
                    It.Is<string>(language => language == expectedLanguage)),
                Times.Once);
        }

        [Test]
        public void GetMovies_WhenInvoked_UsesSpecifiedCacheKey()
        {
            var expectedQuery = "another query";
            int expectedPage = 2;
            var pagingInfo = new MovieResponse();

            GenreResponse genreInfo = null;

            var apiClientMock = new Mock<ITmdbApiClient>();
            apiClientMock
                .Setup(x => x.GetMovies(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .Returns(() => Observable.Return(pagingInfo));
            apiClientMock
                .Setup(x => x.GetGenres(
                    It.IsAny<string>()))
                .Returns(() => Observable.Return(genreInfo));

            var apiServiceMock = new Mock<IApiService>();
            apiServiceMock
                .Setup(x => x.GetClient())
                .Returns(apiClientMock.Object);

            var cacheMock = new Mock<ICache>();
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<MovieResponse>>>()))
                .Returns<string, Func<IObservable<MovieResponse>>>((arg, targetMethod) => targetMethod());
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<GenreResponse>>>()))
                .Returns<string, Func<IObservable<GenreResponse>>>((arg, targetMethod) => targetMethod());

            var target = new MovieService(
                apiServiceMock.Object,
                cacheMock.Object);

            target
                .GetMovies(expectedQuery, expectedPage)
                .Subscribe();

            cacheMock.Verify(
                x => x.GetAndFetchLatest(
                    It.Is<string>(cacheKey => cacheKey == $"movies_{expectedQuery}_{expectedPage}"),
                    It.IsAny<Func<IObservable<MovieResponse>>>()),
                Times.Once);
        }

        [Test]
        public void GetMovies_MovieHasGenres_MovieWithGenres()
        {
            string expectedQuery = "query";
            int expectedPage = 2;
            var pagingInfo = new MovieResponse()
            {
                Results = new List<MovieResult>()
                {
                    new MovieResult() { Id = 1, Title = "Movie 1", GenreIds = new List<int>() { 1, 2, 3 }, ReleaseDate = "2004-01-02" },
                    new MovieResult() { Id = 2, Title = "Movie 2", GenreIds = new List<int>() { 1, 3 }, ReleaseDate = "2003-10-10" }
                }
            };
            var genreInfo = new GenreResponse()
            {
                Genres = new List<GenreResult>()
                {
                    new GenreResult() { Id = 1, Name = "Genre 1" },
                    new GenreResult() { Id = 2, Name = "Genre 2" },
                    new GenreResult() { Id = 3, Name = "Genre 3" }
                }
            };

            var apiClientMock = new Mock<ITmdbApiClient>();
            apiClientMock
                .Setup(x => x.GetMovies(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .Returns(() => Observable.Return(pagingInfo));
            apiClientMock
               .Setup(x => x.GetGenres(
                   It.IsAny<string>()))
               .Returns(() => Observable.Return(genreInfo));

            var apiServiceMock = new Mock<IApiService>();
            apiServiceMock
                .Setup(x => x.GetClient())
                .Returns(apiClientMock.Object);

            var cacheMock = new Mock<ICache>();
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<MovieResponse>>>()))
                .Returns<string, Func<IObservable<MovieResponse>>>((arg, targetMethod) => targetMethod());
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<GenreResponse>>>()))
                .Returns<string, Func<IObservable<GenreResponse>>>((arg, targetMethod) => targetMethod());

            IList<Movie> results = null;

            var target = new MovieService(
                apiServiceMock.Object,
                cacheMock.Object);

            target
                .GetMovies(expectedQuery, expectedPage)
                .Subscribe(movies => results = movies.ToList());

            Assert.AreEqual(pagingInfo.Results.Count, results.Count());
            Assert.AreEqual(pagingInfo.Results[0].GenreIds.Count, results[0].Genres.Count());
            Assert.AreEqual(pagingInfo.Results[1].GenreIds.Count, results[1].Genres.Count());
        }

        [Test]
        public void GetMovieGenres_GenreInfoIsNull_ReturnsEmpty()
        {
            GenreResponse genreInfo = null;

            var apiClientMock = new Mock<ITmdbApiClient>();
            apiClientMock
                .Setup(x => x.GetGenres(It.IsAny<string>()))
                .Returns(() => Observable.Return(genreInfo));

            var apiServiceMock = new Mock<IApiService>();
            apiServiceMock
                .Setup(x => x.GetClient())
                .Returns(apiClientMock.Object);

            var cacheMock = new Mock<ICache>();
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<GenreResponse>>>()))
                .Returns<string, Func<IObservable<GenreResponse>>>((arg, targetMethod) => targetMethod());

            var target = new MovieService(
                apiServiceMock.Object,
                cacheMock.Object);

            IEnumerable<Genre> resultData = null;
            Assert.DoesNotThrow(
                () => target
                    .GetGenres()
                    .Subscribe(results => resultData = results));

            Assert.IsEmpty(resultData);
        }

        [Test]
        public void GetMovieGenres_WhenInvoked_InvokeServiceWithCorrectParameters()
        {
            var expectedLanguage = "en-US";
            var genreInfo = new GenreResponse();

            var apiClientMock = new Mock<ITmdbApiClient>();
            apiClientMock
                .Setup(x => x.GetGenres(It.IsAny<string>()))
                .Returns(() => Observable.Return(genreInfo));

            var apiServiceMock = new Mock<IApiService>();
            apiServiceMock
                .Setup(x => x.GetClient())
                .Returns(apiClientMock.Object);

            var cacheMock = new Mock<ICache>();
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<GenreResponse>>>()))
                .Returns<string, Func<IObservable<GenreResponse>>>((arg, targetMethod) => targetMethod());

            var target = new MovieService(
                apiServiceMock.Object,
                cacheMock.Object);

            target.GetGenres().Subscribe();

            apiServiceMock.Verify(x => x.GetClient(), Times.Once);
            apiClientMock.Verify(
                x => x.GetGenres(It.Is<string>(language => language == expectedLanguage)),
                Times.Once);
        }

        [Test]
        public void GetMovieGenres_WhenInvoked_UsesSpecifiedCacheKey()
        {
            var genreInfo = new GenreResponse();

            var apiClientMock = new Mock<ITmdbApiClient>();
            apiClientMock
                .Setup(x => x.GetGenres(It.IsAny<string>()))
                .Returns(() => Observable.Return(genreInfo));

            var apiServiceMock = new Mock<IApiService>();
            apiServiceMock
                .Setup(x => x.GetClient())
                .Returns(apiClientMock.Object);

            var cacheMock = new Mock<ICache>();
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<GenreResponse>>>()))
                .Returns<string, Func<IObservable<GenreResponse>>>((arg, targetMethod) => targetMethod());

            var target = new MovieService(
                apiServiceMock.Object,
                cacheMock.Object);

            target.GetGenres().Subscribe();

            cacheMock.Verify(
                x => x.GetAndFetchLatest(
                    It.Is<string>(cacheKey => cacheKey == "genres"),
                    It.IsAny<Func<IObservable<GenreResponse>>>()),
                Times.Once);
        }
    }
}
