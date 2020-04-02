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
    /// <summary>
    /// Supporter命令处理程序
    /// 用来处理该Supporter下的所有命令
    /// 注意必须要继承接口IRequestHandler<,>，这样才能实现各个命令的Handle方法
    /// </summary>
    public class SupporterCommandHandler : CommandHandler,
        IRequestHandler<UpdateSupporterPasswordCommand,Unit>
    {
        // 注入仓储接口
        private readonly ISupporterRepository _supporterRepository;
        // 注入总线
        private readonly IMediatorHandler Bus;
        private IMemoryCache Cache;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="studentRepository"></param>
        /// <param name="uow"></param>
        /// <param name="bus"></param>
        /// <param name="cache"></param>
        public SupporterCommandHandler(ISupporterRepository supporterRepository,
                                      IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      IMemoryCache cache
                                      ) : base(uow, bus, cache)
        {
            _supporterRepository = supporterRepository;
            Bus = bus;
            Cache = cache;
        }

        // 手动回收
        public void Dispose()
        {
            _supporterRepository.Dispose();
        }

        //// UpdateSupporterPasswordCommand命令的处理程序
        //// 整个命令处理程序的核心都在这里
        //// 不仅包括命令验证的收集，持久化，还有领域事件和通知的添加
        //public Task<Unit> Handle(UpdateSupporterPasswordCommand request, CancellationToken cancellationToken)
        //{
        //    // 命令验证
        //    if (!request.IsValid())
        //    {
        //        // 错误信息收集
        //        NotifyValidationErrors(request);
        //        // 返回，结束当前线程
        //        return Task.FromResult(new Unit());
        //    }

        //    var existingSupporter = _supporterRepository.GetById(request.OID);
        //    if (existingSupporter == null)
        //    {
        //        //用户不存在
        //        return Task.FromResult(new Unit());
        //    }

        //    if (existingSupporter.Password == request.Password)
        //    {
        //        //密码相同，无需修改
        //        return Task.FromResult(new Unit());
        //    }

        //    // 统一提交
        //    if (Commit())
        //    {
        //        // 提交成功后，这里可以发布领域事件，
        //        // 比如欢迎用户注册邮件呀，短信呀等

        //        //Bus.RaiseEvent(new StudentRegisteredEvent(student.Id, student.Name, student.Email, student.BirthDate, student.Phone));
        //    }

        //    return Task.FromResult(new Unit());
        //}

        //public Task<Unit> Handle(GetAllSupporterCommand request, CancellationToken cancellationToken)
        //{
        //    // 命令验证
        //    if (!request.IsValid())
        //    {
        //        // 错误信息收集
        //        NotifyValidationErrors(request);
        //        // 返回，结束当前线程
        //        return Task.FromResult(new Unit());
        //    }

        //    _supporterRepository.GetAllValidSupporters.GetAll();

        //    //统一提交
        //    if (Commit())
        //    { }

        //    return Task.FromResult(new Unit());
        //}



        /// <summary>
        /// 客服修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Unit> Handle(UpdateSupporterPasswordCommand request, CancellationToken cancellationToken)
        {
            // 命令验证
            if (!request.IsValid())
            {
                // 错误信息收集
                NotifyValidationErrors(request);//主要为验证的信息
                // 返回，结束当前线程
                return Task.FromResult(new Unit());
            }

            //验证用户是否存在
            var existingSupporter = _supporterRepository.GetById(request.OID);
            if (existingSupporter == null)
            {
                //引发错误事件
                Bus.RaiseEvent(new DomainNotification("", "用户不存在！"));
                return Task.FromResult(new Unit());
            }

            if(existingSupporter.Password!=request.Password)
            {
                //引发错误事件
                Bus.RaiseEvent(new DomainNotification("", "用户旧密码错误，无法修改！"));
                return Task.FromResult(new Unit());
            }

            existingSupporter.UpdatePassword(request.NewPassword);

            if(Commit())
            {
                // 提交成功后，这里可以发布领域事件，比如短信通知
            }
            return Task.FromResult(new Unit());
        }
    }
}
