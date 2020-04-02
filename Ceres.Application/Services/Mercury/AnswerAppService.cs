using AutoMapper;
using Ceres.Application.Interfaces;
using Ceres.Application.ViewModels;
using Ceres.Domain.Commands;
using Ceres.Domain.Core.Bus;
using Ceres.Domain.Core.Notifications;
using Ceres.Domain.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceres.Application.Services
{
    public class AnswerAppService : IAnswerAppService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerJobRepository _customerJobRepository;
        private readonly IQuestionnaireRepository _questionnaireRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly ICustomerAssistDingRepository _customerAssistDingRepository;
        private readonly ISupporterRepository _supporterRepository;
        private readonly IUserInformationRepository _userInformationRepository;
        //DTO
        private readonly IMapper _mapper;
        //中介者 总线
        private readonly IMediatorHandler Bus;

        public AnswerAppService(
            IAnswerRepository answerRepository,
            ICustomerRepository customerRepository,
            ICustomerJobRepository customerJobRepository,
            IQuestionnaireRepository questionnaireRepository,
            IQuestionRepository questionRepository,
            IServiceRepository serviceRepository,
            ICustomerAssistDingRepository customerAssistDingRepository,
            ISupporterRepository supporterRepository,
            IUserInformationRepository userInformationRepository,
            IMapper mapper,
            IMediatorHandler bus
            )
        {
            _answerRepository = answerRepository;
            _customerRepository = customerRepository;
            _customerJobRepository = customerJobRepository;
            _questionnaireRepository = questionnaireRepository;
            _questionRepository = questionRepository;
            _serviceRepository = serviceRepository;
            _customerAssistDingRepository = customerAssistDingRepository;
            _supporterRepository = supporterRepository;
            _userInformationRepository = userInformationRepository;
            _mapper = mapper;
            Bus = bus;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public GetOneCustomerWeightListResponse GetOneCustomerWeightList(Guid oid, int dataCount)
        {
            GetOneCustomerWeightListResponse result;
            try
            {
                #region 获取体重数据列表方法1
                //查询打卡列表的总条数
                var existingAnswerCount = 0;

                var oneCustomerDingList = GetOneCustomerMecuryAnswerList(Guid.Parse("AA46576C-9FD5-4CB9-87B1-CCEDBF68A92D"),oid,0, dataCount,ref existingAnswerCount);//所有问答包含多套问答

                //转换数据，提取体重回答
                //体重问题GUID
                var weightGuid = Guid.Parse("A9F3D741-A83D-40E9-A108-89F7FC743384");
                var convertFloat = 0.0f;
                List<CustomerWeight> noRepeatWeightList = new List<CustomerWeight>();
                CustomerWeight customerWeight = new CustomerWeight();
                foreach (var answer in oneCustomerDingList)
                {
                    customerWeight = new CustomerWeight();
                    customerWeight.RecordTime = answer.AnswerTime;
                    customerWeight.Weight = 0f;
                    foreach (var dingAnswer in answer.MecuryAnswerList)
                    {
                        if (dingAnswer.QuestionGuid == weightGuid)
                        {
                            //var tempAnswer = dingWeightAnswer.Answer.Substring(1, dingWeightAnswer.Answer.Length - 2);
                            var tempAnswer = JsonConvert.DeserializeObject(dingAnswer.Answer).ToString();
                            customerWeight.Weight = Single.TryParse(tempAnswer, out convertFloat) == true ? convertFloat : 0f;
                            break;
                        }
                    }

                    noRepeatWeightList.Add(customerWeight);
                }

                //插入申请会员时，填写入的首次初始体重，身高，按照时间插入
                var existingCustomer = _customerRepository.GetById(oid);
                if (existingCustomer != null)
                {
                    customerWeight = new CustomerWeight();
                    customerWeight.RecordTime = existingCustomer.CreateTime;
                    customerWeight.Weight = existingCustomer.InitWeight;
                    noRepeatWeightList.Add(customerWeight);

                    //新数据时间排序
                    var newAnswer = noRepeatWeightList.OrderByDescending(t => t.RecordTime);

                    //新数据去除当天重复数据
                    var newNoRepeatAnswer =
                        from dL in newAnswer
                        group dL by new { dL.RecordTime.Date }
                        into mygroup
                        select mygroup.FirstOrDefault();

                    //数据合并
                    noRepeatWeightList = new List<CustomerWeight>();
                    noRepeatWeightList = newNoRepeatAnswer.Take(dataCount).ToList();
                }
                #endregion 获取体重数据列表方法1

                #region 获取体重数据列表方法3
                //System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
                //System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch();
                //System.Diagnostics.Stopwatch sw3 = new System.Diagnostics.Stopwatch();
                //System.Diagnostics.Stopwatch sw4 = new System.Diagnostics.Stopwatch();
                //System.Diagnostics.Stopwatch sw5 = new System.Diagnostics.Stopwatch();
                //System.Diagnostics.Stopwatch sw6 = new System.Diagnostics.Stopwatch();
                //System.Diagnostics.Stopwatch sw7 = new System.Diagnostics.Stopwatch();
                //sw1.Start();
                ////查询所有问题
                //var questionaire = _questionnaireRepository.GetById(Guid.Parse("AA46576C-9FD5-4CB9-87B1-CCEDBF68A92D"));//打卡问题集
                //if (questionaire == null)
                //{
                //    return null;
                //}
                //Guid[] questionGuids = JsonConvert.DeserializeObject<Guid[]>(questionaire.Question);//所有打卡问题的GUID号
                //if (questionGuids == null)
                //{
                //    return null;
                //}

                ////以第一条数据为基准判断一个打卡数据
                ////体重问题GUID
                //var weightGuid = Guid.Parse("A9F3D741-A83D-40E9-A108-89F7FC743384");

                ////查询所有的打卡数据的开始日期
                //var existingDingAnswer = _answerRepository.QueryDingList(oid, questionGuids[0]);//第一条打卡问题的回答
                //var firstAnswerDingList = existingDingAnswer.ToList();

                //Ding ding = new Ding();//一条问答
                //OneCustomerDingByPage oneCustomerDing = new OneCustomerDingByPage();//一套问答包含多条问答
                //List<OneCustomerDingByPage> oneCustomerDingList = new List<OneCustomerDingByPage>();//所有问答包含多套问答
                //sw1.Stop();
                //var s1 = sw1.ElapsedMilliseconds;
                //sw2.Start();
                //for (int i=0;i< firstAnswerDingList.Count();i++)//有第一条打卡问题的回答，就认为是一次完整的回答
                //{
                //    DateTime startTime = firstAnswerDingList[i].Ctime;
                //    oneCustomerDing = new OneCustomerDingByPage();
                //    oneCustomerDing.DingList = new List<Ding>();

                //    //第一条问答单独处理
                //    ding = new Ding();
                //    ding.QuestionGuid = questionGuids[0];
                //    ding.Answer = firstAnswerDingList[i].Content;
                //    ding.CreateTime = startTime;
                //    oneCustomerDing.DingList.Add(ding);
                //    oneCustomerDing.DingTime = startTime;

                //    //剩余问答处理
                //    for (int j = 1; j < questionGuids.Count(); j++)//j=1的原因是第一个回答已经查询到了
                //    {
                //        var answer = _answerRepository.GetDingAnswer(oid, questionGuids[j], startTime);
                //        if (answer == null)
                //        {
                //            break;//问题没有回答结束，跳出
                //        }
                //        ding = new Ding();
                //        ding.QuestionGuid = questionGuids[j];
                //        ding.Answer = answer.Content;
                //        ding.CreateTime = startTime;
                //        oneCustomerDing.DingList.Add(ding);
                //    }

                //    //汇总所有问答
                //    oneCustomerDingList.Add(oneCustomerDing);
                //}
                //sw2.Stop();
                //var s2 = sw2.ElapsedMilliseconds;
                //sw3.Start();
                ////获取所有问答中体重回答，并且去除非法数据
                //var convertFloat = 0.0f;
                //var legalAnswer =
                //    from dL in oneCustomerDingList
                //    from d in dL.DingList
                //    where d.QuestionGuid == weightGuid //打卡中的体重问题问卷唯一号
                //    && Single.TryParse(d.Answer.Substring(1, d.Answer.Length - 2), out convertFloat) == true //获取内部除了引号"之外的数据，例如："50" 提取出来50
                //    && (convertFloat >= 30)//体重范围30kg～300kg
                //    && (convertFloat <= 300)
                //    select dL;
                //sw3.Stop();
                //var s3 = sw3.ElapsedMilliseconds;
                //sw4.Start();
                ////去除当天重复数据
                //var noRepeatAnswer =
                //    from dL in legalAnswer
                //    group dL by new { dL.DingTime.Date }
                //    into mygroup
                //    select mygroup.FirstOrDefault();
                //sw4.Stop();
                //var s4 = sw4.ElapsedMilliseconds;
                //sw5.Start();
                ////转换数据，提取体重回答
                //List<CustomerWeight> noRepeatWeightList = new List<CustomerWeight>();
                //CustomerWeight customerWeight = new CustomerWeight();
                //foreach (var answer in noRepeatAnswer)
                //{
                //    customerWeight = new CustomerWeight();
                //    customerWeight.RecordTime = answer.DingTime;
                //    customerWeight.Weight = 0f;
                //    foreach (var dingWeightAnswer in answer.DingList)
                //    {
                //        if(dingWeightAnswer.QuestionGuid== weightGuid)
                //        {
                //            var tempAnswer = dingWeightAnswer.Answer.Substring(1, dingWeightAnswer.Answer.Length - 2);
                //            customerWeight.Weight = Single.TryParse(tempAnswer, out convertFloat) == true ? convertFloat : 0f;
                //            break;
                //        }
                //    }

                //    noRepeatWeightList.Add(customerWeight);
                //}
                //sw5.Stop();
                //var s5 = sw5.ElapsedMilliseconds;
                //sw6.Start();
                ////插入申请会员时，填写入的首次初始体重，身高，按照时间插入
                //var existingCustomer = _customerRepository.GetById(oid);
                //sw6.Stop();
                //var s6 = sw6.ElapsedMilliseconds;
                //sw7.Start();
                //if (existingCustomer != null)
                //{
                //    customerWeight = new CustomerWeight();
                //    customerWeight.RecordTime = existingCustomer.CreateTime;
                //    customerWeight.Weight = existingCustomer.InitWeight;
                //    noRepeatWeightList.Add(customerWeight);

                //    //新数据时间排序
                //    var newAnswer = noRepeatWeightList.OrderByDescending(t => t.RecordTime);

                //    //新数据去除当天重复数据
                //    var newNoRepeatAnswer =
                //        from dL in newAnswer
                //        group dL by new { dL.RecordTime.Date }
                //        into mygroup
                //        select mygroup.FirstOrDefault();

                //    //数据合并
                //    noRepeatWeightList = new List<CustomerWeight>();
                //    noRepeatWeightList = newNoRepeatAnswer.ToList();
                //}
                //sw7.Stop();
                //var s7 = sw7.ElapsedMilliseconds;
                //var s8 = "阶段：" + s1 + "\r\n阶段：" + s2 + "\r\n阶段：" + s3 + "\r\n阶段：" + s4 + "\r\n阶段：" + s5 + "\r\n阶段：" + s6 + "\r\n阶段：" + s7;
                #endregion 获取体重数据列表方法3

                #region 获取体重数据列表方法2
                //var questionVersion = Guid.Parse("A630D59D-3BD1-4D79-8F30-DD3118573C36");//打卡中的体重问题版本号
                //var questionGuid = Guid.Parse("A9F3D741-A83D-40E9-A108-89F7FC743384");//打卡中的体重问题问卷唯一号

                //var originalAnswer = _answerRepository.GetAnswerByConditions(oid, questionVersion, questionGuid);

                ////去除非法数据
                //var convertFloat = 0.0f;
                //var legalAnswer = (from t in originalAnswer
                //                   where
                //                   (
                //                   Single.TryParse(t.Content.Substring(1, t.Content.Length - 2), out convertFloat) == true //获取内部除了引号"之外的数据，例如："50" 提取出来50
                //                   )
                //                   && (convertFloat >= 30)
                //                   && (convertFloat <= 300)
                //                   select t
                //    );//体重范围30kg～300kg

                ////去除当天重复数据
                //var noRepeatAnswer = from t in legalAnswer
                //                     group t by new { t.Ctime.Date }
                //                     into mygroup
                //                     select mygroup.FirstOrDefault();

                ////转换后的数据
                //List<CustomerWeight> noRepeatWeightList = _mapper.Map<List<CustomerWeight>>(noRepeatAnswer);


                ////插入申请会员时，填写入的首次初始体重，身高，按照时间插入
                //var existingCustomer = _customerRepository.GetById(oid);
                //if (existingCustomer != null)
                //{
                //    CustomerWeight customerWeight = new CustomerWeight();
                //    customerWeight.RecordTime = existingCustomer.CreateTime;
                //    customerWeight.Weight = existingCustomer.InitWeight;
                //    noRepeatWeightList.Add(customerWeight);

                //    //新数据时间排序
                //    var newAnswer = noRepeatWeightList.OrderByDescending(t => t.RecordTime);

                //    //新数据去除当天重复数据
                //    var newNoRepeatAnswer = from s in newAnswer
                //                            group s by new { s.RecordTime.Date }
                //                           into mygroup
                //                            select mygroup.FirstOrDefault();

                //    //数据合并
                //    noRepeatWeightList = new List<CustomerWeight>();
                //    noRepeatWeightList = newNoRepeatAnswer.ToList();
                //}

                #endregion 获取体重数据列表方法2

                //获取前dataCount个数据
                result = new GetOneCustomerWeightListResponse();
                result.WeightList = noRepeatWeightList.Take(dataCount).ToList();
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public GetOneCustomerWXWeightListResponse GetOneCustomerWXWeightList(Guid customerOid, int dataCount)
        {
            GetOneCustomerWXWeightListResponse result;
            try
            {
                #region 获取体重数据列表方法1
                //查询打卡列表的总条数
                var existingAnswerCount = 0;

                var oneCustomerDingList = GetOneCustomerMecuryAnswerList(Guid.Parse("AA46576C-9FD5-4CB9-87B1-CCEDBF68A92D"), customerOid, 0, dataCount, ref existingAnswerCount);//所有问答包含多套问答

                //转换数据，提取体重回答
                //体重问题GUID
                var weightGuid = Guid.Parse("A9F3D741-A83D-40E9-A108-89F7FC743384");
                var convertFloat = 0.0f;
                List<CustomerWeight> noRepeatWeightList = new List<CustomerWeight>();
                CustomerWeight customerWeight = new CustomerWeight();
                foreach (var answer in oneCustomerDingList)
                {
                    customerWeight = new CustomerWeight();
                    customerWeight.RecordTime = answer.AnswerTime;
                    customerWeight.Weight = 0f;
                    foreach (var dingAnswer in answer.MecuryAnswerList)
                    {
                        if (dingAnswer.QuestionGuid == weightGuid)
                        {
                            //var tempAnswer = dingWeightAnswer.Answer.Substring(1, dingWeightAnswer.Answer.Length - 2);
                            var tempAnswer = JsonConvert.DeserializeObject(dingAnswer.Answer).ToString();
                            customerWeight.Weight = Single.TryParse(tempAnswer, out convertFloat) == true ? convertFloat : 0f;
                            break;
                        }
                    }

                    noRepeatWeightList.Add(customerWeight);
                }

                //查询客户是否存在，VIP
                var existingCustomer = _customerRepository.GetById(customerOid);
                if(existingCustomer==null)
                {
                    return null;
                }

                result = new GetOneCustomerWXWeightListResponse();
                //查询专属客服
                var existingSupporter = _supporterRepository.GetById(existingCustomer.SupporterOid);
                if(existingSupporter!=null)
                {
                    result.SupporterName = existingSupporter.UserName;
                    result.Cellphone = existingSupporter.Cellphone;
                }
                else
                {
                    result.SupporterName = "上海滇峰";
                    result.Cellphone = "021-39881115";
                }

                //获取初始体重
                if (noRepeatWeightList.Count<=0)
                {
                    result.InitWeight = existingCustomer.InitWeight;
                    result.InitRecordTime = existingCustomer.CreateTime;
                    customerWeight = new CustomerWeight();
                    customerWeight.Weight = existingCustomer.InitWeight;
                    customerWeight.RecordTime = existingCustomer.CreateTime;
                    noRepeatWeightList.Add(customerWeight);
                }
                else
                {
                    //查询所有问题
                    var questionaire = _questionnaireRepository.GetById(Guid.Parse("AA46576C-9FD5-4CB9-87B1-CCEDBF68A92D"));//打卡问题集
                    if (questionaire == null)
                    {
                        return null;
                    }
                    Guid[] questionGuids = JsonConvert.DeserializeObject<Guid[]>(questionaire.Question);
                    if (questionGuids == null)
                    {
                        return null;
                    }

                    //以第一条数据为基准判断一个打卡数据
                    //查询打卡列表的总条数
                    var existingDingCount = _answerRepository.QueryMecuryAnswerListCount(customerOid, questionGuids[0]);//第一条打卡问题

                    //查询所有的打卡数据的开始日期
                    var existingDing = _answerRepository.QueryMecuryAnswerList(customerOid, questionGuids[0]);//第一条打卡问题

                    //获取最老的首次回答
                    var firstAnswerDing = existingDing.LastOrDefault();
                    if(firstAnswerDing!=null)
                    {
                        result.InitRecordTime = firstAnswerDing.Ctime;
                    }

                    //获取最老的体重回答答案
                    var firstWeightAnswerDing = _answerRepository.GetFirstAnswerContent(customerOid, weightGuid);
                    if(firstWeightAnswerDing!=null)
                    {
                        var tempAnswer = JsonConvert.DeserializeObject(firstWeightAnswerDing.Content).ToString();
                        result.InitWeight= Single.TryParse(tempAnswer, out convertFloat) == true ? convertFloat : 0f;
                    }
                }
                #endregion 获取体重数据列表方法1

                //获取前dataCount个数据
                result.WeightList = noRepeatWeightList.Take(dataCount).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }

        /// <summary>
        /// 私有方法，获取指定客户的指定回答列表
        /// </summary>
        /// <param name="questionnaireGuid"></param>
        /// <param name="oid"></param>
        /// <param name="skipCount"></param>
        /// <param name="takeCount"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        private List<OneCustomerMecuryAnswerByPage> GetOneCustomerMecuryAnswerList(Guid questionnaireGuid,Guid oid,int skipCount, int takeCount,ref int totalCount)
        {
            List<OneCustomerMecuryAnswerByPage> oneCustomerMecuryAnswerList = new List<OneCustomerMecuryAnswerByPage>();//所有问答包含多套问答
            try
            {
                //查询所有问题
                var questionaire = _questionnaireRepository.GetById(questionnaireGuid);//打卡问题集
                if (questionaire == null)
                {
                    return null;
                }
                Guid[] questionGuids = JsonConvert.DeserializeObject<Guid[]>(questionaire.Question);//所有打卡问题的GUID号
                if (questionGuids == null)
                {
                    return null;
                }

                //以第一条数据为基准判断一个打卡数据
                //查询所有的打卡数据的开始日期
                var existingMecuryAnswer = _answerRepository.QueryMecuryAnswerList(oid, questionGuids[0]);//第一条问题的回答

                //去除当天重复数据
                var noRepeatAnswer =
                    from dL in existingMecuryAnswer
                    group dL by new { dL.Ctime.Date }
                    into mygroup
                    select mygroup.FirstOrDefault();

                //所有问答的第一个问题的回答
                totalCount = noRepeatAnswer.Count();
                var firstAnswerList = noRepeatAnswer.Skip(skipCount).Take(takeCount).ToList();

                //添加所有的回答
                MecuryAnswer mecuryAnswer = new MecuryAnswer();//一条问答
                OneCustomerMecuryAnswerByPage oneCustomerMecuryAnswer = new OneCustomerMecuryAnswerByPage();//一套问答包含多条问答
                for (int i = 0; i < firstAnswerList.Count(); i++)//有第一条问答问题的回答，就认为是应该有完整的回答
                {
                    DateTime startTime = firstAnswerList[i].Ctime;
                    oneCustomerMecuryAnswer = new OneCustomerMecuryAnswerByPage();
                    oneCustomerMecuryAnswer.MecuryAnswerList = new List<MecuryAnswer>();

                    //第一条问答单独处理
                    mecuryAnswer = new MecuryAnswer();
                    mecuryAnswer.QuestionGuid = questionGuids[0];
                    mecuryAnswer.Answer = firstAnswerList[i].Content;
                    mecuryAnswer.CreateTime = startTime;
                    oneCustomerMecuryAnswer.MecuryAnswerList.Add(mecuryAnswer);
                    oneCustomerMecuryAnswer.AnswerTime = startTime;

                    //剩余问答处理
                    for (int j = 1; j < questionGuids.Count(); j++)//j=1的原因是第一个回答已经查询到了
                    {
                        var answer = _answerRepository.GetDingAnswer(oid, questionGuids[j], startTime);
                        if (answer == null)
                        {
                            //两道性别相关的问题用户必然缺失一道
                            //if (questionGuids[j] == Guid.Parse("23E317C9-D691-402E-8249-C0E4D3ED78D0") || questionGuids[j] == Guid.Parse("A6C1DBC2-233D-4FA8-927B-8BAED0F709B3"))
                            //{
                            //    continue;
                            //}
                            continue;//问题没有回答结束，跳出
                        }
                        mecuryAnswer = new MecuryAnswer();
                        mecuryAnswer.QuestionGuid = questionGuids[j];
                        mecuryAnswer.Answer = answer.Content;
                        mecuryAnswer.CreateTime = startTime;
                        oneCustomerMecuryAnswer.MecuryAnswerList.Add(mecuryAnswer);
                    }

                    //汇总所有问答
                    oneCustomerMecuryAnswerList.Add(oneCustomerMecuryAnswer);
                }
            }
            catch(Exception)
            {
                return null;
            }
            return oneCustomerMecuryAnswerList;
        }

        public GetOneCustomerBMIListResponse GetOneCustomerBMIList(Guid oid, int dataCount)
        {
            GetOneCustomerBMIListResponse result;
            try
            {
                #region 获取BMI数据列表方法1
                //查询打卡列表的总条数
                var existingAnswerCount = 0;

                var oneCustomerDingList = GetOneCustomerMecuryAnswerList(Guid.Parse("AA46576C-9FD5-4CB9-87B1-CCEDBF68A92D"),oid,0, dataCount,ref existingAnswerCount);//所有问答包含多套问答

                //转换数据，提取体重，以及身高回答
                //体重问题GUID
                var weightGuid = Guid.Parse("A9F3D741-A83D-40E9-A108-89F7FC743384");
                var convertFloat = 0.0f;
                List<CustomerWeight> noRepeatWeightList = new List<CustomerWeight>();
                CustomerWeight customerWeight = new CustomerWeight();

                //身高问题GUID
                var heightGuid = Guid.Parse("B180EAC0-127D-4ECB-BDB6-C2599D310BD4");
                List<CustomerHeight> noRepeatHeightList = new List<CustomerHeight>();
                CustomerHeight customerHeight = new CustomerHeight();

                foreach (var answer in oneCustomerDingList)
                {
                    //体重
                    customerWeight = new CustomerWeight();
                    customerWeight.RecordTime = answer.AnswerTime;
                    customerWeight.Weight = 0f;

                    //身高
                    customerHeight = new CustomerHeight();
                    customerHeight.RecordTime = answer.AnswerTime;
                    customerHeight.Height = 0f;

                    int findBoth = 0;
                    foreach (var dingAnswer in answer.MecuryAnswerList)
                    {
                        if (dingAnswer.QuestionGuid == weightGuid)
                        {
                            //var tempAnswer = dingWeightAnswer.Answer.Substring(1, dingWeightAnswer.Answer.Length - 2);
                            var tempAnswer = JsonConvert.DeserializeObject(dingAnswer.Answer).ToString();
                            customerWeight.Weight = Single.TryParse(tempAnswer, out convertFloat) == true ? convertFloat : 0f;
                            findBoth++;
                        }

                        if (dingAnswer.QuestionGuid == heightGuid)
                        {
                            //var tempAnswer = dingHeightAnswer.Answer.Substring(1, dingHeightAnswer.Answer.Length - 2);
                            var tempAnswer = JsonConvert.DeserializeObject(dingAnswer.Answer).ToString();
                            customerHeight.Height = Single.TryParse(tempAnswer, out convertFloat) == true ? convertFloat : 0f;
                            findBoth++;
                        }

                        if (findBoth>=2)
                        {
                            break;
                        }
                    }
                    //体重
                    noRepeatWeightList.Add(customerWeight);

                    //身高
                    noRepeatHeightList.Add(customerHeight);
                }

                //插入申请会员时，填写入的首次初始体重，身高，按照时间插入
                var existingCustomer = _customerRepository.GetById(oid);//体重时，已经获取过
                if (existingCustomer != null)
                {
                    customerWeight = new CustomerWeight();
                    customerWeight.RecordTime = existingCustomer.CreateTime;
                    customerWeight.Weight = existingCustomer.InitWeight;
                    noRepeatWeightList.Add(customerWeight);

                    customerHeight = new CustomerHeight();
                    customerHeight.RecordTime = existingCustomer.CreateTime;
                    customerHeight.Height = existingCustomer.InitHeight;
                    noRepeatHeightList.Add(customerHeight);

                    //新数据时间排序
                    var newWeightAnswer = noRepeatWeightList.OrderByDescending(t => t.RecordTime);
                    var newHeightAnswer = noRepeatHeightList.OrderByDescending(t => t.RecordTime);

                    //新数据去除当天重复数据
                    var newNoRepeatWeightAnswer =
                        from s in newWeightAnswer
                        group s by new { s.RecordTime.Date }
                        into mygroup
                        select mygroup.FirstOrDefault();

                    var newNoRepeatHeightAnswer =
                        from s in newHeightAnswer
                        group s by new { s.RecordTime.Date }
                        into mygroup
                        select mygroup.FirstOrDefault();

                    //数据合并
                    noRepeatWeightList = new List<CustomerWeight>();
                    noRepeatWeightList = newNoRepeatWeightAnswer.Take(dataCount).ToList();

                    noRepeatHeightList = new List<CustomerHeight>();
                    noRepeatHeightList = newNoRepeatHeightAnswer.Take(dataCount).ToList();
                }
                //计算BMI
                float currentWeight = 0.0f;//当前体重
                float currentHeight = 0.0f;//当前身高
                List<CustomerBMI> noRepeatIBMList = new List<CustomerBMI>();
                for (int i=0;i<noRepeatHeightList.Count;i++)
                {
                    currentWeight = 0.0f; 
                    currentHeight = 0.0f;
                    try
                    {
                        currentWeight= noRepeatWeightList[i].Weight;//初始化
                        currentHeight = noRepeatHeightList[i].Height;//初始化
                        if(currentWeight != 0&&currentHeight!=0)
                        {
                            CustomerBMI customerBMI = new CustomerBMI();
                            customerBMI.RecordTime = noRepeatWeightList[i].RecordTime;
                            customerBMI.BMI = Convert.ToSingle(currentWeight / Math.Pow(currentHeight / 100, 2));
                            noRepeatIBMList.Add(customerBMI);
                        }
                        else
                        {
                            noRepeatIBMList.Add(new CustomerBMI { RecordTime = noRepeatWeightList[i].RecordTime, BMI = 0 });
                        }
                    }
                    catch(Exception)
                    {
                        noRepeatIBMList.Add(new CustomerBMI { RecordTime = noRepeatWeightList[i].RecordTime, BMI = 0 });
                    }

                }
                #endregion 获取BMI数据列表方法1

                #region 获取BMI数据列表方法2
                //#region 获取体重数据列表
                //var questionVersion = Guid.Parse("A630D59D-3BD1-4D79-8F30-DD3118573C36");//打卡中的体重问题版本号
                //var questionGuid = Guid.Parse("A9F3D741-A83D-40E9-A108-89F7FC743384");//打卡中的体重问题问卷唯一号

                //var originalAnswer = _answerRepository.GetAnswerByConditions(oid, questionVersion, questionGuid);

                ////去除非法数据
                //var convertFloat = 0.0f;
                //var legalAnswer = (from t in originalAnswer
                //                   where
                //                   (
                //                   //Single.TryParse(JsonConvert.DeserializeObject(t.Content).ToString(), out convertFloat) == true //获取内部除了引号"之外的数据，例如："50" 提取出来50
                //                   Single.TryParse(t.Content.Substring(1, t.Content.Length - 2), out convertFloat) == true //获取内部除了引号"之外的数据，例如："50" 提取出来50
                //                   )
                //                   && (convertFloat >= 30)
                //                   && (convertFloat <= 300)
                //                   select t
                //                    );//体重范围30kg～300kg

                ////去除当天重复数据
                //var noRepeatAnswer = from t in legalAnswer
                //                     group t by new { t.Ctime.Date }
                //                     into mygroup
                //                     select mygroup.FirstOrDefault();
                ////转换后的数据
                //List<CustomerWeight> noRepeatWeightList = _mapper.Map<List<CustomerWeight>>(noRepeatAnswer);

                ////插入申请会员时，填写入的首次初始体重，身高，按照时间插入
                //var existingCustomer = _customerRepository.GetById(oid);
                //if (existingCustomer != null)
                //{
                //    CustomerWeight customerWeight = new CustomerWeight();
                //    customerWeight.RecordTime = existingCustomer.CreateTime;
                //    customerWeight.Weight = existingCustomer.InitWeight;
                //    noRepeatWeightList.Add(customerWeight);

                //    //新数据时间排序
                //    var newAnswer = noRepeatWeightList.OrderByDescending(t => t.RecordTime);

                //    //新数据去除当天重复数据
                //    var newNoRepeatAnswer = from s in newAnswer
                //                            group s by new { s.RecordTime.Date }
                //                           into mygroup
                //                            select mygroup.FirstOrDefault();

                //    //数据合并
                //    noRepeatWeightList = new List<CustomerWeight>();
                //    noRepeatWeightList = newNoRepeatAnswer.ToList();
                //}
                //#endregion 获取体重数据列表

                //#region 获取身高数据列表
                //questionVersion = Guid.Parse("358CA924-8DF2-47F8-8AEC-7612D4B3BD97");//打卡中的身高问题版本号
                //questionGuid = Guid.Parse("B180EAC0-127D-4ECB-BDB6-C2599D310BD4");//打卡中的身高问题问卷唯一号

                //originalAnswer = _answerRepository.GetAnswerByConditions(oid, questionVersion, questionGuid);

                ////去除非法数据
                //convertFloat = 0.0f;
                //legalAnswer = (from t in originalAnswer
                //               where
                //               (
                //               //Single.TryParse(JsonConvert.DeserializeObject(t.Content) != null ? JsonConvert.DeserializeObject(t.Content).ToString() : null, out convertFloat) == true //获取内部除了引号"之外的数据，例如："50" 提取出来50
                //               Single.TryParse(t.Content.Substring(1, t.Content.Length - 2), out convertFloat) == true //获取内部除了引号"之外的数据，例如："50" 提取出来50
                //               )
                //               && (convertFloat >= 50)
                //               && (convertFloat <= 300)
                //               select t
                //                );//身高范围50cm～300cm

                ////去除当天重复数据
                //noRepeatAnswer = from t in legalAnswer
                //                 group t by new { t.Ctime.Date }
                //                     into mygroup
                //                 select mygroup.FirstOrDefault();

                ////转换后的数据
                //List<CustomerHeight> noRepeatHeightList = _mapper.Map<List<CustomerHeight>>(noRepeatAnswer);

                ////插入申请会员时，填写入的首次初始体重，身高，按照时间插入
                ////var existingCustomer = _customerRepository.GetById(oid);//体重时，已经获取过
                //if (existingCustomer != null)
                //{
                //    CustomerHeight customerHeight = new CustomerHeight();
                //    customerHeight.RecordTime = existingCustomer.CreateTime;
                //    customerHeight.Height = existingCustomer.InitHeight;
                //    noRepeatHeightList.Add(customerHeight);

                //    //新数据时间排序
                //    var newAnswer = noRepeatHeightList.OrderByDescending(t => t.RecordTime);

                //    //新数据去除当天重复数据
                //    var newNoRepeatAnswer = from s in newAnswer
                //                            group s by new { s.RecordTime.Date }
                //                           into mygroup
                //                            select mygroup.FirstOrDefault();

                //    //数据合并
                //    noRepeatHeightList = new List<CustomerHeight>();
                //    noRepeatHeightList = newNoRepeatAnswer.ToList();
                //}
                //#endregion 获取身高数据列表

                //#region 处理BMI数值，以最少数据为基准，获取的同一天数据进行处理
                //float currentWeight = 0.0f;//当前体重
                //float currentHeight = 0.0f;//当前身高
                //List<CustomerBMI> noRepeatIBMList = new List<CustomerBMI>();
                //if (noRepeatWeightList.Count == noRepeatHeightList.Count && noRepeatWeightList.Count == 0)
                //{
                //    result = new GetOneCustomerBMIListResponse();
                //    result.BMIList = new List<CustomerBMI>();
                //    return result;
                //}

                //if (noRepeatWeightList.Count <= noRepeatHeightList.Count)
                //{
                //    for (int i = 0; i < noRepeatWeightList.Count; i++)
                //    {
                //        currentWeight = 0.0f;//初始化
                //        currentHeight = 0.0f;//初始化

                //        currentWeight = noRepeatWeightList[i].Weight;//当前体重
                //        for (int j = 0; j < noRepeatHeightList.Count; j++)
                //        {
                //            if (string.Equals(noRepeatWeightList[i].RecordTime.Date, noRepeatHeightList[j].RecordTime.Date))
                //            {
                //                currentHeight = noRepeatHeightList[j].Height;
                //                break;
                //            }
                //        }

                //        if (currentWeight != 0 && currentHeight != 0)
                //        {
                //            CustomerBMI customerBMI = new CustomerBMI();
                //            customerBMI.RecordTime = noRepeatWeightList[i].RecordTime;
                //            customerBMI.BMI = Convert.ToSingle(currentWeight / Math.Pow(currentHeight / 100, 2));
                //            noRepeatIBMList.Add(customerBMI);
                //        }
                //        else
                //        {
                //            noRepeatIBMList.Add(new CustomerBMI { RecordTime = noRepeatWeightList[i].RecordTime, BMI = 0 });
                //        }
                //    }
                //}
                //else
                //{
                //    for (int i = 0; i < noRepeatHeightList.Count; i++)
                //    {
                //        currentWeight = 0.0f;//初始化
                //        currentHeight = 0.0f;//初始化

                //        currentHeight = noRepeatHeightList[i].Height;//当前身高
                //        for (int j = 0; j < noRepeatWeightList.Count; j++)
                //        {
                //            if (string.Equals(noRepeatHeightList[i].RecordTime.Date, noRepeatWeightList[j].RecordTime.Date))
                //            {
                //                currentWeight = noRepeatWeightList[j].Weight;
                //                break;
                //            }
                //        }

                //        if (currentWeight != 0 && currentHeight != 0)
                //        {
                //            CustomerBMI customerBMI = new CustomerBMI();
                //            customerBMI.RecordTime = noRepeatHeightList[i].RecordTime;
                //            customerBMI.BMI = Convert.ToSingle(currentWeight / Math.Pow(currentHeight / 100, 2));
                //            noRepeatIBMList.Add(customerBMI);
                //        }
                //        else
                //        {
                //            noRepeatIBMList.Add(new CustomerBMI { RecordTime = noRepeatHeightList[i].RecordTime, BMI = 0 });
                //        }
                //    }
                //}

                //#endregion 处理BMI数值，以最少数据为基准，获取的同一天数据进行处理
                #endregion 获取BMI数据列表方法2

                //获取前dataCount个数据
                result = new GetOneCustomerBMIListResponse();
                result.BMIList = noRepeatIBMList.Take(dataCount).ToList();
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public QueryOneCustomerPhysiqueListByPageResponse QueryOneCustomerPhysiqueListByPage(Guid oid, int pageIndex, int pageSize)
        {
            QueryOneCustomerPhysiqueListByPageResponse result;
            try
            {
                //查询所有问题
                var questionaire = _questionnaireRepository.GetById(Guid.Parse("B546B709-2B2B-4F6E-9F1F-64F281DE8D5B"));//打卡问题集
                if (questionaire == null)
                {
                    return null;
                }
                Guid[] questionGuids = JsonConvert.DeserializeObject<Guid[]>(questionaire.Question);//所有打卡问题的GUID号
                if (questionGuids == null)
                {
                    return null;
                }
                //查询打卡列表的总条数
                var existingAnswerCount = 0;

                //查询所有回答
                var oneCustomerAnswerList = GetOneCustomerMecuryAnswerList(Guid.Parse("B546B709-2B2B-4F6E-9F1F-64F281DE8D5B"), oid, pageIndex * pageSize, pageSize,ref existingAnswerCount);//所有问答包含多套问答

                //计分板
                Dictionary<string, double> scoreBoard = new Dictionary<string, double>
                {
                {"阴虚质",0d },
                {"阳虚质",0d },
                {"气虚质",0d },
                {"气郁质",0d },
                {"血瘀质",0d },
                {"湿热质",0d },
                {"痰湿质",0d },
                {"特禀质",0d },
                {"平和质",0d }
                };

                //创建数组，储存每道问题属于哪一种体质
                #region 题目类型
                string jsonString =
                    "[" +
                    "{\"Constitution\":\"平和质\",\"QuestionGuid\":\"ae4b39ed-501e-4654-a16c-45ce98fc53e2\"}" +
                    ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"06bb9ddc-1725-4900-8ea8-9c2f1f2d24ca\"}" +
                    ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"5ea1223c-9497-4530-a0c2-a66bea9dbd2f\"}" +
                    ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"7b17ef28-bf0e-4635-ad1d-b68f22dd60a6\"}" +
                    ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"c6dd0bee-f6f5-46f6-8cdb-e278ff6ba5c7\"}" +
                    ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"966ae059-f094-4f7b-8045-eaa05e539944\"}" +
                    ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"01220615-766b-4ded-9097-eb15f0287507\"}" +
                    ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"48fa4d9e-01a1-44d6-9226-efe6ea4a2dbe\"}" +
                    ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"f721faf1-602e-4167-82f0-0b0fc7ca5974\"}" +
                    ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"1ae0eac1-8b9c-4d13-bf3b-134f0f1ab60c\"}" +
                    ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"be14714a-d174-4180-a330-1c601d6e79b9\"}" +
                    ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"a857027c-71ea-48ef-b991-23104db9dc4c\"}" +
                    ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"8f4d90f2-055f-4d99-8c2e-4c115e290d5b\"}" +
                    ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"3d1df631-aad5-4786-8f46-76c768f42133\"}" +
                    ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"66c27b32-e168-404c-b20e-7bf5c895c143\"}" +
                    ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"86cd1cff-5029-4ce7-90bf-cab9710a7e5d\"}" +
                    ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"02f5d0f4-a4d8-41d1-8ee2-08d8f5778d2b\"}" +
                    ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"683a60a8-ced6-480c-89f7-1dcf7eac829d\"}" +
                    ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"db72a506-edd9-4d7f-981b-57b6c36e0fcd\"}" +
                    ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"c58bbc67-131c-4872-a610-6679f9fa4cf9\"}" +
                    ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"bdb48675-8933-4e74-a824-9442bf11a600\"}" +
                    ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"d01ea475-3cc8-4c5b-966f-a8bd762a1081\"}" +
                    ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"8eafa69f-8fb9-411f-9f9b-cea3391db8c3\"}" +
                    ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"604045a2-92c3-4da8-a675-46a162d68d9d\"}" +
                    ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"9df2ecc2-07dc-4253-b7c4-504515396dd2\"}" +
                    ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"a6c1dbc2-233d-4fa8-927b-8baed0f709b3\"}" +
                    ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"7ca4bddd-8b36-4454-8513-bec3100ec2a0\"}" +
                    ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"23e317c9-d691-402e-8249-c0e4d3ed78d0\"}" +
                    ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"5d2abf28-71c5-470d-959a-c75237f0689e\"}" +
                    ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"856a18ee-9d37-4348-9a1a-ff3fca7ca2e4\"}" +
                    ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"f6180ada-9871-45c4-aadd-28ed8b1b1841\"}" +
                    ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"4eff98fe-1957-44b5-b084-37bf67e5356c\"}" +
                    ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"23982696-2722-4dc3-a1e2-524d72a38197\"}" +
                    ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"b1a5da63-a4fc-4505-b71e-86dacbcec6f8\"}" +
                    ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"6d18ad7c-2781-4b73-ad9b-9186b2e87fde\"}" +
                    ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"16b8dbc7-4d7f-42c1-90c2-aae5e8870549\"}" +
                    ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"824b8f3c-7df5-4ed0-8473-ba167a94dcec\"}" +
                    ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"068e7f5b-a70e-46ab-8186-e1ed89e20a96\"}" +
                    ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"b26af282-ed74-45cc-95ed-03e55739d0d9\"}" +
                    ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"cacf9241-580e-485b-8c39-4cd474319d11\"}" +
                    ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"7744a6a9-8dc8-4491-b62a-58b0bbb97468\"}" +
                    ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"8f6ea2be-e7a1-4120-a016-5e8b50c6f5f2\"}" +
                    ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"f74f9607-73aa-468b-90a4-6e0f435b7455\"}" +
                    ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"99c780f6-b005-4d40-9660-6e565baf556c\"}" +
                    ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"d15cf76c-c8b6-46d8-9ce5-c2754ee33db9\"}" +
                    ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"0324dd99-8371-42c4-a6ef-0c558c88a40b\"}" +
                    ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"0b4231ed-1340-4a57-832f-38c0f8b3d795\"}" +
                    ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"4669abf8-d1f7-42e7-a96b-3df34da8dbc0\"}" +
                    ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"3b8c85d9-a8df-404e-8410-4669817d8139\"}" +
                    ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"c05d4411-3043-4d72-bd28-ecdca6a6a1a1\"}" +
                    ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"393cd5f8-1bcf-434f-9208-f64a497622af\"}" +
                    ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"dff885fd-df8c-4075-9484-fc3e9afe8e7d\"}" +
                    ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"e1f6f091-3d1a-42c0-abf6-07babde77e93\"}" +
                    ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"d9228fcf-5467-49ce-861f-67e603b8e5c0\"}" +
                    ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"78bc2059-ade0-4478-93ab-9314e8d817c3\"}" +
                    ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"e585e827-3b70-4b8c-9c86-9f7fe59df6d0\"}" +
                    ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"6a51c98d-2900-4cd5-8eab-a20b405533ca\"}" +
                    ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"4460bf86-7caa-4180-bd8b-e19f04596adc\"}" +
                    ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"29dfedf4-4ddf-4a23-99f3-fc399e15df1e\"}" +
                    ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"79b98ed2-58e7-465f-9985-12041dfb6332\"}" +
                    ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"25801e21-5e5d-4b70-b688-30b273ee8b23\"}" +
                    ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"c0290174-8f90-405d-86c3-5de55a814a7c\"}" +
                    ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"dff9548d-d0f1-4c62-80ec-6b1444c76ee0\"}" +
                    ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"fdf47022-406f-470c-860f-711a03c6cf4f\"}" +
                    ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"1433f352-b2d6-41ab-8c59-75bbdb9a8a53\"}" +
                    ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"6b354226-1925-4d0e-959c-cc2dc731e9cd\"}" +
                    ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"f2effb5f-b75f-4be4-bcb0-eac877a8f344\"}" +
                    "]";
                #endregion 题目类型

                //记录每一道问题属于哪种体质的数组
                QuestionConstitution[] questionConstitutions = JsonConvert.DeserializeObject<QuestionConstitution[]>(jsonString);

                bool answerComplete = true;
                List<OneCustomerPhysiqueByPage> customerPhysiqueList = new List<OneCustomerPhysiqueByPage>();
                int i = 0;//用于编号的累加
                foreach (var oneQuestionaireAnswer in oneCustomerAnswerList)
                {
                    answerComplete = true;
                    foreach (var question in questionGuids)
                    {
                        try
                        {
                            scoreBoard[questionConstitutions.First(x => x.QuestionGuid == question).Constitution] += GetScore(question, oneQuestionaireAnswer.MecuryAnswerList.First(x => x.QuestionGuid == question).Answer, questionConstitutions);
                        }
                        catch (InvalidOperationException ex) when (ex.Message == "Sequence contains no matching element" || ex.Message == "Sequence contains no elements")
                        {
                            //两道性别相关的问题用户必然缺失一道，捕获异常
                            if (question == Guid.Parse("23E317C9-D691-402E-8249-C0E4D3ED78D0") || question == Guid.Parse("A6C1DBC2-233D-4FA8-927B-8BAED0F709B3"))
                            {
                                continue;
                            }
                            else
                            {
                                //如果用户未能答完问卷，将返回此错误
                                answerComplete = false;
                            }
                        }
                        catch(Exception)
                        {
                            //如果用户未能答完问卷，将返回此错误
                            answerComplete = false;
                        }
                    }

                    //体质测试结果
                    OneCustomerPhysiqueByPage customerPhysique = new OneCustomerPhysiqueByPage();
                    //结果列表
                    List<Physique> physiqueList = new List<Physique>();

                    #region 统计体质结果列表

                    //表现为平和质
                    bool mayPingheCons = true;
                    //倾向于平和质
                    bool tendPingheCons = true;

                    //遍历计分板的九个key
                    foreach (KeyValuePair<string, double> score in scoreBoard)
                    {
                        //平和质计算需要其他体质的计算结果，先跳过
                        if (score.Key == "平和质")
                        {
                            continue;
                        }
                        else
                        {
                            int questionCount;
                            switch (score.Key)
                            {
                                case "湿热质":
                                    questionCount = 6;
                                    break;
                                case "阳虚质":
                                case "血瘀质":
                                case "气郁质":
                                case "特禀质":
                                    questionCount = 7;
                                    break;
                                case "阴虚质":
                                case "气虚质":
                                case "痰湿质":
                                    questionCount = 8;
                                    break;
                                default:
                                    throw new InvalidOperationException("体质定义错误");
                            }
                            double transScore = (score.Value - questionCount) * 100 / questionCount / 4;
                            if (transScore >= 40)
                            {
                                physiqueList.Add(new Physique { Title = score.Key, CurrentCent = transScore, StandCent = 40 });
                                //当体质偏颇时，不符合平和质标准，置false
                                mayPingheCons = false;
                                tendPingheCons = false;
                            }
                            else if (transScore <= 39 && transScore >= 30)
                            {
                                physiqueList.Add(new Physique { Title = $"倾向{score.Key}", CurrentCent = transScore, StandCent = 30 });
                                mayPingheCons = false;
                            }

                        }
                    }

                    //如果没有任何体质偏颇，检查是否符合平和质标准
                    if (tendPingheCons)
                    {
                        double transScore = (scoreBoard["平和质"] - 8) * 25 / 8;
                        if (transScore >= 60)
                        {
                            physiqueList.Add(new Physique { Title = mayPingheCons ? "平和质" : "倾向平和质", CurrentCent = transScore, StandCent = 60 });
                        }
                        else
                        {
                            physiqueList.Add(new Physique { Title = "平和质", CurrentCent = transScore, StandCent = 0 });
                        }
                    }
                    #endregion 统计体质结果列表

                    customerPhysique.ID = (pageIndex * pageSize) + i + 1;
                    customerPhysique.RecordTime = oneQuestionaireAnswer.AnswerTime;
                    customerPhysique.Physique = physiqueList;
                    if (answerComplete == false)
                    {
                        //当前日期回答不完整
                        customerPhysique.PhysiqueStatus = -1;
                        customerPhysique.PhysiqueDescription = "问题回答不完整，结论可能不准确";
                    }

                    customerPhysiqueList.Add(customerPhysique);
                    i++;
                }

                //获取前dataCount个数据
                result = new QueryOneCustomerPhysiqueListByPageResponse();
                result.PhysiqueList = new PageModel<OneCustomerPhysiqueByPage>();
                result.PhysiqueList.Data = customerPhysiqueList.ToList();

                //增加页码信息
                result.PhysiqueList.PageIndex = pageIndex + 1;//页码索引
                result.PhysiqueList.PageSize = pageSize;//每页大小
                result.PhysiqueList.DataCount = existingAnswerCount;//数据总数
                result.PhysiqueList.PageCount = Convert.ToInt32(Math.Ceiling(existingAnswerCount * 1.0d / pageSize));//总页数
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        /// <summary>
        /// ------来源于Mercury程序中【TraditionalMedicalConstitutionController】
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public GetOneCustomerPhysiqueResponse GetOneCustomerPhysique(Guid oid)
        {
            GetOneCustomerPhysiqueResponse result;
            try
            {
                //查询所有问题
                var questionaire = _questionnaireRepository.GetById(Guid.Parse("B546B709-2B2B-4F6E-9F1F-64F281DE8D5B"));//打卡问题集
                if (questionaire == null)
                {
                    return null;
                }
                Guid[] questionGuids = JsonConvert.DeserializeObject<Guid[]>(questionaire.Question);//所有打卡问题的GUID号
                if (questionGuids == null)
                {
                    return null;
                }

                //查询所有回答
                //查询打卡列表的总条数
                var existingAnswerCount = 0;

                var oneCustomerAnswerList = GetOneCustomerMecuryAnswerList(Guid.Parse("B546B709-2B2B-4F6E-9F1F-64F281DE8D5B"), oid,0, 1,ref existingAnswerCount);//所有问答包含多套问答

                //计分板
                Dictionary<string, double> scoreBoard = new Dictionary<string, double>
                {
                {"阴虚质",0d },
                {"阳虚质",0d },
                {"气虚质",0d },
                {"气郁质",0d },
                {"血瘀质",0d },
                {"湿热质",0d },
                {"痰湿质",0d },
                {"特禀质",0d },
                {"平和质",0d }
                };

                //创建数组，储存每道问题属于哪一种体质
                #region 题目类型
                string jsonString =
                    "[" +
                    "{\"Constitution\":\"平和质\",\"QuestionGuid\":\"ae4b39ed-501e-4654-a16c-45ce98fc53e2\"}" +
                    ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"06bb9ddc-1725-4900-8ea8-9c2f1f2d24ca\"}" +
                    ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"5ea1223c-9497-4530-a0c2-a66bea9dbd2f\"}" +
                    ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"7b17ef28-bf0e-4635-ad1d-b68f22dd60a6\"}" +
                    ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"c6dd0bee-f6f5-46f6-8cdb-e278ff6ba5c7\"}" +
                    ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"966ae059-f094-4f7b-8045-eaa05e539944\"}" +
                    ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"01220615-766b-4ded-9097-eb15f0287507\"}" +
                    ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"48fa4d9e-01a1-44d6-9226-efe6ea4a2dbe\"}" +
                    ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"f721faf1-602e-4167-82f0-0b0fc7ca5974\"}" +
                    ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"1ae0eac1-8b9c-4d13-bf3b-134f0f1ab60c\"}" +
                    ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"be14714a-d174-4180-a330-1c601d6e79b9\"}" +
                    ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"a857027c-71ea-48ef-b991-23104db9dc4c\"}" +
                    ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"8f4d90f2-055f-4d99-8c2e-4c115e290d5b\"}" +
                    ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"3d1df631-aad5-4786-8f46-76c768f42133\"}" +
                    ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"66c27b32-e168-404c-b20e-7bf5c895c143\"}" +
                    ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"86cd1cff-5029-4ce7-90bf-cab9710a7e5d\"}" +
                    ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"02f5d0f4-a4d8-41d1-8ee2-08d8f5778d2b\"}" +
                    ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"683a60a8-ced6-480c-89f7-1dcf7eac829d\"}" +
                    ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"db72a506-edd9-4d7f-981b-57b6c36e0fcd\"}" +
                    ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"c58bbc67-131c-4872-a610-6679f9fa4cf9\"}" +
                    ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"bdb48675-8933-4e74-a824-9442bf11a600\"}" +
                    ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"d01ea475-3cc8-4c5b-966f-a8bd762a1081\"}" +
                    ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"8eafa69f-8fb9-411f-9f9b-cea3391db8c3\"}" +
                    ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"604045a2-92c3-4da8-a675-46a162d68d9d\"}" +
                    ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"9df2ecc2-07dc-4253-b7c4-504515396dd2\"}" +
                    ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"a6c1dbc2-233d-4fa8-927b-8baed0f709b3\"}" +
                    ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"7ca4bddd-8b36-4454-8513-bec3100ec2a0\"}" +
                    ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"23e317c9-d691-402e-8249-c0e4d3ed78d0\"}" +
                    ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"5d2abf28-71c5-470d-959a-c75237f0689e\"}" +
                    ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"856a18ee-9d37-4348-9a1a-ff3fca7ca2e4\"}" +
                    ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"f6180ada-9871-45c4-aadd-28ed8b1b1841\"}" +
                    ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"4eff98fe-1957-44b5-b084-37bf67e5356c\"}" +
                    ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"23982696-2722-4dc3-a1e2-524d72a38197\"}" +
                    ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"b1a5da63-a4fc-4505-b71e-86dacbcec6f8\"}" +
                    ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"6d18ad7c-2781-4b73-ad9b-9186b2e87fde\"}" +
                    ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"16b8dbc7-4d7f-42c1-90c2-aae5e8870549\"}" +
                    ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"824b8f3c-7df5-4ed0-8473-ba167a94dcec\"}" +
                    ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"068e7f5b-a70e-46ab-8186-e1ed89e20a96\"}" +
                    ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"b26af282-ed74-45cc-95ed-03e55739d0d9\"}" +
                    ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"cacf9241-580e-485b-8c39-4cd474319d11\"}" +
                    ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"7744a6a9-8dc8-4491-b62a-58b0bbb97468\"}" +
                    ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"8f6ea2be-e7a1-4120-a016-5e8b50c6f5f2\"}" +
                    ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"f74f9607-73aa-468b-90a4-6e0f435b7455\"}" +
                    ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"99c780f6-b005-4d40-9660-6e565baf556c\"}" +
                    ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"d15cf76c-c8b6-46d8-9ce5-c2754ee33db9\"}" +
                    ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"0324dd99-8371-42c4-a6ef-0c558c88a40b\"}" +
                    ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"0b4231ed-1340-4a57-832f-38c0f8b3d795\"}" +
                    ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"4669abf8-d1f7-42e7-a96b-3df34da8dbc0\"}" +
                    ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"3b8c85d9-a8df-404e-8410-4669817d8139\"}" +
                    ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"c05d4411-3043-4d72-bd28-ecdca6a6a1a1\"}" +
                    ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"393cd5f8-1bcf-434f-9208-f64a497622af\"}" +
                    ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"dff885fd-df8c-4075-9484-fc3e9afe8e7d\"}" +
                    ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"e1f6f091-3d1a-42c0-abf6-07babde77e93\"}" +
                    ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"d9228fcf-5467-49ce-861f-67e603b8e5c0\"}" +
                    ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"78bc2059-ade0-4478-93ab-9314e8d817c3\"}" +
                    ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"e585e827-3b70-4b8c-9c86-9f7fe59df6d0\"}" +
                    ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"6a51c98d-2900-4cd5-8eab-a20b405533ca\"}" +
                    ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"4460bf86-7caa-4180-bd8b-e19f04596adc\"}" +
                    ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"29dfedf4-4ddf-4a23-99f3-fc399e15df1e\"}" +
                    ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"79b98ed2-58e7-465f-9985-12041dfb6332\"}" +
                    ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"25801e21-5e5d-4b70-b688-30b273ee8b23\"}" +
                    ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"c0290174-8f90-405d-86c3-5de55a814a7c\"}" +
                    ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"dff9548d-d0f1-4c62-80ec-6b1444c76ee0\"}" +
                    ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"fdf47022-406f-470c-860f-711a03c6cf4f\"}" +
                    ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"1433f352-b2d6-41ab-8c59-75bbdb9a8a53\"}" +
                    ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"6b354226-1925-4d0e-959c-cc2dc731e9cd\"}" +
                    ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"f2effb5f-b75f-4be4-bcb0-eac877a8f344\"}" +
                    "]";
                #endregion 题目类型

                //记录每一道问题属于哪种体质的数组
                QuestionConstitution[] questionConstitutions = JsonConvert.DeserializeObject<QuestionConstitution[]>(jsonString);

                //统计体质结果
                bool answerComplete = true;
                foreach (var question in questionGuids)
                {
                    try
                    {
                        scoreBoard[questionConstitutions.First(x => x.QuestionGuid == question).Constitution] += GetScore(question, oneCustomerAnswerList[0].MecuryAnswerList.First(x=>x.QuestionGuid==question).Answer, questionConstitutions);
                    }
                    catch (InvalidOperationException ex) when (ex.Message == "Sequence contains no matching element" || ex.Message == "Sequence contains no elements")
                    {
                        //两道性别相关的问题用户必然缺失一道，捕获异常
                        if (question == Guid.Parse("23E317C9-D691-402E-8249-C0E4D3ED78D0") || question == Guid.Parse("A6C1DBC2-233D-4FA8-927B-8BAED0F709B3"))
                        {
                            continue;
                        }
                        else
                        {
                            //如果用户未能答完问卷，将返回此错误
                            answerComplete = false;
                        }
                    }
                    catch(Exception ex)
                    {
                        //如果用户未能答完问卷，将返回此错误
                        answerComplete = false;
                    }
                }

                //体质测试结果
                CustomerPhysique customerPhysique = new CustomerPhysique();
                //结果列表
                List<Physique> physiqueList = new List<Physique>();

                result = new GetOneCustomerPhysiqueResponse();
                result.CustomerPhysique = new CustomerPhysique();

                #region 统计体质结果列表

                //表现为平和质
                bool mayPingheCons = true;
                //倾向于平和质
                bool tendPingheCons = true;

                //遍历计分板的九个key
                foreach (KeyValuePair<string, double> score in scoreBoard)
                {
                    //平和质计算需要其他体质的计算结果，先跳过
                    if (score.Key == "平和质")
                    {
                        continue;
                    }
                    else
                    {
                        int questionCount;
                        switch (score.Key)
                        {
                            case "湿热质":
                                questionCount = 6;
                                break;
                            case "阳虚质":
                            case "血瘀质":
                            case "气郁质":
                            case "特禀质":
                                questionCount = 7;
                                break;
                            case "阴虚质":
                            case "气虚质":
                            case "痰湿质":
                                questionCount = 8;
                                break;
                            default:
                                throw new InvalidOperationException("体质定义错误");
                        }
                        double transScore = (score.Value - questionCount) * 100 / questionCount / 4;
                        if (transScore >= 40)
                        {
                            physiqueList.Add(new Physique { Title = score.Key, CurrentCent = transScore, StandCent = 40 });
                            //当体质偏颇时，不符合平和质标准，置false
                            mayPingheCons = false;
                            tendPingheCons = false;
                        }
                        else if (transScore <= 39 && transScore >= 30)
                        {
                            physiqueList.Add(new Physique { Title = $"倾向{score.Key}", CurrentCent = transScore, StandCent = 30 });
                            mayPingheCons = false;
                        }

                    }
                }

                //如果没有任何体质偏颇，检查是否符合平和质标准
                if (tendPingheCons)
                {
                    double transScore = (scoreBoard["平和质"] - 8) * 25 / 8;
                    if (transScore >= 60)
                    {
                        physiqueList.Add(new Physique { Title = mayPingheCons ? "平和质" : "倾向平和质", CurrentCent = transScore, StandCent = 60 });
                    }
                    else
                    {
                        physiqueList.Add(new Physique { Title = "平和质", CurrentCent = transScore, StandCent = 0 });
                    }
                }
                #endregion 统计体质结果列表

                customerPhysique.RecordTime = oneCustomerAnswerList[0].AnswerTime;
                customerPhysique.Physique = physiqueList;
                if (answerComplete == false)
                {
                    //当前日期回答不完整
                    customerPhysique.PhysiqueStatus = -1;
                    customerPhysique.PhysiqueDescription = "问题回答不完整，结论可能不准确";
                }

                //获取数据
                result.CustomerPhysique = customerPhysique;
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public GetOneCustomerDailyEnergyResponse GetOneCustomerDailyEnergy(Guid oid)
        {
            GetOneCustomerDailyEnergyResponse result;
            try
            {
                #region 获取指定客户的每日摄入量方法1
                //查询打卡列表的总条数
                var existingAnswerCount = 0;

                var oneCustomerDingList = GetOneCustomerMecuryAnswerList(Guid.Parse("AA46576C-9FD5-4CB9-87B1-CCEDBF68A92D"), oid, 0,1,ref existingAnswerCount);//所有问答包含多套问答

                CustomerWeight customerWeight = new CustomerWeight();
                customerWeight.Weight = 0f;
                CustomerHeight customerHeight = new CustomerHeight();
                customerHeight.Height = 0f;
                if (oneCustomerDingList.Any())
                {
                    //转换数据，提取体重，以及身高回答
                    //体重问题GUID
                    var weightGuid = Guid.Parse("A9F3D741-A83D-40E9-A108-89F7FC743384");
                    var convertFloat = 0.0f;
                    customerWeight.RecordTime = oneCustomerDingList[0].AnswerTime;

                    //身高问题GUID
                    var heightGuid = Guid.Parse("B180EAC0-127D-4ECB-BDB6-C2599D310BD4");
                    customerHeight.RecordTime = oneCustomerDingList[0].AnswerTime;
                    
                    int findBoth = 0;
                    foreach (var dingAnswer in oneCustomerDingList[0].MecuryAnswerList)
                    {
                        if (dingAnswer.QuestionGuid == weightGuid)
                        {
                            //var tempAnswer = dingWeightAnswer.Answer.Substring(1, dingWeightAnswer.Answer.Length - 2);
                            var tempAnswer = JsonConvert.DeserializeObject(dingAnswer.Answer).ToString();
                            customerWeight.Weight = Single.TryParse(tempAnswer, out convertFloat) == true ? convertFloat : 0f;
                            findBoth++;
                        }

                        if (dingAnswer.QuestionGuid == heightGuid)
                        {
                            //var tempAnswer = dingHeightAnswer.Answer.Substring(1, dingHeightAnswer.Answer.Length - 2);
                            var tempAnswer = JsonConvert.DeserializeObject(dingAnswer.Answer).ToString();
                            customerHeight.Height = Single.TryParse(tempAnswer, out convertFloat) == true ? convertFloat : 0f;
                            findBoth++;
                        }

                        if (findBoth >= 2)
                        {
                            break;
                        }
                    }
                }

                //插入申请会员时，填写入的首次初始体重，身高，按照时间插入
                var existingCustomer = _customerRepository.GetById(oid);//体重时，已经获取过
                if(existingCustomer!=null)
                {
                    if(oneCustomerDingList.Any())
                    {
                        if (existingCustomer.CreateTime > oneCustomerDingList[0].AnswerTime)
                        {
                            customerWeight = new CustomerWeight();
                            customerWeight.RecordTime = existingCustomer.CreateTime;
                            customerWeight.Weight = existingCustomer.InitWeight;

                            customerHeight = new CustomerHeight();
                            customerHeight.RecordTime = existingCustomer.CreateTime;
                            customerHeight.Height = existingCustomer.InitHeight;
                        }
                    }
                    else
                    {
                        customerWeight = new CustomerWeight();
                        customerWeight.RecordTime = existingCustomer.CreateTime;
                        customerWeight.Weight = existingCustomer.InitWeight;

                        customerHeight = new CustomerHeight();
                        customerHeight.RecordTime = existingCustomer.CreateTime;
                        customerHeight.Height = existingCustomer.InitHeight;
                    }
                }

                float currentWeight = customerWeight.Weight;//当前体重
                float currentHeight = customerHeight.Height;//当前身高

                if(currentWeight==0f||currentHeight==0f)
                {
                    result = new GetOneCustomerDailyEnergyResponse();
                    result.Value = 0;
                    result.Unit = "kcal";
                    return result;
                }

                //客户理想体重
                float customerIdealWeight = currentHeight - 105.0f;

                //客户肥胖度
                float customerObesityDegree = (currentWeight - customerIdealWeight) / customerIdealWeight;

                //获取客户职业强度
                int jobStrength = Int32.MinValue;
                var customerJob = _customerJobRepository.GetCustomerJobByCustomerOid(oid);
                if (customerJob != null)
                {
                    if (int.TryParse(customerJob.Job.Strength, out jobStrength) == false)
                    {
                        return null;
                    }
                }

                //每日每公斤体重所需热量数（kcal/kg）
                int dailyEnergyPerKg = Int32.MinValue;
                switch (jobStrength)
                {
                    case 1://极轻体力
                        if (customerObesityDegree >= 0.2f)//肥胖>=20%
                        {
                            dailyEnergyPerKg = 20;
                        }
                        else if (customerObesityDegree <= -0.2f)//消瘦<=-20%
                        {
                            dailyEnergyPerKg = 30;
                        }
                        else//正常
                        {
                            dailyEnergyPerKg = 25;
                        }
                        break;
                    case 2://轻体力
                        if (customerObesityDegree >= 0.2f)//肥胖>=20%
                        {
                            dailyEnergyPerKg = 25;
                        }
                        else if (customerObesityDegree <= -0.2f)//消瘦<=-20%
                        {
                            dailyEnergyPerKg = 35;
                        }
                        else//正常
                        {
                            dailyEnergyPerKg = 30;
                        }
                        break;
                    case 3://中体力
                        if (customerObesityDegree >= 0.2f)//肥胖>=20%
                        {
                            dailyEnergyPerKg = 30;
                        }
                        else if (customerObesityDegree <= -0.2f)//消瘦<=-20%
                        {
                            dailyEnergyPerKg = 40;
                        }
                        else//正常
                        {
                            dailyEnergyPerKg = 35;
                        }
                        break;
                    case 4://重体力
                        if (customerObesityDegree >= 0.2f)//肥胖>=20%
                        {
                            dailyEnergyPerKg = 35;
                        }
                        else if (customerObesityDegree <= -0.2f)//消瘦<=-20%
                        {
                            dailyEnergyPerKg = 45;
                        }
                        else//正常
                        {
                            dailyEnergyPerKg = 40;
                        }
                        break;
                    default:
                        dailyEnergyPerKg = 0;
                        break;
                }

                //每日理想体重所需热量数（kcal/kg）
                float customerDailyEnergy = dailyEnergyPerKg * customerIdealWeight;

                if (customerDailyEnergy == 0)
                {
                    return null;
                }

                result = new GetOneCustomerDailyEnergyResponse();
                result.Value = customerDailyEnergy;
                result.Unit = "kcal";

                #endregion 获取指定客户的每日摄入量方法1

                #region 获取指定客户的每日摄入量方法2
                //#region 获取体重数据列表
                //var questionVersion = Guid.Parse("A630D59D-3BD1-4D79-8F30-DD3118573C36");//打卡中的体重问题版本号
                //var questionGuid = Guid.Parse("A9F3D741-A83D-40E9-A108-89F7FC743384");//打卡中的体重问题问卷唯一号
                //var originalAnswer = _answerRepository.GetAnswerByConditions(oid, questionVersion, questionGuid);
                //var convertFloat = 0.0f;

                ////去除非法数据
                //var legalAnswer = (from t in originalAnswer
                //                   where
                //                   (
                //                   Single.TryParse(t.Content.Substring(1, t.Content.Length - 2), out convertFloat) == true //获取内部除了引号"之外的数据，例如："50" 提取出来50
                //                   )
                //                   && (convertFloat >= 30)
                //                   && (convertFloat <= 300)
                //                   select t
                //                    );//体重范围30kg～300kg

                ////去除当天重复数据
                //var noRepeatAnswer = from t in legalAnswer
                //                     group t by new { t.Ctime.Date }
                //                     into mygroup
                //                     select mygroup.FirstOrDefault();
                ////转换后的数据
                //List<CustomerWeight> noRepeatWeightList = _mapper.Map<List<CustomerWeight>>(noRepeatAnswer);

                ////插入申请会员时，填写入的首次初始体重，身高，按照时间插入
                //var existingCustomer = _customerRepository.GetById(oid);
                //if (existingCustomer != null)
                //{
                //    CustomerWeight customerWeight = new CustomerWeight();
                //    customerWeight.RecordTime = existingCustomer.CreateTime;
                //    customerWeight.Weight = existingCustomer.InitWeight;
                //    noRepeatWeightList.Add(customerWeight);

                //    //新数据时间排序
                //    var newAnswer = noRepeatWeightList.OrderByDescending(t => t.RecordTime);

                //    //新数据去除当天重复数据
                //    var newNoRepeatAnswer = from s in newAnswer
                //                            group s by new { s.RecordTime.Date }
                //                           into mygroup
                //                            select mygroup.FirstOrDefault();

                //    //数据合并
                //    noRepeatWeightList = new List<CustomerWeight>();
                //    noRepeatWeightList = newNoRepeatAnswer.ToList();
                //}
                //#endregion 获取体重数据列表

                //#region 获取身高数据列表
                //questionVersion = Guid.Parse("358CA924-8DF2-47F8-8AEC-7612D4B3BD97");//打卡中的身高问题版本号
                //questionGuid = Guid.Parse("B180EAC0-127D-4ECB-BDB6-C2599D310BD4");//打卡中的身高问题问卷唯一号
                ////currentTime = DateTime.Now;//当前日期//体重时，已经获取过
                ////lastTime = DateTime.Now.AddDays(-31);//最近三十天//体重时，已经获取过
                //originalAnswer = _answerRepository.GetAnswerByConditions(oid, questionVersion, questionGuid);
                //convertFloat = 0.0f;

                ////去除非法数据
                //legalAnswer = (from t in originalAnswer
                //               where
                //               (
                //               Single.TryParse(t.Content.Substring(1, t.Content.Length - 2), out convertFloat) == true //获取内部除了引号"之外的数据，例如："50" 提取出来50
                //               )
                //               && (convertFloat >= 50)
                //               && (convertFloat <= 300)
                //               select t
                //                );//身高范围50cm～300cm

                ////去除当天重复数据
                //noRepeatAnswer = from t in legalAnswer
                //                 group t by new { t.Ctime.Date }
                //                     into mygroup
                //                 select mygroup.FirstOrDefault();
                ////转换后的数据
                //List<CustomerHeight> noRepeatHeightList = _mapper.Map<List<CustomerHeight>>(noRepeatAnswer);

                ////插入申请会员时，填写入的首次初始体重，身高，按照时间插入
                ////var existingCustomer = _customerRepository.GetById(oid);//体重时，已经获取过
                //if (existingCustomer != null)
                //{
                //    CustomerHeight customerHeight = new CustomerHeight();
                //    customerHeight.RecordTime = existingCustomer.CreateTime;
                //    customerHeight.Height = existingCustomer.InitHeight;
                //    noRepeatHeightList.Add(customerHeight);

                //    //新数据时间排序
                //    var newAnswer = noRepeatHeightList.OrderByDescending(t => t.RecordTime);

                //    //新数据去除当天重复数据
                //    var newNoRepeatAnswer = from s in newAnswer
                //                            group s by new { s.RecordTime.Date }
                //                           into mygroup
                //                            select mygroup.FirstOrDefault();

                //    //数据合并
                //    noRepeatHeightList = new List<CustomerHeight>();
                //    noRepeatHeightList = newNoRepeatAnswer.ToList();
                //}
                //#endregion 获取身高数据列表

                //#region 处理BMI数值，以最少数据为基准，获取的同一天数据进行处理
                //float currentWeight = 0.0f;//当前体重
                //float currentHeight = 0.0f;//当前身高
                //CustomerBMI customerBMI = null;
                //if (noRepeatWeightList.Count == noRepeatHeightList.Count && noRepeatWeightList.Count == 0)
                //{
                //    return null;
                //}

                //if (noRepeatWeightList.Count <= noRepeatHeightList.Count)
                //{
                //    for (int i = 0; i < noRepeatWeightList.Count; i++)
                //    {
                //        currentWeight = 0.0f;//初始化
                //        currentHeight = 0.0f;//初始化

                //        currentWeight = noRepeatWeightList[i].Weight;//当前体重
                //        for (int j = 0; j < noRepeatHeightList.Count; j++)
                //        {
                //            if (string.Equals(noRepeatWeightList[i].RecordTime.Date, noRepeatHeightList[j].RecordTime.Date))
                //            {
                //                currentHeight = noRepeatHeightList[j].Height;
                //                break;
                //            }
                //        }

                //        if (currentWeight != 0 && currentHeight != 0)
                //        {
                //            customerBMI = new CustomerBMI();
                //            customerBMI.RecordTime = noRepeatWeightList[i].RecordTime;
                //            customerBMI.BMI = Convert.ToSingle(currentWeight / Math.Pow(currentHeight / 100, 2));
                //            break;
                //        }
                //    }
                //}
                //else
                //{
                //    for (int i = 0; i < noRepeatHeightList.Count; i++)
                //    {
                //        currentWeight = 0.0f;//初始化
                //        currentHeight = 0.0f;//初始化

                //        currentHeight = noRepeatHeightList[i].Height;//当前身高
                //        for (int j = 0; j < noRepeatWeightList.Count; j++)
                //        {
                //            if (string.Equals(noRepeatHeightList[i].RecordTime.Date, noRepeatWeightList[j].RecordTime.Date))
                //            {
                //                currentWeight = noRepeatWeightList[j].Weight;
                //                break;
                //            }
                //        }

                //        if (currentWeight != 0 && currentHeight != 0)
                //        {
                //            customerBMI = new CustomerBMI();
                //            customerBMI.RecordTime = noRepeatHeightList[i].RecordTime;
                //            customerBMI.BMI = Convert.ToSingle(currentWeight / Math.Pow(currentHeight / 100, 2));
                //            break;
                //        }
                //    }
                //}

                //#endregion 处理BMI数值，以最少数据为基准，获取的同一天数据进行处理

                ////customerBMI客户BMI，BMI暂时不用
                //if (customerBMI==null)
                //{
                //    return null;
                //}

                ////客户理想体重
                //float customerIdealWeight = currentHeight - 105.0f;

                ////客户肥胖度
                //float customerObesityDegree = (currentWeight - customerIdealWeight) / customerIdealWeight;

                ////获取客户职业强度
                //int jobStrength=Int32.MinValue;
                //var customerJob = _customerJobRepository.GetCustomerJobByCustomerOid(oid);
                //if (customerJob != null)
                //{
                //    if(int.TryParse(customerJob.Job.Strength,out jobStrength) ==false)
                //    {
                //        return null;
                //    }
                //}

                ////每日每公斤体重所需热量数（kcal/kg）
                //int dailyEnergyPerKg= Int32.MinValue;
                //switch(jobStrength)
                //{
                //    case 1://极轻体力
                //        if (customerObesityDegree>= 0.2f)//肥胖>=20%
                //        {
                //            dailyEnergyPerKg = 20;
                //        }
                //        else if(customerObesityDegree<=-0.2f)//消瘦<=-20%
                //        {
                //            dailyEnergyPerKg = 30;
                //        }
                //        else//正常
                //        {
                //            dailyEnergyPerKg = 25;
                //        }
                //        break;
                //    case 2://轻体力
                //        if (customerObesityDegree >= 0.2f)//肥胖>=20%
                //        {
                //            dailyEnergyPerKg = 25;
                //        }
                //        else if (customerObesityDegree <= -0.2f)//消瘦<=-20%
                //        {
                //            dailyEnergyPerKg = 35;
                //        }
                //        else//正常
                //        {
                //            dailyEnergyPerKg = 30;
                //        }
                //        break;
                //    case 3://中体力
                //        if (customerObesityDegree >= 0.2f)//肥胖>=20%
                //        {
                //            dailyEnergyPerKg = 30;
                //        }
                //        else if (customerObesityDegree <= -0.2f)//消瘦<=-20%
                //        {
                //            dailyEnergyPerKg = 40;
                //        }
                //        else//正常
                //        {
                //            dailyEnergyPerKg = 35;
                //        }
                //        break;
                //    case 4://重体力
                //        if (customerObesityDegree >= 0.2f)//肥胖>=20%
                //        {
                //            dailyEnergyPerKg = 35;
                //        }
                //        else if (customerObesityDegree <= -0.2f)//消瘦<=-20%
                //        {
                //            dailyEnergyPerKg = 45;
                //        }
                //        else//正常
                //        {
                //            dailyEnergyPerKg = 40;
                //        }
                //        break;
                //    default:
                //        dailyEnergyPerKg = 0;
                //        break;
                //}

                ////每日理想体重所需热量数（kcal/kg）
                //float customerDailyEnergy = dailyEnergyPerKg * customerIdealWeight;

                //if(customerDailyEnergy==0)
                //{
                //    return null;
                //}
               
                //result = new GetOneCustomerDailyEnergyResponse();
                //result.Value = customerDailyEnergy;
                //result.Unit = "kcal";
                #endregion 获取指定客户的每日摄入量方法2
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public QueryOneCustomerDingListByPageResponse QueryOneCustomerDingListByPage(Guid oid, int pageIndex, int pageSize)
        {
            QueryOneCustomerDingListByPageResponse result;
            try
            {
                //查询所有问题
                var questionaire = _questionnaireRepository.GetById(Guid.Parse("AA46576C-9FD5-4CB9-87B1-CCEDBF68A92D"));//打卡问题集
                if(questionaire==null)
                {
                    return null;
                }
                Guid[] questionGuids = JsonConvert.DeserializeObject<Guid[]>(questionaire.Question);
                if (questionGuids==null)
                {
                    return null;
                }

                //以第一条数据为基准判断一个打卡数据
                //查询打卡列表的总条数
                var existingDingCount = _answerRepository.QueryMecuryAnswerListCount(oid, questionGuids[0]);//第一条打卡问题

                //查询所有的打卡数据的开始日期
                var existingDing = _answerRepository.QueryMecuryAnswerList(oid, questionGuids[0]);//第一条打卡问题

                //获取分页的第一个问题的所有回答，作为起始点
                var firstAnswerDingList= existingDing.Skip(pageIndex * pageSize).Take(pageSize).ToList();

                //查询所有的问题列表
                List<string> questionList = new List<string>();
                List<string> questionTypeList = new List<string>();
                foreach(var questionGuid in questionGuids)
                {
                    var question= _questionRepository.GetById(questionGuid);
                    if (question != null)
                    {
                        questionList.Add(question.Content);
                        questionTypeList.Add(question.Type);
                    }
                }

                OneCustomerDingByPage oneCustomerDingByPage = new OneCustomerDingByPage();

                List<Ding> dingList = new List<Ding>();
                result = new QueryOneCustomerDingListByPageResponse();

                //查询个人信息
                var customer = _customerRepository.GetById(oid);
                if (customer == null)
                {
                    return null;
                }
                result.UserName = customer.UserName;
                result.Cellphone = customer.Cellphone;

                var service = _serviceRepository.GetServiceByCustomerOid(oid);
                if (service == null)
                {
                    return null;
                }
                result.ServiceName = service.Name;

                //增加所有的答案
                result.DingList = new PageModel<OneCustomerDingByPage>();
                result.DingList.Data = new List<OneCustomerDingByPage>();
                for (int i=0;i< firstAnswerDingList.Count();i++)//依据时间来判断
                {
                    dingList = new List<Ding>();
                    oneCustomerDingByPage = new OneCustomerDingByPage();
                    
                    Ding ding = new Ding();
                    //第一个问题
                    var firstAnswer = firstAnswerDingList[i];
                    Question question= JsonConvert.DeserializeObject<Question>(questionList[0]);
                    ding.Question = question.content;
                    ding.QuestionType = questionTypeList[0];
                    if (firstAnswer.Content.IndexOf("\"")>0)//兼容性考虑
                    {
                        try
                        {
                            ding.Answer = JsonConvert.DeserializeObject(firstAnswer.Content).ToString();
                        }
                        catch (Exception)
                        {
                            ding.Answer = firstAnswer.Content;
                        }
                    }
                    else
                    {
                        ding.Answer = firstAnswer.Content;
                    }
                    
                    ding.CreateTime = firstAnswer.Ctime;
                    dingList.Add(ding);
                    oneCustomerDingByPage.ID= (pageIndex * pageSize) + i + 1;
                    oneCustomerDingByPage.DingTime = firstAnswer.Ctime;

                    //查询是否有协助者oneCustomerDingByPage.Assister
                    var customerAssistDing = _customerAssistDingRepository.GetByCustomerOid(Guid.Parse("AA46576C-9FD5-4CB9-87B1-CCEDBF68A92D"), oid, firstAnswer.Ctime);
                    if (customerAssistDing != null)
                    {
                        var supporter = _supporterRepository.GetById(customerAssistDing.SupporterOid);
                        if (supporter != null)
                        {
                            oneCustomerDingByPage.Assister = supporter.UserName;
                        }
                    }
                    else
                    {
                        oneCustomerDingByPage.Assister = _customerRepository.GetById(oid).UserName+"【本人】";
                    }

                    //增加删除标记
                    oneCustomerDingByPage.FirstAnswerGuid = firstAnswer.AnswerGuid;

                    DateTime startTime = firstAnswer.Ctime;
                    for (int j=1;j <questionGuids.Count();j++)
                    {
                        var answer = _answerRepository.GetDingAnswer(oid, questionGuids[j], startTime);
                        if(answer == null)
                        {
                            break;//问题没有回答结束，跳出
                        }

                        string answerContent = null;//兼容性考虑
                        try
                        {
                            if (questionTypeList[j] == "PhotoGraph")
                            {
                                answerContent = answer.Content;
                            }
                            else
                            {
                                answerContent = JsonConvert.DeserializeObject(answer.Content).ToString();
                            }
                        }
                        catch(Exception)
                        { 
                        }
                        if(answerContent== "此回答自动屏蔽")
                        {
                            continue;
                        }

                        ding = new Ding();
                        question = new Question();
                        question=JsonConvert.DeserializeObject<Question>(questionList[j]);
                        ding.Question = question.content;
                        ding.QuestionType = questionTypeList[j];
                        ding.Answer = answerContent;
                        ding.CreateTime = answer.Ctime;
                        dingList.Add(ding);
                        startTime = answer.Ctime;
                    }
                    oneCustomerDingByPage.DingList = dingList;

                    result.DingList.Data.Add(oneCustomerDingByPage);
                }

                //增加页码信息
                result.DingList.PageIndex = pageIndex + 1;//页码索引
                result.DingList.PageSize = pageSize;//每页大小
                result.DingList.DataCount = existingDingCount;//数据总数
                result.DingList.PageCount = Convert.ToInt32(Math.Ceiling(existingDingCount * 1.0d / pageSize));//总页数
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public void CreateOneCustomerAssistDing(CreateOneCustomerAssistDingRequest request)
        {
            var createOneCustomerAssistDingCommand = _mapper.Map<CreateOneCustomerAssistDingCommand>(request);
            if (request.AssistDing == null || request.AssistDing.Count > 0)
            {
                createOneCustomerAssistDingCommand.AssistDing = new List<Domain.Commands.MiddleDing>();
                createOneCustomerAssistDingCommand.AssistDing = _mapper.Map<List<Domain.Commands.MiddleDing>>(request.AssistDing);
            }
            Bus.SendCommand(createOneCustomerAssistDingCommand);
        }

        public void CreateOneCustomerDing(CreateOneCustomerDingRequest request)
        {
            var createOneCustomerDingCommand = _mapper.Map<CreateOneCustomerDingCommand>(request);
            if (request.MiddleDingList == null || request.MiddleDingList.Count > 0)
            {
                createOneCustomerDingCommand.MiddleDingList = new List<Domain.Commands.MiddleDing>();
                createOneCustomerDingCommand.MiddleDingList = _mapper.Map<List<Domain.Commands.MiddleDing>>(request.MiddleDingList);
            }
            Bus.SendCommand(createOneCustomerDingCommand);
        }

        public void DeleteOneCustomerAssistDing(DeleteOneCustomerAssistDingRequest request)
        {
            var deleteOneCustomerAssistDingCommand = _mapper.Map<DeleteOneCustomerAssistDingCommand>(request);
            Bus.SendCommand(deleteOneCustomerAssistDingCommand);
        }

        public GetOneCustomerTodayDingResponse GetOneCustomerTodayDing(Guid customerOid)
        {
            GetOneCustomerTodayDingResponse result;
            try
            {
                //查询所有问题
                var questionaire = _questionnaireRepository.GetById(Guid.Parse("AA46576C-9FD5-4CB9-87B1-CCEDBF68A92D"));//打卡问题集
                if (questionaire == null)
                {
                    return null;
                }
                Guid[] questionGuids = JsonConvert.DeserializeObject<Guid[]>(questionaire.Question);
                if (questionGuids == null)
                {
                    return null;
                }

                DateTime today = DateTime.Now;
                //获取今天的打卡信息
                var oneDingAnswer = _answerRepository.GetOneDingAnswerByQuestionGuid(customerOid, questionGuids[0], today);
                if (oneDingAnswer == null)
                {
                    return null;
                }

                //查询所有的问题列表
                List<string> questionList = new List<string>();
                List<string> questionTypeList = new List<string>();
                foreach (var questionGuid in questionGuids)
                {
                    var question = _questionRepository.GetById(questionGuid);
                    if (question != null)
                    {
                        questionList.Add(question.Content);
                        questionTypeList.Add(question.Type);
                    }
                }

                //增加所有的答案
                int answerComplete = 0;//回答是否完整
                List<Ding> dingList = new List<Ding>();
                Ding ding = new Ding();
                DateTime startTime = oneDingAnswer.Ctime;
                for(int i=0;i<questionList.Count();i++)
                {
                    ding = new Ding();
                    ding.QuestionGuid = questionGuids[i];
                    ding.Question = questionList[i];
                    ding.QuestionType = questionTypeList[i];
                    
                    var answer= _answerRepository.GetDingAnswer(customerOid, questionGuids[i], startTime);
                    if(answer==null)
                    {
                        ding.Answer = "";
                        ding.CreateTime = startTime;
                        answerComplete = -1;//回答不完整
                    }
                    else
                    {
                        try
                        {
                            if (questionTypeList[i] == "PhotoGraph")
                            {
                                ding.Answer = answer.Content;
                            }
                            else
                            {
                                ding.Answer = JsonConvert.DeserializeObject(answer.Content).ToString();
                            }
                        }
                        catch (Exception)
                        {
                        }
                        ding.CreateTime = answer.Ctime;

                        if (ding.Answer == "此回答自动屏蔽")
                        {
                            continue;
                        }

                        dingList.Add(ding);
                        startTime = answer.Ctime;
                    }
                }
                result = new GetOneCustomerTodayDingResponse();
                result.DingList = new List<Ding>();
                result.DingList = dingList;
                result.CreateTime = oneDingAnswer.Ctime;
                if(answerComplete==0)
                {
                    result.DingStatus = 0;
                    result.DingStatusDescription = "【" + oneDingAnswer.Ctime.ToString("yyyy-MM-dd HH:mm:ss") + "】" + "今天已经打卡，并且回答完整";
                }
                else
                {
                    result.DingStatus = -1;
                    result.DingStatusDescription = "【" + oneDingAnswer.Ctime.ToString("yyyy-MM-dd HH:mm:ss") + "】" + "今天已经打卡，但是回答不完整";
                }

            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public GetOneCustomerTodayPhysiqueResponse GetOneCustomerTodayPhysique(string cellphone)
        {
            GetOneCustomerTodayPhysiqueResponse result;
            try
            {
                //查询所有问题
                var questionaire = _questionnaireRepository.GetById(Guid.Parse("B546B709-2B2B-4F6E-9F1F-64F281DE8D5B"));//打卡问题集
                if (questionaire == null)
                {
                    return null;
                }
                Guid[] questionGuids = JsonConvert.DeserializeObject<Guid[]>(questionaire.Question);
                if (questionGuids == null)
                {
                    return null;
                }

                //查询参加Mercury的指定用户
                var originalCustomer = _userInformationRepository.GetUserByPhoneNumber(cellphone);
                var originalCustomerResult  = _mapper.Map<GetOneOriginalCustomerResponse>(originalCustomer);
                if (originalCustomerResult == null)
                {
                    return null;
                }

                DateTime today = DateTime.Now;
                //获取首次体脂测试的第一个回答信息
                var existingMecuryAnswer = _answerRepository.GetOneDingAnswerByQuestionGuid(originalCustomerResult.OID, questionGuids[0], today);
                if (existingMecuryAnswer == null)
                {
                    return null;
                }

                //剩余回答处理
                int answerComplete = 0;//回答是否完整
                var startTime = existingMecuryAnswer.Ctime;

                for (int i=1;i<questionGuids.Count();i++)
                {
                    var answer = _answerRepository.GetDingAnswer(originalCustomerResult.OID, questionGuids[i], startTime);
                    if(answer==null)
                    {
                        //两道性别相关的问题用户必然缺失一道
                        if (questionGuids[i] == Guid.Parse("23E317C9-D691-402E-8249-C0E4D3ED78D0") || questionGuids[i] == Guid.Parse("A6C1DBC2-233D-4FA8-927B-8BAED0F709B3"))
                        {
                            continue;
                        }
                        answerComplete = -1;//回答不完整
                        break;
                    }
                }

                //处理返回
                result = new GetOneCustomerTodayPhysiqueResponse();
                if (answerComplete==-1)
                {
                    result.CreateTime = existingMecuryAnswer.Ctime;
                    result.PhysiqueStatus = -1;
                    result.PhysiqueStatusDescription= "【" + existingMecuryAnswer.Ctime.ToString("yyyy-MM-dd HH:mm:ss") + "】" + "今天已经开始体脂测试，但是回答不完整，可以继续答题";
                }
                else if(answerComplete==0)
                {
                    result.CreateTime = existingMecuryAnswer.Ctime;
                    result.PhysiqueStatus = -1;
                    result.PhysiqueStatusDescription = "【" + existingMecuryAnswer.Ctime.ToString("yyyy-MM-dd HH:mm:ss") + "】" + "今天已经完成体脂测试，每人每天只有一次体脂测试机会";
                }
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        /// <summary>
        /// 问题模型
        /// </summary>
        private class Question
        {
            public string type { get; set; }
            public Guid guid { get; set; }
            public string title { get; set; }
            public string content { get; set; }
            public string description { get; set; }
            public string progress { get; set; }
            public string sources { get; set; }
        }

        #region private方法
        /// <summary>
        /// 问题GUID-辩证体质关系对
        /// </summary>
        private class QuestionConstitution
        {
#warning 把Regime.Constitution改成string[]类型后，此处应同步传入Constitution字段，或string[].Index
            public string Constitution { get; set; }

            public Guid QuestionGuid { get; set; }
        }

        /// <summary>
        /// 传入问题Guid和用户回答JsonString，计算分数
        /// </summary>
        /// <param name="questionGuid"></param>
        /// <param name="answerContent"></param>
        /// <param name="questionConstitutions"></param>
        /// <returns>分数</returns>
        private double GetScore(Guid questionGuid, string answerContent, QuestionConstitution[] questionConstitutions)
        {
#warning 把Constitution改成string[]即可解决同一个问题隶属于多个体质时题目重复的问题

            //SqlServer中储值为JsonString，反序列化
            string content = JsonConvert.DeserializeObject<string>(answerContent);
            //偏颇体质的问题计分方式均为Common
            if (questionConstitutions.First(x => x.QuestionGuid == questionGuid).Constitution != "平和质")
            {
                return Common();
            }
            //平和质中有两道题的计分方式为Common
            else if (questionGuid == Guid.Parse("ae4b39ed-501e-4654-a16c-45ce98fc53e2") || questionGuid == Guid.Parse("48fa4d9e-01a1-44d6-9226-efe6ea4a2dbe"))
            {
                return Common();
            }
            //平和质中有一些题的计分方式是Special
            else
            {
                return Special();
            }

            //内联函数，顺序的加分方式
            double Common()
            {
                switch (content)
                {
                    case "没有(根本不)": return 1d;
                    case "很少(有一点)": return 2d;
                    case "有时(有些)": return 3d;
                    case "经常(相当)": return 4d;
                    case "总是(非常)": return 5d;
                    default: return 0d;
                }
            }

            //内联函数，反序的加分方式
            double Special()
            {
                switch (content)
                {
                    case "没有(根本不)": return 5d;
                    case "很少(有一点)": return 4d;
                    case "有时(有些)": return 3d;
                    case "经常(相当)": return 2d;
                    case "总是(非常)": return 1d;
                    default: return 0d;
                }
            }
        }
        #endregion private方法

        #region 以前版本
        //public GetOneCustomerWeightListResponse GetOneCustomerWeightList(Guid oid,int dataCount)
        //{
        //    GetOneCustomerWeightListResponse result;
        //    try
        //    {
        //        #region 获取体重数据列表
        //        var questionVersion = Guid.Parse("A630D59D-3BD1-4D79-8F30-DD3118573C36");//打卡中的体重问题版本号
        //        var questionGuid = Guid.Parse("A9F3D741-A83D-40E9-A108-89F7FC743384");//打卡中的体重问题问卷唯一号
        //        var currentTime = DateTime.Now;//当前日期
        //        var lastTime = DateTime.Now.AddDays(-31);//最近三十天
        //        var originalAnswer = _answerRepository.GetAnswerByConditions(oid, questionVersion, questionGuid, currentTime, lastTime);
        //        var convertFloat=0.0f;

        //        //去除非法数据
        //        var legalAnswer = (from t in originalAnswer
        //                            where 
        //                            (
        //                            Single.TryParse(t.Content.Substring(1,t.Content.Length-2),out convertFloat) ==true //获取内部除了引号"之外的数据，例如："50" 提取出来50
        //                            ) 
        //                            && (convertFloat>=30)
        //                            && (convertFloat <= 300) 
        //                            select t
        //                            );//体重范围30kg～300kg

        //        //去除当天重复数据
        //        var noRepeatAnswer = from t in legalAnswer
        //                             group t by new { t.Ctime.Date} 
        //                             into mygroup 
        //                             select mygroup.FirstOrDefault();
        //        //转换后的数据
        //        List<CustomerWeight> noRepeatWeightList = _mapper.Map<List<CustomerWeight>>(noRepeatAnswer);

        //        //插入申请会员时，填写入的首次初始体重，身高，按照时间插入
        //        var existingCustomer = _customerRepository.GetById(oid);
        //        if(existingCustomer!=null)
        //        {
        //            CustomerWeight customerWeight = new CustomerWeight();
        //            customerWeight.RecordTime = existingCustomer.CreateTime;
        //            customerWeight.Weight = existingCustomer.InitWeight;
        //            noRepeatWeightList.Add(customerWeight);

        //            //新数据时间排序
        //            var newAnswer= noRepeatWeightList.OrderByDescending(t => t.RecordTime);

        //            //新数据去除当天重复数据
        //            var newNoRepeatAnswer= from s in newAnswer
        //                                   group s by new { s.RecordTime.Date }
        //                                   into mygroup
        //                                   select mygroup.FirstOrDefault();

        //            //数据合并
        //            noRepeatWeightList = new List<CustomerWeight>();
        //            noRepeatWeightList = newNoRepeatAnswer.ToList();
        //        }

        //        #endregion 获取体重数据列表

        //        //获取前dataCount个数据
        //        result = new GetOneCustomerWeightListResponse();
        //        result.WeightList = noRepeatWeightList.Take(dataCount).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //    return result;
        //}



        //public GetOneCustomerBMIListResponse GetOneCustomerBMIList(Guid oid, int dataCount)
        //{
        //    GetOneCustomerBMIListResponse result;
        //    try
        //    {
        //        #region 获取体重数据列表
        //        var questionVersion = Guid.Parse("A630D59D-3BD1-4D79-8F30-DD3118573C36");//打卡中的体重问题版本号
        //        var questionGuid = Guid.Parse("A9F3D741-A83D-40E9-A108-89F7FC743384");//打卡中的体重问题问卷唯一号
        //        var currentTime = DateTime.Now;//当前日期
        //        var lastTime = DateTime.Now.AddDays(-31);//最近三十天
        //        var originalAnswer = _answerRepository.GetAnswerByConditions(oid, questionVersion, questionGuid, currentTime, lastTime);
        //        var convertFloat = 0.0f;

        //        //去除非法数据
        //        var legalAnswer = (from t in originalAnswer
        //                           where
        //                           (
        //                           //Single.TryParse(JsonConvert.DeserializeObject(t.Content).ToString(), out convertFloat) == true //获取内部除了引号"之外的数据，例如："50" 提取出来50
        //                           Single.TryParse(t.Content.Substring(1, t.Content.Length - 2), out convertFloat) == true //获取内部除了引号"之外的数据，例如："50" 提取出来50
        //                           )
        //                           && (convertFloat >= 30)
        //                           && (convertFloat <= 300)
        //                           select t
        //                            );//体重范围30kg～300kg

        //        //去除当天重复数据
        //        var noRepeatAnswer = from t in legalAnswer
        //                             group t by new { t.Ctime.Date }
        //                             into mygroup
        //                             select mygroup.FirstOrDefault();
        //        //转换后的数据
        //        List<CustomerWeight> noRepeatWeightList = _mapper.Map<List<CustomerWeight>>(noRepeatAnswer);

        //        //插入申请会员时，填写入的首次初始体重，身高，按照时间插入
        //        var existingCustomer = _customerRepository.GetById(oid);
        //        if (existingCustomer != null)
        //        {
        //            CustomerWeight customerWeight = new CustomerWeight();
        //            customerWeight.RecordTime = existingCustomer.CreateTime;
        //            customerWeight.Weight = existingCustomer.InitWeight;
        //            noRepeatWeightList.Add(customerWeight);

        //            //新数据时间排序
        //            var newAnswer = noRepeatWeightList.OrderByDescending(t => t.RecordTime);

        //            //新数据去除当天重复数据
        //            var newNoRepeatAnswer = from s in newAnswer
        //                                    group s by new { s.RecordTime.Date }
        //                                   into mygroup
        //                                    select mygroup.FirstOrDefault();

        //            //数据合并
        //            noRepeatWeightList = new List<CustomerWeight>();
        //            noRepeatWeightList = newNoRepeatAnswer.ToList();
        //        }
        //        #endregion 获取体重数据列表

        //        #region 获取身高数据列表
        //        questionVersion = Guid.Parse("358CA924-8DF2-47F8-8AEC-7612D4B3BD97");//打卡中的身高问题版本号
        //        questionGuid = Guid.Parse("B180EAC0-127D-4ECB-BDB6-C2599D310BD4");//打卡中的身高问题问卷唯一号
        //        //currentTime = DateTime.Now;//当前日期//体重时，已经获取过
        //        //lastTime = DateTime.Now.AddDays(-31);//最近三十天//体重时，已经获取过
        //        originalAnswer = _answerRepository.GetAnswerByConditions(oid, questionVersion, questionGuid, currentTime, lastTime);
        //        convertFloat = 0.0f;

        //        //去除非法数据
        //        legalAnswer = (from t in originalAnswer
        //                        where
        //                        (
        //                        //Single.TryParse(JsonConvert.DeserializeObject(t.Content) != null ? JsonConvert.DeserializeObject(t.Content).ToString() : null, out convertFloat) == true //获取内部除了引号"之外的数据，例如："50" 提取出来50
        //                        Single.TryParse(t.Content.Substring(1, t.Content.Length - 2), out convertFloat) == true //获取内部除了引号"之外的数据，例如："50" 提取出来50
        //                        )
        //                        && (convertFloat >= 50)
        //                        && (convertFloat <= 300)
        //                        select t
        //                        );//身高范围50cm～300cm

        //        //去除当天重复数据
        //        noRepeatAnswer = from t in legalAnswer
        //                             group t by new { t.Ctime.Date }
        //                             into mygroup
        //                             select mygroup.FirstOrDefault();
        //        //转换后的数据
        //        List<CustomerHeight> noRepeatHeightList = _mapper.Map<List<CustomerHeight>>(noRepeatAnswer);

        //        //插入申请会员时，填写入的首次初始体重，身高，按照时间插入
        //        //var existingCustomer = _customerRepository.GetById(oid);//体重时，已经获取过
        //        if (existingCustomer != null)
        //        {
        //            CustomerHeight customerHeight = new CustomerHeight();
        //            customerHeight.RecordTime = existingCustomer.CreateTime;
        //            customerHeight.Height = existingCustomer.InitHeight;
        //            noRepeatHeightList.Add(customerHeight);

        //            //新数据时间排序
        //            var newAnswer = noRepeatHeightList.OrderByDescending(t => t.RecordTime);

        //            //新数据去除当天重复数据
        //            var newNoRepeatAnswer = from s in newAnswer
        //                                    group s by new { s.RecordTime.Date }
        //                                   into mygroup
        //                                    select mygroup.FirstOrDefault();

        //            //数据合并
        //            noRepeatHeightList = new List<CustomerHeight>();
        //            noRepeatHeightList = newNoRepeatAnswer.ToList();
        //        }
        //        #endregion 获取身高数据列表

        //        #region 处理BMI数值，以最少数据为基准，获取的同一天数据进行处理
        //        float currentWeight = 0.0f;//当前体重
        //        float currentHeight = 0.0f;//当前身高
        //        List<CustomerBMI> noRepeatIBMList = new List<CustomerBMI>();
        //        if (noRepeatWeightList.Count == noRepeatHeightList.Count&& noRepeatWeightList.Count==0)
        //        {
        //            result = new GetOneCustomerBMIListResponse();
        //            result.BMIList = new List<CustomerBMI>();
        //            return result;
        //        }

        //        if (noRepeatWeightList.Count<= noRepeatHeightList.Count)
        //        {
        //            for(int i=0;i<noRepeatWeightList.Count;i++)
        //            {
        //                currentWeight = 0.0f;//初始化
        //                currentHeight = 0.0f;//初始化

        //                currentWeight = noRepeatWeightList[i].Weight;//当前体重
        //                for (int j=0;j< noRepeatHeightList.Count;j++)
        //                {
        //                    if(string.Equals(noRepeatWeightList[i].RecordTime.Date, noRepeatHeightList[j].RecordTime.Date))
        //                    {
        //                        currentHeight = noRepeatHeightList[j].Height;
        //                        break;
        //                    }
        //                }

        //                if (currentWeight != 0 && currentHeight != 0)
        //                {
        //                    CustomerBMI customerBMI = new CustomerBMI();
        //                    customerBMI.RecordTime = noRepeatWeightList[i].RecordTime;
        //                    customerBMI.BMI = Convert.ToSingle(currentWeight / Math.Pow(currentHeight / 100, 2));
        //                    noRepeatIBMList.Add(customerBMI);
        //                }
        //                else
        //                {
        //                    noRepeatIBMList.Add(new CustomerBMI { RecordTime = noRepeatWeightList[i].RecordTime, BMI = 0 });
        //                }
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 0; i < noRepeatHeightList.Count; i++)
        //            {
        //                currentWeight = 0.0f;//初始化
        //                currentHeight = 0.0f;//初始化

        //                currentHeight = noRepeatHeightList[i].Height;//当前身高
        //                for (int j = 0; j < noRepeatWeightList.Count; j++)
        //                {
        //                    if (string.Equals(noRepeatHeightList[i].RecordTime.Date, noRepeatWeightList[j].RecordTime.Date))
        //                    {
        //                        currentWeight = noRepeatWeightList[j].Weight;
        //                        break;
        //                    }
        //                }

        //                if (currentWeight != 0 && currentHeight != 0)
        //                {
        //                    CustomerBMI customerBMI = new CustomerBMI();
        //                    customerBMI.RecordTime = noRepeatHeightList[i].RecordTime;
        //                    customerBMI.BMI = Convert.ToSingle(currentWeight / Math.Pow(currentHeight / 100, 2));
        //                    noRepeatIBMList.Add(customerBMI);
        //                }
        //                else
        //                {
        //                    noRepeatIBMList.Add(new CustomerBMI { RecordTime = noRepeatHeightList[i].RecordTime, BMI = 0 });
        //                }
        //            }
        //        }

        //        #endregion 处理BMI数值，以最少数据为基准，获取的同一天数据进行处理

        //        //获取前dataCount个数据
        //        result = new GetOneCustomerBMIListResponse();
        //        result.BMIList = noRepeatIBMList.Take(dataCount).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //    return result;
        //}


        //public GetOneCustomerPhysiqueResponse GetOneCustomerPhysique(Guid oid)
        //{
        //    GetOneCustomerPhysiqueResponse result;
        //    try
        //    {
        //        #region 获取体质测试结果列表------来源于Mercury程序中【TraditionalMedicalConstitutionController】
        //        //计分板
        //        Dictionary<string, double> scoreBoard = new Dictionary<string, double>
        //        {
        //        {"阴虚质",0d },
        //        {"阳虚质",0d },
        //        {"气虚质",0d },
        //        {"气郁质",0d },
        //        {"血瘀质",0d },
        //        {"湿热质",0d },
        //        {"痰湿质",0d },
        //        {"特禀质",0d },
        //        {"平和质",0d }
        //        };

        //        //创建数组，储存每道问题属于哪一种体质
        //        #region 题目类型
        //        string jsonString =
        //            "[" +
        //            "{\"Constitution\":\"平和质\",\"QuestionGuid\":\"ae4b39ed-501e-4654-a16c-45ce98fc53e2\"}" +
        //            ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"06bb9ddc-1725-4900-8ea8-9c2f1f2d24ca\"}" +
        //            ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"5ea1223c-9497-4530-a0c2-a66bea9dbd2f\"}" +
        //            ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"7b17ef28-bf0e-4635-ad1d-b68f22dd60a6\"}" +
        //            ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"c6dd0bee-f6f5-46f6-8cdb-e278ff6ba5c7\"}" +
        //            ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"966ae059-f094-4f7b-8045-eaa05e539944\"}" +
        //            ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"01220615-766b-4ded-9097-eb15f0287507\"}" +
        //            ",{\"Constitution\":\"平和质\",\"QuestionGuid\":\"48fa4d9e-01a1-44d6-9226-efe6ea4a2dbe\"}" +
        //            ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"f721faf1-602e-4167-82f0-0b0fc7ca5974\"}" +
        //            ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"1ae0eac1-8b9c-4d13-bf3b-134f0f1ab60c\"}" +
        //            ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"be14714a-d174-4180-a330-1c601d6e79b9\"}" +
        //            ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"a857027c-71ea-48ef-b991-23104db9dc4c\"}" +
        //            ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"8f4d90f2-055f-4d99-8c2e-4c115e290d5b\"}" +
        //            ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"3d1df631-aad5-4786-8f46-76c768f42133\"}" +
        //            ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"66c27b32-e168-404c-b20e-7bf5c895c143\"}" +
        //            ",{\"Constitution\":\"气虚质\",\"QuestionGuid\":\"86cd1cff-5029-4ce7-90bf-cab9710a7e5d\"}" +
        //            ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"02f5d0f4-a4d8-41d1-8ee2-08d8f5778d2b\"}" +
        //            ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"683a60a8-ced6-480c-89f7-1dcf7eac829d\"}" +
        //            ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"db72a506-edd9-4d7f-981b-57b6c36e0fcd\"}" +
        //            ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"c58bbc67-131c-4872-a610-6679f9fa4cf9\"}" +
        //            ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"bdb48675-8933-4e74-a824-9442bf11a600\"}" +
        //            ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"d01ea475-3cc8-4c5b-966f-a8bd762a1081\"}" +
        //            ",{\"Constitution\":\"气郁质\",\"QuestionGuid\":\"8eafa69f-8fb9-411f-9f9b-cea3391db8c3\"}" +
        //            ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"604045a2-92c3-4da8-a675-46a162d68d9d\"}" +
        //            ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"9df2ecc2-07dc-4253-b7c4-504515396dd2\"}" +
        //            ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"a6c1dbc2-233d-4fa8-927b-8baed0f709b3\"}" +
        //            ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"7ca4bddd-8b36-4454-8513-bec3100ec2a0\"}" +
        //            ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"23e317c9-d691-402e-8249-c0e4d3ed78d0\"}" +
        //            ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"5d2abf28-71c5-470d-959a-c75237f0689e\"}" +
        //            ",{\"Constitution\":\"湿热质\",\"QuestionGuid\":\"856a18ee-9d37-4348-9a1a-ff3fca7ca2e4\"}" +
        //            ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"f6180ada-9871-45c4-aadd-28ed8b1b1841\"}" +
        //            ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"4eff98fe-1957-44b5-b084-37bf67e5356c\"}" +
        //            ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"23982696-2722-4dc3-a1e2-524d72a38197\"}" +
        //            ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"b1a5da63-a4fc-4505-b71e-86dacbcec6f8\"}" +
        //            ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"6d18ad7c-2781-4b73-ad9b-9186b2e87fde\"}" +
        //            ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"16b8dbc7-4d7f-42c1-90c2-aae5e8870549\"}" +
        //            ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"824b8f3c-7df5-4ed0-8473-ba167a94dcec\"}" +
        //            ",{\"Constitution\":\"痰湿质\",\"QuestionGuid\":\"068e7f5b-a70e-46ab-8186-e1ed89e20a96\"}" +
        //            ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"b26af282-ed74-45cc-95ed-03e55739d0d9\"}" +
        //            ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"cacf9241-580e-485b-8c39-4cd474319d11\"}" +
        //            ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"7744a6a9-8dc8-4491-b62a-58b0bbb97468\"}" +
        //            ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"8f6ea2be-e7a1-4120-a016-5e8b50c6f5f2\"}" +
        //            ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"f74f9607-73aa-468b-90a4-6e0f435b7455\"}" +
        //            ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"99c780f6-b005-4d40-9660-6e565baf556c\"}" +
        //            ",{\"Constitution\":\"特禀质\",\"QuestionGuid\":\"d15cf76c-c8b6-46d8-9ce5-c2754ee33db9\"}" +
        //            ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"0324dd99-8371-42c4-a6ef-0c558c88a40b\"}" +
        //            ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"0b4231ed-1340-4a57-832f-38c0f8b3d795\"}" +
        //            ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"4669abf8-d1f7-42e7-a96b-3df34da8dbc0\"}" +
        //            ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"3b8c85d9-a8df-404e-8410-4669817d8139\"}" +
        //            ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"c05d4411-3043-4d72-bd28-ecdca6a6a1a1\"}" +
        //            ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"393cd5f8-1bcf-434f-9208-f64a497622af\"}" +
        //            ",{\"Constitution\":\"血瘀质\",\"QuestionGuid\":\"dff885fd-df8c-4075-9484-fc3e9afe8e7d\"}" +
        //            ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"e1f6f091-3d1a-42c0-abf6-07babde77e93\"}" +
        //            ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"d9228fcf-5467-49ce-861f-67e603b8e5c0\"}" +
        //            ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"78bc2059-ade0-4478-93ab-9314e8d817c3\"}" +
        //            ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"e585e827-3b70-4b8c-9c86-9f7fe59df6d0\"}" +
        //            ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"6a51c98d-2900-4cd5-8eab-a20b405533ca\"}" +
        //            ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"4460bf86-7caa-4180-bd8b-e19f04596adc\"}" +
        //            ",{\"Constitution\":\"阳虚质\",\"QuestionGuid\":\"29dfedf4-4ddf-4a23-99f3-fc399e15df1e\"}" +
        //            ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"79b98ed2-58e7-465f-9985-12041dfb6332\"}" +
        //            ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"25801e21-5e5d-4b70-b688-30b273ee8b23\"}" +
        //            ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"c0290174-8f90-405d-86c3-5de55a814a7c\"}" +
        //            ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"dff9548d-d0f1-4c62-80ec-6b1444c76ee0\"}" +
        //            ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"fdf47022-406f-470c-860f-711a03c6cf4f\"}" +
        //            ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"1433f352-b2d6-41ab-8c59-75bbdb9a8a53\"}" +
        //            ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"6b354226-1925-4d0e-959c-cc2dc731e9cd\"}" +
        //            ",{\"Constitution\":\"阴虚质\",\"QuestionGuid\":\"f2effb5f-b75f-4be4-bcb0-eac877a8f344\"}" +
        //            "]";
        //        #endregion 题目类型

        //        //记录每一道问题属于哪种体质的数组
        //        QuestionConstitution[] questionConstitutions = JsonConvert.DeserializeObject<QuestionConstitution[]>(jsonString);

        //        //记录本问卷的问题列表
        //        Guid[] questionGuids = (from questionConstitution in questionConstitutions select questionConstitution.QuestionGuid).ToArray();

        //        //拉取指定用户所有答案
        //        var originalAnswer = _answerRepository.GetAnswerByConditions(oid);

        //        if(!originalAnswer.Any())
        //        {
        //            return null;
        //        }

        //        //排除重复数据
        //        var noRepeatAnswer = from t in originalAnswer
        //                             group t by new { t.QuestionGuid, t.QuestionVersion }
        //                             into mygroup
        //                             select mygroup.FirstOrDefault();

        //        //统计体质结果
        //        bool answerComplete = true;
        //        answerComplete = true;
        //        //遍历问卷问题数据，在用户答案中查找匹配值
        //        foreach (Guid question in questionGuids)
        //        {
        //            try
        //            {
        //                scoreBoard[questionConstitutions.First(x => x.QuestionGuid == question).Constitution] += GetScore(question, originalAnswer.First(x => x.QuestionGuid == question).Content, questionConstitutions);
        //            }
        //            catch (InvalidOperationException ex) when (ex.Message == "Sequence contains no matching element" || ex.Message == "Sequence contains no elements")
        //            {
        //                //两道性别相关的问题用户必然缺失一道，捕获异常
        //                if (question == Guid.Parse("23E317C9-D691-402E-8249-C0E4D3ED78D0") || question == Guid.Parse("A6C1DBC2-233D-4FA8-927B-8BAED0F709B3"))
        //                {
        //                    continue;
        //                }
        //                else
        //                {
        //                    //如果用户未能答完问卷，将返回此错误
        //                    answerComplete = false;
        //                    break;
        //                }
        //            }
        //        }

        //        //体质测试结果
        //        CustomerPhysique customerPhysique = new CustomerPhysique();
        //        //结果列表
        //        List<Physique> physiqueList = new List<Physique>();

        //        result = new GetOneCustomerPhysiqueResponse();
        //        result.CustomerPhysique = new CustomerPhysique();

        //        if (answerComplete == false)
        //        {
        //            //当前日期回答不完整
        //            physiqueList.Add(new Physique { Title = "问题回答不完整，无结论" });
        //            customerPhysique.RecordTime = originalAnswer.LastOrDefault(x => x.Ctime != null).Ctime;
        //            customerPhysique.Physique = physiqueList;

        //            result.CustomerPhysique = customerPhysique;
        //            return result;
        //        }

        //        #region 统计体质结果列表

        //        //表现为平和质
        //        bool mayPingheCons = true;
        //        //倾向于平和质
        //        bool tendPingheCons = true;

        //        //遍历计分板的九个key
        //        foreach (KeyValuePair<string, double> score in scoreBoard)
        //        {
        //            //平和质计算需要其他体质的计算结果，先跳过
        //            if (score.Key == "平和质")
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                int questionCount;
        //                switch (score.Key)
        //                {
        //                    case "湿热质":
        //                        questionCount = 6;
        //                        break;
        //                    case "阳虚质":
        //                    case "血瘀质":
        //                    case "气郁质":
        //                    case "特禀质":
        //                        questionCount = 7;
        //                        break;
        //                    case "阴虚质":
        //                    case "气虚质":
        //                    case "痰湿质":
        //                        questionCount = 8;
        //                        break;
        //                    default:
        //                        throw new InvalidOperationException("体质定义错误");
        //                }
        //                double transScore = (score.Value - questionCount) * 100 / questionCount / 4;
        //                if (transScore >= 40)
        //                {
        //                    physiqueList.Add(new Physique { Title = score.Key, CurrentCent = transScore, StandCent = 40 });
        //                    //当体质偏颇时，不符合平和质标准，置false
        //                    mayPingheCons = false;
        //                    tendPingheCons = false;
        //                }
        //                else if (transScore <= 39 && transScore >= 30)
        //                {
        //                    physiqueList.Add(new Physique { Title = $"倾向{score.Key}", CurrentCent = transScore, StandCent = 30 });
        //                    mayPingheCons = false;
        //                }

        //            }
        //        }

        //        //如果没有任何体质偏颇，检查是否符合平和质标准
        //        if (tendPingheCons)
        //        {
        //            double transScore = (scoreBoard["平和质"] - 8) * 25 / 8;
        //            if (transScore >= 60)
        //            {
        //                physiqueList.Add(new Physique { Title = mayPingheCons ? "平和质" : "倾向平和质", CurrentCent = transScore, StandCent = 60 });
        //            }
        //            else
        //            {
        //                physiqueList.Add(new Physique { Title = "平和质", CurrentCent = transScore, StandCent = 0 });
        //            }
        //        }

        //        customerPhysique.RecordTime = originalAnswer.LastOrDefault(x => x.Ctime != null).Ctime;
        //        customerPhysique.Physique = physiqueList;
        //        #endregion 统计体质结果列表

        //        #endregion 获取体质测试结果列表------来源于Mercury程序中【TraditionalMedicalConstitutionController】

        //        //获取数据
        //        result.CustomerPhysique = customerPhysique;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //    return result;
        //}




        #endregion 以前版本
    }
}
