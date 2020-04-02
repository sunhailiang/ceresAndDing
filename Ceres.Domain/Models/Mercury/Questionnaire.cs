using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ceres.Domain.Models
{
    /// <summary>
    /// 这是Mercury中的数据
    /// </summary>
    public class Questionnaire
    {
        protected Questionnaire()
        { }

        [Key]
        public Guid QuestionnaireGuid { get; private set; }
        public string Content { get; private set; }
        public string Question { get; private set; }
        public string Calculator { get; private set; }
        public DateTime Ctime { get; private set; }
        public Guid Version { get; private set; }
        public bool IsDeleted { get; private set; }
        public int QuestionnairePK { get; private set; }
    }
}
