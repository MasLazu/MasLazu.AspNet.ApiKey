using FluentValidation;
using Mapster;
using MasLazu.AspNet.ApiKey.Abstraction.Interfaces;
using MasLazu.AspNet.ApiKey.Abstraction.Models;
using MasLazu.AspNet.ApiKey.Domain.Entities;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.Framework.Application.Services;

namespace MasLazu.AspNet.ApiKey.Services;

public class ApiKeyScopeService : CrudService<ApiKeyScope, ApiKeyScopeDto, CreateApiKeyScopeRequest, UpdateApiKeyScopeRequest>, IApiKeyScopeService
{
    public ApiKeyScopeService(
        IRepository<ApiKeyScope> repository,
        IReadRepository<ApiKeyScope> readRepository,
        IUnitOfWork unitOfWork,
        IEntityPropertyMap<ApiKeyScope> propertyMap,
        IPaginationValidator<ApiKeyScope> paginationValidator,
        ICursorPaginationValidator<ApiKeyScope> cursorPaginationValidator,
        IValidator<CreateApiKeyScopeRequest>? createValidator = null,
        IValidator<UpdateApiKeyScopeRequest>? updateValidator = null)
        : base(repository, readRepository, unitOfWork, propertyMap, paginationValidator, cursorPaginationValidator, createValidator, updateValidator)
    {
    }

    public async Task<ApiKeyScopeDto> AddScopeAsync(Guid userId, ApiKeyScopeRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new ApiKeyScope
        {
            ApiKeyId = request.ApiKeyId,
            PermissionId = request.PermissionId
        };
        await Repository.AddAsync(entity, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return entity.Adapt<ApiKeyScopeDto>();
    }

    public async Task RemoveScopeAsync(Guid userId, ApiKeyScopeRequest request, CancellationToken cancellationToken = default)
    {
        ApiKeyScope? scope = await ReadRepository.FirstOrDefaultAsync(x => x.ApiKeyId == request.ApiKeyId && x.PermissionId == request.PermissionId, cancellationToken);
        if (scope != null)
        {
            await Repository.DeleteAsync(scope, cancellationToken);
            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
