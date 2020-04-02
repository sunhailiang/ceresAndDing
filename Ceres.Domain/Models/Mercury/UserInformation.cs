using Ceres.Domain.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ceres.Domain.Models
{
    /// <summary>
    /// 这是Mercury中的数据
    /// </summary>
    public class UserInformation
    {
        protected UserInformation()
        { }

        [Key]
        public int RecordID { get; private set; }
        public string Province { get; private set; }
        public string City { get; private set; }
        public double Height { get; private set; }
        public string PhoneNumber { get; private set; }
        public int UserGender { get; private set; }
        public Guid UserGuid { get; private set; }
        public string UserName { get; private set; }
        public double Weight { get; private set; }
        public int Age { get; private set; }
    }
}
