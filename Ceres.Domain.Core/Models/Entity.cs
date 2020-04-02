using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ceres.Domain.Core.Models
{
    public abstract class Entity
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [Key]
        public Guid OID { get; protected set; }

    }
}
