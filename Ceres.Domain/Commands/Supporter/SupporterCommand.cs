using Ceres.Domain.Core.Commands;
using System;

namespace Ceres.Domain.Commands
{
    /// <summary>
    /// 定义一个抽象的 Supporter 命令模型
    /// 继承 Command
    /// 这个模型主要作用就是用来创建命令动作的，所以是一个抽象类
    /// </summary>
    public abstract class SupporterCommand:Command
    {
        public Guid OID { get; protected set; }
        public string LoginName { get; protected set; }
        public string Cellphone { get; protected set; }
        public string Password { get; protected set; }
        public string UserName { get; protected set; }
        public string Image { get; protected set; }
        public DateTime CreateTime { get; protected set; }
        public int Status { get; protected set; }
    }
}
