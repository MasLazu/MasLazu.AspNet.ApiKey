using MasLazu.AspNet.ApiKey.Utils;
using System.Linq.Expressions;
using Xunit;

namespace MasLazu.AspNet.ApiKey.Test.Utils;

public class ApiKeyEntityPropertyMapTests
{
    [Fact]
    public void ApiKeyEntityPropertyMap_CanBeInstantiated()
    {
        // Act
        var propertyMap = new ApiKeyEntityPropertyMap();

        // Assert
        Assert.NotNull(propertyMap);
        Assert.IsType<ApiKeyEntityPropertyMap>(propertyMap);
    }

    [Fact]
    public void Get_WithValidProperty_ReturnsExpression()
    {
        // Arrange
        var propertyMap = new ApiKeyEntityPropertyMap();

        // Act
        Expression<Func<Domain.Entities.ApiKey, object>> expression = propertyMap.Get("id");

        // Assert
        Assert.NotNull(expression);
        Assert.IsAssignableFrom<Expression<Func<Domain.Entities.ApiKey, object>>>(expression);
    }

    [Fact]
    public void Get_WithInvalidProperty_ThrowsArgumentException()
    {
        // Arrange
        var propertyMap = new ApiKeyEntityPropertyMap();

        // Act & Assert
        ArgumentException exception = Assert.Throws<ArgumentException>(() => propertyMap.Get("invalidProperty"));
        Assert.Contains("Property 'invalidProperty' is not supported", exception.Message);
    }

    [Theory]
    [InlineData("id")]
    [InlineData("userId")]
    [InlineData("key")]
    [InlineData("name")]
    [InlineData("expiresDate")]
    [InlineData("lastUsed")]
    [InlineData("revokedDate")]
    [InlineData("createdAt")]
    [InlineData("updatedAt")]
    public void Get_WithSupportedProperties_DoesNotThrow(string propertyName)
    {
        // Arrange
        var propertyMap = new ApiKeyEntityPropertyMap();

        // Act & Assert
        Exception exception = Record.Exception(() => propertyMap.Get(propertyName));
        Assert.Null(exception);
    }
}
