using FastEndpoints;
using MasLazu.AspNet.ApiKey.Abstraction.Interfaces;
using MasLazu.AspNet.ApiKey.Abstraction.Models;
using MasLazu.AspNet.ApiKey.Endpoint.EndpointGroups;
using MasLazu.AspNet.Framework.Application.Exceptions;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;

namespace MasLazu.AspNet.ApiKey.Endpoint.Endpoints.ApiKeyScopes;

public class UpdateApiKeyScopeEndpoint : BaseEndpoint<UpdateApiKeyScopeRequest, ApiKeyScopeDto>
{
    public IApiKeyScopeService ApiKeyScopeService { get; set; } = default!;

    public override void ConfigureEndpoint()
    {
        Put("{id}");
        Group<ApiKeyScopesEndpointGroup>();
    }

    public override async Task HandleAsync(UpdateApiKeyScopeRequest request, CancellationToken ct)
    {
        string userIdString = User.FindFirst("sub")?.Value ?? throw new UnauthorizedException();
        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            throw new UnauthorizedException();
        }

        ApiKeyScopeDto result = await ApiKeyScopeService.UpdateAsync(userId, request, ct);
        await SendOkResponseAsync(result, "Api key scope updated successfully", ct);
    }
}
