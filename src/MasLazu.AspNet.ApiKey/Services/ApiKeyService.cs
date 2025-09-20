using FluentValidation;
using Mapster;
using MasLazu.AspNet.ApiKey.Abstraction.Interfaces;
using MasLazu.AspNet.ApiKey.Abstraction.Models;
using MasLazu.AspNet.ApiKey.Domain.Entities;
using MasLazu.AspNet.Framework.Application.Exceptions;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.Framework.Application.Services;
using System.Linq;

namespace MasLazu.AspNet.ApiKey.Services;

public class ApiKeyService : CrudService<Domain.Entities.ApiKey, ApiKeyDto, CreateApiKeyRequest, UpdateApiKeyRequest>, IApiKeyService
{
    public ApiKeyService(
        IRepository<Domain.Entities.ApiKey> repository,
        IReadRepository<Domain.Entities.ApiKey> readRepository,
        IUnitOfWork unitOfWork,
        IEntityPropertyMap<Domain.Entities.ApiKey> propertyMap,
        IPaginationValidator<Domain.Entities.ApiKey> paginationValidator,
        ICursorPaginationValidator<Domain.Entities.ApiKey> cursorPaginationValidator,
        IValidator<CreateApiKeyRequest>? createValidator = null,
        IValidator<UpdateApiKeyRequest>? updateValidator = null)
        : base(repository, readRepository, unitOfWork, propertyMap, paginationValidator, cursorPaginationValidator, createValidator, updateValidator)
    {
    }

    public async Task RevokeAsync(Guid userId, Guid apiKeyId, CancellationToken ct = default)
    {
        Domain.Entities.ApiKey? apiKey = await Repository.GetByIdAsync(apiKeyId, ct) ??
            throw new NotFoundException("API key not found.");

        apiKey.RevokedDate = DateTime.UtcNow;
        await Repository.UpdateAsync(apiKey, ct);
        await UnitOfWork.SaveChangesAsync(ct);
    }

    public async Task<ApiKeyDto> RotateAsync(Guid userId, Guid apiKeyId, CancellationToken ct = default)
    {
        Domain.Entities.ApiKey? apiKey = await Repository.GetByIdAsync(apiKeyId, ct) ??
            throw new NotFoundException("API key not found.");

        apiKey.Key = Guid.NewGuid().ToString("N");
        await Repository.UpdateAsync(apiKey, ct);
        await UnitOfWork.SaveChangesAsync(ct);
        return apiKey.Adapt<ApiKeyDto>();
    }

    public async Task RevokeAllForUserAsync(Guid userId, CancellationToken ct = default)
    {
        IEnumerable<Domain.Entities.ApiKey> apiKeys = await ReadRepository.FindAsync(x => x.UserId == userId, ct);
        foreach (Domain.Entities.ApiKey apiKey in apiKeys)
        {
            apiKey.RevokedDate = DateTime.UtcNow;
            await Repository.UpdateAsync(apiKey, ct);
        }
        await UnitOfWork.SaveChangesAsync(ct);
    }

    public async Task UpdateLastUsedAsync(Guid userId, Guid apiKeyId, CancellationToken ct = default)
    {
        Domain.Entities.ApiKey? apiKey = await Repository.GetByIdAsync(apiKeyId, ct) ??
            throw new NotFoundException("API key not found.");
        apiKey.LastUsed = DateTime.UtcNow;
        await Repository.UpdateAsync(apiKey, ct);
        await UnitOfWork.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<ApiKeyDto>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        IEnumerable<Domain.Entities.ApiKey> apiKeys = await ReadRepository.FindAsync(x => x.UserId == userId, ct);
        return apiKeys.Select(x => x.Adapt<ApiKeyDto>());
    }

    public async Task<bool> ValidateAsync(ValidateApiKeyRequest request, CancellationToken ct = default)
    {
        return await ReadRepository.ExistsAsync(x =>
            x.Key == request.Key &&
            x.RevokedDate == null &&
            (x.ExpiresDate == null || x.ExpiresDate > DateTime.UtcNow) &&
            x.Scopes.Any(s => s.PermissionId == request.PermissionId), ct);
    }

}
