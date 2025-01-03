using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace OnlyJWT.Controllers;

public class HomeController : AbpController
{
    public ActionResult Index()
    {
        return Redirect("~/swagger");
    }
}
