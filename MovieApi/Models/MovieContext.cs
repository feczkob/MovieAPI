using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieApi.Models.Authentication;

namespace MovieApi.Models
{
    public class MovieContext : IdentityDbContext<ApplicationUser>
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Movie)
                .WithMany(m => m.Ratings)
                .HasForeignKey(r => r.MovieId);

            modelBuilder.Entity<CategoryMovie>(entity =>
            {
                entity.HasKey(cm => new { cm.CategoryId, cm.MovieId });

                entity.HasOne(cm => cm.Movie)
                    .WithMany(m => m.CategoryMovies)
                    .HasForeignKey(cm => cm.MovieId);

                entity.HasOne(cm => cm.Category)
                    .WithMany(c => c.CategoryMovies)
                    .HasForeignKey(cm => cm.CategoryId);
            });
            //.HasKey(cm => new { cm.CategoryId, cm.MovieId });

            // seed
            modelBuilder.Entity<Movie>().HasData(
                new Movie { Id = 1, Name = "The Dark Tower", 
                    Description = "A boy haunted by visions of a dark tower from a parallel reality teams up with the tower's disillusioned guardian to stop an evil warlock known as the Man in Black who plans to use the boy to destroy the tower and open the gates of Hell.",
                    Length = 95, Year = 2017},
                new Movie { Id = 2, Name = "Star Wars: A New Hope (Episode IV)",
                    Description = "Princess Leia gets abducted by the insidious Darth Vader. Luke Skywalker then teams up with a Jedi Knight, a pilot and two droids to free her and to save the galaxy from the violent Galactic Empire.",
                    Length = 121, Year = 1977 });
            modelBuilder.Entity<Rating>().HasData(
                new Rating { Id = 1, Value = 4, Description = "Too dark for me.", MovieId = 1 },
                new Rating { Id = 2, Value = 8, Description = "Dark enough for me.", MovieId = 1 },
                new Rating { Id = 3, Value = 9, Description = "The old Star Wars movies are much better then the ones made by Disney.", MovieId = 2 });
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Sci-fi" },
                new Category { Id = 2, Name = "Adventure"},
                new Category { Id = 3, Name = "Action" },
                new Category { Id = 4, Name = "Drama"});
            modelBuilder.Entity<CategoryMovie>().HasData(
                new CategoryMovie { CategoryId = 1, MovieId = 1 },
                new CategoryMovie { CategoryId = 1, MovieId = 2 },
                new CategoryMovie { CategoryId = 2, MovieId = 1 },
                new CategoryMovie { CategoryId = 2, MovieId = 2 },
                new CategoryMovie { CategoryId = 3, MovieId = 1 },
                new CategoryMovie { CategoryId = 4, MovieId = 2 });
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryMovie> CategoryMovies { get; set; }
    }
}
