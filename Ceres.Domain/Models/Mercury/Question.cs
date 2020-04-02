using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ceres.Domain.Models
{
    /// <summary>
    /// 这是Mercury中的数据
    /// </summary>
    public class Question
    {
        protected Question()
        { }

        [Key]
        public Guid QuestionGuid { get; private set; }
        public string Content { get; private set; }
        public string Calculator { get; private set; }
        public DateTime Ctime { get; private set; }
        public Guid Version { get; private set; }
        public string Type { get; private set; }
        public bool IsEnable { get; private set; }
        public int QuestionPK { get; private set; }
    }
}
