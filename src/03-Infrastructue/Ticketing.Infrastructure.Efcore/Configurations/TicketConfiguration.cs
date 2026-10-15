using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketing.Domain.Entites;
using Ticketing.Infrastructure.Efcore.Persistence;

namespace Ticketing.Infrastructure.Efcore.Persistence.Configurations;

public sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasData(SampleSeedData.Tickets);

        builder.HasKey(ticket => ticket.Id);

        builder.Property(ticket => ticket.Subject)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ticket => ticket.TicketLifeCycle)
            .HasConversion<string>()
            .HasMaxLength(32);

        builder.Property(ticket => ticket.TicketPriority)
            .HasConversion<string>()
            .HasMaxLength(32);

        builder.HasOne(ticket => ticket.NormalUser)
            .WithMany(user => user.Tickets)
            .HasForeignKey(ticket => ticket.NormalUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(ticket => ticket.ClosedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ticket => ticket.AssignedAgent)
            .WithMany(agent => agent.AssignedTickets)
            .HasForeignKey(ticket => ticket.AssignedAgentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(ticket => new { ticket.NormalUserId, ticket.CreatedAt });
        builder.HasIndex(ticket => new { ticket.TicketLifeCycle, ticket.TicketPriority });
        builder.HasIndex(ticket => new { ticket.AssignedAgentId, ticket.TicketLifeCycle });

        ConfigureAuditFields(builder);
    }

    private static void ConfigureAuditFields(EntityTypeBuilder<Ticket> builder)
    {
        builder.Property(ticket => ticket.CreatedAt).IsRequired();
        builder.Property(ticket => ticket.UpdatedAt).IsRequired();
        builder.Property(ticket => ticket.CreatedById).IsRequired();
        builder.Property(ticket => ticket.UpdatedById).IsRequired();
    }
}
