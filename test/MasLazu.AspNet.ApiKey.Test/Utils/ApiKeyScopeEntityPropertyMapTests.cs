using MasLazu.AspNet.ApiKey.Domain.Entities;
using MasLazu.AspNet.ApiKey.Utils;
using System.Linq.Expressions;
using Xunit;

namespace MasLazu.AspNet.ApiKey.Test.Utils;

public class ApiKeyScopeEntityPropertyMapTests
{
    [Fact]
    public void ApiKeyScopeEntityPropertyMap_CanBeInstantiated()
    {
        // Act
        var propertyMap = new ApiKeyScopeEntityPropertyMap();

        // Assert
        Assert.NotNull(propertyMap);
        Assert.IsType<ApiKeyScopeEntityPropertyMap>(propertyMap);
    }

    [Fact]
    public void Get_WithValidProperty_ReturnsExpression()
    {
        // Arrange
        var propertyMap = new ApiKeyScopeEntityPropertyMap();

        // Act
        Expression<Func<ApiKeyScope, object>> expression = propertyMap.Get("id");

        // Assert
        Assert.NotNull(expression);
        Assert.IsAssignableFrom<Expression<Func<ApiKeyScope, object>>>(expression);
    }

    [Fact]
    public void Get_WithInvalidProperty_ThrowsArgumentException()
    {
        // Arrange
        var propertyMap = new ApiKeyScopeEntityPropertyMap();

        // Act & Assert
        ArgumentException exception = Assert.Throws<ArgumentException>(() => propertyMap.Get("invalidProperty"));
        Assert.Contains("Property 'invalidProperty' is not supported", exception.Message);
    }

    [Theory]
    [InlineData("id")]
    [InlineData("apiKeyId")]
    [InlineData("permissionId")]
    [InlineData("createdAt")]
    [InlineData("updatedAt")]
    public void Get_WithSupportedProperties_DoesNotThrow(string propertyName)
    {
        // Arrange
        var propertyMap = new ApiKeyScopeEntityPropertyMap();

        // Act & Assert
        Exception exception = Record.Exception(() => propertyMap.Get(propertyName));
        Assert.Null(exception);
    }
}
