using System;
using System.Collections.Generic;
using System.Linq;
using MovieApi.Models;

namespace MovieApi.Services
{
    public class MovieServiceInMemory
    {

        private List<Movie> movies = new List<Movie>
        {
            new Movie
            {
                Id = 1,
                Name = "The Dark Tower",
                Description = "A boy haunted by visions of a dark tower from a parallel reality teams up with the tower's disillusioned guardian " +
                "to stop an evil warlock known as the Man in Black who plans to use the boy to destroy the tower and open the gates of Hell.",
                Length = 95,
                Year = 2017
            }
        };

        public Movie CreateMovie(Movie m)
        {
            m.Id = movies.Max(x => x.Id) + 1;
            movies.Add(m);
            return m;
        }

        public bool DeleteMovie(int id)
        {
            var n = movies.RemoveAll(x => x.Id == id);

            return n == 1;
        }

        public List<Movie> GetAll()
        {
            return movies.ToList();
        }

        public Movie GetMovieById(int id)
        {
            return movies.SingleOrDefault(x => x.Id == id);
        }

        public List<Movie> GetMovieWhere(Func<Movie, bool> predicate)
        {
            return movies.Where(predicate).ToList();
        }

        public bool UpdateMovie(int id, Movie m)
        {
            var movieToUpdate = movies.SingleOrDefault(x => x.Id == id);

            if (movieToUpdate == null)
                return false;

            movieToUpdate.Name = m.Name;
            movieToUpdate.Description = m.Description;
            movieToUpdate.Length = m.Length;
            movieToUpdate.Year = m.Year;

            return true;
        }
    }
}
