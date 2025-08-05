using Microsoft.EntityFrameworkCore;
using MovieTracker.Data.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MovieTracker.Data
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<TVSeries> TVSeries { get; set; }
        public DbSet<TVSeriesSeason> TVSeriesSeasons { get; set; }
        public DbSet<TVSeriesEpisode> TVSeriesEpisodes { get; set; }
        public DbSet<TVSeriesGenre> TVSeriesGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // MovieGenre (many-to-many)
            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Movie)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(mg => mg.MovieId);

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Genre)
                .WithMany(g => g.MovieGenres)
                .HasForeignKey(mg => mg.GenreId);

            // TVSeriesGenre (many-to-many)
            modelBuilder.Entity<TVSeriesGenre>()
                .HasKey(sg => new { sg.TVSeriesId, sg.GenreId });

            modelBuilder.Entity<TVSeriesGenre>()
                .HasOne(sg => sg.TVSeries)
                .WithMany(s => s.Genres)
                .HasForeignKey(sg => sg.TVSeriesId);

            modelBuilder.Entity<TVSeriesGenre>()
                .HasOne(sg => sg.Genre)
                .WithMany(g => g.TVSeriesGenres)
                .HasForeignKey(sg => sg.GenreId);

            // TVSeriesSeason
            modelBuilder.Entity<TVSeriesSeason>()
                .HasOne(s => s.Series)
                .WithMany(tv => tv.Seasons)
                .HasForeignKey(s => s.SeriesId);

            // TVSeriesEpisode
            modelBuilder.Entity<TVSeriesEpisode>()
                .HasOne(e => e.Season)
                .WithMany(s => s.Episodes)
                .HasForeignKey(e => e.SeasonId);
        }
    }
}
