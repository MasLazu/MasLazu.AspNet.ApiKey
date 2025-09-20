using FastEndpoints;
using MasLazu.AspNet.ApiKey.Abstraction.Interfaces;
using MasLazu.AspNet.ApiKey.Abstraction.Models;
using MasLazu.AspNet.ApiKey.Endpoint.EndpointGroups;
using MasLazu.AspNet.Framework.Application.Exceptions;
using MasLazu.AspNet.Framework.Application.Models;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;

namespace MasLazu.AspNet.ApiKey.Endpoint.Endpoints.ApiKeys;

public class UpdateApiKeyEndpoint : BaseEndpoint<UpdateApiKeyRequest, ApiKeyDto>
{
    public IApiKeyService ApiKeyService { get; set; } = default!;

    public override void ConfigureEndpoint()
    {
        Put("/{id}");
        Group<ApiKeysEndpointGroup>();
    }

    public override async Task HandleAsync(UpdateApiKeyRequest request, CancellationToken ct)
    {
        string userIdString = User.FindFirst("sub")?.Value ?? throw new UnauthorizedException();
        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            throw new UnauthorizedException();
        }

        ApiKeyDto result = await ApiKeyService.UpdateAsync(userId, request, ct);
        await SendOkResponseAsync(result, "Api key updated successfully", ct);
    }
}
