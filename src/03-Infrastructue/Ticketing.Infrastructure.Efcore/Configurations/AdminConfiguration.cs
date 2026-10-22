using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketing.Domain.Entites;

namespace Ticketing.Infrastructure.Efcore.Configurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasOne(admin => admin.AppUser)
                .WithOne(appUser => appUser.Admin)
                .HasForeignKey<Admin>(admin => admin.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
