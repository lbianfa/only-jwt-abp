using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlyJWT.Data;
using Volo.Abp.DependencyInjection;

namespace OnlyJWT.EntityFrameworkCore;

public class EntityFrameworkCoreOnlyJWTDbSchemaMigrator
    : IOnlyJWTDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreOnlyJWTDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the OnlyJWTDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<OnlyJWTDbContext>()
            .Database
            .MigrateAsync();
    }
}
