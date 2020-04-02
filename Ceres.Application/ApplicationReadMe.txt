Ceres.Application应用层
---视图类
---视图←→实体互相映射

---ViewModels文件夹下存放视图类，与UI，WebApi需求保持一致
---ViewModels中类名称后缀加上ViewModel

---Interfaces中接口名称为IxxxAppService

---Services文件夹下存放服务接口实现类
---Services中接口必须继承IxxxAppService.cs，获取通用方法

---AutoMapper用于领域模型与视图模型之间的映射