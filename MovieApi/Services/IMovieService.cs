using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MovieApi.Models;
using MovieApi.ViewModels;
using MovieApi.Dtos;
using MovieApi.Filters;

namespace MovieApi.Services
{
    public interface IMovieService
    {
        Task<MovieRowVm> CreateMovie(NewMovieDto m);
        Task<bool> DeleteMovie(int id);
        Task<List<MovieRowVm>> GetAll(GenericQueryOption<MovieFilter> option);
        Task<MovieVm> GetMovieById(int id);
        Task<List<MovieRowVm>> GetMovieWhere(Expression<Func<Movie, bool>> predicate);
        Task<bool> UpdateMovie(int id, UpdateMovieDto m);
    }
}
