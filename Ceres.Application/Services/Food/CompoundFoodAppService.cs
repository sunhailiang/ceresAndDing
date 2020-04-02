using AutoMapper;
using Ceres.Application.Interfaces;
using Ceres.Application.ViewModels;
using Ceres.Domain.Commands;
using Ceres.Domain.Core.Bus;
using Ceres.Domain.Core.Notifications;
using Ceres.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ceres.Application.Services
{
    public class CompoundFoodAppService : ICompoundFoodAppService
    {
        private readonly ICustomerDislikeFoodRepository _customerDislikeFoodRepository;
        private readonly IFoodRepository _foodRepository;
        private readonly IFoodComponentRepository _foodComponentRepository;
        private readonly IComponentRepository _componentRepository;
        //DTO
        private readonly IMapper _mapper;
        //中介者 总线
        private readonly IMediatorHandler Bus;
        public CompoundFoodAppService(
            ICustomerDislikeFoodRepository customerDislikeFoodRepository,
            IFoodRepository foodRepository,
            IFoodComponentRepository foodComponentRepository,
            IComponentRepository componentRepository,
            IMapper mapper,
            IMediatorHandler bus
            )
        {
            _customerDislikeFoodRepository = customerDislikeFoodRepository;
            _foodRepository = foodRepository;
            _foodComponentRepository = foodComponentRepository;
            _componentRepository = componentRepository;
            _mapper = mapper;
            Bus = bus;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public GetComponentPercentageByDailyEnergyResponse GetComponentPercentageByDailyEnergy()
        {
            GetComponentPercentageByDailyEnergyResponse result;
            try
            {
                List<FoodComponent> dailyComponentList = new List<FoodComponent>();
                FoodComponent dailyComponent = new FoodComponent();
                dailyComponent.Name = "蛋白质";
                dailyComponent.EnglishName = "Protein";
                dailyComponent.NameCode = "P";
                dailyComponent.Value = 14;
                dailyComponent.Unit = "%";
                dailyComponentList.Add(dailyComponent);

                dailyComponent = new FoodComponent();
                dailyComponent.Name = "脂肪";
                dailyComponent.EnglishName = "Fats";
                dailyComponent.NameCode = "F";
                dailyComponent.Value = 27;
                dailyComponent.Unit = "%";
                dailyComponentList.Add(dailyComponent);

                dailyComponent = new FoodComponent();
                dailyComponent.Name = "碳水化合物";
                dailyComponent.EnglishName = "Carbohydrates";
                dailyComponent.NameCode = "C";
                dailyComponent.Value = 59;
                dailyComponent.Unit = "%";
                dailyComponentList.Add(dailyComponent);

                result = new GetComponentPercentageByDailyEnergyResponse();
                result.ComponentList = dailyComponentList;
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public GetDailyFoodComponentResponse GetDailyFoodComponent(float dailyEnergy, string pNameCode, float pValue, string fNameCode, float fValue, string cNameCode, float cValue)
        {
            GetDailyFoodComponentResponse result;
            try
            {
                //蛋白质能量(kcal)
                float proteinEnergy = (dailyEnergy * pValue) / 100f;
                //蛋白质重量(g)
                float proteinWeight = proteinEnergy / 4f;

                //脂肪能量(kcal)
                float fatsEnergy = (dailyEnergy * fValue) / 100f;
                //脂肪重量(g)
                float fatsWeight = fatsEnergy / 9f;

                //碳水化合物能量(kcal)
                float carbohydratesEnergy = (dailyEnergy * cValue) / 100f;
                //碳水化合物重量(g)
                float carbohydratesWeight = carbohydratesEnergy / 4f;

                //3,4,3分配方式
                List<MealFoodComponent> mealFoodComponentList = new List<MealFoodComponent>();
                MealFoodComponent mealFoodComponent;
                List<FoodComponent> foodComponentList;

                #region 早餐
                foodComponentList = new List<FoodComponent>();
                FoodComponent foodComponent = new FoodComponent();
                foodComponent.Name = "蛋白质";
                foodComponent.EnglishName = "Protein";
                foodComponent.NameCode = pNameCode;
                foodComponent.Value = proteinWeight / 3f;
                foodComponent.Unit = "g";
                foodComponentList.Add(foodComponent);

                foodComponent = new FoodComponent();
                foodComponent.Name = "脂肪";
                foodComponent.EnglishName = "Fats";
                foodComponent.NameCode = fNameCode;
                foodComponent.Value = fatsWeight / 3f;
                foodComponent.Unit = "g";
                foodComponentList.Add(foodComponent);

                foodComponent = new FoodComponent();
                foodComponent.Name = "碳水化合物";
                foodComponent.EnglishName = "Carbohydrates";
                foodComponent.NameCode = cNameCode;
                foodComponent.Value = carbohydratesWeight / 3f;
                foodComponent.Unit = "g";
                foodComponentList.Add(foodComponent);

                mealFoodComponent = new MealFoodComponent();
                mealFoodComponent.Name = "早餐";
                mealFoodComponent.EnglishName = "Breakfast";
                mealFoodComponent.ComponentList = foodComponentList;

                mealFoodComponentList.Add(mealFoodComponent);
                #endregion 早餐

                #region 午餐
                foodComponentList = new List<FoodComponent>();
                foodComponent = new FoodComponent();
                foodComponent.Name = "蛋白质";
                foodComponent.EnglishName = "Protein";
                foodComponent.NameCode = pNameCode;
                foodComponent.Value = proteinWeight / 4f;
                foodComponent.Unit = "g";
                foodComponentList.Add(foodComponent);

                foodComponent = new FoodComponent();
                foodComponent.Name = "脂肪";
                foodComponent.EnglishName = "Fats";
                foodComponent.NameCode = fNameCode;
                foodComponent.Value = fatsWeight / 4f;
                foodComponent.Unit = "g";
                foodComponentList.Add(foodComponent);

                foodComponent = new FoodComponent();
                foodComponent.Name = "碳水化合物";
                foodComponent.EnglishName = "Carbohydrates";
                foodComponent.NameCode = cNameCode;
                foodComponent.Value = carbohydratesWeight / 4f;
                foodComponent.Unit = "g";
                foodComponentList.Add(foodComponent);

                mealFoodComponent = new MealFoodComponent();
                mealFoodComponent.Name = "午餐";
                mealFoodComponent.EnglishName = "Lunch";
                mealFoodComponent.ComponentList = foodComponentList;

                mealFoodComponentList.Add(mealFoodComponent);
                #endregion 午餐

                #region 晚餐
                foodComponentList = new List<FoodComponent>();
                foodComponent = new FoodComponent();
                foodComponent.Name = "蛋白质";
                foodComponent.EnglishName = "Protein";
                foodComponent.NameCode = pNameCode;
                foodComponent.Value = proteinWeight / 3f;
                foodComponent.Unit = "g";
                foodComponentList.Add(foodComponent);

                foodComponent = new FoodComponent();
                foodComponent.Name = "脂肪";
                foodComponent.EnglishName = "Fats";
                foodComponent.NameCode = fNameCode;
                foodComponent.Value = fatsWeight / 3f;
                foodComponent.Unit = "g";
                foodComponentList.Add(foodComponent);

                foodComponent = new FoodComponent();
                foodComponent.Name = "碳水化合物";
                foodComponent.EnglishName = "Carbohydrates";
                foodComponent.NameCode = cNameCode;
                foodComponent.Value = carbohydratesWeight / 3f;
                foodComponent.Unit = "g";
                foodComponentList.Add(foodComponent);

                mealFoodComponent = new MealFoodComponent();
                mealFoodComponent.Name = "晚餐";
                mealFoodComponent.EnglishName = "Dinner";
                mealFoodComponent.ComponentList = foodComponentList;

                mealFoodComponentList.Add(mealFoodComponent);
                #endregion 晚餐

                result = new GetDailyFoodComponentResponse();
                result.MealList = mealFoodComponentList;
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }


        //componentValue为营养成分数值，所以需要反过来计算食品应该是多少克
        private List<Food> GetRecommendFood(string nameCode,string classify, float componentValue, int dataCount,int click, List<Guid> dislikeFoodOidList)
        {
            //依据食材分类，重要程度，获取指定食材
            List<Food> foodList = _mapper.Map<List<Food>>(_foodRepository.GetFoodByClassify(classify, click));

            //去除客户讨厌的食物
            List<Food> likeFoodList = foodList;
            foreach (var foodOid in dislikeFoodOidList)
            {

                var temp = likeFoodList.Where(c => c.OID != foodOid);
                likeFoodList = new List<Food>();
                likeFoodList = temp.ToList();
            }

            //获取指定数目食材
            List<Food> recommendFoodList = likeFoodList.OrderBy(c => Guid.NewGuid()).Take(dataCount).ToList();

            //增加营养元素的数据
            foreach (var recommendFood in recommendFoodList)
            {
                //获取Food所对应的Component列表
                var recommendFoodComponentList = _foodComponentRepository.GetFoodComponentByFoodOid(recommendFood.OID);
                //获取Food重量
                foreach (var recommendFoodComponent in recommendFoodComponentList)
                {
                    var component = _componentRepository.GetById(recommendFoodComponent.ComponentOid);
                    if(component.NameCode==nameCode)
                    {
                        if(string.IsNullOrEmpty(recommendFoodComponent.Value.ToString())||string.IsNullOrWhiteSpace(recommendFoodComponent.Value.ToString()))
                        {
                            recommendFood.Value = -1f;
                        }
                        else
                        {
                            recommendFood.Value = componentValue * 100f / recommendFoodComponent.Value;
                        }
                        recommendFood.Unit = "g";
                        break;
                    }
                }
                //增加营养元素的数据
                List<FoodComponent> foodComponentList = new List<FoodComponent>();
                foreach (var recommendFoodComponent in recommendFoodComponentList)
                {
                    var recommendComponent = _mapper.Map<FoodComponent>(_componentRepository.GetById(recommendFoodComponent.ComponentOid));
                    recommendComponent.Value = (recommendFoodComponent.Value* recommendFood.Value) /100f;
                    foodComponentList.Add(recommendComponent);
                }
                recommendFood.FoodComponentList = foodComponentList;
            }

            return recommendFoodList.OrderByDescending(c=>c.Value).ToList();
        }

        private void GetRecommendFood(string nameCode,string classify ,float componentValue,int dataCount, List<Guid> dislikeFoodOidList,ref List<ClassifyFood> classifyFoodList)
        {
            ClassifyFood classifyFood = new ClassifyFood();
            classifyFood.Classify = classify;
            classifyFood.FoodList = new List<Food>();

            //随机获取指定数量的第一等级的食材
            int click = 200;
            int dataCountTemp = int.Parse(Math.Round(dataCount * 0.7d).ToString());
            classifyFood.FoodList.AddRange(GetRecommendFood(nameCode, classify, componentValue, dataCountTemp, click, dislikeFoodOidList));

            //随机获取指定数量的第二等级的食材
            click = 100;
            dataCountTemp = dataCount - dataCountTemp;
            classifyFood.FoodList.AddRange(GetRecommendFood(nameCode, classify, componentValue, dataCountTemp, click, dislikeFoodOidList));

            //合并数据
            if(classifyFood.FoodList.Count>0)
            {
                classifyFoodList.Add(classifyFood);
            }
        }

        public GetRecommendFoodResponse GetRecommendFood(Guid oid, string nameCode, float componentValue, int dataCount)
        {
            GetRecommendFoodResponse result=new GetRecommendFoodResponse();
            try
            {
                //获取客户讨厌的食物
                var customerDislikeFood = _customerDislikeFoodRepository.GetCustomerDislikeFoodByCustomerOid(oid);

                //转换
                List<Guid> dislikeFoodOidList = new List<Guid>();
                foreach(var dislikeFood in customerDislikeFood)
                {
                    dislikeFoodOidList.Add(dislikeFood.FoodOid);
                }

                //判断所需的食材分类
                List<ClassifyFood> classifyFoodList = new List<ClassifyFood>();

                string classify = "";     
                switch(nameCode)
                {
                    case "P"://蛋白质
                        classify = "鱼类";
                        GetRecommendFood(nameCode,classify, componentValue, dataCount, dislikeFoodOidList, ref classifyFoodList);

                        classify = "肉类";
                        GetRecommendFood(nameCode, classify, componentValue, dataCount, dislikeFoodOidList, ref classifyFoodList);

                        classify = "蛋类";
                        GetRecommendFood(nameCode, classify, componentValue, dataCount, dislikeFoodOidList, ref classifyFoodList);

                        classify = "奶类";
                        GetRecommendFood(nameCode, classify, componentValue, dataCount, dislikeFoodOidList, ref classifyFoodList);

                        classify = "豆类";
                        GetRecommendFood(nameCode, classify, componentValue, dataCount, dislikeFoodOidList, ref classifyFoodList);

                        break;
                    case "F"://脂肪
                        classify = "油脂类";
                        GetRecommendFood(nameCode, classify, componentValue, dataCount, dislikeFoodOidList, ref classifyFoodList);

                        classify = "坚果类";
                        GetRecommendFood(nameCode, classify, componentValue, dataCount, dislikeFoodOidList, ref classifyFoodList);

                        break;
                    case "C"://碳水化合物
                        classify = "谷薯类";
                        GetRecommendFood(nameCode, classify, componentValue, dataCount, dislikeFoodOidList, ref classifyFoodList);

                        classify = "蔬菜类";
                        GetRecommendFood(nameCode, classify, componentValue, dataCount, dislikeFoodOidList, ref classifyFoodList);

                        classify = "水果类";
                        GetRecommendFood(nameCode, classify, componentValue, dataCount, dislikeFoodOidList, ref classifyFoodList);

                        break;
                }

                result = new GetRecommendFoodResponse();
                result.ClassifyFoodList = classifyFoodList;
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }
    }
}
