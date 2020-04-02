using Ceres.Domain.Interfaces;
using Ceres.Infrastruct.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Infrastruct.UoW
{
    /// <summary>
    /// 工作单元类
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        //数据库上下文
        private readonly CeresContext _context;

        //构造函数注入
        public UnitOfWork(CeresContext context)
        {
            _context = context;
        }

        //上下文提交
        public bool Commit()
        {
            return _context.SaveChanges()>0;
        }

        //手动回收
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
