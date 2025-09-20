using MasLazu.AspNet.ApiKey.Abstraction.Models;
using Xunit;

namespace MasLazu.AspNet.ApiKey.Abstraction.Test.Models;

public class UpdateApiKeyRequestTests
{
    [Fact]
    public void UpdateApiKeyRequest_CanBeInstantiated()
    {
        // Arrange
        var id = Guid.NewGuid();
        string name = "Updated API Key";
        DateTime expiresDate = DateTime.UtcNow.AddDays(60);
        DateTime revokedDate = DateTime.UtcNow.AddDays(90);

        // Act
        var request = new UpdateApiKeyRequest(
            Id: id,
            Name: name,
            ExpiresDate: expiresDate,
            RevokedDate: revokedDate
        );

        // Assert
        Assert.NotNull(request);
        Assert.Equal(id, request.Id);
        Assert.Equal(name, request.Name);
        Assert.Equal(expiresDate, request.ExpiresDate);
        Assert.Equal(revokedDate, request.RevokedDate);
    }

    [Fact]
    public void UpdateApiKeyRequest_WithNullValues_CanBeCreated()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var request = new UpdateApiKeyRequest(
            Id: id,
            Name: null,
            ExpiresDate: null,
            RevokedDate: null
        );

        // Assert
        Assert.Equal(id, request.Id);
        Assert.Null(request.Name);
        Assert.Null(request.ExpiresDate);
        Assert.Null(request.RevokedDate);
    }

    [Fact]
    public void UpdateApiKeyRequest_InheritsFrom_BaseUpdateRequest()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var request = new UpdateApiKeyRequest(
            Id: id,
            Name: null,
            ExpiresDate: null,
            RevokedDate: null
        );

        // Assert
        Assert.IsAssignableFrom<MasLazu.AspNet.Framework.Application.Models.BaseUpdateRequest>(request);
    }
}
