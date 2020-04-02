using AutoMapper;
using Ceres.Application.ViewModels;
using Ceres.Domain.Commands;
using Ceres.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile:Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            //BomFoodMainViewModel →→→ BOM_Food_Main
            //CreateMap<BomFoodMainViewModel, BOM_Food_Main>()
            //    .ForMember(d => d.Classify, s => s.Ignore())
            //    .ForMember(d => d.Type, s => s.Ignore())
            //    .ForMember(d => d.Code, s => s.Ignore())
            //    .ForMember(d => d.Coding, s => s.Ignore())
            //    .ForMember(d => d.Image, s => s.Ignore())
            //    .ForMember(d => d.Click, s => s.Ignore())
            //    .ForMember(d => d.Status, s => s.Ignore())
            //    ;

            //CreateMap<SupporterViewModel, Supporter>();

            //CreateMap<SupporterViewModel, GetAllSupporterCommand>();

            CreateMap<UpdateSupporterPasswordRequest, UpdateSupporterPasswordCommand>()
                .ConvertUsing(c => new UpdateSupporterPasswordCommand(c.OID, c.OldPassword, c.NewPassword));

            CreateMap<CreateOneCustomerRequest, CreateOneCustomerCommand>()
                .ConvertUsing(c => new CreateOneCustomerCommand(c.OID, c.UserName,c.Sex,c.Age,c.Province,c.City,c.InitHeight,c.InitWeight,c.AgenterOid,c.SupporterOid,c.SupporterOid,c.JobName,c.JobStrength,c.ServiceOid));

            CreateMap<CreateOneCustomerDietRequest, CreateOneCustomerDietCommand>()
                .ConvertUsing(c => new CreateOneCustomerDietCommand(c.DietOid,c.CustomerOid, c.ServiceOid, new Recommend(c.RecommendDailyEnergy, c.RecommendDailyComponentPercentage,c.RecommendDailyFoodComponent),new Current(c.CurrentDailyEnergy,c.CurrentDailyComponentPercentage), c.CurrentDiet, c.SupporterOid));

            CreateMap<DeleteOneCustomerDietRequest, DeleteOneCustomerDietCommand>()
                .ConvertUsing(c => new DeleteOneCustomerDietCommand(c.OID,new Discard(c.DiscardReason),
                              new SupportOperate(c.LastOperateOid,DateTime.Now)
                              ));

            CreateMap<ViewModels.DislikeFood, Domain.Models.CustomerDislikeFood>()
                .ConvertUsing(c=>new CustomerDislikeFood(c.FoodOid));

            CreateMap<CreateOneCustomerAssistDingRequest, CreateOneCustomerAssistDingCommand>()
                .ConvertUsing(c => new CreateOneCustomerAssistDingCommand(c.CustomerOid,c.SupporterOid,c.AssistDate));

            CreateMap<CreateOneCustomerDingRequest, CreateOneCustomerDingCommand>()
                .ConvertUsing(c => new CreateOneCustomerDingCommand(c.CustomerOid));

            CreateMap<ViewModels.MiddleDing, Domain.Commands.MiddleDing>()
                .ConvertUsing(c =>new Domain.Commands.MiddleDing(c.QuestionOID,c.AnswerContent));

            CreateMap<DeleteOneCustomerAssistDingRequest, DeleteOneCustomerAssistDingCommand>()
                .ConvertUsing(c => new DeleteOneCustomerAssistDingCommand(c.FirstAnswerGuid));

            CreateMap<CreateOneCustomerDislikeFoodRequest, CreateOneCustomerDislikeFoodCommand>()
                .ConvertUsing(c => new CreateOneCustomerDislikeFoodCommand(c.CustomerOid,c.DislikeFoodList,c.OperaterOid));

            CreateMap<DeleteOneCustomerDislikeFoodRequest, DeleteOneCustomerDislikeFoodCommand>()
                .ConvertUsing(c => new DeleteOneCustomerDislikeFoodCommand(c.CustomerOid, c.DislikeFoodList));

            CreateMap<CreateOneWeChatAuthorizeRequest, CreateOneWeChatAuthorizeCommand>()
                .ConvertUsing(c => new CreateOneWeChatAuthorizeCommand(c.OID, c.Code2Session));

            CreateMap<UpdateWeChatAuthorizeRequest, UpdateWeChatAuthorizeCommand>()
                .ConvertUsing(c => new UpdateWeChatAuthorizeCommand(c.RandomString, c.EncryptedData,c.IV,c.PhoneJson));

        }
    }
}
