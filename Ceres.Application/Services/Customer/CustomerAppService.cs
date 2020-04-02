using AutoMapper;
using Ceres.Application.Interfaces;
using Ceres.Application.ViewModels;
using Ceres.Domain.Commands;
using Ceres.Domain.Core.Bus;
using Ceres.Domain.Core.Notifications;
using Ceres.Domain.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ceres.Application.Services
{
    public class CustomerAppService : ICustomerAppService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly ISupporterRepository _supporterRepository;
        private readonly IAgenterRepository _agenterRepository;
        private readonly ICustomerJobRepository _customerJobRepository;
        private readonly IAnswerRepository _answerRepository;
        //DTO
        private readonly IMapper _mapper;
        //中介者 总线
        private readonly IMediatorHandler Bus;
        public CustomerAppService(
            ICustomerRepository customerRepository,
            IServiceRepository serviceRepository,
            ISupporterRepository supporterRepository,
            IAgenterRepository agenterRepository,
            ICustomerJobRepository customerJobRepository,
            IAnswerRepository answerRepository,
            IMapper mapper,
            IMediatorHandler bus
            )
        {
            _customerRepository = customerRepository;
            _serviceRepository = serviceRepository;
            _supporterRepository = supporterRepository;
            _agenterRepository = agenterRepository;
            _customerJobRepository = customerJobRepository;
            _answerRepository = answerRepository;
            _mapper = mapper;
            Bus = bus;
        }

        public void CreateOneCustomer(CreateOneCustomerRequest request)
        {
            var createOneCustomerCommand=_mapper.Map<CreateOneCustomerCommand>(request);
            Bus.SendCommand(createOneCustomerCommand);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public GetOneCustomerBasicInformationResponse GetOneCustomerBasicInformation(Guid oid)
        {
            GetOneCustomerBasicInformationResponse result;
            try
            {
                //依据条件查找客户基本信息
                var existingCustomer = _customerRepository.GetById(oid);
                if(existingCustomer==null)
                {
                    return null;
                }

                //映射为返回类型
                result = _mapper.Map<GetOneCustomerBasicInformationResponse>(existingCustomer);

                //增加服务信息
                var service = _serviceRepository.GetServiceByCustomerOid(existingCustomer.OID);
                if(service!=null)
                {
                    result.ServiceOid = service.OID;
                    result.ServiceName = service.Name;
                }

                //增加归属客服信息
                var supporter = _supporterRepository.GetById(existingCustomer.SupporterOid);
                if(supporter!=null)
                {
                    result.SupporterName = supporter.UserName;
                }

                //增加归属代理信息
                var agenter = _agenterRepository.GetById(existingCustomer.AgenterOid);
                if(agenter!=null)
                {
                    result.AgenterName = agenter.UserName;
                }

                //增加职业信息
                var customerJob = _customerJobRepository.GetCustomerJobByCustomerOid(existingCustomer.OID);
                if(customerJob!=null)
                {
                    result.JobName = customerJob.Job.Name;
                }

            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public QueryCustomerListByPageResponse QueryCustomerListByPage(int pageIndex, int pageSize)
        {
            QueryCustomerListByPageResponse result;
            try
            {
                //依据条件查询客户列表
                var existingCustomer = _customerRepository.QueryCustomerListByPage(pageIndex, pageSize);

                //查询客户列表的总条数
                var existingCustomerCount = _customerRepository.QueryCustomerListCount();

                //映射为返回类型
                result = new QueryCustomerListByPageResponse();
                result.CustomerList = new PageModel<CustomerByPage>();
                result.CustomerList.Data =_mapper.Map<List<CustomerByPage>>(existingCustomer);

                //组合页码数据
                result.CustomerList.PageIndex = pageIndex + 1;//页码索引
                result.CustomerList.PageSize = pageSize;//每页大小
                result.CustomerList.DataCount = existingCustomerCount;//数据总数
                result.CustomerList.PageCount =Convert.ToInt32(Math.Ceiling(existingCustomerCount*1.0d / pageSize));//总页数
                
                //增加类别，归属客服信息，归属代理信息
                int i = 0;
                foreach (var item in existingCustomer)
                {
                    result.CustomerList.Data[i].ServiceName = _serviceRepository.GetServiceByCustomerOid(item.OID).Name;
                    result.CustomerList.Data[i].SupporterName = _supporterRepository.GetById(item.SupporterOid).UserName;
                    result.CustomerList.Data[i].AgenterName = _agenterRepository.GetById(item.AgenterOid).UserName;
                    result.CustomerList.Data[i].ID = (pageIndex* pageSize) +i+1;
                    i++;
                    
                }

            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public QueryCustomerListByPageResponse QueryCustomerListByPage(string customerName, string cellphone, Guid serviceOid, Guid supporterOid, Guid agenterOid, int pageIndex, int pageSize)
        {
            QueryCustomerListByPageResponse result;
            try
            {
                //依据条件查询客户列表
                var existingCustomer = _customerRepository.QueryCustomerListByPage(customerName, cellphone, serviceOid, supporterOid, agenterOid, pageIndex, pageSize);

                //查询客户列表的总条数
                var existingCustomerCount = _customerRepository.QueryCustomerListCount(customerName, cellphone, serviceOid, supporterOid, agenterOid);

                //映射为返回类型
                result = new QueryCustomerListByPageResponse();
                result.CustomerList = new PageModel<CustomerByPage>();
                result.CustomerList.Data = _mapper.Map<List<CustomerByPage>>(existingCustomer);

                //组合页码数据
                result.CustomerList.PageIndex = pageIndex + 1;//页码索引
                result.CustomerList.PageSize = pageSize;//每页大小
                result.CustomerList.DataCount = existingCustomerCount;//数据总数
                result.CustomerList.PageCount = Convert.ToInt32(Math.Ceiling(existingCustomerCount * 1.0d / pageSize));//总页数

                //增加类别，归属客服信息，归属代理信息
                int i = 0;
                foreach (var item in existingCustomer)
                {
                    result.CustomerList.Data[i].ServiceName = _serviceRepository.GetServiceByCustomerOid(item.OID).Name;
                    result.CustomerList.Data[i].SupporterName = _supporterRepository.GetById(item.SupporterOid).UserName;
                    result.CustomerList.Data[i].AgenterName = _agenterRepository.GetById(item.AgenterOid).UserName;
                    result.CustomerList.Data[i].ID = (pageIndex * pageSize) + i + 1;
                    i++;
                }

            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public GetOneVIPCustomerResponse GetOneVIPCustomer(string cellphone)
        {
            GetOneVIPCustomerResponse result;
            try
            {
                result = new GetOneVIPCustomerResponse();
                result.Cellphone = cellphone;
                //验证用户是否为VIP用户
                var existingCustomer = _customerRepository.GetByCellphone(cellphone);
                result = _mapper.Map<GetOneVIPCustomerResponse>(existingCustomer);
                if(result==null)
                {
                    return null;
                }
                result.IsVip = true;
                result.IsVipDescription = "当前用户是VIP，可以进行打卡";

                //查询用户上一次打卡的身高数据
                //B180EAC0-127D-4ECB-BDB6-C2599D310BD4  为身高的GUID
                var heightDing = _answerRepository.QueryMecuryAnswerList(existingCustomer.OID, Guid.Parse("B180EAC0-127D-4ECB-BDB6-C2599D310BD4")).FirstOrDefault();
                if(heightDing!=null)
                {
                    //兼容之前各种非法字符
                    var tempAnswer = JsonConvert.DeserializeObject(heightDing.Content).ToString();
                    var convertFloat = 0.0f;
                    if(Single.TryParse(tempAnswer, out convertFloat)==true)
                    {
                        result.DefaultHeight = convertFloat;
                    }
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
