using GreenFluxAssignment.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IConnectorRepository Connectors { get; }
        IChargeStationRepository ChargeStations { get; }
        IGroupRepository Groups { get; }
        Task<int> CommitAsync();
    }
}
