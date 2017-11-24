using Cinelovers.Core.Services.Models;
using System;
using System.Collections.Generic;

namespace Cinelovers.Core.Services
{
    public interface IMovieService
    {
        IObservable<IEnumerable<Movie>> GetUpcomingMovies(int page);

        IObservable<IEnumerable<Genre>> GetMovieGenres();
    }
}
