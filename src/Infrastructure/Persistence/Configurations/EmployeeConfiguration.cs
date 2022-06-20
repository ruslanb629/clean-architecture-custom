using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : AuditableEntityConfiguration<Employee>
{
    public override void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(t => t.Id)
            .HasDefaultValueSql("newid()")
            .IsRequired();

        builder.Property(t => t.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.Surname)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.BirthDate)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(t => t.IsActive)
            .HasColumnType("bit")
            .HasDefaultValueSql("1")
            .IsRequired();

        base.Configure(builder);
    }
}
