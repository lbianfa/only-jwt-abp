using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace OnlyJWT;

[Dependency(ReplaceServices = true)]
public class OnlyJWTBrandingProvider : DefaultBrandingProvider
{
    public OnlyJWTBrandingProvider()
    {
    }
}
