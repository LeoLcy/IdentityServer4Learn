## 微软Identity认证授权;

## 数据迁移
Update-Database

## 自定义用户数据需注意
1、services.AddDefaultIdentity<ApplicationUser>()
2、需要更改 DbContext 继承
https://docs.microsoft.com/zh-cn/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-2.1#custom-user-data


## 搭建Identity的基础架构

添加->新搭建基架的项目->标识（Identity）->生成Identity架构