using System;
using AutoMapper;
using BForms.Models;
using ZelectroCom.Data.Models;
using ZelectroCom.Web.Areas.Member.ViewModels.Article;
using ZelectroCom.Web.Areas.Member.ViewModels.OldMedia;
using ZelectroCom.Web.Areas.Member.ViewModels.Profile;
using ZelectroCom.Web.Areas.Member.ViewModels.Section;
using ZelectroCom.Web.Areas.Member.ViewModels.ZDev;
using ZelectroCom.Web.ViewModels;
using ZelectroCom.Web.ViewModels.Home;
using ZelectroCom.Web.ViewModels.Post;
using ZelectroCom.Web.ViewModels.ZDev;

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
            Mapper.CreateMap<Section, SectionRowVm>().ForMember(dest => dest.Action, opt => opt.Ignore());
            Mapper.CreateMap<Section, ViewModels.Section.SectionVm>();


            Mapper.CreateMap<Article, DraftVm>()
                .ForMember(dest => dest.PublishTime, opt => opt.MapFrom(src => new BsDateTime{ DateValue = src.PublishTime }));
            Mapper.CreateMap<DraftVm, Article>()
                .ForMember(dest => dest.PublishTime, opt => opt.MapFrom(src => src.PublishTime.DateValue))
                //view model should not change article state
                .ForMember(dest => dest.ArticleState, opt => opt.Ignore());

            Mapper.CreateMap<Article, DraftRowVm>()
                .ForMember(dest => dest.ArticleState, opt => opt.MapFrom(src => src.ArticleState.GetDisplayName()));
            Mapper.CreateMap<Article, PublicationRowVm>()
                .ForMember(dest => dest.ArticleState, opt => opt.MapFrom(src => src.ArticleState.GetDisplayName()))
                .ForMember(dest => dest.AuthorName, opt => opt.Ignore());
            Mapper.CreateMap<Article, PubRequestRowVm>()
                .ForMember(dest => dest.ArticleState, opt => opt.MapFrom(src => src.ArticleState.GetDisplayName()))
                .ForMember(dest => dest.AuthorName, opt => opt.Ignore());

            Mapper.CreateMap<Article, PreviewArticleVm>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.UserName));

            Mapper.CreateMap<Article, PostIndexVm>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.UserName));

            Mapper.CreateMap<Article, ArticleVm>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.UserName));

            Mapper.CreateMap<ApplicationUser, UserDataVm>();
            
            Mapper.CreateMap<ApplicationUser, UserProfileVm>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => string.Format("{0} {1}", src.Firstname, src.Lastname)))
                .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.UserName));

            Mapper.CreateMap<ApplicationUser, ProfileVm>()
                .ForMember(dest => dest.UserData,
                    opt => opt.MapFrom(src => Mapper.Map<ApplicationUser, UserDataVm>(src)))
                .ForMember(dest => dest.UserProfile,
                    opt => opt.MapFrom(src => Mapper.Map<ApplicationUser, UserProfileVm>(src)));

            Mapper.CreateMap<ApplicationUser, EditableUserDataVm>();

            Mapper.CreateMap<ApplicationUser, EditableProfileVm>()
                .ForMember(dest => dest.UserData,
                    opt => opt.MapFrom(src => Mapper.Map<ApplicationUser, EditableUserDataVm>(src)));

            Mapper.CreateMap<EditableUserDataVm, ApplicationUser>();

            Mapper.CreateMap<OldMedia, OldMediaRowVm>();

            Mapper.CreateMap<ZDev, ZDevVm>()
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => new BsRangeItem<int>()
                {
                    ItemValue = src.Order,
                    MinValue = 0,
                    MaxValue = 100,
                    TextValue = "0-100",
                    Display = Resources.Resources.SectionVm_Order
                }));
            Mapper.CreateMap<ZDev, ZDevRowVm>();

            Mapper.CreateMap<ZDevVm, ZDev>()
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order.ItemValue));

            Mapper.CreateMap<ZDev, ZDevIndexVm>();
        }
    }
}