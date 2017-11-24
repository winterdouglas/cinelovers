using Cinelovers.ViewModels.Movies;
using NUnit.Framework;
using System;

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
    }
}
