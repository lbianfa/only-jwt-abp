using Volo.Abp.Settings;

namespace OnlyJWT.Settings;

public class OnlyJWTSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(OnlyJWTSettings.MySetting1));
    }
}
