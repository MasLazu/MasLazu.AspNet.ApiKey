using MasLazu.AspNet.ApiKey.Abstraction.Models;
using Xunit;

namespace MasLazu.AspNet.ApiKey.Abstraction.Test.Models;

public class CreateApiKeyScopeRequestTests
{
    [Fact]
    public void CreateApiKeyScopeRequest_CanBeInstantiated()
    {
        // Arrange
        var apiKeyId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        // Act
        var request = new CreateApiKeyScopeRequest(
            ApiKeyId: apiKeyId,
            PermissionId: permissionId
        );

        // Assert
        Assert.NotNull(request);
        Assert.Equal(apiKeyId, request.ApiKeyId);
        Assert.Equal(permissionId, request.PermissionId);
    }

    [Fact]
    public void CreateApiKeyScopeRequest_PropertiesAreSetCorrectly()
    {
        // Arrange
        var apiKeyId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        // Act
        var request = new CreateApiKeyScopeRequest(
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
