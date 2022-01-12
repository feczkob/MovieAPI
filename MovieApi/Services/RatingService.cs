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

namespace MovieApi.Services
{
    public class RatingService
    {
        private readonly MovieContext _context;
        private readonly IMapper _mapper;

        public RatingService(MovieContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RatingVm> CreateRating(NewRatingDto r)
        {
            var _r = _mapper.Map<Rating>(r);
            var movie = await _context.Movies.FindAsync(r.MovieId);
            if (movie == null)
                return null;

            _r.Movie = movie;
            _context.Ratings.Add(_r);
            await _context.SaveChangesAsync();
            return _mapper.Map<RatingVm>(_r);
        }

        public async Task<bool> DeleteRating(int id)
        {
            var r = await _context.Ratings.FindAsync(id);
            if (r == null)
                return false;

            _context.Ratings.Remove(r);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<RatingVm> GetRatingById(int id)
        {
            return await _context.Ratings
                .Where(x => x.Id == id)
                .ProjectTo<RatingVm>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<List<RatingVm>> GetAllByMovieId(int movieId)
        {
            var r = await _context.Movies
                .Where(x => x.Id == movieId)
                .SingleOrDefaultAsync();
            if (r == null)
                return null;
                 
            return await _context.Ratings
                .Where(x => x.MovieId == movieId)
                .ProjectTo<RatingVm>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // kell ez?
        public async Task<bool> UpdateRating(int id, UpdateRatingDto r)
        {
            var _r = _mapper.Map<Rating>(r);
            _r.Id = id;

            _context.Entry(_r).State = EntityState.Modified;
            var n = await _context.SaveChangesAsync();

            return n == 1;
        }
    }
}
