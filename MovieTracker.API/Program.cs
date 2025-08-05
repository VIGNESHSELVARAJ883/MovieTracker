using Microsoft.EntityFrameworkCore;
using MovieTracker.Service;
using MovieTracker.Data;
using MovieTracker.Data.Repository;

using System.Data;
using Microsoft.Data.SqlClient;

namespace MovieTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddScoped<ITMDBMovieService, TMDBMovieService>();
            //builder.Services.AddScoped<ITMDBMovieRepository, TMDBMovieRepository>();
            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<ITVSeriesRepository, TVSeriesRepository>();
            builder.Services.AddScoped<ITvSeriesService, TvSeriesService>();

            // Register EF Core DbContext
            builder.Services.AddDbContext<MovieDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptions => sqlServerOptions.CommandTimeout(120)
                ));

            // Register Dapper as a transient service
            builder.Services.AddTransient<IDbConnection>(sp =>
                new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add CORS policy to allow all origins
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
