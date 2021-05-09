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
    public class ChargeStationConfiguration : IEntityTypeConfiguration<ChargeStation>
    {
        public void Configure(EntityTypeBuilder<ChargeStation> builder)
        {
            builder.ToTable("ChargeStation");

            builder.HasKey(cs => cs.Id).HasName("PK_ChargeStation_Id");

            builder.Property(cs => cs.Id).ValueGeneratedNever();

            builder.Property(e => e.Name).IsRequired();

            builder.OwnsMany(cs => cs.Connectors, connectorBuilder =>
            {
                connectorBuilder
                    .WithOwner()
                    .HasForeignKey(c => c.ChargeStationId)
                    .HasConstraintName("FK_Connector_ChargeStation_ChargeStationId");

                connectorBuilder.Property(c => c.Id).ValueGeneratedNever();

                connectorBuilder.HasKey(c => new { c.ChargeStationId, c.Id }).HasName("PK_Connector_ChargeStationId_Id");

                connectorBuilder.Property(c => c.MaxCurrent).ValueGeneratedNever().HasColumnType("decimal(9,2)");
            });
        }
    }
}
