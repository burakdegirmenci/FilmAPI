using FilmAPI.API.Controllers;
using FilmAPI.API.Models;
using FilmAPI.Core.Interfaces;
using FilmAPI.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FilmAPI.UnitTests.Controllers
{
    public class MoviesControllerTests
    {
        [Fact]
        public async Task GetAllMovies_ShouldReturnOkResult_WithMovies()
        {
            // Arrange
            var mockService = new Mock<IMovieService>();
            var testMovies = new List<Movie>
            {
                new Movie { Id = 1, Title = "Test Film 1", Director = "Test Yönetmen 1", ReleaseYear = 2020, Genre = "Aksiyon", Rating = 8.5m },
                new Movie { Id = 2, Title = "Test Film 2", Director = "Test Yönetmen 2", ReleaseYear = 2021, Genre = "Komedi", Rating = 7.5m }
            };
            
            mockService.Setup(service => service.GetAllMoviesAsync())
                .ReturnsAsync(testMovies);
            
            var controller = new MoviesController(mockService.Object);
            
            // Act
            var result = await controller.GetAllMovies();
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<Movie>>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(2, apiResponse.Data.Count());
        }
        
        [Fact]
        public async Task GetMovieById_ShouldReturnOkResult_WhenMovieExists()
        {
            // Arrange
            var mockService = new Mock<IMovieService>();
            var testMovie = new Movie { Id = 1, Title = "Test Film 1", Director = "Test Yönetmen 1", ReleaseYear = 2020, Genre = "Aksiyon", Rating = 8.5m };
            
            mockService.Setup(service => service.GetMovieByIdAsync(1))
                .ReturnsAsync(testMovie);
            
            var controller = new MoviesController(mockService.Object);
            
            // Act
            var result = await controller.GetMovieById(1);
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<Movie>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(1, apiResponse.Data.Id);
            Assert.Equal("Test Film 1", apiResponse.Data.Title);
        }
        
        [Fact]
        public async Task GetMovieById_ShouldReturnNotFound_WhenMovieDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IMovieService>();
            
            mockService.Setup(service => service.GetMovieByIdAsync(999))
                .ReturnsAsync((Movie)null);
            
            var controller = new MoviesController(mockService.Object);
            
            // Act
            var result = await controller.GetMovieById(999);
            
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<Movie>>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
        }
        
        [Fact]
        public async Task AddMovie_ShouldReturnCreatedAtAction_WithNewId()
        {
            // Arrange
            var mockService = new Mock<IMovieService>();
            var newMovie = new Movie { Title = "Yeni Film", Director = "Yeni Yönetmen", ReleaseYear = 2022, Genre = "Dram", Rating = 9.0m };
            
            mockService.Setup(service => service.AddMovieAsync(It.IsAny<Movie>()))
                .ReturnsAsync(3); // Yeni film ID'si
            
            var controller = new MoviesController(mockService.Object);
            
            // Act
            var result = await controller.AddMovie(newMovie);
            
            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<int>>(createdAtActionResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(3, apiResponse.Data);
            Assert.Equal("GetMovieById", createdAtActionResult.ActionName);
            Assert.Equal(3, createdAtActionResult.RouteValues["id"]);
        }
        
        [Fact]
        public async Task UpdateMovie_ShouldReturnNoContent_WhenUpdateSucceeds()
        {
            // Arrange
            var mockService = new Mock<IMovieService>();
            var movie = new Movie { Id = 1, Title = "Güncellenmiş Film", Director = "Güncellenmiş Yönetmen", ReleaseYear = 2022, Genre = "Dram", Rating = 9.0m };
            
            mockService.Setup(service => service.UpdateMovieAsync(It.IsAny<Movie>()))
                .ReturnsAsync(true);
            
            var controller = new MoviesController(mockService.Object);
            
            // Act
            var result = await controller.UpdateMovie(1, movie);
            
            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        
        [Fact]
        public async Task UpdateMovie_ShouldReturnBadRequest_WhenIdMismatch()
        {
            // Arrange
            var mockService = new Mock<IMovieService>();
            var movie = new Movie { Id = 2, Title = "Güncellenmiş Film", Director = "Güncellenmiş Yönetmen", ReleaseYear = 2022, Genre = "Dram", Rating = 9.0m };
            
            var controller = new MoviesController(mockService.Object);
            
            // Act
            var result = await controller.UpdateMovie(1, movie);
            
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(badRequestResult.Value);
            Assert.False(apiResponse.Success);
        }
        
        [Fact]
        public async Task DeleteMovie_ShouldReturnNoContent_WhenDeleteSucceeds()
        {
            // Arrange
            var mockService = new Mock<IMovieService>();
            
            mockService.Setup(service => service.DeleteMovieAsync(1))
                .ReturnsAsync(true);
            
            var controller = new MoviesController(mockService.Object);
            
            // Act
            var result = await controller.DeleteMovie(1);
            
            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        
        [Fact]
        public async Task DeleteMovie_ShouldReturnNotFound_WhenMovieDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IMovieService>();
            
            mockService.Setup(service => service.DeleteMovieAsync(999))
                .ReturnsAsync(false);
            
            var controller = new MoviesController(mockService.Object);
            
            // Act
            var result = await controller.DeleteMovie(999);
            
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
        }
    }
} 