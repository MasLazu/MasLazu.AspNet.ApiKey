using System;
using MasLazu.AspNet.Framework.Domain.Entities;

namespace MasLazu.AspNet.ApiKey.Domain.Entities;

public class ApiKeyScope : BaseEntity
{
    public Guid ApiKeyId { get; set; }
    public Guid PermissionId { get; set; }

    public ApiKey? ApiKey { get; set; }
}
