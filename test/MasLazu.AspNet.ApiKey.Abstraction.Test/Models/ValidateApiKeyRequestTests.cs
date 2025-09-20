using MasLazu.AspNet.ApiKey.Abstraction.Models;
using Xunit;

namespace MasLazu.AspNet.ApiKey.Abstraction.Test.Models;

public class ValidateApiKeyRequestTests
{
    [Fact]
    public void ValidateApiKeyRequest_CanBeInstantiated()
    {
        // Arrange
        string key = "test-api-key";
        var permissionId = Guid.NewGuid();

        // Act
        var request = new ValidateApiKeyRequest(
            Key: key,
            PermissionId: permissionId
        );

        // Assert
        Assert.NotNull(request);
        Assert.Equal(key, request.Key);
        Assert.Equal(permissionId, request.PermissionId);
    }

    [Fact]
    public void ValidateApiKeyRequest_WithNullPermissionId_CanBeCreated()
    {
        // Arrange
        string key = "test-api-key";

        // Act
        var request = new ValidateApiKeyRequest(
            Key: key,
            PermissionId: null
        );

        // Assert
        Assert.Equal(key, request.Key);
        Assert.Null(request.PermissionId);
    }

    [Fact]
    public void ValidateApiKeyRequest_KeyIsRequired()
    {
        // Arrange
        string key = "test-api-key";

        // Act
        var request = new ValidateApiKeyRequest(
            Key: key,
            PermissionId: null
        );

        // Assert
        Assert.NotNull(request.Key);
        Assert.NotEmpty(request.Key);
    }
}
