using System;
using AutoMapper;
using BForms.Models;
using ZelectroCom.Data.Models;
using ZelectroCom.Web.Areas.Member.ViewModels.Article;
using ZelectroCom.Web.Areas.Member.ViewModels.Section;

namespace ZelectroCom.Web.Infrastructure
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<Section, SectionVM>();
            Mapper.CreateMap<SectionVM, Section>();

            Mapper.CreateMap<Article, DraftVm>()
                .ForMember(dest => dest.PublishTime, opt => opt.MapFrom(src => new BsDateTime{ DateValue = src.PublishTime }));
            Mapper.CreateMap<DraftVm, Article>()
                .ForMember(dest => dest.PublishTime, opt => opt.MapFrom(src => src.PublishTime.DateValue));
            Mapper.CreateMap<Article, DraftRowVm>()
                .ForMember(dest => dest.ArticleState, opt => opt.MapFrom(src => src.ArticleState.GetDisplayName()));
        }
    }
}