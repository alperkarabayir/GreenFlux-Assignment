using GreenFluxAssignment.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Core.Repositories
{
    public interface IConnectorRepository : IRepository<Connector>
    {
        Task<IEnumerable<Connector>> GetAllConnectorsByChargeStationId(Guid chargeStationId);
        Task<Connector> GetConnectorsById(Guid chargeStationId, int connectorId);
    }
}
