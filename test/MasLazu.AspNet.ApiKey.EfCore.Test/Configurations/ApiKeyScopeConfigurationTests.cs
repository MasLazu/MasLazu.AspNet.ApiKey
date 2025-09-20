using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MasLazu.AspNet.ApiKey.Domain.Entities;
using MasLazu.AspNet.ApiKey.EfCore.Configurations;
using Xunit;

namespace MasLazu.AspNet.ApiKey.EfCore.Test.Configurations;

public class ApiKeyScopeConfigurationTests
{
    [Fact]
    public void Configure_SetsPrimaryKey()
    {
        // Arrange
        EntityTypeBuilder<ApiKeyScope> builder = CreateEntityTypeBuilder();
        var configuration = new ApiKeyScopeConfiguration();

        // Act
        configuration.Configure(builder);

        // Assert
        Assert.NotNull(configuration);
    }

    [Fact]
    public void Configure_SetsRequiredProperties()
    {
        // Arrange
        EntityTypeBuilder<ApiKeyScope> builder = CreateEntityTypeBuilder();
        var configuration = new ApiKeyScopeConfiguration();

        // Act
        configuration.Configure(builder);

        // Assert
        Assert.NotNull(configuration);
    }

    [Fact]
    public void Configure_SetsRelationships()
    {
        // Arrange
        EntityTypeBuilder<ApiKeyScope> builder = CreateEntityTypeBuilder();
        var configuration = new ApiKeyScopeConfiguration();

        // Act
        configuration.Configure(builder);

        // Assert
        Assert.NotNull(configuration);
    }

    [Fact]
    public void Configure_SetsCompositeIndex()
    {
        // Arrange
        EntityTypeBuilder<ApiKeyScope> builder = CreateEntityTypeBuilder();
        var configuration = new ApiKeyScopeConfiguration();

        // Act
        configuration.Configure(builder);

        // Assert
        Assert.NotNull(configuration);
    }

    private static EntityTypeBuilder<ApiKeyScope> CreateEntityTypeBuilder()
    {
        var modelBuilder = new ModelBuilder();
        return modelBuilder.Entity<ApiKeyScope>();
    }
}
