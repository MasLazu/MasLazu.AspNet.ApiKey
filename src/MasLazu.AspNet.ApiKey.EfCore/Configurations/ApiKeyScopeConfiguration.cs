using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MasLazu.AspNet.ApiKey.Domain.Entities;

namespace MasLazu.AspNet.ApiKey.EfCore.Configurations;

public class ApiKeyScopeConfiguration : IEntityTypeConfiguration<ApiKeyScope>
{
    public void Configure(EntityTypeBuilder<ApiKeyScope> builder)
    {
        builder.HasKey(aks => aks.Id);

        builder.Property(aks => aks.ApiKeyId)
            .IsRequired();

        builder.Property(aks => aks.PermissionId)
            .IsRequired();

        builder.HasOne(aks => aks.ApiKey)
            .WithMany(ak => ak.Scopes)
            .HasForeignKey(aks => aks.ApiKeyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(aks => new { aks.ApiKeyId, aks.PermissionId })
            .IsUnique();
    }
}
