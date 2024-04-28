using AutoMapper;
using inventar_api.ArticleLocations.DTOs;
using inventar_api.ArticleLocations.Models;
using inventar_api.ArticleLocations.Repository.Interfaces;
using inventar_api.Data;
using Microsoft.EntityFrameworkCore;

namespace inventar_api.ArticleLocations.Repository;

public class ArticleLocationsRepository : IArticleLocationsRepository
{
    private AppDbContext _context;
    private IMapper _mapper;

    public ArticleLocationsRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ArticleLocation>> GetAllAsync()
    {
        return await _context.ArticleLocations.ToListAsync();
    }

    public async Task<IEnumerable<ArticleLocationHistory>> GetHistoryAsync()
    {
        return await _context.ArticleLocationHistory.ToListAsync();
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

    public async Task<ArticleLocationHistory?> GetHistoryByIdAsync(int id)
    {
        return await _context.ArticleLocationHistory
            .Include(alh => alh.Article)
            .Include(alh => alh.Location)
            .FirstOrDefaultAsync(alh => alh.Id == id);
    }

    public async Task<ArticleLocation> CreateAsync(CreateArticleLocationRequest request)
    {
        ArticleLocation articleLocation = new ArticleLocation
        {
            ArticleCode = request.ArticleCode,
            LocationCode = request.LocationCode,
            Count = request.Count
        };
        
        _context.ArticleLocations.Add(articleLocation);
        await _context.SaveChangesAsync();
        return articleLocation;
    }

    public async Task<ArticleLocationHistory> CreateHistoryAsync(CreateStockHistoryRequest request)
    {
        ArticleLocationHistory alh = new ArticleLocationHistory
        {
            Date = request.Date,
            ArticleCode = request.ArticleCode,
            LocationCode = request.LocationCode,
            StockIn = request.StockIn,
            StockOut = request.StockOut,
            Order = request.Order,
            Necessary = request.Necessary,
            Source = request.Source
        };
        
        _context.ArticleLocationHistory.Add(alh);
        await _context.SaveChangesAsync();
        return alh;
    }

    public async Task<ArticleLocation> UpdateAsync(UpdateArticleLocationRequest request)
    {
        ArticleLocation articleLocation = (await _context.ArticleLocations.FirstOrDefaultAsync(al =>
            al.ArticleCode == request.ArticleCode && al.LocationCode.Equals(request.LocationCode)))!;

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

    public async Task<ArticleLocationHistory> DeleteHistoryAsync(int id)
    {
        ArticleLocationHistory alh = (await _context.ArticleLocationHistory.FirstOrDefaultAsync(alh => alh.Id == id))!;
        _context.ArticleLocationHistory.Remove(alh);
        await _context.SaveChangesAsync();
        return alh;
    }
}