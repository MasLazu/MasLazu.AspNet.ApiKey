using Microsoft.EntityFrameworkCore;

using MasLazu.AspNet.Framework.EfCore.Data;
using MasLazu.AspNet.ApiKey.Domain.Entities;

namespace MasLazu.AspNet.ApiKey.EfCore.Data;

public class ApiKeyDbContext : BaseDbContext
{
    public ApiKeyDbContext(DbContextOptions<ApiKeyDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.ApiKey> ApiKeys { get; set; }
    public DbSet<ApiKeyScope> ApiKeyScopes { get; set; }
}
