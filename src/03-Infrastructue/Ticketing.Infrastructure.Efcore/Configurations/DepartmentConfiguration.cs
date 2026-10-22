using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketing.Domain.Entites;
using Ticketing.Infrastructure.Efcore.Persistence;

namespace Ticketing.Infrastructure.Efcore.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasData(SampleSeedData.Departments);
            builder.Property(department => department.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(department => department.NormalUsers)
                .WithOne(normalUser => normalUser.Department)
                .HasForeignKey(normalUser => normalUser.DepartmentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
