using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace OnlyJWT.Data;

/* This is used if database provider does't define
 * IOnlyJWTDbSchemaMigrator implementation.
 */
public class NullOnlyJWTDbSchemaMigrator : IOnlyJWTDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
