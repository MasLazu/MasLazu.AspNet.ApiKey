using FastEndpoints;
using MasLazu.AspNet.ApiKey.Abstraction.Interfaces;
using MasLazu.AspNet.ApiKey.Abstraction.Models;
using MasLazu.AspNet.ApiKey.Endpoint.EndpointGroups;
using MasLazu.AspNet.Framework.Application.Exceptions;
using MasLazu.AspNet.Framework.Application.Models;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;

namespace MasLazu.AspNet.ApiKey.Endpoint.Endpoints.ApiKeys;

public class RevokeApiKeyEndpoint : BaseEndpointWithoutResponse<IdRequest>
{
    public IApiKeyService ApiKeyService { get; set; } = default!;

    public override void ConfigureEndpoint()
    {
        Post("/{id}/revoke");
        Group<ApiKeysEndpointGroup>();
    }


    public override async Task HandleAsync(IdRequest request, CancellationToken ct)
    {
        string userIdString = User.FindFirst("sub")?.Value ?? throw new UnauthorizedException();
        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            throw new UnauthorizedException();
        }

        await ApiKeyService.RevokeAsync(userId, request.Id, ct);
        await SendOkResponseAsync("Api key revoked successfully", ct);
    }
}
