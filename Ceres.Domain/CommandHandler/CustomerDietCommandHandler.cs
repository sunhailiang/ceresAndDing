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
    public class CustomerDietCommandHandler : CommandHandler,
        IRequestHandler<CreateOneCustomerDietCommand, Unit>,
        IRequestHandler<DeleteOneCustomerDietCommand, Unit>,
        IRequestHandler<CreateOneCustomerDislikeFoodCommand, Unit>,
        IRequestHandler<DeleteOneCustomerDislikeFoodCommand, Unit>
    {
        // 注入仓储接口
        private readonly ICustomerDietRepository _customerDietRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerDislikeFoodRepository _customerDislikeFoodRepository;
        // 注入总线
        private readonly IMediatorHandler Bus;
        private IMemoryCache Cache;

        public CustomerDietCommandHandler(ICustomerDietRepository customerDietRepository,
                                      ICustomerRepository customerRepository,
                                      ICustomerDislikeFoodRepository customerDislikeFoodRepository,
                                      IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      IMemoryCache cache
                                      ) : base(uow, bus, cache)
        {
            _customerDietRepository = customerDietRepository;
            _customerRepository = customerRepository;
            _customerDislikeFoodRepository = customerDislikeFoodRepository;
            Bus = bus;
            Cache = cache;
        }

        public void Dispose()
        {
            _customerDietRepository.Dispose();
            _customerRepository.Dispose();
            _customerDislikeFoodRepository.Dispose();
        }

        public Task<Unit> Handle(CreateOneCustomerDietCommand request, CancellationToken cancellationToken)
        {
            // 命令验证
            if (!request.IsValid())
            {
                // 错误信息收集
                NotifyValidationErrors(request);//主要为验证的信息
                // 返回，结束当前线程
                return Task.FromResult(new Unit());
            }

            //判断客户是否存在
            var existingCustomer = _customerRepository.GetById(request.CustomerOid);
            if(existingCustomer==null)
            {
                //引发错误事件
                Bus.RaiseEvent(new DomainNotification("", "非VIP客户，无法保存食谱"));
                return Task.FromResult(new Unit());
            }

            //每天给同一个客户，只能推荐5条食谱
            var customerDietCount =_customerDietRepository.QueryCustomerDietCountOnTheSameDayByCustomerOid(request.CustomerOid, DateTime.Now);
            if(customerDietCount>=5)
            {
                //引发错误事件
                Bus.RaiseEvent(new DomainNotification("", "每天给同一个客户，只能推荐5条食谱！"));
                return Task.FromResult(new Unit());
            }

            //休息一下吧，每个营养师只能推荐500条食谱
            var supporterRecommendDietCount = _customerDietRepository.QueryCustomerDietCountOnTheSameDayBySupporterOid(request.SupporterOid, DateTime.Now);
            if (supporterRecommendDietCount >= 500)
            {
                //引发错误事件
                Bus.RaiseEvent(new DomainNotification("", "休息一下吧，每个营养师只能推荐500条食谱！"));
                return Task.FromResult(new Unit());
            }

            //上一条推荐与下一条推荐时间间隔至少30秒钟
            //var supporterRecommendLastestDiet = _customerDietRepository.GetSupporterRecommendLastestDiet(request.SupporterOid);
            //if (supporterRecommendLastestDiet != null && supporterRecommendLastestDiet.CreateTime.AddSeconds(30) > DateTime.Now)
            //{
            //    //引发错误事件
            //    TimeSpan ts = (supporterRecommendLastestDiet.CreateTime.AddSeconds(30) - DateTime.Now);
            //    Bus.RaiseEvent(new DomainNotification("", "休息一下吧，" +  ts.Seconds+ "秒后，再试！"));
            //    return Task.FromResult(new Unit());
            //}

            //新增食谱
            var customerDiet = new CustomerDiet(
                request.OID,
                request.CustomerOid,
                request.ServiceOid,
                request.Recommend,
                request.Current,
                request.CurrentDiet,
                request.SupporterOid,
                0,
                DateTime.Now,
                new SupportOperate(request.SupporterOid, DateTime.Now)
                );
            _customerDietRepository.Add(customerDiet);

            if (Commit())
            {
                // 提交成功后，这里可以发布领域事件，比如短信通知
            }
            return Task.FromResult(new Unit());
        }

        public Task<Unit> Handle(DeleteOneCustomerDietCommand request, CancellationToken cancellationToken)
        {
            // 命令验证
            if (!request.IsValid())
            {
                // 错误信息收集
                NotifyValidationErrors(request);//主要为验证的信息
                // 返回，结束当前线程
                return Task.FromResult(new Unit());
            }

            //获取食谱
            var existingCustomerDiet = _customerDietRepository.GetById(request.OID);
            if (existingCustomerDiet == null)
            {
                //引发错误事件
                Bus.RaiseEvent(new DomainNotification("", "食谱不存在"));
                return Task.FromResult(new Unit());
            }

            //上一条操作与下一条操作时间间隔至少30秒钟
            //var supporterOperateLastestDiet = _customerDietRepository.GetSupporterOperateLastestDiet(request.LastOperate.Oid);
            //if (supporterOperateLastestDiet != null && supporterOperateLastestDiet.LastOperate.Time.AddSeconds(30) > DateTime.Now)
            //{
            //    //引发错误事件
            //    TimeSpan ts = (supporterOperateLastestDiet.LastOperate.Time.AddSeconds(30) - DateTime.Now);
            //    Bus.RaiseEvent(new DomainNotification("", "休息一下吧，" + ts.Seconds + "秒后，再试！"));
            //    return Task.FromResult(new Unit());
            //}

            //更新食谱信息
            existingCustomerDiet.UpdateDiscard(request.Discard, request.LastOperate);

            //判断是否存在需要加入到不喜欢列表中
            if(request.DislikeList!=null)
            {
                foreach(var dislikeFood in request.DislikeList)
                {
                    if (_customerDislikeFoodRepository.GetCustomerDislikeFood(existingCustomerDiet.CustomerOid, dislikeFood.FoodOid) == null)
                    {
                        CustomerDislikeFood customerDislikeFood = new CustomerDislikeFood(
                            Guid.NewGuid(),
                            existingCustomerDiet.CustomerOid,
                            dislikeFood.FoodOid,
                            request.LastOperate.Oid,
                            DateTime.Now
                            );
                        _customerDislikeFoodRepository.Add(customerDislikeFood);
                    }
                }
            }

            if (Commit())
            {
                // 提交成功后，这里可以发布领域事件，比如短信通知
            }
            return Task.FromResult(new Unit());
        }

        public Task<Unit> Handle(CreateOneCustomerDislikeFoodCommand request, CancellationToken cancellationToken)
        {
            // 命令验证
            if (!request.IsValid())
            {
                // 错误信息收集
                NotifyValidationErrors(request);//主要为验证的信息
                // 返回，结束当前线程
                return Task.FromResult(new Unit());
            }

            //验证食材
            if(request.DislikeFoodList.Count<=0)
            {
                //引发错误事件
                Bus.RaiseEvent(new DomainNotification("", "食材不能空"));
                return Task.FromResult(new Unit());
            }

            //上一条操作与下一条操作时间间隔至少30秒钟
            //var supporterOperateLastestDislikeFood = _customerDislikeFoodRepository.GetSupporterOperateLastestDislikeFood(request.OperaterOid);
            //if (supporterOperateLastestDislikeFood != null && supporterOperateLastestDislikeFood.CreateTime.AddSeconds(30) > DateTime.Now)
            //{
            //    //引发错误事件
            //    TimeSpan ts = (supporterOperateLastestDislikeFood.CreateTime.AddSeconds(30) - DateTime.Now);
            //    Bus.RaiseEvent(new DomainNotification("", "休息一下吧，" + ts.Seconds + "秒后，再试！"));
            //    return Task.FromResult(new Unit());
            //}

            foreach (var dislikeFoodOid in request.DislikeFoodList)
            {
                var existingDislikeFood = _customerDislikeFoodRepository.GetCustomerDislikeFood(request.CustomerOid,dislikeFoodOid);
                if(existingDislikeFood==null)
                {
                    CustomerDislikeFood customerDislikeFood = new CustomerDislikeFood(
                        Guid.NewGuid(),
                        request.CustomerOid,
                        dislikeFoodOid,
                        request.OperaterOid,
                        DateTime.Now
                        );
                    _customerDislikeFoodRepository.Add(customerDislikeFood);
                }
                else
                {
                    existingDislikeFood = _customerDislikeFoodRepository.GetById(existingDislikeFood.OID);
                    existingDislikeFood.UpdateCreateTime(DateTime.Now);
                }
            }

            if (Commit())
            {
                // 提交成功后，这里可以发布领域事件，比如短信通知
            }
            return Task.FromResult(new Unit());

        }

        public Task<Unit> Handle(DeleteOneCustomerDislikeFoodCommand request, CancellationToken cancellationToken)
        {
            // 命令验证
            if (!request.IsValid())
            {
                // 错误信息收集
                NotifyValidationErrors(request);//主要为验证的信息
                // 返回，结束当前线程
                return Task.FromResult(new Unit());
            }

            //验证食材
            if (request.DislikeFoodList.Count <= 0)
            {
                //引发错误事件
                Bus.RaiseEvent(new DomainNotification("", "食材不能空"));
                return Task.FromResult(new Unit());
            }

            foreach (var dislikeFoodOid in request.DislikeFoodList)
            {
                var existingDislikeFood = _customerDislikeFoodRepository.GetCustomerDislikeFood(request.CustomerOid, dislikeFoodOid);
                if (existingDislikeFood != null)
                {
                    _customerDislikeFoodRepository.Remove(existingDislikeFood.OID);
                }
            }

            if (Commit())
            {
                // 提交成功后，这里可以发布领域事件，比如短信通知
            }
            return Task.FromResult(new Unit());

        }
    }
}
