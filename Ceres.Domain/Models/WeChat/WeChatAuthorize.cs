using Ceres.Domain.Core.Models;
using System;

namespace Ceres.Domain.Models
{
    public class WeChatAuthorize: AggregationRoot
    {
        protected WeChatAuthorize()
        { }

        public WeChatAuthorize(Guid oid, string code2Session,DateTime createTime)
        {
            OID = oid;
            Code2Session = code2Session;
            CreateTime = createTime;
        }

        public string Code2Session { get; private set; }
        public string EncryptedData { get; private set; }
        public string IV { get; private set; }
        public string PhoneJson { get; private set; }
        public DateTime CreateTime { get; private set; }

        /// <summary>
        /// 更新前端发过来的授权加密数据
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="iv"></param>
        public void UpdateEncryptedData(string encryptedData,string iv,string phoneJson)
        {
            this.EncryptedData = encryptedData;
            this.IV = iv;
            this.PhoneJson = phoneJson;
        }
    }
}
