using FastEndpoints;
using MasLazu.AspNet.ApiKey.Abstraction.Interfaces;
using MasLazu.AspNet.ApiKey.Abstraction.Models;
using MasLazu.AspNet.ApiKey.Endpoint.EndpointGroups;
using MasLazu.AspNet.Framework.Application.Exceptions;
using MasLazu.AspNet.Framework.Application.Models;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;

namespace MasLazu.AspNet.ApiKey.Endpoint.Endpoints.ApiKeyScopes;

public class GetApiKeyScopeByIdEndpoint : BaseEndpoint<IdRequest, ApiKeyScopeDto>
{
    public IApiKeyScopeService ApiKeyScopeService { get; set; } = default!;

    public override void ConfigureEndpoint()
    {
        Get("/{id}");
        Group<ApiKeyScopesEndpointGroup>();
    }

    public override async Task HandleAsync(IdRequest request, CancellationToken ct)
    {
        string userIdString = User.FindFirst("sub")?.Value ?? throw new UnauthorizedException();
        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            throw new UnauthorizedException();
        }

        ApiKeyScopeDto? result = await ApiKeyScopeService.GetByIdAsync(userId, request.Id, ct) ??
            throw new NotFoundException("Api key scope not found");

        await SendOkResponseAsync(result, "Api key scope retrieved successfully", ct);
    }
}
