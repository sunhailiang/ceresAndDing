using AutoMapper;
using Ceres.Application.Interfaces;
using Ceres.Application.ViewModels;
using Ceres.Domain.Commands;
using Ceres.Domain.Core.Bus;
using Ceres.Domain.Core.Notifications;
using Ceres.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ceres.Application.Services
{
    public class AgenterAppService : IAgenterAppService
    {
        private readonly IAgenterRepository _agenterRepository;
        //DTO
        private readonly IMapper _mapper;
        //中介者 总线
        private readonly IMediatorHandler Bus;

        public AgenterAppService(
            IAgenterRepository agenterRepository,
            IMapper mapper,
            IMediatorHandler bus
            )
        {
            _agenterRepository = agenterRepository;
            _mapper = mapper;
            Bus = bus;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IEnumerable<GetAgenterListResponse> GetAgenterList()
        {
            try
            {
                var agenterList = _agenterRepository.GetAllValidAgenters();
                return _mapper.Map<IEnumerable<GetAgenterListResponse>>(agenterList);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<GetAgenterListResponse> QueryAgenterList(string agenterName)
        {
            try
            {
                var agenterList = _agenterRepository.QueryAgenterList(agenterName);
                return _mapper.Map<IEnumerable<GetAgenterListResponse>>(agenterList);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
