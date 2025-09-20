using System;
using MasLazu.AspNet.Framework.Domain.Entities;

namespace MasLazu.AspNet.ApiKey.Domain.Entities;

public class ApiKey : BaseEntity
{
    public Guid UserId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string? Name { get; set; }
    public DateTime? ExpiresDate { get; set; }
    public DateTime? LastUsed { get; set; }
    public DateTime? RevokedDate { get; set; }

    public ICollection<ApiKeyScope> Scopes { get; set; } = [];
}
