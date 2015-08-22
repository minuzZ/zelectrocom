using AutoMapper;
using ZelectroCom.Data.Models;
using ZelectroCom.Web.Areas.Member.ViewModels.Article;
using ZelectroCom.Web.Areas.Member.ViewModels.Section;
using ZelectroCom.Web.Infrastructure;

namespace ZelectroCom.Web.Tools
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<Section, SectionVM>();
            Mapper.CreateMap<SectionVM, Section>();

            Mapper.CreateMap<Article, DraftVM>();
            Mapper.CreateMap<DraftVM, Article>();

            Mapper.CreateMap<Article, DraftListItemVM>()
                .ForMember(dest => dest.ArticleState, opt => opt.MapFrom(src => src.ArticleState.GetDisplayName()));
        }
    }
}