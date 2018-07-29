using Cinelovers.Core.Services.Models;
using DynamicData;
using System;
using System.Reactive;

namespace Cinelovers.Core.Services
{
    public interface IMovieService
    {
        IObservableCache<Movie, int> Movies { get; }

        IObservableCache<Genre, int> Genres { get; }

        IObservable<Unit> GetUpcomingMovies(int page);

        IObservable<Unit> GetMovies(string query, int page);

        IObservable<Unit> GetGenres();
    }
}
