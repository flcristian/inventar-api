using System.Text;
using FluentMigrator.Runner;
using inventar_api.ArticleLocations.Repository;
using inventar_api.ArticleLocations.Repository.Interfaces;
using inventar_api.ArticleLocations.Services;
using inventar_api.ArticleLocations.Services.Interfaces;
using inventar_api.Articles.Repository;
using inventar_api.Articles.Repository.Interfaces;
using inventar_api.Articles.Services;
using inventar_api.Articles.Services.Interfaces;
using inventar_api.Data;
using inventar_api.Locations.Repository;
using inventar_api.Locations.Repository.Interfaces;
using inventar_api.Locations.Services;
using inventar_api.Locations.Services.Interfaces;
using inventar_api.Users.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace inventar_api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // SERVICES AND REPOSITORIES
        
        builder.Services.AddScoped<ILocationsRepository, LocationsRepository>();
        builder.Services.AddScoped<ILocationsQueryService, LocationsQueryService>();
        builder.Services.AddScoped<ILocationsCommandService, LocationsCommandService>();

        builder.Services.AddScoped<IArticlesRepository, ArticlesRepository>();
        builder.Services.AddScoped<IArticlesQueryService, ArticlesQueryService>();
        builder.Services.AddScoped<IArticlesCommandService, ArticlesCommandService>();
        
        builder.Services.AddScoped<IArticleLocationsRepository, ArticleLocationsRepository>();
        builder.Services.AddScoped<IArticleLocationsQueryService, ArticleLocationsQueryService>();
        builder.Services.AddScoped<IArticleLocationsCommandService, ArticleLocationsCommandService>();
        
        #region BASE

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("inventar-client", domain => domain.WithOrigins("http://localhost:4200", "http://clientapp")
                .AllowAnyHeader()
                .AllowAnyMethod()
            );
        }); 

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(builder.Configuration.GetConnectionString("Default")!,
                new MySqlServerVersion(new Version(8, 0, 21))));
        
        builder.Services.AddIdentity<User, IdentityRole<int>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddMySql5()
                .WithGlobalConnectionString(builder.Configuration.GetConnectionString("Default"))
                .ScanIn(typeof(Program).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
        });
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
        
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            // Define the Bearer Auth scheme that's in use
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",  // Name of the header
                In = ParameterLocation.Header,  // where to find the security token (in the header)
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"  // Must be "Bearer"
            });

            // Make sure swagger UI requires a Bearer token to be specified
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"  
                        }
                    },
                    new string[] {}
                }
            });
        });

        #endregion

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseAuthentication();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
      
        app.UseExceptionHandler("/Home/Error");
        app.UseDeveloperExceptionPage();    

        using (var scope = app.Services.CreateScope())
        {
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }

        app.UseCors("inventar-client");

        app.Run();
    }
}