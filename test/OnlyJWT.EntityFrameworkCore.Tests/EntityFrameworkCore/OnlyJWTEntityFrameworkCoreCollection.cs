using Xunit;

namespace OnlyJWT.EntityFrameworkCore;

[CollectionDefinition(OnlyJWTTestConsts.CollectionDefinitionName)]
public class OnlyJWTEntityFrameworkCoreCollection : ICollectionFixture<OnlyJWTEntityFrameworkCoreFixture>
{

}
