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
using Microsoft.EntityFrameworkCore;

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

        builder.Services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddMySql5()
                .WithGlobalConnectionString(builder.Configuration.GetConnectionString("Default"))
                .ScanIn(typeof(Program).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        #endregion

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

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