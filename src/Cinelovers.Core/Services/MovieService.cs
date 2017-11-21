using System;
using Cinelovers.Core.Services.Models;

namespace Cinelovers.Core.Services
{
    public class MovieService : IMovieService
    {
        public IObservable<Movie> FetchUpcomingMovies(int page)
        {
            throw new NotImplementedException();
        }
    }
}
