using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieApi.Dtos;
using MovieApi.Models;
using MovieApi.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public class CategoryService
    {
        private readonly MovieContext _context;
        private readonly IMapper _mapper;

        public CategoryService(MovieContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryVm> CreateCategory(NewCategoryDto c)
        {
            var _c = _mapper.Map<Category>(c);
            _context.Categories.Add(_c);
            await _context.SaveChangesAsync();
            return _mapper.Map<CategoryVm>(_c);
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var c = await _context.Categories.FindAsync(id);
            if (c == null)
                return false;

            _context.Categories.Remove(c);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CategoryVm>> GetAll()
        {
            return await _context.Categories
                .ProjectTo<CategoryVm>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<bool> AddMovieToCategory(int movieId, int categoryId)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            var category = await _context.Categories.FindAsync(categoryId);

            if (movie == null || category == null)
                return false;

            CategoryMovie cm = new CategoryMovie
            {
                Category = category,
                CategoryId = category.Id,
                Movie = movie,
                MovieId = movie.Id
            };
            _context.CategoryMovies.Add(cm);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CategoryVm>> GetAllByMovieId(int movieId)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null)
                return null; 

            return await _context.Categories
                .Where(x => x.CategoryMovies.Any(cm => cm.MovieId == movieId))
                .ProjectTo<CategoryVm>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<MovieRowVm>> GetAllByCategoryId(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
                return null;

            return await _context.Movies
                .Where(x => x.CategoryMovies.Any(cm => cm.CategoryId == categoryId))
                .ProjectTo<MovieRowVm>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
