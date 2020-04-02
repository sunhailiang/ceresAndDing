using Ceres.Domain.Interfaces;
using Ceres.Domain.Models;
using Ceres.Infrastruct.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ceres.Infrastruct.Repository
{
    public class CustomerDietRepository : Repository<CustomerDiet>, ICustomerDietRepository
    {
        public CustomerDietRepository(CeresContext context)
            : base(context)
        {

        }

        public CustomerDiet GetSupporterRecommendLastestDiet(Guid supporterOid)
        {
            return DbSet.AsNoTracking().Where(c => c.SupporterOid == supporterOid && c.Status == 0).OrderByDescending(c=>c.CreateTime).FirstOrDefault();
        }

        public int QueryCustomerDietCountOnTheSameDayByCustomerOid(Guid customerOid, DateTime currentTime)
        {
            return DbSet.AsNoTracking().Where(c => c.CustomerOid== customerOid && c.Status == 0 && c.CreateTime.Date ==currentTime.Date).Count();
        }

        public int QueryCustomerDietCountOnTheSameDayBySupporterOid(Guid supporterOid, DateTime currentTime)
        {
            return DbSet.AsNoTracking().Where(c => c.SupporterOid == supporterOid && c.Status == 0 && c.CreateTime.Date == currentTime.Date).Count();
        }

        //查询有效的配餐列表
        public int QueryDietListCount()
        {
            return DbSet.AsNoTracking().Where(c=>c.Status==0).Count();
        }
        public IEnumerable<CustomerDiet> QueryDietListByPage(int pageIndex, int pageSize)
        {
            return DbSet.AsNoTracking().Where(c => c.Status == 0).OrderByDescending(c => c.CreateTime).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int QueryCustomerDietListCount(Guid customerOid)
        {
            return DbSet.AsNoTracking().Where(c => c.CustomerOid == customerOid && c.Status == 0).Count();
        }
        public IEnumerable<CustomerDiet> QueryCustomerDietListByPage(Guid customerOid, int pageIndex, int pageSize)
        {
            return DbSet.AsNoTracking().Where(c=>c.CustomerOid==customerOid&&c.Status==0).OrderByDescending(c => c.CreateTime).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public IEnumerable<CustomerDiet> QueryCustomerDietListByPage(string customerName, string cellphone, Guid serviceOid, Guid supporterOid, DateTime startTime, DateTime endTime, int pageIndex, int pageSize)
        {
            var result = (from c in this.Db.CustomerDiet.AsNoTracking()
                          where 1 == 1 &&c.Status==0
                          && ((Guid.Empty==serviceOid || c.ServiceOid==serviceOid))
                          && ((Guid.Empty==supporterOid || c.SupporterOid==supporterOid))
                          && ((string.IsNullOrEmpty(customerName) ||
                            (
                           (from s in this.Db.Customer.AsNoTracking()
                            where s.OID == c.CustomerOid
                            select s).AsNoTracking().FirstOrDefault().UserName
                            )
                            .Contains(customerName)
                          ))
                          && ((string.IsNullOrEmpty(cellphone) ||
                            (
                              (from s in this.Db.Customer.AsNoTracking()
                               where s.OID == c.CustomerOid
                               select s).AsNoTracking().FirstOrDefault().Cellphone
                             )
                           .Contains(cellphone)
                          ))
                          && ((startTime ==DateTime.MinValue ||
                           (c.CreateTime >= startTime)
                          ))
                          && ((endTime == DateTime.MinValue ||
                           (c.CreateTime <= endTime)
                          ))
                          select c
                          ).AsNoTracking().Skip(pageIndex * pageSize).Take(pageSize);
            return result;
        }

        public int QueryCustomerDietListCount(string customerName, string cellphone, Guid serviceOid, Guid supporterOid, DateTime startTime, DateTime endTime)
        {
            var result = (from c in this.Db.CustomerDiet.AsNoTracking()
                          where 1 == 1 && c.Status == 0
                          && ((Guid.Empty == serviceOid || c.ServiceOid == serviceOid))
                          && ((Guid.Empty == supporterOid || c.SupporterOid == supporterOid))
                          && ((string.IsNullOrEmpty(customerName) ||
                            (
                           (from s in this.Db.Customer.AsNoTracking()
                            where s.OID == c.CustomerOid
                            select s).AsNoTracking().FirstOrDefault().UserName
                            )
                            .Contains(customerName)
                          ))
                          && ((string.IsNullOrEmpty(cellphone) ||
                            (
                              (from s in this.Db.Customer.AsNoTracking()
                               where s.OID == c.CustomerOid
                               select s).AsNoTracking().FirstOrDefault().Cellphone
                             )
                           .Contains(cellphone)
                          ))
                          && ((startTime == DateTime.MinValue ||
                           (c.CreateTime >= startTime)
                          ))
                          && ((endTime == DateTime.MinValue ||
                           (c.CreateTime <= endTime)
                          ))
                          select c
                          ).AsNoTracking().Count();
            return result;
        }

        public CustomerDiet GetSupporterOperateLastestDiet(Guid lastOperaterOid)
        {
            return DbSet.AsNoTracking().Where(c => c.LastOperate.Oid == lastOperaterOid).OrderByDescending(c => c.LastOperate.Time).FirstOrDefault();
        }
    }
}
