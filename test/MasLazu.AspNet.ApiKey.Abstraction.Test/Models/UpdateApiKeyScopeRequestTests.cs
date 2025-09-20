using MasLazu.AspNet.ApiKey.Abstraction.Models;
using Xunit;

namespace MasLazu.AspNet.ApiKey.Abstraction.Test.Models;

public class UpdateApiKeyScopeRequestTests
{
    [Fact]
    public void UpdateApiKeyScopeRequest_CanBeInstantiated()
    {
        // Arrange
        var id = Guid.NewGuid();
        var apiKeyId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        // Act
        var request = new UpdateApiKeyScopeRequest(
            Id: id,
            ApiKeyId: apiKeyId,
            PermissionId: permissionId
        );

        // Assert
        Assert.NotNull(request);
        Assert.Equal(id, request.Id);
        Assert.Equal(apiKeyId, request.ApiKeyId);
        Assert.Equal(permissionId, request.PermissionId);
    }

    [Fact]
    public void UpdateApiKeyScopeRequest_WithNullValues_CanBeCreated()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var request = new UpdateApiKeyScopeRequest(
            Id: id,
            ApiKeyId: null,
            PermissionId: null
        );

        // Assert
        Assert.Equal(id, request.Id);
        Assert.Null(request.ApiKeyId);
        Assert.Null(request.PermissionId);
    }

    [Fact]
    public void UpdateApiKeyScopeRequest_InheritsFrom_BaseUpdateRequest()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var request = new UpdateApiKeyScopeRequest(
            Id: id,
            ApiKeyId: null,
            PermissionId: null
        );

        // Assert
        Assert.IsAssignableFrom<MasLazu.AspNet.Framework.Application.Models.BaseUpdateRequest>(request);
    }
}
