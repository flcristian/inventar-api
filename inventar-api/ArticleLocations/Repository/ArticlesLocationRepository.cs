using AutoMapper;
using inventar_api.ArticleLocations.DTOs;
using inventar_api.ArticleLocations.Models;
using inventar_api.ArticleLocations.Repository.Interfaces;
using inventar_api.Data;
using Microsoft.EntityFrameworkCore;

namespace inventar_api.ArticleLocations.Repository;

public class ArticlesLocationRepository : IArticleLocationsRepository
{
    private AppDbContext _context;
    private IMapper _mapper;

    public ArticlesLocationRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ArticleLocation>> GetAllAsync()
    {
        return await _context.ArticleLocations.ToListAsync();
    }

    public async Task<ArticleLocation?> GetAsync(GetArticleLocationRequest request)
    {
        return await _context.ArticleLocations
            .Include(al => al.Article)
            .Include(al => al.Location)
            .FirstOrDefaultAsync(al =>
                al.Article.Code == request.ArticleCode &&
                al.Location.Code.Equals(request.LocationCode));
    }

    public async Task<ArticleLocation> CreateAsync(CreateArticleLocationRequest request)
    {
        ArticleLocation articleLocation = _mapper.Map<ArticleLocation>(request);
        _context.ArticleLocations.Add(articleLocation);
        await _context.SaveChangesAsync();
        return articleLocation;
    }

    public async Task<ArticleLocation> UpdateAsync(UpdateArticleLocationRequest request)
    {
        ArticleLocation articleLocation = (await _context.ArticleLocations.FirstOrDefaultAsync(al =>
            al.ArticleId == request.ArticleId && al.LocationId == request.LocationId))!;

        articleLocation.Count = request.Count;
        _context.ArticleLocations.Update(articleLocation);
        await _context.SaveChangesAsync();
        return articleLocation;
    }

    public async Task<ArticleLocation> DeleteAsync(DeleteArticleLocationRequest request)
    {
        ArticleLocation articleLocation = (await GetAsync(_mapper.Map<GetArticleLocationRequest>(request)))!;
        _context.ArticleLocations.Remove(articleLocation);
        await _context.SaveChangesAsync();
        return articleLocation;
    }
}