using Ceres.Domain.Core.Commands;
using Ceres.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ceres.Domain.Core.Bus
{
    /// <summary>
    /// 中介处理程序接口
    /// 可以定义多个处理程序
    /// 是异步的
    /// </summary>
    public interface IMediatorHandler
    {
        /// <summary>
        /// 发送命令，将命令模型发布到中介者模块
        /// </summary>
        /// <typeparam name="T"> 泛型 </typeparam>
        /// <param name="command"> 命令模型，比如LoginSupporterCommand </param>
        /// <returns></returns>
        Task SendCommand<T>(T command) where T : Command;

        /// <summary>
        /// 引发事件，通过总线，发布事件
        /// </summary>
        /// <typeparam name="T"> 泛型 继承 Event：INotification</typeparam>
        /// <param name="event"> 事件模型，比如SupporterLoginEvent，</param>
        /// 请注意一个细节：这个命名方法和Command不一样，一个是request 登陆命令之前,一个是SupporterLoginEvent登陆成功事件之后
        /// <returns></returns>
        Task RaiseEvent<T>(T @event) where T : Event;
    }
}
