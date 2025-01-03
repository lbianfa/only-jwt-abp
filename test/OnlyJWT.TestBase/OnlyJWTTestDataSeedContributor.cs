using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace OnlyJWT;

public class OnlyJWTTestDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    public Task SeedAsync(DataSeedContext context)
    {
        /* Seed additional test data... */

        return Task.CompletedTask;
    }
}
