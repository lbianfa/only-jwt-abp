using Volo.Abp.Modularity;

namespace OnlyJWT;

public abstract class OnlyJWTApplicationTestBase<TStartupModule> : OnlyJWTTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
