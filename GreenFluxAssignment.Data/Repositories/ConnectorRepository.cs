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
    public class ConnectorRepository : Repository<Connector>, IConnectorRepository
    {
        public ConnectorRepository(GreenFluxDbContext context)
            : base(context)
        { } 
        private GreenFluxDbContext GreenFluxDbContext
        {
            get { return Context as GreenFluxDbContext; }
        }

        public async Task<IEnumerable<Connector>> GetAllConnectorsByChargeStationId(Guid chargeStationId)
        {
            return await GreenFluxDbContext.ChargeStations
                .Where(a => a.Id == chargeStationId)
                .SelectMany(a => a.Connectors)
                .ToListAsync();
        }

        public async Task<Connector> GetConnectorsById(Guid chargeStationId, int id)
        {
            return await GreenFluxDbContext.ChargeStations
                .Where(a => a.Id == chargeStationId)
                .SelectMany(a => a.Connectors)
                .SingleOrDefaultAsync(a => a.Id == id);
        }
    }
}
