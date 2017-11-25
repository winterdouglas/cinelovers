using Cinelovers.Core.Services;
using Cinelovers.ViewModels.Movies;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;

namespace Cinelovers.ViewModels.UnitTests.Movies
{
    [TestFixture]
    public class UpcomingMoviesViewModelTests
    {
        [Test]
        public void GetUpcomingMovies_HasMovies_SetsMovies()
        {
            var scheduler = new TestScheduler();

            var movieServiceMock = new Mock<IMovieService>();

            var tarde = new UpcomingMoviesViewModel(
                movieServiceMock.Object,
                scheduler,
                scheduler,
                null);
        }
    }
}
