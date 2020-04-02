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
    public class ServiceAppService : IServiceAppService
    {
        private readonly IServiceRepository _serviceRepository;
        //DTO
        private readonly IMapper _mapper;
        //中介者 总线
        private readonly IMediatorHandler Bus;
        public ServiceAppService(
            IServiceRepository serviceRepository,
            IMapper mapper,
            IMediatorHandler bus
            )
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
            Bus = bus;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IEnumerable<GetServiceListResponse> GetServiceList()
        {
            IEnumerable<GetServiceListResponse> result;
            try
            {
                var serviceList = _serviceRepository.GetAllValidServices();
                result = _mapper.Map<IEnumerable<GetServiceListResponse>>(serviceList);
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }
    }
}
