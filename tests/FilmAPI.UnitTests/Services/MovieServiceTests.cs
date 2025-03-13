using FilmAPI.Core.Helpers;
using FilmAPI.Core.Interfaces;
using FilmAPI.Core.Models;
using FilmAPI.Application.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FilmAPI.UnitTests.Services
{
    public class MovieServiceTests
    {
        [Fact]
        public async Task GetAllMovies_ShouldReturnFromCache_WhenCalledTwice()
        {
            // Arrange
            var mockRepository = new Mock<IMovieRepository>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            
            var testMovies = new List<Movie>
            {
                new Movie { Id = 1, Title = "Test Film 1", Director = "Test Yönetmen 1", ReleaseYear = 2020, Genre = "Aksiyon", Rating = 8.5m },
                new Movie { Id = 2, Title = "Test Film 2", Director = "Test Yönetmen 2", ReleaseYear = 2021, Genre = "Komedi", Rating = 7.5m }
            };
            
            mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(testMovies)
                .Verifiable(); // Verify edilebilir olarak işaretlenir
            
            var movieService = new MovieService(mockRepository.Object, memoryCache);
            
            // Act
            // İlk çağrı - veritabanından alınmalı
            var result1 = await movieService.GetAllMoviesAsync();
            
            // İkinci çağrı - cache'den alınmalı
            var result2 = await movieService.GetAllMoviesAsync();
            
            // Assert
            Assert.Equal(testMovies.Count, result1.Count());
            Assert.Equal(testMovies.Count, result2.Count());
            
            // Repository'nin GetAllAsync metodu sadece bir kez çağrılmalı
            mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task AddMovie_ShouldClearCache_AfterAddingNewMovie()
        {
            // Arrange
            var mockRepository = new Mock<IMovieRepository>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            
            var testMovies = new List<Movie>
            {
                new Movie { Id = 1, Title = "Test Film 1", Director = "Test Yönetmen 1", ReleaseYear = 2020, Genre = "Aksiyon", Rating = 8.5m },
                new Movie { Id = 2, Title = "Test Film 2", Director = "Test Yönetmen 2", ReleaseYear = 2021, Genre = "Komedi", Rating = 7.5m }
            };
            
            var newMovie = new Movie { Title = "Yeni Film", Director = "Yeni Yönetmen", ReleaseYear = 2022, Genre = "Dram", Rating = 9.0m };
            
            mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(testMovies);
            
            mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Movie>()))
                .ReturnsAsync(3); // Yeni film ID'si
            
            var movieService = new MovieService(mockRepository.Object, memoryCache);
            
            // Act
            // Önce filmleri getir (cache'e eklenecek)
            var initialMovies = await movieService.GetAllMoviesAsync();
            
            // Yeni film ekle (cache temizlenmeli)
            await movieService.AddMovieAsync(newMovie);
            
            // Repository'nin GetAllAsync metodunu tekrar çağıracak şekilde ayarla
            var updatedMovies = testMovies.ToList();
            updatedMovies.Add(new Movie { Id = 3, Title = "Yeni Film", Director = "Yeni Yönetmen", ReleaseYear = 2022, Genre = "Dram", Rating = 9.0m });
            
            mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(updatedMovies);
            
            // Filmleri tekrar getir (cache temizlendiği için repository'den alınmalı)
            var result = await movieService.GetAllMoviesAsync();
            
            // Assert
            Assert.Equal(3, result.Count());
            
            // Repository'nin GetAllAsync metodu iki kez çağrılmalı
            mockRepository.Verify(repo => repo.GetAllAsync(), Times.Exactly(2));
            
            // Repository'nin AddAsync metodu bir kez çağrılmalı
            mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Movie>()), Times.Once);
        }

        [Fact]
        public async Task GetMovieById_ShouldReturnMovie_WhenMovieExists()
        {
            // Arrange
            var mockRepository = new Mock<IMovieRepository>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            
            var testMovie = new Movie { Id = 1, Title = "Test Film 1", Director = "Test Yönetmen 1", ReleaseYear = 2020, Genre = "Aksiyon", Rating = 8.5m };
            
            mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(testMovie);
            
            var movieService = new MovieService(mockRepository.Object, memoryCache);
            
            // Act
            var result = await movieService.GetMovieByIdAsync(1);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test Film 1", result.Title);
            
            // Repository'nin GetByIdAsync metodu bir kez çağrılmalı
            mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetMovieById_ShouldReturnNull_WhenMovieDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IMovieRepository>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            
            mockRepository.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Movie)null);
            
            var movieService = new MovieService(mockRepository.Object, memoryCache);
            
            // Act
            var result = await movieService.GetMovieByIdAsync(999);
            
            // Assert
            Assert.Null(result);
            
            // Repository'nin GetByIdAsync metodu bir kez çağrılmalı
            mockRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
        }
    }
} 