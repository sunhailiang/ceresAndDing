using Ceres.Domain.Interfaces;
using Ceres.Domain.Models;
using Ceres.Infrastruct.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ceres.Infrastruct.Repository
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        public AnswerRepository(CeresContext context)
            : base(context)
        {

        }

        public IEnumerable<Answer> GetAnswerByConditions(Guid userid, Guid questionVersion, Guid questionGuid, DateTime currentTime, DateTime lastTime)
        {
            var result = DbSet.AsNoTracking()
                .Where
                (
                c => c.Userid == userid
                && c.QuestionVersion == questionVersion
                && c.QuestionGuid == questionGuid
                && c.Ctime <= currentTime
                && c.Ctime >= lastTime
                )
                .OrderByDescending(c=>c.Ctime)
                ;
            return result;
        }

        public IEnumerable<Answer> GetAnswerByConditions(Guid userid, DateTime currentTime, DateTime lastTime)
        {
            var result = DbSet.AsNoTracking()
                .Where
                (
                c => c.Userid == userid
                && c.Ctime <= currentTime
                && c.Ctime >= lastTime
                )
                .OrderByDescending(c => c.Ctime)
                ;
            return result;
        }

        public IEnumerable<Answer> GetAnswerByConditions(Guid userid)
        {
            var result = DbSet.AsNoTracking()
                .Where
                (
                c => c.Userid == userid
                )
                .OrderByDescending(c => c.Ctime)
                ;
            return result;
        }

        public IEnumerable<Answer> GetAnswerByConditions(Guid userid, Guid questionVersion, Guid questionGuid)
        {
            var result = DbSet.AsNoTracking()
                .Where
                (
                c => c.Userid == userid
                && c.QuestionVersion == questionVersion
                && c.QuestionGuid == questionGuid
                )
                .OrderByDescending(c => c.Ctime)
                ;
            return result;
        }

        public Answer GetDingAnswer(Guid userid, Guid questionGuid, DateTime startTime)
        {
            var result = DbSet.AsNoTracking()
                .Where
                (
                c => c.Userid == userid
                && c.QuestionGuid == questionGuid
                && c.Ctime>=startTime
                && c.Ctime<=startTime.AddMinutes(15)
                )
                .OrderByDescending(c => c.Ctime).LastOrDefault()//原因是当有2个或者多个查询到时，应该获取时间早的那个
                ;
            return result;
        }

        public Answer GetOneDingAnswerByQuestionGuid(Guid userid, Guid questionGuid,DateTime ctime)
        {
            var result = DbSet.AsNoTracking()
                .Where
                (
                c => c.Userid == userid
                && c.QuestionGuid == questionGuid
                && c.Ctime.Date == ctime.Date
                )
                .OrderByDescending(c => c.Ctime).FirstOrDefault()
                ;
            return result;
        }

        public IEnumerable<Answer> QueryMecuryAnswerList(Guid userid, Guid questionGuid)
        {
            var result = DbSet.AsNoTracking()
                .Where
                (
                c => c.Userid == userid
                && c.QuestionGuid == questionGuid
                )
                .OrderByDescending(c => c.Ctime)
                ;
            return result;
        }

        public int QueryMecuryAnswerListCount(Guid userid, Guid firstQuestionGuid)
        {

            var result = DbSet.AsNoTracking()
                .Where
                (
                c => c.Userid == userid
                && c.QuestionGuid == firstQuestionGuid
                )
                .Count();
            return result;
        }
    
        public Answer GetFirstAnswerContent(Guid userid,Guid firstQuestionGuid)
        {
            var result = DbSet.AsNoTracking()
                .Where
                (
                c => c.Userid == userid
                &&c.QuestionGuid==firstQuestionGuid
                )
                .OrderByDescending(c=>c.Ctime)
                .LastOrDefault();
            return result;
        }
    }
}
