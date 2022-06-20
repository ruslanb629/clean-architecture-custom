using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public abstract class AuditableEntityConfiguration<TBase> : IEntityTypeConfiguration<TBase>
    where TBase : AuditableEntity
{
    public virtual void Configure(EntityTypeBuilder<TBase> builder)
    {
        builder.Property(t => t.Created)
            .HasColumnType("datetime")
            .HasDefaultValueSql("getdate()")
            .IsRequired();

        builder.Property(t => t.LastModified)
            .HasColumnType("datetime");
    }
}