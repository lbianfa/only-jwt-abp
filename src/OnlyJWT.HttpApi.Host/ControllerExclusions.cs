using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.ApiExploring;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.FeatureManagement;

namespace OnlyJWT
{
    public class ControllerExclusions : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            application.Controllers.RemoveAll(x => x.ControllerType == typeof(AbpApiDefinitionController));
            application.Controllers.RemoveAll(x => x.ControllerType == typeof(AbpApplicationConfigurationController));
            application.Controllers.RemoveAll(x => x.ControllerType == typeof(AbpApplicationLocalizationController));
            application.Controllers.RemoveAll(x => x.ControllerType == typeof(TimeZoneSettingsController));
            application.Controllers.RemoveAll(x => x.ControllerType == typeof(EmailSettingsController));
            application.Controllers.RemoveAll(x => x.ControllerType == typeof(TenantController));
            application.Controllers.RemoveAll(x => x.ControllerType == typeof(FeaturesController));
            application.Controllers.RemoveAll(x => x.ControllerType == typeof(IdentityUserLookupController));
        }
    }
}
