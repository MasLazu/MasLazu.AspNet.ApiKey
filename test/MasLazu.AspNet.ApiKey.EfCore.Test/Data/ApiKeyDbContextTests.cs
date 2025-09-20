using Microsoft.EntityFrameworkCore;
using MasLazu.AspNet.ApiKey.Domain.Entities;
using MasLazu.AspNet.ApiKey.EfCore.Data;
using Xunit;

namespace MasLazu.AspNet.ApiKey.EfCore.Test.Data;

public class ApiKeyDbContextTests
{
    [Fact]
    public void ApiKeyDbContext_CanBeInstantiated()
    {
        // Arrange
        DbContextOptions<ApiKeyDbContext> options = CreateInMemoryDbContextOptions();

        // Act
        using var context = new ApiKeyDbContext(options);

        // Assert
        Assert.NotNull(context);
        Assert.IsType<ApiKeyDbContext>(context);
    }

    [Fact]
    public void ApiKeyDbContext_InheritsFrom_BaseDbContext()
    {
        // Arrange
        DbContextOptions<ApiKeyDbContext> options = CreateInMemoryDbContextOptions();

        // Act
        using var context = new ApiKeyDbContext(options);

        // Assert
        Assert.IsAssignableFrom<MasLazu.AspNet.Framework.EfCore.Data.BaseDbContext>(context);
    }

    [Fact]
    public void ApiKeyDbContext_HasApiKeysDbSet()
    {
        // Arrange
        DbContextOptions<ApiKeyDbContext> options = CreateInMemoryDbContextOptions();

        // Act
        using var context = new ApiKeyDbContext(options);

        // Assert
        Assert.NotNull(context.ApiKeys);
        Assert.IsAssignableFrom<DbSet<Domain.Entities.ApiKey>>(context.ApiKeys);
    }

    [Fact]
    public void ApiKeyDbContext_HasApiKeyScopesDbSet()
    {
        // Arrange
        DbContextOptions<ApiKeyDbContext> options = CreateInMemoryDbContextOptions();

        // Act
        using var context = new ApiKeyDbContext(options);

        // Assert
        Assert.NotNull(context.ApiKeyScopes);
        Assert.IsAssignableFrom<DbSet<ApiKeyScope>>(context.ApiKeyScopes);
    }

    [Fact]
    public void ApiKeyDbContext_CanAddApiKey()
    {
        // Arrange
        DbContextOptions<ApiKeyDbContext> options = CreateInMemoryDbContextOptions();
        var apiKey = new Domain.Entities.ApiKey
        {
            UserId = Guid.NewGuid(),
            Key = "test-key",
            Name = "Test API Key"
        };

        // Act
        using var context = new ApiKeyDbContext(options);
        context.ApiKeys.Add(apiKey);
        int count = context.SaveChanges();

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void ApiKeyDbContext_CanAddApiKeyScope()
    {
        // Arrange
        DbContextOptions<ApiKeyDbContext> options = CreateInMemoryDbContextOptions();
        var apiKeyScope = new ApiKeyScope
        {
            ApiKeyId = Guid.NewGuid(),
            PermissionId = Guid.NewGuid()
        };

        // Act
        using var context = new ApiKeyDbContext(options);
        context.ApiKeyScopes.Add(apiKeyScope);
        int count = context.SaveChanges();

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void ApiKeyDbContext_CanQueryApiKeys()
    {
        // Arrange
        DbContextOptions<ApiKeyDbContext> options = CreateInMemoryDbContextOptions();
        var apiKey = new Domain.Entities.ApiKey
        {
            UserId = Guid.NewGuid(),
            Key = "test-key",
            Name = "Test API Key"
        };

        using (var context = new ApiKeyDbContext(options))
        {
            context.ApiKeys.Add(apiKey);
            context.SaveChanges();
        }

        // Act
        using var queryContext = new ApiKeyDbContext(options);
        Domain.Entities.ApiKey? result = queryContext.ApiKeys.FirstOrDefault();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(apiKey.Key, result.Key);
    }

    private static DbContextOptions<ApiKeyDbContext> CreateInMemoryDbContextOptions()
    {
        return new DbContextOptionsBuilder<ApiKeyDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }
}
