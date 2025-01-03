using Volo.Abp.Modularity;

namespace OnlyJWT;

[DependsOn(
    typeof(OnlyJWTApplicationModule),
    typeof(OnlyJWTDomainTestModule)
)]
public class OnlyJWTApplicationTestModule : AbpModule
{

}
