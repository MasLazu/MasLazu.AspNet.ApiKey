using MasLazu.AspNet.ApiKey.Abstraction.Models;
using Xunit;

namespace MasLazu.AspNet.ApiKey.Abstraction.Test.Models;

public class ApiKeyScopeRequestTests
{
    [Fact]
    public void ApiKeyScopeRequest_CanBeInstantiated()
    {
        // Arrange
        var apiKeyId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        // Act
        var request = new ApiKeyScopeRequest(
            ApiKeyId: apiKeyId,
            PermissionId: permissionId
        );

        // Assert
        Assert.NotNull(request);
        Assert.Equal(apiKeyId, request.ApiKeyId);
        Assert.Equal(permissionId, request.PermissionId);
    }

    [Fact]
    public void ApiKeyScopeRequest_PropertiesAreSetCorrectly()
    {
        // Arrange
        var apiKeyId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        // Act
        var request = new ApiKeyScopeRequest(
            ApiKeyId: apiKeyId,
            PermissionId: permissionId
        );

        // Assert
        Assert.NotEqual(Guid.Empty, request.ApiKeyId);
        Assert.NotEqual(Guid.Empty, request.PermissionId);
        Assert.Equal(apiKeyId, request.ApiKeyId);
        Assert.Equal(permissionId, request.PermissionId);
    }
}
