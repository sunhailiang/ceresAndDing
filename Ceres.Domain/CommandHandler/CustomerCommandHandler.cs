using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Ceres.Domain.Commands;
using System.Threading.Tasks;
using System.Threading;
using Ceres.Domain.Interfaces;
using Ceres.Domain.Core.Bus;
using Microsoft.Extensions.Caching.Memory;
using Ceres.Domain.Models;
using Ceres.Domain.Core.Notifications;

namespace Ceres.Domain.CommandHandler
{
    public class CustomerCommandHandler : CommandHandler,
        IRequestHandler<CreateOneCustomerCommand, Unit>
    {
        // 注入仓储接口
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserInformationRepository _userInformationRepository;
        private readonly ICustomerServiceRepository _customerServiceRepository;
        private readonly ICustomerJobRepository _customerJobRepository;
        // 注入总线
        private readonly IMediatorHandler Bus;
        private IMemoryCache Cache;

        public CustomerCommandHandler(ICustomerRepository customerRepository,
                                      IUserInformationRepository userInformationRepository,
                                      ICustomerServiceRepository customerServiceRepository,
                                      ICustomerJobRepository customerJobRepository,
                                      IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      IMemoryCache cache
                                      ) : base(uow, bus, cache)
        {
            _customerRepository = customerRepository;
            _userInformationRepository = userInformationRepository;
            _customerServiceRepository = customerServiceRepository;
            _customerJobRepository = customerJobRepository;
            Bus = bus;
            Cache = cache;
        }

        public void Dispose()
        {
            _customerRepository.Dispose();
            _userInformationRepository.Dispose();
            _customerServiceRepository.Dispose();
            _customerJobRepository.Dispose();
        }

        public Task<Unit> Handle(CreateOneCustomerCommand request, CancellationToken cancellationToken)
        {
            // 命令验证
            if (!request.IsValid())
            {
                // 错误信息收集
                NotifyValidationErrors(request);//主要为验证的信息
                // 返回，结束当前线程
                return Task.FromResult(new Unit());
            }

            //依据Oid查询Mecury用户的信息
            var existingOldCustomer = _userInformationRepository.GetUserByUserGuid(request.OID);

            //验证手机号是否已经注册过
            var existingCustomer = _customerRepository.GetByCellphone(existingOldCustomer.PhoneNumber);
            if(existingCustomer != null)
            {
                //引发错误事件
                Bus.RaiseEvent(new DomainNotification("", "用户手机号已注册，无法重复注册！"));
                return Task.FromResult(new Unit());
            }

            //新增客户
            var customerAddress = new CustomerAddress(request.Province, request.City);
            var customer = new Customer(
                request.OID,
                request.UserName,
                request.Sex,
                request.Age,
                customerAddress,
                existingOldCustomer.PhoneNumber,
                request.InitHeight,
                request.InitWeight,
                request.AgenterOid,
                request.SupporterOid,
                request.LastOperaterOid,
                DateTime.Now,
                0
                ) ;

            _customerRepository.Add(customer);

            //新增客户工作信息
            var customerJob = new CustomerJob(Guid.NewGuid(),new Job(request.JobName,request.JobStrength),customer.OID);
            _customerJobRepository.Add(customerJob);

            //新增客户服务信息
            var customerService = new CustomerService(Guid.NewGuid(), customer.OID, request.ServiceOid, 0);
            _customerServiceRepository.Add(customerService);

            if (Commit())
            {
                // 提交成功后，这里可以发布领域事件，比如短信通知
            }
            return Task.FromResult(new Unit());
        }
    }
}
