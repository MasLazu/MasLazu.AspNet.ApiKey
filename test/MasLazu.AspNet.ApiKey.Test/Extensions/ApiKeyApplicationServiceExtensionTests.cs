using Microsoft.Extensions.DependencyInjection;
using MasLazu.AspNet.ApiKey.Extensions;
using MasLazu.AspNet.ApiKey.Abstraction.Interfaces;
using Xunit;

namespace MasLazu.AspNet.ApiKey.Test.Extensions;

public class ApiKeyApplicationServiceExtensionTests
{
    [Fact]
    public void AddApiKeyApplicationServices_ReturnsServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        IServiceCollection result = services.AddApiKeyApplicationServices();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ServiceCollection>(result);
        Assert.Same(services, result);
    }

    [Fact]
    public void AddApiKeyApplicationServices_CanBeChained()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        IServiceCollection result = services
            .AddApiKeyApplicationServices()
            .AddSingleton(typeof(object));

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ServiceCollection>(result);
    }
}
