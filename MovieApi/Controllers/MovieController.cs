using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApi.Models;
using MovieApi.Services;
using MovieApi.Dtos;
using MovieApi.ViewModels;
using Microsoft.Extensions.Logging;
using MovieApi.Filters;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly RatingService _ratingService;
        private readonly ILogger<MovieController> _logger;

        public MovieController(IMovieService movieService, ILogger<MovieController> logger, RatingService ratingService)
        {
            _movieService = movieService;
            _logger = logger;
            _ratingService = ratingService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewMovieDto m)
        {
            _logger.LogInformation("Movie Post called.");
            var createdMovie = await _movieService.CreateMovie(m);
            return CreatedAtAction(nameof(Get), new { id = createdMovie.Id }, createdMovie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Movie Delete called.");
            return await _movieService.DeleteMovie(id)
                ? NoContent()
                : NotFound();
        }

        [HttpGet]
        public async Task<IEnumerable<MovieRowVm>> GetAll([FromQuery] GenericQueryOption<MovieFilter> option)
        {
            _logger.LogInformation("Movie GetAll called.");
            return await _movieService.GetAll(option);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Movie Get called.");
            var m = await _movieService.GetMovieById(id);
            if (m == null)
                return NotFound();
            
            return Ok(m);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateMovieDto m)
        {
            _logger.LogInformation("Movie Put called.");
            return await _movieService.UpdateMovie(id, m)
                ? NoContent()
                : NotFound();
        }
    }
}
