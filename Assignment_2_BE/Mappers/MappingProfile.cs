using AutoMapper;
using Assignment_2_BE.Models;
using Assignment_2_BE.DTOs;

namespace Assignment_2_BE.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SystemAccount, SystemAccountResponseDTO>().ReverseMap();
            CreateMap<NewsArticle, NewsArticleResponseDTO>().ReverseMap();
            CreateMap<CreateNewsDTO, NewsArticle>();
            CreateMap<UpdateNewsDTO, NewsArticle>();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Tag, TagDTO>().ReverseMap();
        }
    }
}
