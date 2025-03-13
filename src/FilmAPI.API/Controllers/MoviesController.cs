using FilmAPI.API.Models;
using FilmAPI.Core.Interfaces;
using FilmAPI.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmAPI.API.Controllers
{
    /// <summary>
    /// Film işlemleri için API controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        /// <summary>
        /// MoviesController constructor
        /// </summary>
        /// <param name="movieService">Film servisi</param>
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        /// <summary>
        /// Tüm filmleri getirir
        /// </summary>
        /// <returns>Film listesi</returns>
        /// <response code="200">Filmler başarıyla getirildi</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Movie>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Movie>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Movie>>>> GetAllMovies()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            return Ok(ApiResponse<IEnumerable<Movie>>.SuccessResponse(movies, "Filmler başarıyla getirildi"));
        }

        /// <summary>
        /// Belirtilen ID'ye sahip filmi getirir
        /// </summary>
        /// <param name="id">Film ID'si</param>
        /// <returns>Film detayları</returns>
        /// <response code="200">Film başarıyla getirildi</response>
        /// <response code="404">Film bulunamadı</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Movie>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Movie>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<Movie>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Movie>>> GetMovieById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound(ApiResponse<Movie>.ErrorResponse("Film bulunamadı"));
            }
            return Ok(ApiResponse<Movie>.SuccessResponse(movie, "Film başarıyla getirildi"));
        }

        /// <summary>
        /// Yeni bir film ekler
        /// </summary>
        /// <param name="movie">Film bilgileri</param>
        /// <returns>Eklenen film ID'si</returns>
        /// <response code="201">Film başarıyla eklendi</response>
        /// <response code="400">Geçersiz veri</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<int>>> AddMovie(Movie movie)
        {
            var id = await _movieService.AddMovieAsync(movie);
            var response = ApiResponse<int>.SuccessResponse(id, "Film başarıyla eklendi");
            return CreatedAtAction(nameof(GetMovieById), new { id }, response);
        }

        /// <summary>
        /// Var olan bir filmi günceller
        /// </summary>
        /// <param name="id">Film ID'si</param>
        /// <param name="movie">Güncellenmiş film bilgileri</param>
        /// <returns>İşlem sonucu</returns>
        /// <response code="204">Film başarıyla güncellendi</response>
        /// <response code="400">Geçersiz veri</response>
        /// <response code="404">Film bulunamadı</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Film ID'si eşleşmiyor"));
            }

            var result = await _movieService.UpdateMovieAsync(movie);
            if (!result)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Film bulunamadı"));
            }

            return NoContent();
        }

        /// <summary>
        /// Bir filmi siler
        /// </summary>
        /// <param name="id">Film ID'si</param>
        /// <returns>İşlem sonucu</returns>
        /// <response code="204">Film başarıyla silindi</response>
        /// <response code="404">Film bulunamadı</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var result = await _movieService.DeleteMovieAsync(id);
            if (!result)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Film bulunamadı"));
            }

            return NoContent();
        }
    }
} 