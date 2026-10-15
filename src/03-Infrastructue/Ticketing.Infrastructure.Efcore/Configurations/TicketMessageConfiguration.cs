using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketing.Domain.Entites;
using Ticketing.Infrastructure.Efcore.Persistence;

namespace Ticketing.Infrastructure.Efcore.Persistence.Configurations;

public sealed class TicketMessageConfiguration : IEntityTypeConfiguration<TicketMessage>
{
    public void Configure(EntityTypeBuilder<TicketMessage> builder)
    {
        builder.HasData(SampleSeedData.TicketMessages);

        builder.HasKey(message => message.Id);

        builder.Property(message => message.Message)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(message => message.PictureUrl)
            .HasMaxLength(2048);

        builder.HasOne(message => message.Ticket)
            .WithMany(ticket => ticket.TicketMessages)
            .HasForeignKey(message => message.TicketId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(message => new { message.TicketId, message.CreatedAt });

        builder.Property(message => message.CreatedAt).IsRequired();
        builder.Property(message => message.UpdatedAt).IsRequired();
        builder.Property(message => message.CreatedById).IsRequired();
        builder.Property(message => message.UpdatedById).IsRequired();
    }
}
