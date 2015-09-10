using System;
using AutoMapper;
using BForms.Models;
using ZelectroCom.Data.Models;
using ZelectroCom.Web.Areas.Member.ViewModels.Article;
using ZelectroCom.Web.Areas.Member.ViewModels.Section;
using ZelectroCom.Web.ViewModels;

namespace ZelectroCom.Web.Infrastructure
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<Section, SectionVm>()
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => new BsRangeItem<int>()
                {
                    ItemValue = src.Order,
                    MinValue = 0,
                    MaxValue = 100,
                    TextValue = "0-100",
                    Display = Resources.Resources.SectionVm_Order
                }));
            Mapper.CreateMap<SectionVm, Section>()
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order.ItemValue));

            Mapper.CreateMap<Article, DraftVm>()
                .ForMember(dest => dest.PublishTime, opt => opt.MapFrom(src => new BsDateTime{ DateValue = src.PublishTime }));
            Mapper.CreateMap<DraftVm, Article>()
                .ForMember(dest => dest.PublishTime, opt => opt.MapFrom(src => src.PublishTime.DateValue));

            Mapper.CreateMap<Article, DraftRowVm>()
                .ForMember(dest => dest.ArticleState, opt => opt.MapFrom(src => src.ArticleState.GetDisplayName()));

            Mapper.CreateMap<Section, SectionRowVm>();

            Mapper.CreateMap<Article, PreviewArticleVm>();
        }
    }
}