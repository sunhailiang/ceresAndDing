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
    /// 微信授权命令处理程序
    /// 用来处理微信授权下的所有命令
    /// 注意必须要继承接口IRequestHandler<,>，这样才能实现各个命令的Handle方法
    /// </summary>
    public class WeChatAuthorizeCommandHandler:CommandHandler,
        IRequestHandler<CreateOneWeChatAuthorizeCommand, Unit>,
        IRequestHandler<UpdateWeChatAuthorizeCommand, Unit>
    {
        //注入仓储接口
        private readonly IWeChatAuthorizeRepository _weChatAuthorizeRepository;

        //注入总线
        private readonly IMediatorHandler Bus;
        private IMemoryCache Cache;

        public WeChatAuthorizeCommandHandler(IWeChatAuthorizeRepository weChatAuthorizeRepository,
                                        IUnitOfWork uow,
                                        IMediatorHandler bus,
                                        IMemoryCache cache
                                        ) : base(uow, bus, cache)
        {
            _weChatAuthorizeRepository = weChatAuthorizeRepository;
            Bus = bus;
            Cache = cache;
        }

        public void Dispose()
        {
            _weChatAuthorizeRepository.Dispose();
        }

        public Task<Unit> Handle(CreateOneWeChatAuthorizeCommand request, CancellationToken cancellationToken)
        {
            // 命令验证
            if (!request.IsValid())
            {
                // 错误信息收集
                NotifyValidationErrors(request);//主要为验证的信息
                // 返回，结束当前线程
                return Task.FromResult(new Unit());
            }

            var weChatAuthorize = new WeChatAuthorize(
                request.OID,
                request.Code2Session,
                DateTime.Now
                );

            _weChatAuthorizeRepository.Add(weChatAuthorize);

            if (Commit())
            {
                // 提交成功后，这里可以发布领域事件，比如短信通知
            }
            return Task.FromResult(new Unit());
        }

        public Task<Unit> Handle(UpdateWeChatAuthorizeCommand request, CancellationToken cancellationToken)
        {
            // 命令验证
            if (!request.IsValid())
            {
                // 错误信息收集
                NotifyValidationErrors(request);//主要为验证的信息
                // 返回，结束当前线程
                return Task.FromResult(new Unit());
            }

            var existingWeChatAuthorize = _weChatAuthorizeRepository.GetById(request.OID);
            if (existingWeChatAuthorize == null)
            {
                //引发错误事件
                Bus.RaiseEvent(new DomainNotification("", "授权Code不存在！"));
                return Task.FromResult(new Unit());
            }

            existingWeChatAuthorize.UpdateEncryptedData(request.EncryptedData, request.IV,request.PhoneJson);

            if (Commit())
            {
                // 提交成功后，这里可以发布领域事件，比如短信通知
            }
            return Task.FromResult(new Unit());
        }
    }
}
