using MasLazu.AspNet.ApiKey.Abstraction.Models;
using Xunit;

namespace MasLazu.AspNet.ApiKey.Abstraction.Test.Models;

public class ApiKeyScopeDtoTests
{
    [Fact]
    public void ApiKeyScopeDto_CanBeInstantiated()
    {
        // Arrange
        var id = Guid.NewGuid();
        var apiKeyId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();
        DateTimeOffset createdAt = DateTimeOffset.UtcNow;

        // Act
        var dto = new ApiKeyScopeDto(
            Id: id,
            ApiKeyId: apiKeyId,
            PermissionId: permissionId,
            CreatedAt: createdAt,
            UpdatedAt: null
        );

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(id, dto.Id);
        Assert.Equal(apiKeyId, dto.ApiKeyId);
        Assert.Equal(permissionId, dto.PermissionId);
        Assert.Equal(createdAt, dto.CreatedAt);
    }

    [Fact]
    public void ApiKeyScopeDto_WithUpdatedAt_CanBeCreated()
    {
        // Arrange
        var id = Guid.NewGuid();
        var apiKeyId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();
        DateTimeOffset createdAt = DateTimeOffset.UtcNow;
        DateTimeOffset updatedAt = DateTimeOffset.UtcNow.AddHours(1);

        // Act
        var dto = new ApiKeyScopeDto(
            Id: id,
            ApiKeyId: apiKeyId,
            PermissionId: permissionId,
            CreatedAt: createdAt,
            UpdatedAt: updatedAt
        );

        // Assert
        Assert.Equal(id, dto.Id);
        Assert.Equal(apiKeyId, dto.ApiKeyId);
        Assert.Equal(permissionId, dto.PermissionId);
        Assert.Equal(createdAt, dto.CreatedAt);
        Assert.Equal(updatedAt, dto.UpdatedAt);
    }
}
