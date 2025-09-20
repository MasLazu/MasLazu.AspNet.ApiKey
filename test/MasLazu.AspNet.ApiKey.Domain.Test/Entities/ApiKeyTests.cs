using MasLazu.AspNet.ApiKey.Domain.Entities;
using MasLazu.AspNet.Framework.Domain.Entities;
using Xunit;

namespace MasLazu.AspNet.ApiKey.Domain.Test.Entities;

public class ApiKeyTests
{
    [Fact]
    public void ApiKey_CanBeInstantiated()
    {
        // Act
        var apiKey = new Domain.Entities.ApiKey();

        // Assert
        Assert.NotNull(apiKey);
        Assert.IsType<Domain.Entities.ApiKey>(apiKey);
    }

    [Fact]
    public void ApiKey_InheritsFrom_BaseEntity()
    {
        // Act
        var apiKey = new Domain.Entities.ApiKey();

        // Assert
        Assert.IsAssignableFrom<BaseEntity>(apiKey);
    }

    [Fact]
    public void ApiKey_Properties_CanBeSet()
    {
        // Arrange
        var userId = Guid.NewGuid();
        string key = "test-api-key";
        string name = "Test API Key";
        DateTime expiresDate = DateTime.UtcNow.AddDays(30);
        DateTime lastUsed = DateTime.UtcNow;
        DateTime revokedDate = DateTime.UtcNow.AddDays(60);

        // Act
        var apiKey = new Domain.Entities.ApiKey
        {
            UserId = userId,
            Key = key,
            Name = name,
            ExpiresDate = expiresDate,
            LastUsed = lastUsed,
            RevokedDate = revokedDate
        };

        // Assert
        Assert.Equal(userId, apiKey.UserId);
        Assert.Equal(key, apiKey.Key);
        Assert.Equal(name, apiKey.Name);
        Assert.Equal(expiresDate, apiKey.ExpiresDate);
        Assert.Equal(lastUsed, apiKey.LastUsed);
        Assert.Equal(revokedDate, apiKey.RevokedDate);
    }

    [Fact]
    public void ApiKey_DefaultValues_AreCorrect()
    {
        // Act
        var apiKey = new Domain.Entities.ApiKey();

        // Assert
        Assert.Equal(Guid.Empty, apiKey.UserId);
        Assert.Equal(string.Empty, apiKey.Key);
        Assert.Null(apiKey.Name);
        Assert.Null(apiKey.ExpiresDate);
        Assert.Null(apiKey.LastUsed);
        Assert.Null(apiKey.RevokedDate);
        Assert.NotNull(apiKey.Scopes);
        Assert.Empty(apiKey.Scopes);
    }

    [Fact]
    public void ApiKey_ScopesCollection_IsInitialized()
    {
        // Act
        var apiKey = new Domain.Entities.ApiKey();

        // Assert
        Assert.NotNull(apiKey.Scopes);
        Assert.IsAssignableFrom<ICollection<ApiKeyScope>>(apiKey.Scopes);
    }

    [Fact]
    public void ApiKey_CanAddScopes()
    {
        // Arrange
        var apiKey = new Domain.Entities.ApiKey();
        var scope = new ApiKeyScope { ApiKeyId = Guid.NewGuid(), PermissionId = Guid.NewGuid() };

        // Act
        apiKey.Scopes.Add(scope);

        // Assert
        Assert.Single(apiKey.Scopes);
        Assert.Contains(scope, apiKey.Scopes);
    }

    [Fact]
    public void ApiKey_CanRemoveScopes()
    {
        // Arrange
        var apiKey = new Domain.Entities.ApiKey();
        var scope = new ApiKeyScope { ApiKeyId = Guid.NewGuid(), PermissionId = Guid.NewGuid() };
        apiKey.Scopes.Add(scope);

        // Act
        apiKey.Scopes.Remove(scope);

        // Assert
        Assert.Empty(apiKey.Scopes);
    }

    [Fact]
    public void ApiKey_CanClearScopes()
    {
        // Arrange
        var apiKey = new Domain.Entities.ApiKey();
        apiKey.Scopes.Add(new ApiKeyScope { ApiKeyId = Guid.NewGuid(), PermissionId = Guid.NewGuid() });
        apiKey.Scopes.Add(new ApiKeyScope { ApiKeyId = Guid.NewGuid(), PermissionId = Guid.NewGuid() });

        // Act
        apiKey.Scopes.Clear();

        // Assert
        Assert.Empty(apiKey.Scopes);
    }

    [Fact]
    public void ApiKey_Key_Property_IsRequired()
    {
        // Act
        var apiKey = new Domain.Entities.ApiKey();

        // Assert
        Assert.NotNull(apiKey.Key); // Should be initialized as empty string
        Assert.Equal(string.Empty, apiKey.Key);
    }

    [Fact]
    public void ApiKey_CanSetKeyToEmptyString()
    {
        // Arrange
        var apiKey = new Domain.Entities.ApiKey
        {
            // Act
            Key = string.Empty
        };

        // Assert
        Assert.Equal(string.Empty, apiKey.Key);
    }

    [Fact]
    public void ApiKey_CanSetKeyToValidString()
    {
        // Arrange
        var apiKey = new Domain.Entities.ApiKey();
        string testKey = "valid-api-key-123";

        // Act
        apiKey.Key = testKey;

        // Assert
        Assert.Equal(testKey, apiKey.Key);
    }
}
