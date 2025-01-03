using OnlyJWT.Samples;
using Xunit;

namespace OnlyJWT.EntityFrameworkCore.Domains;

[Collection(OnlyJWTTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<OnlyJWTEntityFrameworkCoreTestModule>
{

}
