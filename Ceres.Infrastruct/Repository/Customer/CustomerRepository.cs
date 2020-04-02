using Ceres.Domain.Interfaces;
using Ceres.Domain.Models;
using Ceres.Infrastruct.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ceres.Infrastruct.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(CeresContext context)
            :base(context)
        {

        }

        public Customer GetByCellphone(string cellphone)
        {
            return DbSet.AsNoTracking().FirstOrDefault(c => c.Cellphone == cellphone && c.Status == 0);
        }



        public IEnumerable<Customer> QueryCustomerListByPage(int pageIndex, int pageSize)
        {
            return DbSet.AsNoTracking().Where(c=>c.Status==0).OrderBy(c=>c.UserName).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public IEnumerable<Customer> QueryCustomerListByPage(string userName, string cellphone, Guid serviceOid, Guid supporterOid, Guid agenterOid, int pageIndex, int pageSize)
        {
            var result = (from c in this.Db.Customer.AsNoTracking()
                          where c.Status == 0
                          && ((string.IsNullOrEmpty(userName) || c.UserName.Contains(userName)))
                          && ((string.IsNullOrEmpty(cellphone) || c.Cellphone.Contains(cellphone)))
                          && ((Guid.Empty == supporterOid || c.SupporterOid == supporterOid))
                          && ((Guid.Empty == agenterOid || c.AgenterOid == agenterOid))
                          && ((Guid.Empty == serviceOid ||
                           (
                           (from s in this.Db.Service.AsNoTracking()
                            join cs in this.Db.CustomerService.AsNoTracking()
                            on s.OID equals cs.ServiceOid
                            where s.Status == 0 && cs.Status == 0 && cs.CustomerOid == c.OID
                            select s).AsNoTracking().FirstOrDefault().OID == serviceOid
                            )
                          ))
                          select c
                          ).AsNoTracking().Skip(pageIndex * pageSize).Take(pageSize);
            return result;
            #region 按照名称查找
            //var result = (from c in this.Db.Customer.AsNoTracking()
            //              where c.Status == 0
            //              && ((string.IsNullOrEmpty(userName) || c.UserName.Contains(userName)))
            //              && ((string.IsNullOrEmpty(cellphone) || c.Cellphone.Contains(cellphone)))
            //              && ((Guid.Empty == supporterOid ||
            //                (
            //               (from s in this.Db.Supporter.AsNoTracking()
            //                where s.Status == 0 && s.OID == c.SupporterOid
            //                select s).AsNoTracking().FirstOrDefault().UserName
            //                )
            //                .Contains(supporterName)
            //              ))
            //              && ((string.IsNullOrEmpty(agenterName) ||
            //                (
            //                  (from s in this.Db.Agenter.AsNoTracking()
            //                   where s.Status == 0 && s.OID == c.AgenterOid
            //                   select s).AsNoTracking().FirstOrDefault().UserName
            //                 )
            //               .Contains(agenterName)
            //              ))
            //              && ((string.IsNullOrEmpty(serviceName) ||
            //               (
            //               (from s in this.Db.Service.AsNoTracking()
            //                join cs in this.Db.CustomerService.AsNoTracking()
            //                on s.OID equals cs.ServiceOid
            //                where s.Status == 0 && cs.Status == 0 && cs.CustomerOid == c.OID
            //                select s).AsNoTracking().FirstOrDefault().Name
            //                )
            //                .Contains(serviceName)
            //              ))
            //              select c
            //              ).AsNoTracking().Skip(pageIndex * pageSize).Take(pageSize);
            //return result;
            #endregion 按照名称查找
        }

        public int QueryCustomerListCount(string userName, string cellphone, Guid serviceOid, Guid supporterOid, Guid agenterOid)
        {
            var result = (from c in this.Db.Customer.AsNoTracking()
                          where c.Status == 0
                          && ((string.IsNullOrEmpty(userName) || c.UserName.Contains(userName)))
                          && ((string.IsNullOrEmpty(cellphone) || c.Cellphone.Contains(cellphone)))
                          && ((Guid.Empty == supporterOid || c.SupporterOid == supporterOid))
                          && ((Guid.Empty == agenterOid || c.AgenterOid == agenterOid))
                          && ((Guid.Empty == serviceOid ||
                           (
                           (from s in this.Db.Service.AsNoTracking()
                            join cs in this.Db.CustomerService.AsNoTracking()
                            on s.OID equals cs.ServiceOid
                            where s.Status == 0 && cs.Status == 0 && cs.CustomerOid == c.OID
                            select s).AsNoTracking().FirstOrDefault().OID == serviceOid
                            )
                          ))
                          select c
                          ).AsNoTracking().Count();
            return result;
        }
        //public int QueryCustomerListCount(string userName, string cellphone, string serviceName, string supporterName, string agenterName)
        //{
        //    var result = (from c in this.Db.Customer.AsNoTracking()
        //                  where c.Status == 0
        //                  && ((string.IsNullOrEmpty(userName) || c.UserName.Contains(userName)))
        //                  && ((string.IsNullOrEmpty(cellphone) || c.Cellphone.Contains(cellphone)))
        //                  && ((string.IsNullOrEmpty(supporterName) ||
        //                    (
        //                   (from s in this.Db.Supporter.AsNoTracking()
        //                    where s.Status == 0 && s.OID == c.SupporterOid
        //                    select s).AsNoTracking().FirstOrDefault().UserName
        //                    )
        //                    .Contains(supporterName)
        //                  ))
        //                  && ((string.IsNullOrEmpty(agenterName) ||
        //                    (
        //                      (from s in this.Db.Agenter.AsNoTracking()
        //                       where s.Status == 0 && s.OID == c.AgenterOid
        //                       select s).AsNoTracking().FirstOrDefault().UserName
        //                     )
        //                   .Contains(agenterName)
        //                  ))
        //                  && ((string.IsNullOrEmpty(serviceName) ||
        //                   (
        //                   (from s in this.Db.Service.AsNoTracking()
        //                    join cs in this.Db.CustomerService.AsNoTracking()
        //                    on s.OID equals cs.ServiceOid
        //                    where s.Status == 0 && cs.Status == 0 && cs.CustomerOid == c.OID
        //                    select s).AsNoTracking().FirstOrDefault().Name
        //                    )
        //                    .Contains(serviceName)
        //                  ))
        //                  select c
        //                  ).AsNoTracking().Count();
        //    return result;
        //}

        public int QueryCustomerListCount()
        {
            return DbSet.AsNoTracking().Where(c => c.Status == 0).Count();
        }

        public IEnumerable<Customer> QueryOneSupporterCustomerList(Guid supporterOid)
        {
            var result = (
                          from c in this.Db.Customer.AsNoTracking()
                          join s in this.Db.Supporter.AsNoTracking()
                          on c.SupporterOid equals s.OID
                          where c.SupporterOid == supporterOid && c.Status == 0
                          select c).AsNoTracking();

            return result;
        }

        public int QueryOneSupporterCustomerListCount(Guid supporterOid)
        {
            var result = (
                          from c in this.Db.Customer.AsNoTracking()
                          join s in this.Db.Supporter.AsNoTracking()
                          on c.SupporterOid equals s.OID
                          where c.SupporterOid == supporterOid && c.Status==0
                          select c).Count();

            return result;
        }

        public IEnumerable<Customer> QueryOneSupporterDoneDietListByDaily(Guid supporterOid, DateTime dateTime)
        {
            var result = (
                          from c1 in
                          (
                          (from c in this.Db.Customer.AsNoTracking()
                           join s in this.Db.Supporter.AsNoTracking()
                           on c.SupporterOid equals s.OID
                           where c.SupporterOid == supporterOid && c.Status == 0
                           select c).AsNoTracking()
                          )
                         join d in this.Db.CustomerDiet.AsNoTracking()
                         on c1.OID equals d.CustomerOid
                         where d.CreateTime.Date == dateTime.Date && d.Status==0
                         select c1).AsNoTracking();
            return result;
        }

        public int QueryOneSupporterDoneDietListCountByDailyCount(Guid supporterOid, DateTime dateTime)
        {
            var result = (
                          from c1 in
                          (
                          (from c in this.Db.Customer.AsNoTracking()
                           join s in this.Db.Supporter.AsNoTracking()
                           on c.SupporterOid equals s.OID
                           where c.SupporterOid == supporterOid && c.Status == 0
                           select c).AsNoTracking()
                          )
                          join d in this.Db.CustomerDiet.AsNoTracking()
                                      on c1.OID equals d.CustomerOid
                          where d.CreateTime.Date == dateTime.Date && d.Status == 0
                          select c1).Count();
            return result;
        }

        public IEnumerable<Customer> QueryOneSupporterDoneDingListByDaily(Guid supporterOid,Guid firstQuestionGuid, DateTime dateTime)
        {
            var result = (
                           from c1 in
                           (
                            (from c in this.Db.Customer.AsNoTracking()
                             join s in this.Db.Supporter.AsNoTracking()
                             on c.SupporterOid equals s.OID
                             where c.SupporterOid == supporterOid && c.Status == 0
                             select c).AsNoTracking()
                           )
                           join a in this.Db.Answer.AsNoTracking()
                               on c1.OID equals a.Userid
                           where a.QuestionGuid == firstQuestionGuid && a.Ctime.Date == dateTime.Date
                           select c1).AsNoTracking();

            return result;
        }
    }
}
