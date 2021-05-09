using AutoMapper;
using GreenFluxAssignment.Core;
using GreenFluxAssignment.Core.Models;
using GreenFluxAssignment.Core.Services;
using GreenFluxAssignment.Services.Domain;
using GreenFluxAssignment.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Services
{
    public class ConnectorService : IConnectorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Connector Service Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public ConnectorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        /// <summary>
        /// Creating Connector Service
        /// </summary>
        /// <param name="groupId">Group Id </param>
        /// <param name="chargeStationId">Charge Station Id</param>
        /// <param name="newConnector">New Connector</param>
        /// <returns>Created Connector</returns>
        public async Task<Connector> CreateConnector(Guid groupId, Guid chargeStationId, Connector newConnector)
        {
            //Getting Group - ChargeStation Data and Mapping to Domain
            var group = await _unitOfWork.Groups.GetWithGroupsByIdAsync(groupId);
            var chargeStation = await _unitOfWork.ChargeStations.GetChargeStationByIdAsync(chargeStationId);
            var groupDomain = _mapper.Map<Group, GroupDomain>(group);
            var chargeStationDomain = groupDomain.ChargeStations.FirstOrDefault(f => f.Id == chargeStationId);

            //Mapping New Connector to Domain - Adding Connector
            var connectorDomain = _mapper.Map<Connector, ConnectorDomain>(newConnector);
            try
            {
                chargeStationDomain.AddConnector(groupDomain, connectorDomain);
            }
            catch (NotEnoughCapacityException nex)
            {
                throw new NotEnoughCapacityException(nex.Capacity, nex.Difference);
            }

            //Map GroupDomain Back To Group
            var addedChargeStation = _mapper.Map<ChargeStationDomain, ChargeStation>(chargeStationDomain);

            chargeStation.Connectors = addedChargeStation.Connectors;

            //Commit Changes
            await _unitOfWork.CommitAsync();

            return newConnector;
        }

        /// <summary>
        /// Getting All Connectors Under Charge Station
        /// </summary>
        /// <param name="chargeStationId">Charge Station Id</param>
        /// <returns>Connector List</returns>
        public async Task<IEnumerable<Connector>> GetAllConnectorsByChargeStationId(Guid chargeStationId)
        {
            var chargeStation = await _unitOfWork.ChargeStations.GetByIdAsync(chargeStationId);
            if(chargeStation != null)
            {
               return chargeStation.Connectors;
            }
            throw new Exception("Connectors can not found");
        }

        /// <summary>
        /// Getting Connector By Id
        /// </summary>
        /// <param name="chargeStationId">Charge Station Id</param>
        /// <param name="connectorId">Connector Id</param>
        /// <returns>Connector Item</returns>
        public async Task<Connector> GetConnector(Guid chargeStationId, int connectorId)
        {
            var chargeStation = await _unitOfWork.ChargeStations.GetChargeStationByIdAsync(chargeStationId);
            if (chargeStation != null)
            {
                var connector = chargeStation.Connectors.FirstOrDefault(x => x.Id == connectorId);
                return connector;
            }
            throw new Exception("Connector can not found");
        }

        /// <summary>
        /// Removing Connector by Id 
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <param name="chargeStationId">Charge Station Id</param>
        /// <param name="connectorId">Connector Id</param>
        /// <returns>Void</returns>
        public async Task RemoveConnector(Guid groupId, Guid chargeStationId, int connectorId)
        {
            var chargeStation = await _unitOfWork.ChargeStations.GetChargeStationByIdAsync(chargeStationId);
            if (chargeStation != null)
            {
                var connector = chargeStation.Connectors.FirstOrDefault(a => a.Id == connectorId);
                if(connector != null)
                {
                    chargeStation.Connectors.Remove(connector);
                    await _unitOfWork.CommitAsync();
                }
            }            
            throw new Exception("Connector can not found");
            
        }

        /// <summary>
        /// Updating Connector
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <param name="chargeStationId">Charge Station Id</param>
        /// <param name="connectorId">Connector Id</param>
        /// <param name="updateConnector">Update Connector</param>
        /// <returns>Updated Connector</returns>
        public async Task<Connector> UpdateConnector(Guid groupId, Guid chargeStationId, int connectorId, Connector updateConnector)
        {
            //Getting Group Data and Mapping to Domain
            var group = await _unitOfWork.Groups.GetByIdAsync(groupId);

            var chargeStation = await _unitOfWork.ChargeStations.GetChargeStationByIdAsync(chargeStationId);
            var connector = chargeStation.Connectors.FirstOrDefault(x => x.Id == connectorId);

            //Mapping Update Connector to Domain
            var updateConnectorDomain = _mapper.Map<Connector, ConnectorDomain>(updateConnector);
            var groupDomain = _mapper.Map<Group, GroupDomain>(group);

            //Calculating new total capacity if we update connector
            var updatedGroupCapacity = groupDomain.ChargeStations.Sum(a => a.TotalCurrent) - connector.MaxCurrent + updateConnectorDomain.MaxCurrent;

            if (updatedGroupCapacity <= group.Capacity)
            {
                connector.MaxCurrent = updateConnectorDomain.MaxCurrent;
                await _unitOfWork.CommitAsync();

                return connector;
            }
            throw new Exception("Capacity is not enough");
        }

        /// <summary>
        /// Suggest Removal Connectors
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <param name="difference">Difference</param>
        /// <returns></returns>
        public async Task<IEnumerable<IEnumerable<Connector>>> SuggestRemovalConnectors(Guid groupId, decimal difference)
        {
            var suggestionList = new List<List<Connector>>();
            var group = await _unitOfWork.Groups.GetWithGroupsByIdAsync(groupId);

            var connectorList = group.ChargeStations
                .SelectMany(s => s.Connectors
                    .Select(c => new Connector { Id = c.Id, MaxCurrent = c.MaxCurrent, ChargeStationId = c.ChargeStationId }))
                .OrderByDescending(o => o.MaxCurrent)
                .ToList();

            //Looking For Exact Match First
            var exactMatch = connectorList.Where(x => x.MaxCurrent == difference).ToList();
            if (exactMatch.Count > 0)
            {
                suggestionList.AddRange(exactMatch.Select(match => new List<Connector> { match }));
            }

            //Ordered List - Start Adding By Highest, Break It When Suggestions big enough
            decimal suggestCount = 0;
            List<Connector> suggesting = new List<Connector>();
            for (int i = 0; i < connectorList.Count; i++)
            {
                suggestCount += connectorList[i].MaxCurrent;
                if (suggestCount >= difference)
                {
                    suggestionList.Add(suggesting);
                    break;
                }
                else
                {
                    suggesting.Add(connectorList[i]);
                }
            }
            return suggestionList;
        }
    }
}
