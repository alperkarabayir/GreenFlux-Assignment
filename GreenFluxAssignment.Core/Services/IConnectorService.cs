using GreenFluxAssignment.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Core.Services
{
    public interface IConnectorService
    {
        Task<IEnumerable<Connector>> GetAllConnectorsByChargeStationId(Guid chargeStationId);
        Task<Connector> GetConnector(Guid chargeStationId, int connectorId);
        Task<Connector> CreateConnector(Guid groupId, Guid chargeStationId, Connector newConnector);
        Task<Connector> UpdateConnector(Guid groupId, Guid chargeStationId, int id, Connector updateConnector);
        Task RemoveConnector(Guid groupId, Guid chargeStationId, int id);
        Task<IEnumerable<IEnumerable<Connector>>> SuggestRemovalConnectors(Guid groupId, decimal difference);
    }
}
