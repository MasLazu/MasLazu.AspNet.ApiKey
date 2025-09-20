using MasLazu.AspNet.ApiKey.Endpoint.EndpointGroups;
using MasLazu.AspNet.Framework.Endpoint.EndpointGroups;
using Xunit;

namespace MasLazu.AspNet.ApiKey.Endpoint.Test.EndpointGroups;

public class ApiKeyScopesEndpointGroupTests
{
    [Fact]
    public void ApiKeyScopesEndpointGroup_CanBeInstantiated()
    {
        // Act
        var endpointGroup = new ApiKeyScopesEndpointGroup();

        // Assert
        Assert.NotNull(endpointGroup);
        Assert.IsType<ApiKeyScopesEndpointGroup>(endpointGroup);
    }

    [Fact]
    public void ApiKeyScopesEndpointGroup_InheritsFrom_SubGroup()
    {
        // Act
        var endpointGroup = new ApiKeyScopesEndpointGroup();

        // Assert
        Assert.IsAssignableFrom<FastEndpoints.SubGroup<V1EndpointGroup>>(endpointGroup);
    }
}
