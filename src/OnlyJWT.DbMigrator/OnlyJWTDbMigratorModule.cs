using OnlyJWT.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace OnlyJWT.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(OnlyJWTEntityFrameworkCoreModule),
    typeof(OnlyJWTApplicationContractsModule)
    )]
public class OnlyJWTDbMigratorModule : AbpModule
{
}
