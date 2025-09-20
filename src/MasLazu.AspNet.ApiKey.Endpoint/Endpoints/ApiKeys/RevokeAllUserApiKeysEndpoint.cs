using FastEndpoints;
using MasLazu.AspNet.ApiKey.Abstraction.Interfaces;
using MasLazu.AspNet.ApiKey.Endpoint.EndpointGroups;
using MasLazu.AspNet.Framework.Application.Models;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;

namespace MasLazu.AspNet.ApiKey.Endpoint.Endpoints.ApiKeys;

public class RevokeAllUserApiKeysEndpoint : BaseEndpointWithoutResponse<IdRequest>
{
    public IApiKeyService ApiKeyService { get; set; } = default!;

    public override void ConfigureEndpoint()
    {
        Post("/users/{id}/apikeys/revoke");
        Group<ApiKeysEndpointGroup>();
    }

    public override async Task HandleAsync(IdRequest request, CancellationToken ct)
    {
        await ApiKeyService.RevokeAllForUserAsync(request.Id, ct);
        await SendOkResponseAsync("All user API keys revoked", ct);
    }
}
