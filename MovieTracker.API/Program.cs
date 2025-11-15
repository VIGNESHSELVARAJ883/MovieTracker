using Datadog.Trace;
using Datadog.Trace.Configuration;

//using Datadog.Trace.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieTracker.Data;
using MovieTracker.Data.Repositories;
using MovieTracker.Data.Repository;
using MovieTracker.Service;
using MovieTracker.Service.Services;
using MovieTracker.Service.Utils;
using Serilog;
using System.Data;
using System.Text;

namespace MovieTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Initialize Datadog Tracer
            var settings = TracerSettings.FromDefaultSources();
            settings.ServiceName = "movie-tracker-api";

            // Create a Tracer instance
            var tracer = new Tracer(settings);

            // Optionally set as global
            Tracer.Instance = tracer;


            var builder = WebApplication.CreateBuilder(args);

            // Read Datadog API key from configuration or environment
            var datadogApiKey = builder.Configuration["Datadog:ApiKey"]
                                ?? Environment.GetEnvironmentVariable("DD_API_KEY");

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.DatadogLogs(
                    apiKey: datadogApiKey,
                    service: "movie-tracker-api",
                    host: Environment.MachineName,
                    source: "csharp",
                    tags: new[] { $"env:{builder.Environment.EnvironmentName}" }
                )
                .CreateLogger();
            //builder.Host.UseSerilog();

            // Add services to the container.
            //builder.Services.AddScoped<ITMDBMovieService, TMDBMovieService>();
            //builder.Services.AddScoped<ITMDBMovieRepository, TMDBMovieRepository>();
            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            //builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<ITVSeriesRepository, TVSeriesRepository>();
            builder.Services.AddScoped<ITvSeriesService, TvSeriesService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserMovieService, UserMovieService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<IUserMovieRepository, UserMovieRepository>();
            builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

            // Register EF Core DbContext
            builder.Services.AddDbContext<MovieDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptions => sqlServerOptions.CommandTimeout(120)
                ));
            // Register HttpClient for TMDB service
            builder.Services.AddHttpClient<IMovieService, MovieService>(client =>
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                client.DefaultRequestHeaders.Add("accept", "application/json");
                client.DefaultRequestHeaders.Add("Authorization",
                    $"Bearer {builder.Configuration["TMDB:ApiKey"]}");
            });

            //builder.Services.AddHttpClient<IMovieRepository, MovieRepository>(client =>
            //{
            //    client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
            //    client.DefaultRequestHeaders.Add("accept", "application/json");
            //    client.DefaultRequestHeaders.Add("Authorization",
            //        $"Bearer {builder.Configuration["TMDB:ApiKey"]}");
            //});
            // Register Dapper as a transient service
            builder.Services.AddTransient<IDbConnection>(sp =>
                new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"] ?? throw new InvalidOperationException("JWT Key missing"));

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true, // Expired tokens are rejected
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero // Optional: no time drift allowed
                    };
                });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Movie Tracker API",
                    Version = "v1"
                });

                // Add JWT Bearer Authorization to Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http, // 👈 IMPORTANT
                    Scheme = "bearer",               // 👈 Lowercase here
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.\n\n" +
                                  "Enter your token.\n\nExample: **12345abcdef**"
                });

                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //             = new BaseOpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            }
                //        },
                //        Array.Empty<string>()
                //    }
                //});
            });


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
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
