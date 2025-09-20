using FastEndpoints;
using MasLazu.AspNet.ApiKey.Abstraction.Interfaces;
using MasLazu.AspNet.ApiKey.Abstraction.Models;
using MasLazu.AspNet.ApiKey.Endpoint.EndpointGroups;
using MasLazu.AspNet.Framework.Application.Models;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using System.Collections.Generic;

namespace MasLazu.AspNet.ApiKey.Endpoint.Endpoints.ApiKeys;

public class GetUserApiKeysEndpoint : BaseEndpoint<IdRequest, IEnumerable<ApiKeyDto>>
{
    public IApiKeyService ApiKeyService { get; set; } = default!;

    public override void ConfigureEndpoint()
    {
        Get("/users/{id}/apikeys");
        Group<ApiKeysEndpointGroup>();
    }

    public override async Task HandleAsync(IdRequest request, CancellationToken ct)
    {
        IEnumerable<ApiKeyDto> apiKeys = await ApiKeyService.GetByUserIdAsync(request.Id, ct);
        await SendOkResponseAsync(apiKeys, "User API keys fetched", ct);
    }
}
