using Ceres.Domain.Models;
using System;
using System.Collections.Generic;

namespace Ceres.Domain.Interfaces
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        public IEnumerable<Answer> GetAnswerByConditions(Guid userid, Guid questionVersion, Guid questionGuid,DateTime currentTime,DateTime lastTime);

        public IEnumerable<Answer> GetAnswerByConditions(Guid userid, DateTime currentTime, DateTime lastTime);

        public IEnumerable<Answer> GetAnswerByConditions(Guid userid);
        public IEnumerable<Answer> GetAnswerByConditions(Guid userid, Guid questionVersion, Guid questionGuid);

        public int QueryMecuryAnswerListCount(Guid userid, Guid firstQuestionGuid);
        public IEnumerable<Answer> QueryMecuryAnswerList(Guid userid, Guid questionGuid);
        public Answer GetDingAnswer(Guid userid, Guid questionGuid,DateTime startTime);
        public Answer GetOneDingAnswerByQuestionGuid(Guid userid, Guid questionGuid, DateTime ctime);
        public Answer GetFirstAnswerContent(Guid userid, Guid firstQuestionGuid);
    }
}
