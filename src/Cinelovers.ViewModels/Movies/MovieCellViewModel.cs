using Cinelovers.Core.Services.Models;
using System;
using System.Collections.Generic;

namespace Cinelovers.ViewModels.Movies
{
    public class MovieCellViewModel
    {
        public string Title => _movie.Title;

        public string PosterUrl => _movie.PosterUrl;

        public string BackdropUrl => _movie.BackdropUrl;

        public IList<string> Genres => _movie.Genres;

        public string Overview => _movie.Overview;

        public DateTime ReleaseDate => _movie.ReleaseDate;

        private readonly Movie _movie;

        public MovieCellViewModel(Movie movie)
        {
            _movie = movie ?? throw new ArgumentNullException(nameof(movie));
        }
    }
}
