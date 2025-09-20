using FastEndpoints;
using MasLazu.AspNet.ApiKey.Abstraction.Interfaces;
using MasLazu.AspNet.ApiKey.Abstraction.Models;
using MasLazu.AspNet.ApiKey.Endpoint.EndpointGroups;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;

namespace MasLazu.AspNet.ApiKey.Endpoint.Endpoints.ApiKeys;

public class ValidateApiKeyEndpoint : BaseEndpoint<ValidateApiKeyRequest, bool>
{
    public IApiKeyService ApiKeyService { get; set; } = default!;

    public override void ConfigureEndpoint()
    {
        Post("/validate");
        Group<ApiKeysEndpointGroup>();
    }

    public override async Task HandleAsync(ValidateApiKeyRequest request, CancellationToken ct)
    {
        bool result = await ApiKeyService.ValidateAsync(request, ct);
        await SendOkResponseAsync(result, "Api key validation result", ct);
    }
}
