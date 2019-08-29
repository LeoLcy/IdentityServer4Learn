## 更新数据库
Add-Migration InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
Update-Database -Context PersistedGrantDbContext
Add-Migration InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
Update-Database -Context ConfigurationDbContext

## 需要生成证书
AddDeveloperSigningCredential(filename: "tempkey.rsa")

## 