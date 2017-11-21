using Cinelovers.Core.Services.Models;
using System;

namespace Cinelovers.Core.Services
{
    public interface IMovieService
    {
        IObservable<Movie> FetchUpcomingMovies(int page);
    }
}
