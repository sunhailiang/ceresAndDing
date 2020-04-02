using AutoMapper;
using Ceres.Application.ViewModels;
using Ceres.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile: Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Supporter, SupporterLoginResponse>();

            //Supporter →→→ SupporterGetAllResponse
            CreateMap<Supporter, GetSupporterListResponse>();
            CreateMap<Agenter, GetAgenterListResponse>()
                .ForPath(d => d.Province, o => o.MapFrom(s => s.Address.Province))
                .ForPath(d => d.City, o => o.MapFrom(s => s.Address.City));

            CreateMap<UserInformation, GetOneOriginalCustomerResponse>()
                .ForPath(d => d.OID, o => o.MapFrom(s => s.UserGuid))
                .ForPath(d => d.Cellphone, o => o.MapFrom(s => s.PhoneNumber))
                .ForPath(d => d.Sex, o => o.MapFrom(s => s.UserGender==1?0:1))//原来系统中的男女与新系统男女存在不一致问题
                .ForPath(d => d.InitHeight, o => o.MapFrom(s => s.Height))
                .ForPath(d => d.InitWeight, o => o.MapFrom(s => s.Weight));

            CreateMap<Service, GetServiceListResponse>();

            CreateMap<Customer, CustomerByPage>()
                .ForPath(d => d.Province, o => o.MapFrom(s => s.Address.Province))
                .ForPath(d => d.City, o => o.MapFrom(s => s.Address.City));

            CreateMap<Customer, GetOneCustomerBasicInformationResponse>()
                .ForPath(d => d.Province, o => o.MapFrom(s => s.Address.Province))
                .ForPath(d => d.City, o => o.MapFrom(s => s.Address.City));

            CreateMap<Answer, CustomerWeight> ()
                .ForPath(d => d.RecordTime, o => o.MapFrom(s => s.Ctime))
                .ForPath(d => d.Weight, o => o.MapFrom(s=>Convert.ToSingle(s.Content.Substring(1, s.Content.Length - 2)) ));

            CreateMap<Answer, CustomerHeight>()
                .ForPath(d => d.RecordTime, o => o.MapFrom(s => s.Ctime))
                .ForPath(d => d.Height, o => o.MapFrom(s => Convert.ToSingle(s.Content.Substring(1, s.Content.Length - 2))));

            CreateMap<Domain.Models.Food, ViewModels.Food>()
                .ForPath(d => d.OID, o => o.MapFrom(s => s.OID))
                .ForPath(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForPath(d => d.Click, o => o.MapFrom(s => s.Click));

            CreateMap<Domain.Models.Food, ViewModels.FoodByPage>()
                .ForPath(d => d.OID, o => o.MapFrom(s => s.OID))
                .ForPath(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForPath(d => d.Click, o => o.MapFrom(s => s.Click));

            CreateMap<Component, ViewModels.FoodComponent>()
                .ForPath(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForPath(d => d.EnglishName, o => o.MapFrom(s => s.EnglishName))
                .ForPath(d => d.NameCode, o => o.MapFrom(s => s.NameCode))
                .ForPath(d => d.Unit, o => o.MapFrom(s => s.Unit));

            CreateMap<CustomerDiet, OneCustomerDietByPage>()
                .ForPath(d => d.OID, o => o.MapFrom(s => s.OID))
                .ForPath(d => d.RecommendDailyEnergy, o => o.MapFrom(s => s.Recommend.DailyEnergy))
                .ForPath(d => d.CurrentDailyEnergy, o => o.MapFrom(s => s.Current.DailyEnergy))
                .ForPath(d => d.CurrentDiet, o => o.MapFrom(s => s.CurrentDiet))
                .ForPath(d => d.Status, o => o.MapFrom(s => s.Status))
                .ForPath(d => d.DietNote, o => o.MapFrom(s => s.Discard==null ? "正常" : s.Discard.Reason))
                .ForPath(d => d.CreateTime, o => o.MapFrom(s => s.CreateTime));

            CreateMap<CustomerDiet, DietByPage>()
                .ForPath(d => d.OID, o => o.MapFrom(s => s.OID))
                .ForPath(d => d.RecommendDailyEnergy, o => o.MapFrom(s => s.Recommend.DailyEnergy))
                .ForPath(d => d.CurrentDailyEnergy, o => o.MapFrom(s => s.Current.DailyEnergy))
                .ForPath(d => d.CurrentDiet, o => o.MapFrom(s => s.CurrentDiet))
                .ForPath(d => d.Status, o => o.MapFrom(s => s.Status))
                .ForPath(d => d.DietNote, o => o.MapFrom(s => s.Discard == null ? "正常": s.Discard.Reason))
                .ForPath(d => d.CreateTime, o => o.MapFrom(s => s.CreateTime));

            CreateMap<CustomerDislikeFood, DislikeFoodByPage>()
                .ForPath(d => d.OID, o => o.MapFrom(s => s.FoodOid));

            CreateMap<Domain.Models.Food, FoodWithDislikeFlagByPage>()
                .ForPath(d => d.OID, o => o.MapFrom(s => s.OID))
                .ForPath(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForPath(d => d.Click, o => o.MapFrom(s => s.Click));

            CreateMap<Customer, TodoListByPage>()
                .ForPath(d => d.UserOid, o => o.MapFrom(s => s.OID))
                .ForPath(d => d.UserName, o => o.MapFrom(s => s.UserName))
                .ForPath(d => d.Cellphone, o => o.MapFrom(s => s.Cellphone));

            CreateMap<Customer, GetOneVIPCustomerResponse>()
                .ForPath(d => d.OID, o => o.MapFrom(s => s.OID))
                .ForPath(d => d.Cellphone, o => o.MapFrom(s => s.Cellphone))
                .ForPath(d => d.DefaultHeight, o => o.MapFrom(s => s.InitHeight));

            CreateMap<WeChatAuthorize, GetOneWeChatAuthorizeResponse>()
                .ForPath(d => d.OID, o => o.MapFrom(s => s.OID))
                .ForPath(d => d.Code2Session, o => o.MapFrom(s => s.Code2Session))
                .ForPath(d => d.CreateTime, o => o.MapFrom(s => s.CreateTime));
        }
    }
}
