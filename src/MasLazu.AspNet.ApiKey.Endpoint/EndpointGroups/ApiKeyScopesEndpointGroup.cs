using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.EndpointGroups;
using Microsoft.AspNetCore.Http;

namespace MasLazu.AspNet.ApiKey.Endpoint.EndpointGroups;

public class ApiKeyScopesEndpointGroup : SubGroup<V1EndpointGroup>
{
    public ApiKeyScopesEndpointGroup()
    {
        Configure("apikey-scopes", ep => ep.Description(x => x.WithTags("ApiKey Scopes")));
    }
}
