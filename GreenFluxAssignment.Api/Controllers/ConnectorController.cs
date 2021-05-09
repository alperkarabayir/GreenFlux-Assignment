using AutoMapper;
using GreenFluxAssignment.Api.Resources;
using GreenFluxAssignment.Api.Validators;
using GreenFluxAssignment.Core.Models;
using GreenFluxAssignment.Core.Services;
using GreenFluxAssignment.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Api.Controllers
{
    [Route("api/Groups/{groupId}/ChargeStations/{chargeStationId}/Connectors")]
    [ApiController]
    public class ConnectorController : ControllerBase
    {
        private readonly IConnectorService _connectorService;
        private readonly IMapper _mapper;
        public ConnectorController(IConnectorService connectorService, IMapper mapper)
        {
            this._connectorService = connectorService;
            this._mapper = mapper;
        }

        /// <summary>
        /// Getting All Connectors By ChargeStationId
        /// </summary>
        /// <param name="chargeStationId">Charge Station Id</param>
        /// <returns>List of Connectors</returns>
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ConnectorResource>>> GetAllConnectorsByChargeStation(Guid chargeStationId)
        {
            var connectors = await _connectorService.GetAllConnectorsByChargeStationId(chargeStationId);
            var connectorResources = _mapper.Map<IEnumerable<Connector>, IEnumerable<ConnectorResource>>(connectors);
            return Ok(connectorResources);
        }

        /// <summary>
        /// Getting a Connector By ChargeStationId and ConnectorID
        /// </summary>
        /// <param name="chargeStationId">Charge Station Id</param>
        /// <param name="connectorId">Connector Id</param>
        /// <returns>Connector</returns>
        [HttpGet("{connectorId}")]
        public async Task<ActionResult<ConnectorResource>> GetConnector(Guid chargeStationId, int connectorId)
        {
            var connector = await _connectorService.GetConnector(chargeStationId, connectorId);
            var connectorResource = _mapper.Map<Connector, ConnectorResource>(connector);

            return Ok(connectorResource);
        }

        /// <summary>
        /// Creating New Connector
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <param name="chargeStationId">Charge Station Id</param>
        /// <param name="saveConnectorResource">New Connector</param>
        /// <returns>Created Connector</returns>
        [HttpPost("")]
        public async Task<ActionResult<ConnectorResource>> CreateConnector(Guid groupId, Guid chargeStationId, [FromBody] SaveConnectorResource saveConnectorResource)
        {
            var validator = new SaveConnectorResourceValidator();
            var validationResult = await validator.ValidateAsync(saveConnectorResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var connectorToCreate = _mapper.Map<SaveConnectorResource, Connector>(saveConnectorResource);
                var newConnector = await _connectorService.CreateConnector(groupId, chargeStationId, connectorToCreate);
                var connectorResource = _mapper.Map<Connector, ConnectorResource>(newConnector);

                return Ok(connectorResource);
            }
            catch (NotEnoughCapacityException nex)
            {
                return RedirectToAction("SuggestRemovalConnectors", new { group = groupId, difference = nex.Difference });
            }
            
            
        }

        /// <summary>
        /// Updating Connector by using GroupId-ChargeStationId-ConnectorId
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <param name="chargeStationId">Charge Station Id</param>
        /// <param name="connectorId">Connector Id</param>
        /// <param name="saveConnectorResource">Connector Update Item</param>
        /// <returns>Updated Connector</returns>
        [HttpPut("{connectorId}")]
        public async Task<ActionResult<ConnectorResource>> UpdateConnector(Guid groupId, Guid chargeStationId, int connectorId, [FromBody] SaveConnectorResource saveConnectorResource)
        {
            var validator = new SaveConnectorResourceValidator();
            var validationResult = await validator.ValidateAsync(saveConnectorResource);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var connectorToBeUpdated = await _connectorService.GetConnector(chargeStationId, connectorId);
            if (connectorToBeUpdated == null)
                return NotFound();

            var connector = _mapper.Map<SaveConnectorResource, Connector>(saveConnectorResource);
            await _connectorService.UpdateConnector(groupId, connectorToBeUpdated.ChargeStationId , connectorToBeUpdated.Id, connector);
            var updatedConnectorResource = _mapper.Map<Connector, ConnectorResource>(connector);

            return Ok(updatedConnectorResource);
        }

        /// <summary>
        /// Removing Connector by using GroupId-ChargeStationId-ConnectorId
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <param name="chargeStationId">Charge Station Id</param>
        /// <param name="connectorId">Connector Id</param>
        /// <returns>No Content</returns>
        [HttpDelete("{connectorId}")]
        public async Task<ActionResult> RemoveConnector(Guid groupId, Guid chargeStationId, int connectorId)
        {
            await _connectorService.RemoveConnector(groupId, chargeStationId, connectorId);
            
            return NoContent();
        }

        [HttpGet("/suggestions")]
        public async Task<ActionResult<IEnumerable<IEnumerable<Connector>>>> SuggestRemovalConnectors(Guid group, decimal difference)
        {
            var suggest = await _connectorService.SuggestRemovalConnectors(group, difference);
            var connectorResources = _mapper.Map<IEnumerable<IEnumerable<Connector>>, IEnumerable<IEnumerable<ConnectorResource>>>(suggest);
            return Ok(connectorResources);
        }
    }
}
