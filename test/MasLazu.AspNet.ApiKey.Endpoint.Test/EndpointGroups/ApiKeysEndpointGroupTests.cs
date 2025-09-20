using MasLazu.AspNet.ApiKey.Endpoint.EndpointGroups;
using MasLazu.AspNet.Framework.Endpoint.EndpointGroups;
using Xunit;

namespace MasLazu.AspNet.ApiKey.Endpoint.Test.EndpointGroups;

public class ApiKeysEndpointGroupTests
{
    [Fact]
    public void ApiKeysEndpointGroup_CanBeInstantiated()
    {
        // Act
        var endpointGroup = new ApiKeysEndpointGroup();

        // Assert
        Assert.NotNull(endpointGroup);
        Assert.IsType<ApiKeysEndpointGroup>(endpointGroup);
    }

    [Fact]
    public void ApiKeysEndpointGroup_InheritsFrom_SubGroup()
    {
        // Act
        var endpointGroup = new ApiKeysEndpointGroup();

        // Assert
        Assert.IsAssignableFrom<FastEndpoints.SubGroup<V1EndpointGroup>>(endpointGroup);
    }
}
