using Ceres.Domain.Core.Commands;
using Ceres.Domain.Models;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Commands
{
    public abstract class CustomerAssistDingCommand: Command
    {
        public Guid OID { get; protected set; }
        public Guid QuestionnaireGuid { get; protected set; }
        public Guid CustomerOid { get; protected set; }
        public Guid SupporterOid { get; protected set; }
        public DateTime AssistTime { get; protected set; }
        public DateTime CreateTime { get; protected set; }
        public int Status { get; protected set; }

        public List<MiddleDing> AssistDing { get; set; }
    }
    public class MiddleDing
    {
        public Guid QuestionOID { get; set; }
        public string AnswerContent { get; set; }

        public MiddleDing(Guid questionOID,string answerContent)
        {
            QuestionOID = questionOID;
            AnswerContent = answerContent;
        }
    }
}
