using Cinelovers.Core.Mappers;
using Cinelovers.Core.Rest.Dtos;
using NUnit.Framework;
using System;

namespace Cinelovers.Core.UnitTests.Mappers
{
    [TestFixture]
    public class GenreMapperTests
    {
        [Test]
        public void ToGenre_HasGenreResult_ReturnsGenre()
        {
            var source = new GenreResult()
            {
                Id = 10,
                Name = "Any genre"
            };
            var target = new GenreMapper();
            var actual = target.ToGenre(source);

            Assert.AreEqual(source.Id, actual.Id);
            Assert.AreEqual(source.Name, actual.Name);
        }

        [Test]
        public void ToGenre_GenreResultIsNull_ThrowsArgumentNullException()
        {
            var target = new GenreMapper();

            Assert.Throws<ArgumentNullException>(() => target.ToGenre(null));
        }
    }
}
