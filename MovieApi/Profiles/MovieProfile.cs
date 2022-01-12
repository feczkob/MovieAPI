using AutoMapper;
using MovieApi.Dtos;
using MovieApi.Models;
using MovieApi.ViewModels;
using MovieApi.Extensions;
using AutoMapper.QueryableExtensions;
using System.Linq;

namespace MovieApi.Profiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<NewMovieDto, Movie>();
            CreateMap<UpdateMovieDto, Movie>();
            CreateMap<Movie, MovieVm>()
                .ForMember(dest => dest.RatingAvg,
                opt => opt.MapFrom(src => src.Ratings.Count > 0 ? src.Ratings.Average(r => r.Value) : 0));
            CreateMap<Movie, MovieRowVm>()
                .ForMember(dest => dest.RatingAvg,
                opt => opt.MapFrom(src => src.Ratings.Count > 0 ? src.Ratings.Average(r => r.Value) : 0));
                //opt => opt.ResolveUsing(src => src.Ratings.Average(r => r.Value)));
            CreateMap<NewRatingDto, Rating>();
            CreateMap<UpdateRatingDto, Rating>();
            CreateMap<Rating, RatingVm>();
            CreateMap<NewCategoryDto, Category>();
            CreateMap<Category, CategoryVm>();
        }
    }
}
