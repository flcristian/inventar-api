using AutoMapper;
using inventar_api.Articles.DTOs;
using inventar_api.Articles.Models;
using inventar_api.Articles.Repository.Interfaces;
using inventar_api.Data;
using Microsoft.EntityFrameworkCore;

namespace inventar_api.Articles.Repository;

public class ArticlesRepository : IArticlesRepository
{
    private AppDbContext _context;
    private IMapper _mapper;

    public ArticlesRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Article>> GetAllAsync()
    {
        return await _context.Articles
            .Include(a => a.ArticleLocations)
            .ToListAsync();
    }

    public async Task<Article?> GetByCodeAsync(int code)
    {
        return await _context.Articles
            .Include(a => a.ArticleLocations)
            .FirstOrDefaultAsync(a => a.Code == code);
    }

    public async Task<Article> CreateAsync(CreateArticleRequest request)
    {
        Article article = _mapper.Map<Article>(request);
        _context.Articles.Add(article);
        await _context.SaveChangesAsync();
        return article;
    }

    public async Task<Article> UpdateAsync(UpdateArticleRequest request)
    {
        Article article = (await GetByCodeAsync(request.Code))!;
        article.Name = request.Name;
        article.Consumption = request.Consumption;
        article.Machinery = request.Machinery;

        _context.Articles.Update(article);
        await _context.SaveChangesAsync();
        return article;
    }

    public async Task<Article> DeleteAsync(int code)
    {
        Article article = (await GetByCodeAsync(code))!;
        _context.Articles.Remove(article);
        await _context.SaveChangesAsync();
        return article;
    }
}