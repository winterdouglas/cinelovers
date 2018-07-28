using Cinelovers.Core.Services.Models;
using ReactiveUI;
using System;

namespace Cinelovers.ViewModels.Movies
{
    public class ReviewCellViewModel : ReactiveObject
    {
        public string Author => _review.Author;

        public string Content => _review.Content;

        private readonly Review _review;

        public ReviewCellViewModel(Review review)
        {
            _review = review ?? throw new ArgumentNullException(nameof(review));
        }
    }
}
