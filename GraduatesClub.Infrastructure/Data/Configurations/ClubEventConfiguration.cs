using GraduatesClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduatesClub.Infrastructure.Data.Configurations;

public sealed class ClubEventConfiguration : IEntityTypeConfiguration<ClubEvent>
{
    public void Configure(EntityTypeBuilder<ClubEvent> builder)
    {
        builder.ToTable("Events");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(2000)
            .IsRequired();

        builder.HasOne(x => x.Alumni)
            .WithMany(x => x.Events)
            .HasForeignKey(x => x.AlumniId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
