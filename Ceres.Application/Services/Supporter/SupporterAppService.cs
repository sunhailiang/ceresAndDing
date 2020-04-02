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
using System.Text;
using System.Threading.Tasks;

namespace Ceres.Application.Services
{
    public class SupporterAppService : ISupporterAppService
    {
        private readonly ISupporterRepository _supporterRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionnaireRepository _questionnaireRepository;
        //DTO
        private readonly IMapper _mapper;
        //中介者 总线
        private readonly IMediatorHandler Bus;
        public SupporterAppService(
            ISupporterRepository supporterRepository,
            ICustomerRepository customerRepository,
            IServiceRepository serviceRepository,
            IAnswerRepository answerRepository,
            IQuestionnaireRepository questionnaireRepository,
            IMapper mapper,
            IMediatorHandler bus
            )
        {
            _supporterRepository = supporterRepository;
            _customerRepository = customerRepository;
            _serviceRepository = serviceRepository;
            _answerRepository = answerRepository;
            _questionnaireRepository = questionnaireRepository;
            _mapper = mapper;
            Bus = bus;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }



        public IEnumerable<GetSupporterListResponse> GetSupporterList()
        {
            IEnumerable<GetSupporterListResponse> result;
            try
            {
                var supporterList = _supporterRepository.GetAllValidSupporters();
                result= _mapper.Map<IEnumerable<GetSupporterListResponse>>(supporterList);
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public SupporterLoginResponse SupporterCellphoneLogin(SupporterLoginRequest request)
        {
            SupporterLoginResponse result;
            try
            {
                var supporter = _supporterRepository.GetByCellphone(request.LoginID, request.Password);
                result= _mapper.Map<SupporterLoginResponse>(supporter);
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public SupporterLoginResponse SupporterLoginNameLogin(SupporterLoginRequest request)
        {
            SupporterLoginResponse result;
            try
            {
                var supporter = _supporterRepository.GetByLoginName(request.LoginID, request.Password);
                result= _mapper.Map<SupporterLoginResponse>(supporter);
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public void UpdateSupporterPassword(UpdateSupporterPasswordRequest request)
        {
            var updateSupporterPasswordCommand = _mapper.Map<UpdateSupporterPasswordCommand>(request);

            Bus.SendCommand(updateSupporterPasswordCommand);
        }

        public GetOneSupporterTodoDietListByDailyPageResponse GetOneSupporterTodoDietListByDailyPage(Guid supporterOid, DateTime dateTime, int pageIndex, int pageSize)
        {
            GetOneSupporterTodoDietListByDailyPageResponse result;
            try
            {
                //查询指定客服管理的所有客户列表
                var needDoneCustomerList = _customerRepository.QueryOneSupporterCustomerList(supporterOid);

                //查询指定客服管理的所有客户的总条数
                var needDoneCustomerListCount = needDoneCustomerList.Count();

                //依据条件查询每日已经完成食谱列表
                var doneCustomerList = _customerRepository.QueryOneSupporterDoneDietListByDaily(supporterOid, dateTime);

                //查询每日已经完成食谱列表的总条数
                var doneCustomerListCount = doneCustomerList.Count();

                //待办事项
                List<TodoListByPage> todoDietList = new List<TodoListByPage>();

                //代办事项的食谱列表
                foreach (var needDoneCustomer in needDoneCustomerList)
                {
                    var existingDiet = (
                                        from t in doneCustomerList
                                        where(t.OID==needDoneCustomer.OID)
                                        select t);
                    if(existingDiet.Count()<=0)
                    {
                        todoDietList.Add(_mapper.Map<TodoListByPage>(needDoneCustomer));
                    }
                }

                //映射为返回类型
                result = new GetOneSupporterTodoDietListByDailyPageResponse();
                result.TodoDietList = new PageModel<TodoListByPage>();
                result.TodoDietList.Data=todoDietList.Skip(pageIndex * pageSize).Take(pageSize).ToList();

                //增加个人信息
                foreach(var customer in result.TodoDietList.Data)
                {
                    var service = _serviceRepository.GetServiceByCustomerOid(customer.UserOid);
                    customer.ServiceName = service.Name;
                }

                //增加其他信息
                result.TodoDate = dateTime;
                var supporter = _supporterRepository.GetById(supporterOid);
                if(supporter==null)
                {
                    result.SupporterName = "未知客服";
                }
                else
                {
                    result.SupporterName = supporter.UserName;
                }

                //组合页码数据
                result.TodoDietList.PageIndex = pageIndex + 1;//页码索引
                result.TodoDietList.PageSize = pageSize;//每页大小
                result.TodoDietList.DataCount = needDoneCustomerListCount- doneCustomerListCount;//数据总数
                result.TodoDietList.PageCount = Convert.ToInt32(Math.Ceiling((needDoneCustomerListCount - doneCustomerListCount) * 1.0d / pageSize));//总页数

            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public GetOneSupporterTodoDietListCountByDailyResponse GetOneSupporterTodoDietListCountByDaily(Guid supporterOid, DateTime dateTime)
        {
            GetOneSupporterTodoDietListCountByDailyResponse result;
            try
            {
                //查询指定客服管理的所有客户列表
                var needDoneCustomerList = _customerRepository.QueryOneSupporterCustomerList(supporterOid);

                //查询指定客服管理的所有客户的总条数
                var needDoneCustomerListCount = needDoneCustomerList.Count();

                //依据条件查询每日已经完成食谱列表
                var doneCustomerList = _customerRepository.QueryOneSupporterDoneDietListByDaily(supporterOid, dateTime);

                //查询每日已经完成食谱列表的总条数
                var doneCustomerListCount = doneCustomerList.Count();

                //映射为返回类型
                result = new GetOneSupporterTodoDietListCountByDailyResponse();
                result.TodoDietCount = needDoneCustomerListCount - doneCustomerListCount;

                //增加其他信息
                result.TodoDate = dateTime;
                var supporter = _supporterRepository.GetById(supporterOid);
                if (supporter == null)
                {
                    result.SupporterName = "未知客服";
                }
                else
                {
                    result.SupporterName = supporter.UserName;
                }

            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public GetOneSupporterTodoDingListByDailyPageResponse GetOneSupporterTodoDingListByDailyPage(Guid supporterOid, DateTime dateTime, int pageIndex, int pageSize)
        {
            GetOneSupporterTodoDingListByDailyPageResponse result;
            try
            {
                //查询指定客服管理的所有客户列表
                var needDoneCustomerList = _customerRepository.QueryOneSupporterCustomerList(supporterOid);

                //查询指定客服管理的所有客户的总条数
                var needDoneCustomerListCount = needDoneCustomerList.Count();

                //依据条件查询每日已经完成打卡列表--以第一个问题为基准
                //查询所有问题
                var questionaire = _questionnaireRepository.GetById(Guid.Parse("AA46576C-9FD5-4CB9-87B1-CCEDBF68A92D"));//打卡问题集
                if (questionaire == null)
                {
                    return null;
                }
                Guid[] questionGuids = JsonConvert.DeserializeObject<Guid[]>(questionaire.Question);
                if (questionGuids == null)
                {
                    return null;
                }

                //以第一条数据为基准判断一个打卡数据
                var doneCustomerList = _customerRepository.QueryOneSupporterDoneDingListByDaily(supporterOid, questionGuids[0], dateTime);

                //查询每日已经完成食谱列表的总条数
                var doneCustomerListCount = doneCustomerList.Count();

                //待办事项
                List<TodoListByPage> todoDingList = new List<TodoListByPage>();

                //代办事项的食谱列表
                foreach (var needDoneCustomer in needDoneCustomerList)
                {
                    var existingDiet = (
                                        from t in doneCustomerList
                                        where (t.OID == needDoneCustomer.OID)
                                        select t);
                    if (existingDiet.Count() <= 0)
                    {
                        todoDingList.Add(_mapper.Map<TodoListByPage>(needDoneCustomer));
                    }
                }

                //映射为返回类型
                result = new GetOneSupporterTodoDingListByDailyPageResponse();
                result.TodoDingList = new PageModel<TodoListByPage>();
                result.TodoDingList.Data = todoDingList.Skip(pageIndex * pageSize).Take(pageSize).ToList();

                //增加个人信息
                foreach (var customer in result.TodoDingList.Data)
                {
                    var service = _serviceRepository.GetServiceByCustomerOid(customer.UserOid);
                    customer.ServiceName = service.Name;
                }

                //增加其他信息
                result.TodoDate = dateTime;
                var supporter = _supporterRepository.GetById(supporterOid);
                if (supporter == null)
                {
                    result.SupporterName = "未知客服";
                }
                else
                {
                    result.SupporterName = supporter.UserName;
                }

                //组合页码数据
                result.TodoDingList.PageIndex = pageIndex + 1;//页码索引
                result.TodoDingList.PageSize = pageSize;//每页大小
                result.TodoDingList.DataCount = needDoneCustomerListCount - doneCustomerListCount;//数据总数
                result.TodoDingList.PageCount = Convert.ToInt32(Math.Ceiling((needDoneCustomerListCount - doneCustomerListCount) * 1.0d / pageSize));//总页数

            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }

        public GetOneSupporterTodoDingListCountByDailyResponse GetOneSupporterTodoDingListCountByDaily(Guid supporterOid, DateTime dateTime)
        {
            GetOneSupporterTodoDingListCountByDailyResponse result;
            try
            {
                //查询指定客服管理的所有客户列表
                var needDoneCustomerList = _customerRepository.QueryOneSupporterCustomerList(supporterOid);

                //查询指定客服管理的所有客户的总条数
                var needDoneCustomerListCount = needDoneCustomerList.Count();

                //依据条件查询每日已经完成打卡列表--以第一个问题为基准
                //查询所有问题
                var questionaire = _questionnaireRepository.GetById(Guid.Parse("AA46576C-9FD5-4CB9-87B1-CCEDBF68A92D"));//打卡问题集
                if (questionaire == null)
                {
                    return null;
                }
                Guid[] questionGuids = JsonConvert.DeserializeObject<Guid[]>(questionaire.Question);
                if (questionGuids == null)
                {
                    return null;
                }

                //以第一条数据为基准判断一个打卡数据
                var doneCustomerList = _customerRepository.QueryOneSupporterDoneDingListByDaily(supporterOid, questionGuids[0], dateTime);

                //查询每日已经完成打卡列表的总条数
                var doneCustomerListCount = doneCustomerList.Count();

                //映射为返回类型
                result = new GetOneSupporterTodoDingListCountByDailyResponse();
                result.TodoDingCount = needDoneCustomerListCount - doneCustomerListCount;

                //增加其他信息
                result.TodoDate = dateTime;
                var supporter = _supporterRepository.GetById(supporterOid);
                if (supporter == null)
                {
                    result.SupporterName = "未知客服";
                }
                else
                {
                    result.SupporterName = supporter.UserName;
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
