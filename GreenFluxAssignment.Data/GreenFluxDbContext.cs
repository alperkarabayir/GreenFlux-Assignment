using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GreenFluxAssignment.Core.Models;
using GreenFluxAssignment.Data.Configurations;

namespace GreenFluxAssignment.Data
{
    public class GreenFluxDbContext : DbContext
    {
        public DbSet<ChargeStation> ChargeStations { get; set; }
        public DbSet<Group> Groups { get; set; }

        public GreenFluxDbContext(DbContextOptions<GreenFluxDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfiguration(new ChargeStationConfiguration());

            builder
                .ApplyConfiguration(new GroupConfiguration());
        }
    }
}
