using MasLazu.AspNet.Framework.Application.Models;

namespace MasLazu.AspNet.ApiKey.Abstraction.Models;

public record ApiKeyDto(
    Guid Id,
    Guid UserId,
    string Key,
    string? Name,
    DateTimeOffset? ExpiresDate,
    DateTimeOffset? LastUsed,
    DateTimeOffset? RevokedDate,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
) : BaseDto(Id, CreatedAt, UpdatedAt);
