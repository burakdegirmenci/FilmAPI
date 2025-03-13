using FilmAPI.Core.Helpers;
using FilmAPI.Core.Interfaces;
using FilmAPI.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmAPI.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public MovieService(IMovieRepository movieRepository, IMemoryCache memoryCache)
        {
            _movieRepository = movieRepository;
            _memoryCache = memoryCache;
            _cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            // Önbellekte var mı kontrol et
            if (!_memoryCache.TryGetValue(CacheKeys.AllMovies, out IEnumerable<Movie> movies))
            {
                // Önbellekte yoksa veritabanından getir
                movies = await _movieRepository.GetAllAsync();

                // Önbelleğe ekle
                _memoryCache.Set(CacheKeys.AllMovies, movies, _cacheOptions);
            }

            return movies;
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            // Önbellekte var mı kontrol et
            var cacheKey = CacheKeys.MovieById(id);
            if (!_memoryCache.TryGetValue(cacheKey, out Movie movie))
            {
                // Önbellekte yoksa veritabanından getir
                movie = await _movieRepository.GetByIdAsync(id);

                // Eğer film bulunduysa önbelleğe ekle
                if (movie != null)
                {
                    _memoryCache.Set(cacheKey, movie, _cacheOptions);
                }
            }

            return movie;
        }

        public async Task<int> AddMovieAsync(Movie movie)
        {
            var id = await _movieRepository.AddAsync(movie);

            // Önbelleği temizle
            _memoryCache.Remove(CacheKeys.AllMovies);

            return id;
        }

        public async Task<bool> UpdateMovieAsync(Movie movie)
        {
            var result = await _movieRepository.UpdateAsync(movie);

            if (result)
            {
                // Önbelleği temizle
                _memoryCache.Remove(CacheKeys.AllMovies);
                _memoryCache.Remove(CacheKeys.MovieById(movie.Id));
            }

            return result;
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            var result = await _movieRepository.DeleteAsync(id);

            if (result)
            {
                // Önbelleği temizle
                _memoryCache.Remove(CacheKeys.AllMovies);
                _memoryCache.Remove(CacheKeys.MovieById(id));
            }

            return result;
        }
    }
} 