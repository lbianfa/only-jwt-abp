using Volo.Abp.Authorization.Permissions;

namespace OnlyJWT.Permissions;

public class OnlyJWTPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(OnlyJWTPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(OnlyJWTPermissions.MyPermission1, L("Permission:MyPermission1"));
    }
}
