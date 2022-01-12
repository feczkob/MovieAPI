using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieApi.Services;
using MovieApi.Dtos;
using MovieApi.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using MovieApi.Models.Authentication;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly CategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(CategoryService category, ILogger<CategoryController> logger)
        {
            _categoryService = category;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Post([FromBody] NewCategoryDto c)
        {
            _logger.LogInformation("Category Post called.");
            var createdCategory = await _categoryService.CreateCategory(c);
            if (createdCategory == null)
                return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Category Delete called.");
            return await _categoryService.DeleteCategory(id)
                ? NoContent()
                : NotFound();
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryVm>> GetAll()
        {
            _logger.LogInformation("Category GetAll called.");
            return await _categoryService.GetAll();
        }

        [HttpGet("categories/{movieId}")]
        public async Task<IActionResult> GetAllByMovieId(int movieId)
        {
            _logger.LogInformation("Category GetAllByMovieId called.");
            var c =  await _categoryService.GetAllByMovieId(movieId);
            if (c == null)
                return NotFound();

            return Ok(c);
        }

        [HttpGet("movies/{categoryId}")]
        public async Task<IActionResult> GetAllByCategoryId(int categoryId)
        {
            _logger.LogInformation("Category GetAllByCategoryId called.");
            var m =  await _categoryService.GetAllByCategoryId(categoryId);
            if (m == null)
                return NotFound();

            return Ok(m);
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<bool> AddMovieToCategory(int movieId, int categoryId)
        {
            _logger.LogInformation("Category AddMovieToCategory called.");
            return await _categoryService.AddMovieToCategory(movieId, categoryId);
        }
    }
}
