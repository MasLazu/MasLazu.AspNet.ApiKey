using FastEndpoints;
using MasLazu.AspNet.ApiKey.Abstraction.Interfaces;
using MasLazu.AspNet.ApiKey.Abstraction.Models;
using MasLazu.AspNet.ApiKey.Endpoint.EndpointGroups;
using MasLazu.AspNet.Framework.Application.Exceptions;
using MasLazu.AspNet.Framework.Application.Models;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;

namespace MasLazu.AspNet.ApiKey.Endpoint.Endpoints.ApiKeys;

public class GetApiKeyByIdEndpoint : BaseEndpoint<IdRequest, ApiKeyDto>
{
    public IApiKeyService ApiKeyService { get; set; } = default!;

    public override void ConfigureEndpoint()
    {
        Get("/{id}");
        Group<ApiKeysEndpointGroup>();
    }

    public override async Task HandleAsync(IdRequest request, CancellationToken ct)
    {
        string userIdString = User.FindFirst("sub")?.Value ?? throw new UnauthorizedException();
        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            throw new UnauthorizedException();
        }

        ApiKeyDto? result = await ApiKeyService.GetByIdAsync(userId, request.Id, ct) ??
            throw new NotFoundException("Api key not found");
        await SendOkResponseAsync(result, "Api key retrieved successfully", ct);
    }
}
