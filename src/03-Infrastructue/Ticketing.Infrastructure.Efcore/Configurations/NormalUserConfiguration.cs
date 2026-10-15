using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketing.Domain.Entites;
using Ticketing.Infrastructure.Efcore.Persistence;

namespace Ticketing.Infrastructure.Efcore.Persistence.Configurations;

public sealed class NormalUserConfiguration : IEntityTypeConfiguration<NormalUser>
{
    public void Configure(EntityTypeBuilder<NormalUser> builder)
    {
        builder.HasData(SampleSeedData.NormalUsers);

        builder.HasKey(user => user.Id);

        builder.HasOne(user => user.AppUser)
            .WithOne(appUser => appUser.NormalUser)
            .HasForeignKey<NormalUser>(user => user.AppUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(user => user.AppUserId).IsUnique();

        builder.Property(user => user.CreatedAt).IsRequired();
        builder.Property(user => user.UpdatedAt).IsRequired();
        builder.Property(user => user.CreatedById).IsRequired();
        builder.Property(user => user.UpdatedById).IsRequired();
    }
}
