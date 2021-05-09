using GreenFluxAssignment.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Core.Repositories
{
    public interface IChargeStationRepository : IRepository<ChargeStation>
    {
        Task<ChargeStation> GetChargeStationByIdAsync(Guid id);
        Task<IEnumerable<ChargeStation>> GetAllChargeStationsByGroupIdAsync(Guid groupId);
    }
}
