using AutoMapper;
using inventar_api.ArticleLocations.DTOs;
using inventar_api.ArticleLocations.Models;
using inventar_api.Articles.DTOs;
using inventar_api.Articles.Models;
using inventar_api.Locations.DTOs;
using inventar_api.Locations.Models;

namespace inventar_api.System;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateArticleRequest, Article>();
        CreateMap<UpdateArticleRequest, Article>();
        CreateMap<CreateLocationRequest, Location>();
        CreateMap<CreateArticleLocationRequest, ArticleLocation>();
        CreateMap<UpdateArticleLocationRequest, ArticleLocation>();
        CreateMap<DeleteArticleLocationRequest, GetArticleLocationRequest>();
    }
}