using System.Threading.Tasks;

namespace OnlyJWT.Data;

public interface IOnlyJWTDbSchemaMigrator
{
    Task MigrateAsync();
}
