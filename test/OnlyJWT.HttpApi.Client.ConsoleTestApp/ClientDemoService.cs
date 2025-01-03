using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace OnlyJWT.HttpApi.Client.ConsoleTestApp;

public class ClientDemoService : ITransientDependency
{
    public ClientDemoService()
    {
    }

    public async Task RunAsync()
    {
    }
}
