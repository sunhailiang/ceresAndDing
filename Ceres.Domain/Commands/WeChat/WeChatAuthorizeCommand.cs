using Ceres.Domain.Core.Commands;
using Ceres.Domain.Models;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Commands
{
    public abstract class WeChatAuthorizeCommand : Command
    {
        public Guid OID { get; protected set; } 
        public string Code2Session { get; protected set; }
        public string EncryptedData { get; protected set; }
        public string IV { get; protected set; }
        public string PhoneJson { get; protected set; }
        public DateTime CreateTime { get; protected set; }
    }
}
