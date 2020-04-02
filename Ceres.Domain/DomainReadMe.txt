Ceres.Domain领域层
---实体类
---Repository接口

---Models文件夹下存放实体类，与数据库一致
---Models中类名称与数据库实体类名称一致
---Models中类必须继承Ceres.Domain.Core中Models文件夹下Entity.cs，获取OID属性

---Interfaces文件夹下IRepository存放泛型接口,所有接口必须继承
---Interfaces中接口必须继承IRepository.cs，获取通用方法