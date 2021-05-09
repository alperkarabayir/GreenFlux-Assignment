using GreenFluxAssignment.Core;
using GreenFluxAssignment.Core.Repositories;
using GreenFluxAssignment.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GreenFluxDbContext _context;

        public UnitOfWork(GreenFluxDbContext context)
        {
            this._context = context;
        }

        private ChargeStationRepository _chargeStationRepository;
        private GroupRepository _groupRepository;
        private ConnectorRepository _connectorRepository;

        public IChargeStationRepository ChargeStations => _chargeStationRepository = _chargeStationRepository ?? new ChargeStationRepository(_context);
        public IGroupRepository Groups => _groupRepository = _groupRepository ?? new GroupRepository(_context);
        public IConnectorRepository Connectors => _connectorRepository = _connectorRepository ?? new ConnectorRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
