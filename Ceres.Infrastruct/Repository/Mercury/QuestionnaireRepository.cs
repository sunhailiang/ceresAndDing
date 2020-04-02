using Ceres.Domain.Interfaces;
using Ceres.Domain.Models;
using Ceres.Infrastruct.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ceres.Infrastruct.Repository
{
    public class QuestionnaireRepository : Repository<Questionnaire>, IQuestionnaireRepository
    {
        public QuestionnaireRepository(CeresContext context)
            : base(context)
        {

        }
    }
}
