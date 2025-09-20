using MasLazu.AspNet.ApiKey.Domain.Entities;
using Xunit;

namespace MasLazu.AspNet.ApiKey.Domain.Test.Entities;

public class ApiKeyScopeTests
{
    [Fact]
    public void ApiKeyScope_CanBeInstantiated()
    {
        // Act
        var apiKeyScope = new ApiKeyScope();

        // Assert
        Assert.NotNull(apiKeyScope);
        Assert.IsType<ApiKeyScope>(apiKeyScope);
    }

    [Fact]
    public void ApiKeyScope_InheritsFrom_BaseEntity()
    {
        // Act
        var apiKeyScope = new ApiKeyScope();

        // Assert
        Assert.IsAssignableFrom<Framework.Domain.Entities.BaseEntity>(apiKeyScope);
    }

    [Fact]
    public void ApiKeyScope_Properties_CanBeSet()
    {
        // Arrange
        var apiKeyId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        // Act
        var apiKeyScope = new ApiKeyScope
        {
            ApiKeyId = apiKeyId,
            PermissionId = permissionId
        };

        // Assert
        Assert.Equal(apiKeyId, apiKeyScope.ApiKeyId);
        Assert.Equal(permissionId, apiKeyScope.PermissionId);
    }

    [Fact]
    public void ApiKeyScope_DefaultValues_AreCorrect()
    {
        // Act
        var apiKeyScope = new ApiKeyScope();

        // Assert
        Assert.Equal(Guid.Empty, apiKeyScope.ApiKeyId);
        Assert.Equal(Guid.Empty, apiKeyScope.PermissionId);
        Assert.Null(apiKeyScope.ApiKey);
    }

    [Fact]
    public void ApiKeyScope_CanSetNavigationProperty()
    {
        // Arrange
        var apiKeyScope = new ApiKeyScope();
        var apiKey = new Domain.Entities.ApiKey { UserId = Guid.NewGuid(), Key = "test-key" };

        // Act
        apiKeyScope.ApiKey = apiKey;

        // Assert
        Assert.NotNull(apiKeyScope.ApiKey);
        Assert.Equal(apiKey, apiKeyScope.ApiKey);
        Assert.Equal(apiKey.UserId, apiKeyScope.ApiKey.UserId);
        Assert.Equal(apiKey.Key, apiKeyScope.ApiKey.Key);
    }

    [Fact]
    public void ApiKeyScope_NavigationProperty_CanBeNull()
    {
        // Act
        var apiKeyScope = new ApiKeyScope();

        // Assert
        Assert.Null(apiKeyScope.ApiKey);
    }
}
