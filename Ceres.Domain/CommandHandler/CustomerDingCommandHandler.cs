using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Ceres.Domain.Commands;
using System.Threading.Tasks;
using System.Threading;
using Ceres.Domain.Interfaces;
using Ceres.Domain.Core.Bus;
using Microsoft.Extensions.Caching.Memory;
using Ceres.Domain.Models;
using Ceres.Domain.Core.Notifications;
using Newtonsoft.Json;
using System.Linq;

namespace Ceres.Domain.CommandHandler
{
    public class CustomerDingCommandHandler : CommandHandler,
        IRequestHandler<CreateOneCustomerAssistDingCommand, Unit>,
        IRequestHandler<DeleteOneCustomerAssistDingCommand, Unit>,
        IRequestHandler<CreateOneCustomerDingCommand, Unit>
    {
        // 注入仓储接口
        private readonly IAnswerRepository _answerRepository;
        private readonly ICustomerAssistDingRepository _customerAssistDingRepository;
        private readonly IQuestionnaireRepository _questionnaireRepository;
        private readonly IQuestionRepository _questionRepository;
        // 注入总线
        private readonly IMediatorHandler Bus;
        private IMemoryCache Cache;

        public CustomerDingCommandHandler(IAnswerRepository answerRepository,
                                      ICustomerAssistDingRepository customerAssistDingRepository,
                                      IQuestionnaireRepository questionnaireRepository,
                                      IQuestionRepository questionRepository,
                                      IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      IMemoryCache cache
                                      ) : base(uow, bus, cache)
        {
            _answerRepository = answerRepository;
            _customerAssistDingRepository = customerAssistDingRepository;
            _questionnaireRepository = questionnaireRepository;
            _questionRepository = questionRepository;
            Bus = bus;
            Cache = cache;
        }

        public void Dispose()
        {
            _answerRepository.Dispose();
            _customerAssistDingRepository.Dispose();
            _questionnaireRepository.Dispose();
            _questionRepository.Dispose();
        }

        public Task<Unit> Handle(CreateOneCustomerAssistDingCommand request, CancellationToken cancellationToken)
        {
            // 命令验证
            if (!request.IsValid())
            {
                // 错误信息收集
                NotifyValidationErrors(request);//主要为验证的信息
                // 返回，结束当前线程
                return Task.FromResult(new Unit());
            }

            ////客服人员的，上一条协助打卡与下一条协助打卡时间间隔至少30秒
            //var lastestAssistDing = _customerAssistDingRepository.GetLastestOperateBySupporter(request.SupporterOid);
            //if (lastestAssistDing != null && lastestAssistDing.CreateTime.AddSeconds(30) > DateTime.Now)
            //{
            //    //引发错误事件
            //    TimeSpan ts = (lastestAssistDing.CreateTime.AddSeconds(30) - DateTime.Now);
            //    Bus.RaiseEvent(new DomainNotification("", "休息一下吧，" + ts.Seconds + "秒后，再试！"));
            //    return Task.FromResult(new Unit());
            //}

            //查询所有问题
            var questionaire = _questionnaireRepository.GetById(Guid.Parse("AA46576C-9FD5-4CB9-87B1-CCEDBF68A92D"));//打卡问题集
            if (questionaire == null)
            {
                //引发错误事件
                Bus.RaiseEvent(new DomainNotification("", "系统异常，操作失败"));
                return Task.FromResult(new Unit());
            }
            Guid[] questionGuids = JsonConvert.DeserializeObject<Guid[]>(questionaire.Question);
            if (questionGuids == null)
            {
                //引发错误事件
                Bus.RaiseEvent(new DomainNotification("", "系统异常，操作失败"));
                return Task.FromResult(new Unit());
            }

            DateTime createTime = DateTime.Now;

            //一个人每天只有一次的打卡机会
            var oneDingAnswer = _answerRepository.GetOneDingAnswerByQuestionGuid(request.CustomerOid, questionGuids[0],request.AssistTime);
            if (oneDingAnswer != null)
            {
                //引发错误事件
                Bus.RaiseEvent(new DomainNotification("", "【"+ oneDingAnswer.Ctime.ToString("yyyy-MM-dd HH:mm:ss") +"】"+"客户已经打卡，每人每天只有一次打卡机会"));
                return Task.FromResult(new Unit());
            }


            //增加协助记录
            var customerAssistDing = new CustomerAssistDing(
                Guid.NewGuid(),
                Guid.Parse("AA46576C-9FD5-4CB9-87B1-CCEDBF68A92D"),
                request.CustomerOid,
                request.SupporterOid,
                request.AssistTime,
                createTime,
                0
                );
            _customerAssistDingRepository.Add(customerAssistDing);
            
            //处理问题的答案
            for (int i = 0; i < questionGuids.Count(); i++)
            {
                //依据question查询question详细信息
                var question = _questionRepository.GetById(questionGuids[i]);


                //查找每一个GUID对于的答案
                string answerContent = "";
                bool findAnswer = false;
                foreach(var questionAnswer in request.AssistDing)
                {
                    if(questionAnswer.QuestionOID==question.QuestionGuid)
                    {
                        findAnswer = true;
                        answerContent = questionAnswer.AnswerContent;
                        break;
                    }
                }

                if(findAnswer==false)
                {
                    answerContent = "此回答自动屏蔽";//客户没有回答这个问题
                }

                if(question.Type!= "PhotoGraph")
                {
                    answerContent=JsonConvert.SerializeObject(answerContent);
                }

                var answer = new Answer(
                    Guid.NewGuid(),
                    request.CustomerOid,
                    0,
                    request.AssistTime,
                    request.AssistTime,
                    answerContent,
                    question.Version,
                    Guid.Empty,
                    questionGuids[i]
                    );
                _answerRepository.Add(answer);
            }

            if (Commit())
            {
                // 提交成功后，这里可以发布领域事件，比如短信通知
            }
            return Task.FromResult(new Unit());
        }

        public Task<Unit> Handle(DeleteOneCustomerAssistDingCommand request, CancellationToken cancellationToken)
        {
            // 命令验证
            if (!request.IsValid())
            {
                // 错误信息收集
                NotifyValidationErrors(request);//主要为验证的信息
                // 返回，结束当前线程
                return Task.FromResult(new Unit());
            }

            //依据第一个问题的回答的GUID，查询Ctime
            var firstAnswer = _answerRepository.GetById(request.OID);
            if(firstAnswer==null)
            {
                //引发错误事件
                Bus.RaiseEvent(new DomainNotification("", "删除失败，系统不存在此记录"));
                return Task.FromResult(new Unit());
            }

            //查询所有问题
            var questionaire = _questionnaireRepository.GetById(Guid.Parse("AA46576C-9FD5-4CB9-87B1-CCEDBF68A92D"));//打卡问题集
            if (questionaire == null)
            {
                //引发错误事件
                Bus.RaiseEvent(new DomainNotification("", "系统异常，操作失败"));
                return Task.FromResult(new Unit());
            }
            Guid[] questionGuids = JsonConvert.DeserializeObject<Guid[]>(questionaire.Question);
            if (questionGuids == null)
            {
                //引发错误事件
                Bus.RaiseEvent(new DomainNotification("", "系统异常，操作失败"));
                return Task.FromResult(new Unit());
            }

            //删除回答
            DateTime startTime = firstAnswer.Ctime;
            for (int j = 1; j < questionGuids.Count(); j++)//删除所有回答，除了第一个回答
            {
                var answer = _answerRepository.GetDingAnswer(firstAnswer.Userid, questionGuids[j], startTime);
                if (answer == null)
                {
                    continue;
                }
                _answerRepository.Remove(answer.AnswerGuid);
            }
            _answerRepository.Remove(firstAnswer.AnswerGuid);//删除第一个回答

            //删除协助记录，采用删除用户当天提交的协助
            var deleteCustomerAssistDing=_customerAssistDingRepository.GetByCustomerOid(Guid.Parse("AA46576C-9FD5-4CB9-87B1-CCEDBF68A92D"), firstAnswer.Userid,firstAnswer.Ctime);
            if(deleteCustomerAssistDing!=null)
            {
                var existingCustomerAssistDing = _customerAssistDingRepository.GetById(deleteCustomerAssistDing.OID);
                existingCustomerAssistDing.UpdateStatus(-1);
            }

            if (Commit())
            {
                // 提交成功后，这里可以发布领域事件，比如短信通知
            }
            return Task.FromResult(new Unit());
        }

        public Task<Unit> Handle(CreateOneCustomerDingCommand request, CancellationToken cancellationToken)
        {
            // 命令验证
            if (!request.IsValid())
            {
                // 错误信息收集
                NotifyValidationErrors(request);//主要为验证的信息
                // 返回，结束当前线程
                return Task.FromResult(new Unit());
            }

            //查询所有问题
            var questionaire = _questionnaireRepository.GetById(Guid.Parse("AA46576C-9FD5-4CB9-87B1-CCEDBF68A92D"));//打卡问题集
            if (questionaire == null)
            {
                //引发错误事件
                Bus.RaiseEvent(new DomainNotification("", "系统异常，操作失败"));
                return Task.FromResult(new Unit());
            }
            Guid[] questionGuids = JsonConvert.DeserializeObject<Guid[]>(questionaire.Question);
            if (questionGuids == null)
            {
                //引发错误事件
                Bus.RaiseEvent(new DomainNotification("", "系统异常，操作失败"));
                return Task.FromResult(new Unit());
            }

            //增加所有的答案
            DateTime today = DateTime.Now;

            //处理问题的答案
            for (int i = 0; i < questionGuids.Count(); i++)
            {
                //依据question查询question详细信息
                var question = _questionRepository.GetById(questionGuids[i]);

                //查找每一个GUID对于的答案
                string answerContent = "";
                bool findAnswer = false;
                foreach (var questionAnswer in request.MiddleDingList)
                {
                    if (questionAnswer.QuestionOID == question.QuestionGuid)
                    {
                        findAnswer = true;
                        answerContent = questionAnswer.AnswerContent;
                        break;
                    }
                }

                if (findAnswer == false)
                {
                    answerContent = "此回答自动屏蔽";//客户没有回答这个问题
                }

                if (question.Type != "PhotoGraph")
                {
                    answerContent = JsonConvert.SerializeObject(answerContent);
                }

                var existingAnswer = _answerRepository.GetOneDingAnswerByQuestionGuid(request.CustomerOid, questionGuids[i], today);
                if (existingAnswer == null)
                {
                    var answer = new Answer(
                        Guid.NewGuid(),
                        request.CustomerOid,
                        0,
                        today,
                        today,
                        answerContent,
                        question.Version,
                        Guid.Empty,
                        questionGuids[i]
                        );
                    _answerRepository.Add(answer);
                }
                else
                {
                    existingAnswer = _answerRepository.GetById(existingAnswer.AnswerGuid);
                    existingAnswer.UpdateContent(answerContent, today, today);
                }
            }

            if (Commit())
            {
                // 提交成功后，这里可以发布领域事件，比如短信通知
            }
            return Task.FromResult(new Unit());
        }
    }
}
