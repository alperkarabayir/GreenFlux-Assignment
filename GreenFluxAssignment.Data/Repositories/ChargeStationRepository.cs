using GreenFluxAssignment.Core.Models;
using GreenFluxAssignment.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Data.Repositories
{
    public class ChargeStationRepository : Repository<ChargeStation>, IChargeStationRepository
    {
        public ChargeStationRepository(GreenFluxDbContext context)
            : base(context)
        { }
        public async Task<ChargeStation> GetChargeStationByIdAsync(Guid id)
        {
            return await GreenFluxDbContext.ChargeStations
                .Include(a => a.Connectors)
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<ChargeStation>> GetAllChargeStationsByGroupIdAsync(Guid groupId)
        {
            return await GreenFluxDbContext.ChargeStations
                .Where(a => a.GroupId == groupId)
                .Include(a => a.Connectors)
                .ToListAsync();
        }

        private GreenFluxDbContext GreenFluxDbContext
        {
            get { return Context as GreenFluxDbContext; }
        }
    }
}
