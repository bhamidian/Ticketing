using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketing.Domain.Entites;
using Ticketing.Infrastructure.Efcore.Persistence;

namespace Ticketing.Infrastructure.Efcore.Persistence.Configurations;

public sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasData(SampleSeedData.AppUsers);

        builder.HasMany(user => user.SentTicketMessages)
            .WithOne(message => message.Sender)
            .HasForeignKey(message => message.SenderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
