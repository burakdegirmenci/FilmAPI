using FilmAPI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmAPI.Core.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<Movie> GetMovieByIdAsync(int id);
        Task<int> AddMovieAsync(Movie movie);
        Task<bool> UpdateMovieAsync(Movie movie);
        Task<bool> DeleteMovieAsync(int id);
    }
} 