using AutoMapper;
using GreenFluxAssignment.Core;
using GreenFluxAssignment.Core.Models;
using GreenFluxAssignment.Core.Services;
using GreenFluxAssignment.Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Services
{
    public class ChargeStationService : IChargeStationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ChargeStationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<ChargeStation> CreateChargeStation(Guid groupId, ChargeStation newChargeStation)
        {
            //Getting Group Data and Mapping to Domain
            var group = await _unitOfWork.Groups.GetWithGroupsByIdAsync(groupId);
            var groupDomain = _mapper.Map<Group, GroupDomain>(group);

            //Mapping New Charge Station to Domain
            var chargeStationDomain = _mapper.Map<ChargeStation, ChargeStationDomain>(newChargeStation);
            var chargeStation = new ChargeStationDomain(groupDomain, chargeStationDomain.Name, chargeStationDomain.Connectors);

            //Add New Charge Station
            groupDomain.AddChargeStation(chargeStation);
            var updatedGroup = _mapper.Map<GroupDomain, Group>(groupDomain);
            group.ChargeStations = updatedGroup.ChargeStations;
            
            await _unitOfWork.CommitAsync();

            return _mapper.Map<ChargeStationDomain, ChargeStation>(chargeStation);
        }

        public async Task<IEnumerable<ChargeStation>> GetAllChargeStationsByGroupId(Guid groupId)
        {
            var chargeStations = await _unitOfWork.ChargeStations.GetAllChargeStationsByGroupIdAsync(groupId);
            if (chargeStations != null)
            {
                return chargeStations;
            }

            throw new Exception("Charge Stations can not found");
        }

        public async Task<ChargeStation> GetChargeStation(Guid chargeStationId)
        {
            var chargeStation = await _unitOfWork.ChargeStations.GetByIdAsync(chargeStationId);
            if (chargeStation != null)
            {
                return chargeStation;
            }
            throw new Exception("Charge Stations can not found");
        }

        public async Task RemoveAllChargeStationsByGroupId(Guid groupId)
        {
            var group = await _unitOfWork.Groups.GetByIdAsync(groupId);

            if(group != null)
            {
                foreach (var item in group.ChargeStations)
                {
                    _unitOfWork.ChargeStations.Remove(item);
                }
                await _unitOfWork.CommitAsync();
            }

            throw new Exception("Charge Stations can not found");
        }

        public async Task RemoveChargeStation(Guid chargeStationId)
        {
            var chargeStation = await _unitOfWork.ChargeStations.GetByIdAsync(chargeStationId);

            if (chargeStation != null)
            {
                _unitOfWork.ChargeStations.Remove(chargeStation);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new Exception("Charge Station can not found");
            }
        }

        public async Task<ChargeStation> UpdateChargeStation(Guid chargeStationId, ChargeStation updateChargeStation)
        {
            //Getting Charge Stations Data and Mapping to Domain
            var chargeStation = await _unitOfWork.ChargeStations.GetByIdAsync(chargeStationId);
            var chargeStationDomain = _mapper.Map<ChargeStation, ChargeStationDomain>(chargeStation);

            chargeStationDomain.UpdateStationName(updateChargeStation.Name);
            var updatedChargeStation = _mapper.Map<ChargeStationDomain, ChargeStation>(chargeStationDomain);
            chargeStation.Name = updatedChargeStation.Name;
            chargeStation.Connectors = updateChargeStation.Connectors;

            await _unitOfWork.CommitAsync();
            return chargeStation;
        }
    }
}
