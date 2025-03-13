using FilmAPI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmAPI.Core.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<Movie> GetByIdAsync(int id);
        Task<int> AddAsync(Movie movie);
        Task<bool> UpdateAsync(Movie movie);
        Task<bool> DeleteAsync(int id);
    }
} 