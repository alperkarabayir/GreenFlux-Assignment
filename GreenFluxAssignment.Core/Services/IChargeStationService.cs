using GreenFluxAssignment.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Core.Services
{
    public interface IChargeStationService
    {
        Task<IEnumerable<ChargeStation>> GetAllChargeStationsByGroupId(Guid groupId);
        Task<ChargeStation> GetChargeStation(Guid chargeStationId);
        Task<ChargeStation> CreateChargeStation(Guid groupId, ChargeStation newChargeStation);
        Task<ChargeStation> UpdateChargeStation(Guid chargeStationId, ChargeStation updateChargeStation);
        Task RemoveChargeStation(Guid chargeStationId);
        Task RemoveAllChargeStationsByGroupId(Guid groupId);
    }
}
