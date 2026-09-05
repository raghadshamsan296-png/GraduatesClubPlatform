using GraduatesClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduatesClub.Infrastructure.Data.Configurations;

public sealed class AlumniConfiguration : IEntityTypeConfiguration<Alumni>
{
    public void Configure(EntityTypeBuilder<Alumni> builder)
    {
        builder.ToTable("Alumni");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Phone)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.PhotoPath)
            .HasMaxLength(500);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.HasOne(x => x.Department)
            .WithMany(x => x.Alumni)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
