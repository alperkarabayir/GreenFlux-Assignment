using GreenFluxAssignment.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Data.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("Group");

            builder.HasKey(g => g.Id).HasName("PK_Group_Id");

            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");

            builder.Property(g => g.Id).ValueGeneratedNever();

            builder.Property(e => e.Name).IsRequired();

            builder.Property(g => g.Capacity).IsRequired().HasColumnType("decimal(9,2)");

            builder.HasMany(g => g.ChargeStations)
                .WithOne()
                .HasForeignKey(cs => cs.GroupId)
                .HasConstraintName("FK_ChargeStation_Groups_GroupId");
        }
    }
}
