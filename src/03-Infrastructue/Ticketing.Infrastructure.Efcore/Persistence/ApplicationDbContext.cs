using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Entites;
using Ticketing.Infrastructure.Efcore.Persistence.Configurations;

namespace Ticketing.Infrastructure.Efcore.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options)
    {
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<TicketMessage> TicketMessages => Set<TicketMessage>();
        public DbSet<NormalUser> NormalUsers => Set<NormalUser>();
        public DbSet<Agent> Agents => Set<Agent>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole<Guid>>().HasData(SampleSeedData.Roles);
            builder.Entity<IdentityUserRole<Guid>>().HasData(SampleSeedData.UserRoles);

            builder.ApplyConfiguration(new TicketConfiguration());
            builder.ApplyConfiguration(new TicketMessageConfiguration());
            builder.ApplyConfiguration(new NormalUserConfiguration());
            builder.ApplyConfiguration(new AgentConfiguration());
            builder.ApplyConfiguration(new AppUserConfiguration());
            builder.ApplyConfiguration(new RefreshTokenConfiguration());
        }
    }
}
