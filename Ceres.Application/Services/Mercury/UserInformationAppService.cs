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
    public class UserInformationAppService : IUserInformationAppService
    {
        private readonly IUserInformationRepository _userInformationRepository;
        private readonly ICustomerRepository _customerRepository;
        //DTO
        private readonly IMapper _mapper;
        //中介者 总线
        private readonly IMediatorHandler Bus;

        public UserInformationAppService(
            IUserInformationRepository userInformationRepository,
            ICustomerRepository customerRepository,
            IMapper mapper,
            IMediatorHandler bus
            )
        {
            _userInformationRepository = userInformationRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
            Bus = bus;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public GetOneOriginalCustomerResponse GetUserByPhoneNumber(string cellphone)
        {
            GetOneOriginalCustomerResponse result;
            try
            {
                //查询参加Mercury的指定用户
                var originalCustomer = _userInformationRepository.GetUserByPhoneNumber(cellphone);
                result= _mapper.Map<GetOneOriginalCustomerResponse>(originalCustomer);
                if(result==null)
                {
                    return null;
                }

                //查询当前指定用户是否是VIP
                if(_customerRepository.GetById(result.OID)!=null)
                {
                    result.IsVip = true;
                }
                else
                { 
                    result.IsVip = false;
                }
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }
    }
}
