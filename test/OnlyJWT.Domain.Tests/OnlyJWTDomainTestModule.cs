using Volo.Abp.Modularity;

namespace OnlyJWT;

[DependsOn(
    typeof(OnlyJWTDomainModule),
    typeof(OnlyJWTTestBaseModule)
)]
public class OnlyJWTDomainTestModule : AbpModule
{

}
