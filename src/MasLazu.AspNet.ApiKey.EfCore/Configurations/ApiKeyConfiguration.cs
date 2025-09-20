using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MasLazu.AspNet.ApiKey.Domain.Entities;

namespace MasLazu.AspNet.ApiKey.EfCore.Configurations;

public class ApiKeyConfiguration : IEntityTypeConfiguration<Domain.Entities.ApiKey>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.ApiKey> builder)
    {
        builder.HasKey(ak => ak.Id);

        builder.Property(ak => ak.UserId)
            .IsRequired();

        builder.Property(ak => ak.Key)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(ak => ak.Name)
            .HasMaxLength(100);

        builder.HasMany(ak => ak.Scopes)
            .WithOne(aks => aks.ApiKey)
            .HasForeignKey(aks => aks.ApiKeyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ak => ak.Key)
            .IsUnique();

        builder.HasIndex(ak => ak.UserId);
    }
}
