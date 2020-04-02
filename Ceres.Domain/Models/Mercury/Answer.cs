using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ceres.Domain.Models
{
    /// <summary>
    /// 这是Mercury中的数据
    /// </summary>
    public class Answer
    {
        protected Answer()
        { }

        public Answer(Guid oid,Guid userid,int recordId,DateTime ctime,DateTime mtime,string content,Guid questionVersion,Guid questionnaireGuid,Guid questionGuid)
        {
            AnswerGuid = oid;
            Userid = userid;
            RecordId = recordId;
            Ctime = ctime;
            Mtime = mtime;
            Content = content;
            QuestionVersion = questionVersion;
            QuestionnaireGuid = questionnaireGuid;
            QuestionGuid = questionGuid;
        }

        [Key]
        public Guid AnswerGuid { get; private set; }
        public Guid Userid { get; private set; }
        public int RecordId { get; private set; }
        public DateTime Ctime { get; private set; }
        public DateTime Mtime { get; private set; }
        public string Content { get; private set; }
        public Guid QuestionVersion { get; private set; }
        public Guid QuestionnaireGuid { get; private set; }
        public Guid QuestionGuid { get; private set; }

        public void UpdateContent(string content,DateTime ctime, DateTime mtime)
        {
            this.Content = content;
            this.Ctime = ctime;
            this.Mtime = mtime;
        }
    }
}
