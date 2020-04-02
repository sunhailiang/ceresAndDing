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
    public class FoodAppService:IFoodAppService
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IFoodComponentRepository _foodComponentRepository;
        private readonly IComponentRepository _componentRepository;
        private readonly ICustomerDislikeFoodRepository _customerDislikeFoodRepository;
        //DTO
        private readonly IMapper _mapper;
        //中介者 总线
        private readonly IMediatorHandler Bus;
        public FoodAppService(
            IFoodRepository foodRepository,
            IFoodComponentRepository foodComponentRepository,
            IComponentRepository componentRepository,
            ICustomerDislikeFoodRepository customerDislikeFoodRepository,
            IMapper mapper,
            IMediatorHandler bus
            )
        {
            _foodRepository = foodRepository;
            _foodComponentRepository = foodComponentRepository;
            _componentRepository = componentRepository;
            _customerDislikeFoodRepository = customerDislikeFoodRepository;
            _mapper = mapper;
            Bus = bus;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public QueryFoodListByPageResponse QueryFoodListByPage(string foodName, float foodValue, int pageIndex, int pageSize)
        {
            QueryFoodListByPageResponse result;
            try
            {
                //依据条件查询食材列表
                var existingFoodList = _foodRepository.QueryFoodListByPage(foodName, pageIndex, pageSize);

                //查询食材列表的总条数
                var existingFoodListCount = _foodRepository.QueryFoodListCount(foodName);

                //映射为返回类型
                result = new QueryFoodListByPageResponse();
                result.FoodList = new PageModel<FoodByPage>();
                result.FoodList.Data = _mapper.Map<List<FoodByPage>>(existingFoodList);

                //组合页码数据
                result.FoodList.PageIndex = pageIndex + 1;//页码索引
                result.FoodList.PageSize = pageSize;//每页大小
                result.FoodList.DataCount = existingFoodListCount;//数据总数
                result.FoodList.PageCount = Convert.ToInt32(Math.Ceiling(existingFoodListCount * 1.0d / pageSize));//总页数

                //增加能量，3大营养素数据
                int i = 0;
                foreach (var existingFood in result.FoodList.Data)
                {
                    result.FoodList.Data[i].Value = foodValue;
                    result.FoodList.Data[i].Unit = "g";

                    //获取Food所对应的Component列表
                    var existingFoodComponentList = _foodComponentRepository.GetFoodComponentByFoodOid(existingFood.OID);
                    //增加营养元素的数据
                    List<FoodComponent> foodComponentList = new List<FoodComponent>();
                    foreach (var existingFoodComponent in existingFoodComponentList)
                    {
                        var existingComponent = _mapper.Map<FoodComponent>(_componentRepository.GetById(existingFoodComponent.ComponentOid));
                        existingComponent.Value = (existingFoodComponent.Value * foodValue) / 100f;
                        foodComponentList.Add(existingComponent);
                    }
                    existingFood.FoodComponentList = foodComponentList;
                    existingFood.ID = (pageIndex * pageSize) + i + 1;
                    i++;
                }
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public QueryFoodListWithDislikeFlagByPageResponse QueryFoodListWithDislikeFlagByPage(Guid oid, string foodName, int pageIndex, int pageSize)
        {
            QueryFoodListWithDislikeFlagByPageResponse result;
            try
            {
                //依据条件查询食材列表
                var existingFoodList = _foodRepository.QueryFoodListByPage(foodName, pageIndex, pageSize);

                //查询食材列表的总条数
                var existingFoodListCount = _foodRepository.QueryFoodListCount(foodName);

                //映射为返回类型
                result = new QueryFoodListWithDislikeFlagByPageResponse();
                result.FoodList = new PageModel<FoodWithDislikeFlagByPage>();
                result.FoodList.Data = _mapper.Map<List<FoodWithDislikeFlagByPage>>(existingFoodList);

                //组合页码数据
                result.FoodList.PageIndex = pageIndex + 1;//页码索引
                result.FoodList.PageSize = pageSize;//每页大小
                result.FoodList.DataCount = existingFoodListCount;//数据总数
                result.FoodList.PageCount = Convert.ToInt32(Math.Ceiling(existingFoodListCount * 1.0d / pageSize));//总页数

                //增加客户是否喜欢标识
                int i = 0;
                foreach (var existingFood in result.FoodList.Data)
                {
                    var dislikeFood = _customerDislikeFoodRepository.GetCustomerDislikeFood(oid, existingFood.OID);
                    if(dislikeFood!=null)
                    {
                        existingFood.CustomerDislikeStatus = -1;
                        existingFood.CustomerDislikeDescription = "客户不喜欢";
                    }
                    existingFood.ID= (pageIndex * pageSize) + i + 1;
                    i++;
                }
            }
            catch(Exception)
            {
                return null;
            }
            return result;
        }

        public QueryDislikeFoodListByPageResponse QueryDislikeFoodListByPage(Guid oid, int pageIndex, int pageSize)
        {
            QueryDislikeFoodListByPageResponse result;
            try
            {
                //依据条件查询食材列表
                var dislikeFoodList = _customerDislikeFoodRepository.QueryCustomerDislikeFoodListByPage(oid, pageIndex, pageSize);

                //查询食材列表的总条数
                var dislikeFoodListCount = _customerDislikeFoodRepository.QueryCustomerDislikeFoodCount(oid);


                //映射为返回类型
                result = new QueryDislikeFoodListByPageResponse();
                result.DislikeFoodList = new PageModel<DislikeFoodByPage>();
                result.DislikeFoodList.Data = _mapper.Map<List<DislikeFoodByPage>>(dislikeFoodList);

                //组合页码数据
                result.DislikeFoodList.PageIndex = pageIndex + 1;//页码索引
                result.DislikeFoodList.PageSize = pageSize;//每页大小
                result.DislikeFoodList.DataCount = dislikeFoodListCount;//数据总数
                result.DislikeFoodList.PageCount = Convert.ToInt32(Math.Ceiling(dislikeFoodListCount * 1.0d / pageSize));//总页数

                //增加食材名称
                int i = 0;
                foreach (var dislikeFood in result.DislikeFoodList.Data)
                {
                    var food=_foodRepository.GetById(dislikeFood.OID);

                    dislikeFood.Name = food.Name;
                    dislikeFood.ID = (pageIndex * pageSize) + i + 1;
                    i++;
                }
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public void CreateOneCustomerDislikeFood(CreateOneCustomerDislikeFoodRequest request)
        {
            var createOneCustomerDislikeFoodCommand = _mapper.Map<CreateOneCustomerDislikeFoodCommand>(request);
            Bus.SendCommand(createOneCustomerDislikeFoodCommand);
        }

        public void DeleteOneCustomerDislikeFood(DeleteOneCustomerDislikeFoodRequest request)
        {
            var deleteOneCustomerDislikeFoodCommand = _mapper.Map<DeleteOneCustomerDislikeFoodCommand>(request);
            Bus.SendCommand(deleteOneCustomerDislikeFoodCommand);
        }
    }
}
