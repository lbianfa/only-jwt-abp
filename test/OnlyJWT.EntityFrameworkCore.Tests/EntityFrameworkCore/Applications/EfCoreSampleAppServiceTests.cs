using OnlyJWT.Samples;
using Xunit;

namespace OnlyJWT.EntityFrameworkCore.Applications;

[Collection(OnlyJWTTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<OnlyJWTEntityFrameworkCoreTestModule>
{

}
