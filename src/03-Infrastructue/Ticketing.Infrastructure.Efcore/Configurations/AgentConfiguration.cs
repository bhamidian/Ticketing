using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketing.Domain.Entites;
using Ticketing.Infrastructure.Efcore.Persistence;

namespace Ticketing.Infrastructure.Efcore.Persistence.Configurations;

public sealed class AgentConfiguration : IEntityTypeConfiguration<Agent>
{
    public void Configure(EntityTypeBuilder<Agent> builder)
    {
        builder.HasData(SampleSeedData.Agents);

        builder.HasKey(agent => agent.Id);

        builder
            .HasOne(agent => agent.AppUser)
            .WithOne(appUser => appUser.Agent)
            .HasForeignKey<Agent>(agent => agent.AppUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(agent => agent.AppUserId).IsUnique();

        builder.Property(agent => agent.CreatedAt).IsRequired();
        builder.Property(agent => agent.UpdatedAt).IsRequired();
        builder.Property(agent => agent.CreatedById).IsRequired();
        builder.Property(agent => agent.UpdatedById).IsRequired();
    }
}
