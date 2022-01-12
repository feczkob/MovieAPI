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
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using MovieApi.Models.Authentication;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly RatingService _ratingService;
        private readonly ILogger<RatingController> _logger;

        public RatingController(RatingService ratingService, ILogger<RatingController> logger)
        {
            _ratingService = ratingService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> Post([FromBody] NewRatingDto r)
        {
            _logger.LogInformation("Rating Post called.");
            var createdRating = await _ratingService.CreateRating(r);
            if (createdRating == null)
                return NotFound();

            return CreatedAtAction(nameof(Get), new { id = createdRating.Id }, createdRating);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Rating Delete called.");
            return await _ratingService.DeleteRating(id)
                ? NoContent()
                : NotFound();
        }

        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetAllByMovieId(int movieId)
        {
            _logger.LogInformation("Rating GetAllByMovieId called.");
            var r = await _ratingService.GetAllByMovieId(movieId);
            if (r == null)
                return NotFound();

            return Ok(r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Rating Get called.");
            var m = await _ratingService.GetRatingById(id);
            if (m == null)
                return NotFound();

            return Ok(m);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateRatingDto r)
        {
            _logger.LogInformation("Rating Put called.");
            return await _ratingService.UpdateRating(id, r)
                ? NoContent()
                : NotFound();
        }
    }
}
