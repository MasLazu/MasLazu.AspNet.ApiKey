using Microsoft.Extensions.DependencyInjection;
using MasLazu.AspNet.ApiKey.EfCore.Extensions;
using Xunit;

namespace MasLazu.AspNet.ApiKey.EfCore.Test.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddApiKeyEntityFrameworkCore_ReturnsServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        IServiceCollection result = services.AddApiKeyEntityFrameworkCore();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ServiceCollection>(result);
        Assert.Same(services, result);
    }

    [Fact]
    public void AddApiKeyEntityFrameworkCore_CanBeChained()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        IServiceCollection result = services
            .AddApiKeyEntityFrameworkCore()
            .AddSingleton(typeof(object));

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ServiceCollection>(result);
    }

    [Fact]
    public void AddApiKeyEntityFrameworkCore_DoesNotThrowException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act & Assert
        Exception exception = Record.Exception(() => services.AddApiKeyEntityFrameworkCore());
        Assert.Null(exception);
    }
}
