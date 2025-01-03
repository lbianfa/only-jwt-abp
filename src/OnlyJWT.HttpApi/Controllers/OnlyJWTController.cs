using Volo.Abp.AspNetCore.Mvc;

namespace OnlyJWT.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class OnlyJWTController : AbpControllerBase
{
    protected OnlyJWTController()
    {
    }
}
