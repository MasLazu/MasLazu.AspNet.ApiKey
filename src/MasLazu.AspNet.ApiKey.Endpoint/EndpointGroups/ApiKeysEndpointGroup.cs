using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.EndpointGroups;
using Microsoft.AspNetCore.Http;

namespace MasLazu.AspNet.ApiKey.Endpoint.EndpointGroups;

public class ApiKeysEndpointGroup : SubGroup<V1EndpointGroup>
{
    public ApiKeysEndpointGroup()
    {
        Configure("apikeys", ep => ep.Description(x => x.WithTags("ApiKeys")));
    }
}
