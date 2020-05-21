using Cinelovers.Core.Services.Models;
using DynamicData;
using System;
using System.Reactive;

namespace Cinelovers.Core.Services
{
    public interface IMovieService
    {
        IObservableCache<Movie, int> Movies { get; }

        IObservable<Unit> LoadUpcomingMovies(int page);

        IObservable<Unit> LoadMovies(string query, int page);
    }
}
