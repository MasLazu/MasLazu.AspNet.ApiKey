using FastEndpoints;
using MasLazu.AspNet.ApiKey.Abstraction.Interfaces;
using MasLazu.AspNet.ApiKey.Abstraction.Models;
using MasLazu.AspNet.ApiKey.Endpoint.EndpointGroups;
using MasLazu.AspNet.Framework.Application.Exceptions;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;

namespace MasLazu.AspNet.ApiKey.Endpoint.Endpoints.ApiKeyScopes;

public class AddApiKeyScopeEndpoint : BaseEndpoint<ApiKeyScopeRequest, ApiKeyScopeDto>
{
    public IApiKeyScopeService ApiKeyScopeService { get; set; } = default!;

    public override void ConfigureEndpoint()
    {
        Post("");
        Group<ApiKeyScopesEndpointGroup>();
    }

    public override async Task HandleAsync(ApiKeyScopeRequest request, CancellationToken ct)
    {
        string userIdString = User.FindFirst("sub")?.Value ?? throw new UnauthorizedException();
        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            throw new UnauthorizedException();
        }

        ApiKeyScopeDto result = await ApiKeyScopeService.AddScopeAsync(userId, request, ct);
        await SendOkResponseAsync(result, "Scope added to API key", ct);
    }
}
