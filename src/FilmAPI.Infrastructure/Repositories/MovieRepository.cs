using Dapper;
using FilmAPI.Core.Interfaces;
using FilmAPI.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FilmAPI.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly string _connectionString;

        public MovieRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Movies";
                return await db.QueryAsync<Movie>(query);
            }
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Movies WHERE Id = @Id";
                return await db.QueryFirstOrDefaultAsync<Movie>(query, new { Id = id });
            }
        }

        public async Task<int> AddAsync(Movie movie)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = @"
                    INSERT INTO Movies (Title, Director, ReleaseYear, Genre, Rating)
                    VALUES (@Title, @Director, @ReleaseYear, @Genre, @Rating);
                    SELECT CAST(SCOPE_IDENTITY() as int)";

                return await db.QuerySingleAsync<int>(query, movie);
            }
        }

        public async Task<bool> UpdateAsync(Movie movie)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = @"
                    UPDATE Movies
                    SET Title = @Title,
                        Director = @Director,
                        ReleaseYear = @ReleaseYear,
                        Genre = @Genre,
                        Rating = @Rating
                    WHERE Id = @Id";

                var affectedRows = await db.ExecuteAsync(query, movie);
                return affectedRows > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Movies WHERE Id = @Id";
                var affectedRows = await db.ExecuteAsync(query, new { Id = id });
                return affectedRows > 0;
            }
        }
    }
} 