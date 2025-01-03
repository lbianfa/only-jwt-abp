using Volo.Abp.Modularity;

namespace OnlyJWT;

/* Inherit from this class for your domain layer tests. */
public abstract class OnlyJWTDomainTestBase<TStartupModule> : OnlyJWTTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
