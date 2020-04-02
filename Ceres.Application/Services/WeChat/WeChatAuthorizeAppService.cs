using AutoMapper;
using Ceres.Application.Interfaces;
using Ceres.Application.ViewModels;
using Ceres.Domain.Commands;
using Ceres.Domain.Core.Bus;
using Ceres.Domain.Core.Notifications;
using Ceres.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Ceres.Application.Services
{
    public class WeChatAuthorizeAppService:IWeChatAuthorizeAppService
    {
        private readonly IWeChatAuthorizeRepository _weChatAuthorizeRepository;
        //DTO
        private readonly IMapper _mapper;
        //中介者 总线
        private readonly IMediatorHandler Bus;
        public WeChatAuthorizeAppService(
            IWeChatAuthorizeRepository weChatAuthorizeRepository,
            IMapper mapper,
            IMediatorHandler bus
            )
        {
            _weChatAuthorizeRepository = weChatAuthorizeRepository;
            _mapper = mapper;
            Bus = bus;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void CreateOneWeChatAuthorize(CreateOneWeChatAuthorizeRequest request)
        {
            var createOneWeChatAuthorizeCommand= _mapper.Map<CreateOneWeChatAuthorizeCommand>(request);
            Bus.SendCommand(createOneWeChatAuthorizeCommand);
        }

        public GetOneWeChatAuthorizeResponse GetValidChatAuthorize(Guid oid)
        {
            GetOneWeChatAuthorizeResponse result;
            try
            {
                var existingWeChatAuthorize = _weChatAuthorizeRepository.GetValidWeChatAuthorize(oid);
                if(existingWeChatAuthorize==null)
                {
                    return null;
                }

                result=_mapper.Map<GetOneWeChatAuthorizeResponse>(existingWeChatAuthorize);
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public void UpdateWeChatAuthorize(UpdateWeChatAuthorizeRequest request)
        {
            var updateWeChatAuthorizeCommand=_mapper.Map<UpdateWeChatAuthorizeCommand>(request);
            Bus.SendCommand(updateWeChatAuthorizeCommand);
        }
    }
}
