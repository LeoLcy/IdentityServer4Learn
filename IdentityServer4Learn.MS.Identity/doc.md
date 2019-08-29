## 微软Identity认证授权;

## 数据迁移
Update-Database

## 自定义用户数据需注意
1、services.AddDefaultIdentity<ApplicationUser>()
2、需要更改 DbContext 继承
https://docs.microsoft.com/zh-cn/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-2.1#custom-user-data