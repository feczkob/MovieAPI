using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MovieApi.ViewModels;
using MovieApi.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MovieApi.Filters;

namespace MovieApi.Services
{

    public class MovieService : IMovieService
    {

        private readonly MovieContext _context;
        private readonly IMapper _mapper;

        public MovieService(MovieContext movieContext, IMapper mapper)
        {
            _context = movieContext;
            _mapper = mapper;
        }

        public async Task<MovieRowVm> CreateMovie(NewMovieDto m)
        {
            var _m = _mapper.Map<Movie>(m);
            _context.Movies.Add(_m);
            await _context.SaveChangesAsync();
            return _mapper.Map<MovieRowVm>(_m);
        }

        public async Task<bool> DeleteMovie(int id)
        {
            var m = await _context.Movies.FindAsync(id);
            if (m == null)
                return false;

            _context.Movies.Remove(m);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<MovieRowVm>> GetAll(GenericQueryOption<MovieFilter> option)
        {
            var q = _context.Movies as IQueryable<Movie>;
            if (!String.IsNullOrEmpty(option.Filter?.NameTerm))
            {
                q = q.Where(x => x.Name.Contains(option.Filter.NameTerm));
            }
            if(option.Filter?.MinRating != null)
            {
                q = q.Where(x => x.Ratings.Average(r => r.Value) >= option.Filter.MinRating);
            }

            q = option.SortOrder == SortOrder.Ascending
                ? q.OrderBy(x => x.Ratings.Average(r => r.Value))
                : q.OrderByDescending(x => x.Ratings.Average(r => r.Value));

            return await q
                .Skip((option.Page - 1) * option.PageSize)
                .Take(option.PageSize)
                .ProjectTo<MovieRowVm>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<MovieVm> GetMovieById(int id)
        {
            return await _context.Movies
                .Include(x => x.Ratings)
                .Include(x => x.CategoryMovies)
                .Where(x => x.Id == id)
                .Select(x => new MovieVm
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Length = x.Length,
                    Year = x.Year,
                    RatingAvg = x.Ratings.Count > 0 ? x.Ratings.Average(r => r.Value) : 0,
                    Ratings = _context.Ratings
                        .Where(y => y.MovieId == x.Id)
                        .Select(y => new RatingVm
                        {
                            Id = y.Id,
                            Value = y.Value,
                            Description = y.Description,
                            MovieId = y.MovieId
                        })
                        .ToList(),
                    Categories = _context.Categories
                        .Where(y => y.CategoryMovies.Any(cm => cm.MovieId == x.Id))
                        .Select(y => new CategoryVm
                        {
                            Id = y.Id,
                            Name = y.Name
                        })
                        //.ProjectTo<CategoryVm>(_mapper.ConfigurationProvider)
                        .ToList()})
                //.ProjectTo<MovieVm>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<List<MovieRowVm>> GetMovieWhere(Expression<Func<Movie, bool>> predicate)
        {
            return await _context.Movies
                .Where(predicate)
                .ProjectTo<MovieRowVm>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<bool> UpdateMovie(int id, UpdateMovieDto m)
        {
            var _m = _mapper.Map<Movie>(m);
            _m.Id = id;

            _context.Entry(_m).State = EntityState.Modified;
            var n = await _context.SaveChangesAsync();

            return n == 1;
        }
    }
}
