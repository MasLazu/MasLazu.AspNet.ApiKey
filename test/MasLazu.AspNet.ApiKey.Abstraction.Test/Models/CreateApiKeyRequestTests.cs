using MasLazu.AspNet.ApiKey.Abstraction.Models;
using Xunit;

namespace MasLazu.AspNet.ApiKey.Abstraction.Test.Models;

public class CreateApiKeyRequestTests
{
    [Fact]
    public void CreateApiKeyRequest_CanBeInstantiated()
    {
        // Arrange
        var userId = Guid.NewGuid();
        string name = "Test API Key";
        DateTime expiresDate = DateTime.UtcNow.AddDays(30);

        // Act
        var request = new CreateApiKeyRequest(
            UserId: userId,
            Name: name,
            ExpiresDate: expiresDate
        );

        // Assert
        Assert.NotNull(request);
        Assert.Equal(userId, request.UserId);
        Assert.Equal(name, request.Name);
        Assert.Equal(expiresDate, request.ExpiresDate);
    }

    [Fact]
    public void CreateApiKeyRequest_WithNullValues_CanBeCreated()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var request = new CreateApiKeyRequest(
            UserId: userId,
            Name: null,
            ExpiresDate: null
        );

        // Assert
        Assert.Equal(userId, request.UserId);
        Assert.Null(request.Name);
        Assert.Null(request.ExpiresDate);
    }
}
