using Cinelovers.Core.Services.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cinelovers.ViewModels.Movies
{
    public class MovieCellViewModel : ReactiveObject
    {
        public int Id => _movie.Id;

        public string Title => _movie.Title;

        public Uri LargePosterUri => _movie.LargePosterUri;

        public Uri SmallPosterUri => _movie.SmallPosterUri;

        public IList<string> Genres => _movie
            .Genres
            .Select(genre => genre.Name)
            .ToList();

        public string GenresText => string.Join(", ", Genres);

        public string Overview => _movie.Overview;

        public DateTime? ReleaseDate => _movie.ReleaseDate;

        public string ReleasedIn => $"Released in {ReleaseDate:yyyy-MM-dd}";

        public double Popularity => _movie.Popularity;

        public double VoteAverage => _movie.VoteAverage;

        public double VoteCount => _movie.VoteCount;

        private Movie _movie;

        public MovieCellViewModel(Movie movie)
        {
            _movie = movie ?? throw new ArgumentNullException(nameof(movie));
        }

        public void UpdateMovie(Movie movie)
        {
            _movie = movie;
            this.RaisePropertyChanged("");
        }
    }
}
