using Cinelovers.Core.Services;
using Cinelovers.Core.Services.Models;
using Cinelovers.ViewModels.Movies;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Cinelovers.ViewModels.UnitTests.Movies
{
    [TestFixture]
    public class UpcomingMoviesViewModelTests
    {
        private TestScheduler _testScheduler;
        private Mock<IScreen> _screenMock;

        [SetUp]
        public void Setup()
        {
            ModeDetector.InUnitTestRunner();

            _testScheduler = new TestScheduler();
            RxApp.MainThreadScheduler = _testScheduler;
            RxApp.TaskpoolScheduler = _testScheduler;

            var routingState = new RoutingState();
            _screenMock = new Mock<IScreen>();
            _screenMock
                .SetupGet(x => x.Router)
                .Returns(() => routingState);
        }

        [Test]
        public void GetUpcomingMovies_HasMovies_SetsMovies()
        {
            int expectedPage = 2;
            var movies = new List<Movie>()
            {
                new Movie() { Id = 1, Title = "Movie 1" },
                new Movie() { Id = 2, Title = "Movie 2" }
            };

            var movieServiceMock = new Mock<IMovieService>();
            movieServiceMock
                .Setup(x => x.GetUpcomingMovies(
                    It.IsAny<int>()))
                .Returns(() => Observable.Return(movies));

            var target = new UpcomingMoviesViewModel(
                movieServiceMock.Object,
                _testScheduler,
                _testScheduler,
                _screenMock.Object);

            target.Activator.Activate();

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(500).Ticks);

            Observable.Return(expectedPage).InvokeCommand(target.GetUpcomingMovies);

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(500).Ticks);

            movieServiceMock
                .Verify(x => x.GetUpcomingMovies(
                    It.Is<int>(page => page == expectedPage)),
                    Times.Once);

            Assert.AreEqual(movies.Count, target.Movies.Count);
        }

        [Test]
        public void GetUpcomingMovies_OperationIsInvokedTwice_DoesNotAddDuplicatedMovies()
        {
            int expectedPage = 2;
            var movies = new List<Movie>()
            {
                new Movie() { Id = 1, Title = "Movie 1" },
                new Movie() { Id = 2, Title = "Movie 2" }
            };

            var movieServiceMock = new Mock<IMovieService>();
            movieServiceMock
                .Setup(x => x.GetUpcomingMovies(
                    It.IsAny<int>()))
                .Returns(() => Observable.Return(movies));

            var target = new UpcomingMoviesViewModel(
                movieServiceMock.Object,
                _testScheduler,
                _testScheduler,
                _screenMock.Object);

            target.Activator.Activate();

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(500).Ticks);

            Observable.Return(expectedPage).InvokeCommand(target.GetUpcomingMovies);
            Observable.Return(expectedPage).InvokeCommand(target.GetUpcomingMovies);

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(500).Ticks);

            movieServiceMock
                .Verify(x => x.GetUpcomingMovies(
                    It.Is<int>(page => page == expectedPage)),
                    Times.Exactly(2));

            Assert.AreEqual(movies.Count, target.Movies.Count);
        }

        [Test]
        public void GetUpcomingMovies_IsExecuting_SetsIsLoading()
        {
            int expectedPage = 2;

            var movieServiceMock = new Mock<IMovieService>();
            movieServiceMock
                .Setup(x => x.GetUpcomingMovies(
                    It.IsAny<int>()))
                .Returns(() => Observable
                    .Return(Enumerable.Empty<Movie>())
                    .Delay(TimeSpan.FromMilliseconds(500), _testScheduler)
                    .DelaySubscription(TimeSpan.FromMilliseconds(500), _testScheduler));

            var target = new UpcomingMoviesViewModel(
                movieServiceMock.Object,
                _testScheduler,
                _testScheduler,
                _screenMock.Object);

            target.Activator.Activate();

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(200).Ticks);

            Assert.IsFalse(target.IsLoading);

            Observable.Return(expectedPage).InvokeCommand(target.GetUpcomingMovies);

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(400).Ticks);

            Assert.IsTrue(target.IsLoading);

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(800).Ticks);

            Assert.IsFalse(target.IsLoading);
        }

        [Test]
        public void GetMovies_HasMovies_SetsMovies()
        {
            string expectedSearch = "query";
            int expectedPage = 2;
            var movies = new List<Movie>()
            {
                new Movie() { Id = 1, Title = "Movie 1" },
                new Movie() { Id = 2, Title = "Movie 2" }
            };

            var movieServiceMock = new Mock<IMovieService>();
            movieServiceMock
                .Setup(x => x.GetMovies(
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(() => Observable.Return(movies));

            var target = new UpcomingMoviesViewModel(
                movieServiceMock.Object,
                _testScheduler,
                _testScheduler,
                _screenMock.Object);

            target.Activator.Activate();

            target.SearchTerm = expectedSearch;

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(500).Ticks);

            Observable.Return(expectedPage).InvokeCommand(target.GetMovies);

            _testScheduler.AdvanceBy(TimeSpan.FromSeconds(2).Ticks);

            movieServiceMock
                .Verify(x => x.GetMovies(
                    It.Is<string>(search => search == expectedSearch),
                    It.Is<int>(page => page == expectedPage)),
                    Times.Once);

            Assert.AreEqual(movies.Count, target.Movies.Count);
        }

        [Test]
        public void SearchTerm_HasAtLeastThreeCharacters_GetMovies()
        {
            string expectedSearch = "Any query";
            int expectedPage = 1;
            var movies = new List<Movie>()
            {
                new Movie() { Id = 1, Title = "Movie 1" },
                new Movie() { Id = 2, Title = "Movie 2" }
            };

            var movieServiceMock = new Mock<IMovieService>();
            movieServiceMock
                .Setup(x => x.GetMovies(
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(() => Observable.Return(movies));

            var target = new UpcomingMoviesViewModel(
                movieServiceMock.Object,
                _testScheduler,
                _testScheduler,
                _screenMock.Object);

            target.Activator.Activate();

            target.SearchTerm = expectedSearch;

            _testScheduler.AdvanceBy(TimeSpan.FromSeconds(2).Ticks);

            movieServiceMock
                .Verify(x => x.GetMovies(
                    It.Is<string>(search => search == expectedSearch),
                    It.Is<int>(page => page == expectedPage)),
                    Times.Once);

            Assert.AreEqual(movies.Count, target.Movies.Count);
        }

        [Test]
        public void SearchTerm_HasLessThanThreeCharacters_DoesNotGetMovies()
        {
            string expectedSearch = "12";
            var movieServiceMock = new Mock<IMovieService>();
            movieServiceMock
                .Setup(x => x.GetMovies(
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(() => Observable
                    .Return(Enumerable.Empty<Movie>()));

            var target = new UpcomingMoviesViewModel(
                movieServiceMock.Object,
                _testScheduler,
                _testScheduler,
                _screenMock.Object);

            target.Activator.Activate();

            target.SearchTerm = expectedSearch;

            _testScheduler.AdvanceBy(TimeSpan.FromSeconds(2).Ticks);

            movieServiceMock
                .Verify(x => x.GetMovies(
                    It.IsAny<string>(),
                    It.IsAny<int>()),
                    Times.Never);
        }

        [Test]
        public void SearchTerm_IsEmpty_GetUpcomingMovies()
        {
            var movieServiceMock = new Mock<IMovieService>();
            movieServiceMock
                .Setup(x => x.GetMovies(
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(() => Observable
                    .Return(Enumerable.Empty<Movie>()));
            movieServiceMock
                .Setup(x => x.GetUpcomingMovies(
                    It.IsAny<int>()))
                .Returns(() => Observable
                    .Return(Enumerable.Empty<Movie>()));

            var target = new UpcomingMoviesViewModel(
                movieServiceMock.Object,
                _testScheduler,
                _testScheduler,
                _screenMock.Object);

            target.Activator.Activate();

            target.SearchTerm = "any";

            _testScheduler.AdvanceBy(TimeSpan.FromSeconds(2).Ticks);

            target.SearchTerm = null;

            _testScheduler.AdvanceBy(TimeSpan.FromSeconds(2).Ticks);

            movieServiceMock
                .Verify(x => x.GetMovies(
                    It.Is<string>(search => search == "any"),
                    It.Is<int>(page => page == 1)),
                    Times.Once);

            movieServiceMock
                .Verify(x => x.GetUpcomingMovies(
                    It.Is<int>(page => page == 1)),
                    Times.Once);
        }

        [Test]
        public void GetMovies_IsExecuting_SetsIsLoading()
        {
            var movieServiceMock = new Mock<IMovieService>();
            movieServiceMock
                .Setup(x => x.GetMovies(
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(() => Observable
                    .Return(Enumerable.Empty<Movie>())
                    .Delay(TimeSpan.FromMilliseconds(500), _testScheduler)
                    .DelaySubscription(TimeSpan.FromMilliseconds(500), _testScheduler));

            var target = new UpcomingMoviesViewModel(
                movieServiceMock.Object,
                _testScheduler,
                _testScheduler,
                _screenMock.Object);

            target.Activator.Activate();

            target.SearchTerm = "Any query";

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(200).Ticks);

            Assert.IsFalse(target.IsLoading);

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(900).Ticks);

            Assert.IsTrue(target.IsLoading);

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(1000).Ticks);

            Assert.IsFalse(target.IsLoading);
        }

        [Test]
        public void SetSelectedMovie_ValueIsNotNull_NavigatesToMovieDetails()
        {
            var selectedMovie = new MovieCellViewModel(new Movie() { Id = 1, Title = "Movie 1" });
            var movieServiceMock = new Mock<IMovieService>();

            var target = new UpcomingMoviesViewModel(
                movieServiceMock.Object,
                _testScheduler,
                _testScheduler,
                _screenMock.Object);

            target.Activator.Activate();

            target.SelectedMovie = selectedMovie;

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(500).Ticks);

            var currentViewModel = target.HostScreen.Router.GetCurrentViewModel() as MovieDetailsViewModel;
            Assert.IsNotNull(currentViewModel);
            Assert.AreEqual(selectedMovie, currentViewModel.Movie);
        }

        [Test]
        public void SetSelectedMovie_ValueIsNull_DoesNotNavigate()
        {
            var movieServiceMock = new Mock<IMovieService>();

            var target = new UpcomingMoviesViewModel(
                movieServiceMock.Object,
                _testScheduler,
                _testScheduler,
                _screenMock.Object);

            target.Activator.Activate();

            target.SelectedMovie = null;

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(500).Ticks);

            var currentViewModel = target.HostScreen.Router.GetCurrentViewModel();
            Assert.IsNull(currentViewModel);
        }

        [Test]
        public void GetUrlPathSegment_VmIsCreated_ReturnsCorrectText()
        {
            var movieServiceMock = new Mock<IMovieService>();

            var target = new UpcomingMoviesViewModel(
                movieServiceMock.Object,
                _testScheduler,
                _testScheduler,
                _screenMock.Object);

            Assert.AreEqual("Upcoming Movies", target.UrlPathSegment);
        }
    }
}
