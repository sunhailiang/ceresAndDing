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
    public class DietAppService : IDietAppService
    {
        private readonly ICustomerDietRepository _customerDietRepository;
        private readonly ISupporterRepository _supporterRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IServiceRepository _serviceRepository;
        //DTO
        private readonly IMapper _mapper;
        //中介者 总线
        private readonly IMediatorHandler Bus;
        public DietAppService(
            ICustomerDietRepository customerDietRepository,
            ISupporterRepository supporterRepository,
            ICustomerRepository customerRepository,
            ICustomerServiceRepository customerServiceRepository,
            IServiceRepository serviceRepository,
            IMapper mapper,
            IMediatorHandler bus
            )
        {
            _customerDietRepository = customerDietRepository;
            _supporterRepository = supporterRepository;
            _customerRepository = customerRepository;
            _serviceRepository = serviceRepository;
            _mapper = mapper;
            Bus = bus;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void CreateOneCustomerDiet(CreateOneCustomerDietRequest request)
        {
            var createOneCustomerDietCommand = _mapper.Map<CreateOneCustomerDietCommand>(request);
            Bus.SendCommand(createOneCustomerDietCommand);
        }

        public QueryOneCustomerDietListByPageResponse QueryOneCustomerDietListByPage(Guid customerOid, int pageIndex, int pageSize)
        {
            QueryOneCustomerDietListByPageResponse result;
            try
            {
                //依据条件查询食谱列表
                var existingDietList = _customerDietRepository.QueryCustomerDietListByPage(customerOid, pageIndex, pageSize);

                //查询食谱列表的总条数
                var existingDietCount = _customerDietRepository.QueryCustomerDietListCount(customerOid);

                //映射为返回类型
                result = new QueryOneCustomerDietListByPageResponse();
                result.DietList = new PageModel<OneCustomerDietByPage>();
                result.DietList.Data= _mapper.Map<List<OneCustomerDietByPage>>(existingDietList);

                //补齐其余部分
                var customer = _customerRepository.GetById(customerOid);
                if(customer==null)
                {
                    return result;
                }
                result.UserName = customer.UserName;
                result.Cellphone = customer.Cellphone;

                var service = _serviceRepository.GetServiceByCustomerOid(customerOid);
                if(service != null)
                {
                    result.ServiceName = service.Name;
                }

                //组合页码数据
                result.DietList.PageIndex = pageIndex + 1;//页码索引
                result.DietList.PageSize = pageSize;//每页大小
                result.DietList.DataCount = existingDietCount;//数据总数
                result.DietList.PageCount = Convert.ToInt32(Math.Ceiling(existingDietCount * 1.0d / pageSize));//总页数

                //增加类别，归属客服信息，归属代理信息
                int i = 0;
                foreach (var item in existingDietList)
                {
                    //supporter
                    var supporter=_supporterRepository.GetById(item.SupporterOid);
                    if(supporter!=null)
                    {
                        result.DietList.Data[i].SupporterName = supporter.UserName;
                    }
                    else
                    {
                        result.DietList.Data[i].SupporterName = "未知";
                    }

                    //status
                    if(result.DietList.Data[i].Status==0)
                    {
                        result.DietList.Data[i].StatusDescription = "正常";
                    }
                    else if(result.DietList.Data[i].Status == -1)
                    {
                        result.DietList.Data[i].StatusDescription = "已删除";
                    }

                    //lastOperater
                    var operater = _supporterRepository.GetById(item.LastOperate.Oid);
                    if (operater != null)
                    {
                        result.DietList.Data[i].LastOperater = operater.UserName;
                    }
                    else
                    {
                        result.DietList.Data[i].LastOperater = "未知";
                    }
                    result.DietList.Data[i].LatOperateTime = item.LastOperate.Time;

                    result.DietList.Data[i].ID = (pageIndex * pageSize) + i + 1;
                    i++;
                }
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public QueryDietListByPageResponse QueryDietListByPage(int pageIndex, int pageSize)
        {
            QueryDietListByPageResponse result;
            try
            {
                //依据条件查询食谱列表
                var existingDietList = _customerDietRepository.QueryDietListByPage(pageIndex, pageSize);

                //查询食谱列表的总条数
                var existingDietCount = _customerDietRepository.QueryDietListCount();

                //映射为返回类型
                result = new QueryDietListByPageResponse();
                result.DietList = new PageModel<DietByPage>();
                result.DietList.Data = _mapper.Map<List<DietByPage>>(existingDietList);

                //组合页码数据
                result.DietList.PageIndex = pageIndex + 1;//页码索引
                result.DietList.PageSize = pageSize;//每页大小
                result.DietList.DataCount = existingDietCount;//数据总数
                result.DietList.PageCount = Convert.ToInt32(Math.Ceiling(existingDietCount * 1.0d / pageSize));//总页数

                //增加其他信息
                int i = 0;
                foreach (var item in existingDietList)
                {
                    //customer
                    var customer = _customerRepository.GetById(item.CustomerOid);
                    if (customer != null)
                    {
                        result.DietList.Data[i].UserName = customer.UserName;
                        result.DietList.Data[i].Cellphone = customer.Cellphone;
                    }
                    else
                    {
                        result.DietList.Data[i].UserName = "未知";
                    }

                    //service
                    var service = _serviceRepository.GetServiceByCustomerOid(item.CustomerOid);
                    if(service!=null)
                    {
                        result.DietList.Data[i].ServiceName = service.Name;
                    }
                    else
                    {
                        result.DietList.Data[i].ServiceName = "未知";
                    }

                    //supporter
                    var supporter = _supporterRepository.GetById(item.SupporterOid);
                    if (supporter != null)
                    {
                        result.DietList.Data[i].SupporterName = supporter.UserName;
                    }
                    else
                    {
                        result.DietList.Data[i].SupporterName = "未知";
                    }

                    //status
                    if (result.DietList.Data[i].Status == 0)
                    {
                        result.DietList.Data[i].StatusDescription = "正常";
                    }
                    else if (result.DietList.Data[i].Status == -1)
                    {
                        result.DietList.Data[i].StatusDescription = "已删除";
                    }

                    //lastOperater
                    var operater = _supporterRepository.GetById(item.LastOperate.Oid);
                    if (operater != null)
                    {
                        result.DietList.Data[i].LastOperater = operater.UserName;
                    }
                    else
                    {
                        result.DietList.Data[i].LastOperater = "未知";
                    }
                    result.DietList.Data[i].LatOperateTime = item.LastOperate.Time;

                    result.DietList.Data[i].ID = (pageIndex * pageSize) + i + 1;
                    i++;
                }
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public QueryDietListByPageResponse QueryDietListByPage(string customerName, string cellphone, Guid serviceOid, Guid supporterOid, DateTime startTime, DateTime endTime, int pageIndex, int pageSize)
        {
            QueryDietListByPageResponse result;
            try
            {
                //依据条件查询食谱列表
                var existingDietList = _customerDietRepository.QueryCustomerDietListByPage(customerName, cellphone, serviceOid, supporterOid, startTime, endTime, pageIndex, pageSize);

                //查询食谱列表的总条数
                var existingDietCount = _customerDietRepository.QueryCustomerDietListCount(customerName, cellphone, serviceOid, supporterOid, startTime, endTime);

                //映射为返回类型
                result = new QueryDietListByPageResponse();
                result.DietList = new PageModel<DietByPage>();
                result.DietList.Data = _mapper.Map<List<DietByPage>>(existingDietList);

                //组合页码数据
                result.DietList.PageIndex = pageIndex + 1;//页码索引
                result.DietList.PageSize = pageSize;//每页大小
                result.DietList.DataCount = existingDietCount;//数据总数
                result.DietList.PageCount = Convert.ToInt32(Math.Ceiling(existingDietCount * 1.0d / pageSize));//总页数

                //增加其他信息
                int i = 0;
                foreach (var item in existingDietList)
                {
                    //customer
                    var customer = _customerRepository.GetById(item.CustomerOid);
                    if (customer != null)
                    {
                        result.DietList.Data[i].UserName = customer.UserName;
                        result.DietList.Data[i].Cellphone = customer.Cellphone;
                    }
                    else
                    {
                        result.DietList.Data[i].UserName = "未知";
                    }

                    //service
                    var service = _serviceRepository.GetServiceByCustomerOid(item.CustomerOid);
                    if (service != null)
                    {
                        result.DietList.Data[i].ServiceName = service.Name;
                    }
                    else
                    {
                        result.DietList.Data[i].ServiceName = "未知";
                    }

                    //supporter
                    var supporter = _supporterRepository.GetById(item.SupporterOid);
                    if (supporter != null)
                    {
                        result.DietList.Data[i].SupporterName = supporter.UserName;
                    }
                    else
                    {
                        result.DietList.Data[i].SupporterName = "未知";
                    }

                    //status
                    if (result.DietList.Data[i].Status == 0)
                    {
                        result.DietList.Data[i].StatusDescription = "正常";
                    }
                    else if (result.DietList.Data[i].Status == -1)
                    {
                        result.DietList.Data[i].StatusDescription = "已删除";
                    }

                    //lastOperater
                    var operater = _supporterRepository.GetById(item.LastOperate.Oid);
                    if (operater != null)
                    {
                        result.DietList.Data[i].LastOperater = operater.UserName;
                    }
                    else
                    {
                        result.DietList.Data[i].LastOperater = "未知";
                    }
                    result.DietList.Data[i].LatOperateTime = item.LastOperate.Time;

                    result.DietList.Data[i].ID = (pageIndex * pageSize) + i + 1;
                    i++;
                }
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public void DeleteOneCustomerDiet(DeleteOneCustomerDietRequest request)
        {

            var deleteOneCustomerDietCommand = _mapper.Map<DeleteOneCustomerDietCommand>(request);
            if (request.DislikeList!=null&&request.DislikeList.Count > 0)
            {
                deleteOneCustomerDietCommand.DislikeList = new List<Domain.Models.CustomerDislikeFood>();
                deleteOneCustomerDietCommand.DislikeList = _mapper.Map<List<Domain.Models.CustomerDislikeFood>>(request.DislikeList);
            }
            Bus.SendCommand(deleteOneCustomerDietCommand);
        }
    }
}
