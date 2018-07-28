using Cinelovers.Core.Services.Models;
using Cinelovers.ViewModels.Movies;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinelovers.ViewModels.UnitTests.Movies
{
    [TestFixture]
    public class ReviewCellViewModelTests
    {
        [Test]
        public void ViewModelInstantiated_ReviewIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ReviewCellViewModel(null));
        }

        [Test]
        public void GetAuthor_ReviewIsNotNull_ReturnsReviewAuthor()
        {
            var review = new Review("1", "Author", "Content");
            var target = new ReviewCellViewModel(review);

            Assert.AreEqual(review.Author, target.Author);
        }

        [Test]
        public void GetContent_ReviewIsNotNull_ReturnsReviewContent()
        {
            var review = new Review("1", "Author", "Content");
            var target = new ReviewCellViewModel(review);

            Assert.AreEqual(review.Content, target.Content);
        }
    }
}
