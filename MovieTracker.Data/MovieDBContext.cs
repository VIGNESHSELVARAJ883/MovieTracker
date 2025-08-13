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
        public DbSet<User> Users { get; set; }
        public DbSet<UserWatchlist> UserWatchlist { get; set; }
        public DbSet<UserToWatch> UserToWatch { get; set; }
        public DbSet<UserFavourite> UserFavourites { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<CollectionMovie> CollectionMovies { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<UserReview> UserReviews { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // MovieGenre (many-to-many)
            modelBuilder.Entity<Language>()
                .HasKey(l => l.Code);

            modelBuilder.Entity<Genre>()
                .HasKey(g => g.GenreId);

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

            modelBuilder.Entity<UserWatchlist>()
                .HasKey(w => new { w.WatchlistId });

            //modelBuilder.Entity<UserWatchlist>()
            //    .HasOne(w => w.Movie)
            //    .WithMany(m => m.UserWatchlists)
            //    .HasForeignKey(w => w.MovieId);

            modelBuilder.Entity<UserWatchlist>()
                .HasOne(w => w.User)
                .WithMany(w => w.Watchlist)
                .HasForeignKey(w => w.UserId);

            modelBuilder.Entity<UserToWatch>()
                .HasKey(t => t.ToWatchId);

            modelBuilder.Entity<UserToWatch>()
                .HasOne(t => t.User)
                .WithMany(u => u.ToWatchList)
                .HasForeignKey(t => t.UserId);

            //modelBuilder.Entity<UserToWatch>()
            //    .HasOne(t => t.Movie)
            //    .WithMany(m => m.ToWatchList)
            //    .HasForeignKey(t => t.MovieId);

            modelBuilder.Entity<UserToWatch>()
                .HasIndex(t => new { t.UserId, t.MovieId })
                .IsUnique();

            // UserToWatch unique constraint
            modelBuilder.Entity<UserToWatch>()
                .HasIndex(w => new { w.UserId, w.MovieId })
                .IsUnique();

            // UserFavourites unique constraint
            modelBuilder.Entity<UserFavourite>()
                .HasKey(f => f.FavouriteId);

            modelBuilder.Entity<UserFavourite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favourites)
                .HasForeignKey(f => f.UserId);

            //modelBuilder.Entity<UserFavourite>()
            //    .HasOne(f => f.Movie)
            //    .WithMany(m => m.Favourites)
            //    .HasForeignKey(f => f.MovieId);

            modelBuilder.Entity<UserFavourite>()
                .HasIndex(f => new { f.UserId, f.MovieId })
                .IsUnique();

            // Collection unique name per user
            modelBuilder.Entity<Collection>()
                .HasKey(c => c.CollectionId);

            modelBuilder.Entity<Collection>()
                .HasOne(c => c.User)
                .WithMany(u => u.Collections)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Collection>()
                .HasIndex(c => new { c.UserId, c.Name })
                .IsUnique();

            // CollectionMovies unique per collection
            modelBuilder.Entity<CollectionMovie>()
                .HasKey(cm => cm.CollectionMovieId);

            modelBuilder.Entity<CollectionMovie>()
                .HasOne(cm => cm.Collection)
                .WithMany(c => c.CollectionMovies)
                .HasForeignKey(cm => cm.CollectionId);

            //modelBuilder.Entity<CollectionMovie>()
            //    .HasOne(cm => cm.Movie)
            //    .WithMany(m => m.CollectionMovies)
            //    .HasForeignKey(cm => cm.MovieId);

            modelBuilder.Entity<CollectionMovie>()
                .HasIndex(cm => new { cm.CollectionId, cm.MovieId })
                .IsUnique();

            modelBuilder.Entity<UserActivity>()
                .HasKey(r => r.ActivityId);

            modelBuilder.Entity<UserActivity>()
                .HasOne(r => r.User)
                .WithMany(u => u.Activities)
                .HasForeignKey(r => r.UserId);

            //modelBuilder.Entity<UserActivity>()
            //    .HasOne(r => r.Movie)
            //    .WithMany(m => m.Reviews)
            //    .HasForeignKey(r => r.MovieId);

            modelBuilder.Entity<UserActivity>()
                .HasIndex(r => new { r.UserId })
                .IsUnique();

            modelBuilder.Entity<UserReview>()
                .HasKey(r => r.ReviewId);

            modelBuilder.Entity<UserReview>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            //modelBuilder.Entity<UserReview>()
            //    .HasOne(r => r.Movie)
            //    .WithMany(m => m.Reviews)
            //    .HasForeignKey(r => r.MovieId);

            modelBuilder.Entity<UserReview>()
                .HasIndex(r => new { r.UserId, r.MovieId })
                .IsUnique(); // Each user can have only one review per movie

            modelBuilder.Entity<RefreshToken>()
                .HasKey(rt => rt.TokenId);

            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId);

            modelBuilder.Entity<RefreshToken>()
                .HasIndex(rt => rt.Token)
                .IsUnique();
            modelBuilder.Entity<RefreshToken>()
                .Property(r => r.RevokedAt)
                .HasColumnType("datetime2");
            modelBuilder.Entity<RefreshToken>()
                .Property(r => r.ExpiresAt)
                .HasColumnType("datetime2");
            modelBuilder.Entity<RefreshToken>()
                .Property(r => r.CreatedAt)
                .HasColumnType("datetime2");
        }
    }
}
