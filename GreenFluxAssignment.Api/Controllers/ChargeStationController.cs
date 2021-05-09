using AutoMapper;
using GreenFluxAssignment.Api.Resources;
using GreenFluxAssignment.Api.Validators;
using GreenFluxAssignment.Core.Models;
using GreenFluxAssignment.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Api.Controllers
{
    [Route("api/Groups/{groupId}/ChargeStations")]
    [ApiController]
    public class ChargeStationController : Controller
    {
        private readonly IChargeStationService _chargeStationService;
        private readonly IMapper _mapper;
        public ChargeStationController(IChargeStationService chargeStationService, IMapper mapper)
        {
            this._chargeStationService = chargeStationService;
            this._mapper = mapper;
        }
        
        /// <summary>
        /// Getting All Charge Stations by Group Id
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <returns>List of Charge Stations</returns>
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ChargeStationResource>>> GetAllChargeStationsByGroup(Guid groupId)
        {
            var chargeStations = await _chargeStationService.GetAllChargeStationsByGroupId(groupId);
            var chargeStationsResources = _mapper.Map<IEnumerable<ChargeStation>, IEnumerable<ChargeStationResource>>(chargeStations);
            return Ok(chargeStationsResources);
        }

        /// <summary>
        /// Getting Charge Station by ChargeStationId
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <param name="chargeStationId">Charge Station Id</param>
        /// <returns>Charge Station</returns>
        [HttpGet("{chargeStationId}")]
        public async Task<ActionResult<ChargeStationResource>> GetChargeStation(Guid groupId, Guid chargeStationId)
        {
            var chargeStation = await _chargeStationService.GetChargeStation(chargeStationId);
            var chargeStationResource = _mapper.Map<ChargeStation, ChargeStationResource>(chargeStation);

            return Ok(chargeStationResource);
        }

        /// <summary>
        /// Creating New Charge Station
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <param name="saveChargeStationResource">New Charge Station</param>
        /// <returns>Created Charge Station</returns>
        [HttpPost("")]
        public async Task<ActionResult<ChargeStationResource>> CreateChargeStation(Guid groupId, [FromBody] SaveChargeStationResource saveChargeStationResource)
        {
            var validator = new SaveChargeStationResourceValidator();
            var validationResult = await validator.ValidateAsync(saveChargeStationResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            
            try
            {
                var connectorToCreate = _mapper.Map<SaveChargeStationResource, ChargeStation>(saveChargeStationResource);
                var newChargeStation = await _chargeStationService.CreateChargeStation(groupId, connectorToCreate);
                var chargeStationResource = _mapper.Map<ChargeStation, ChargeStationResource>(newChargeStation);
                return Ok(chargeStationResource);
            }
            catch (Exception ex)
            {

                throw;
            }

            
        }

        /// <summary>
        /// Updating ChargeStation by using GroupId-ChargeStationId 
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <param name="chargeStationId">Charge Station Id</param>
        /// <param name="saveChargeStationResource">Charge Station Update Item</param>
        /// <returns>Updated Charge Station</returns>
        [HttpPut("{chargeStationId}")]
        public async Task<ActionResult<ChargeStationResource>> UpdateChargeStation(Guid groupId, Guid chargeStationId, [FromBody] SaveChargeStationResource saveChargeStationResource)
        {
            var validator = new SaveChargeStationResourceValidator();
            var validationResult = await validator.ValidateAsync(saveChargeStationResource);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var connectorToBeUpdated = await _chargeStationService.GetChargeStation(chargeStationId);
            if (connectorToBeUpdated == null)
                return NotFound();

            var chargeStation = _mapper.Map<SaveChargeStationResource, ChargeStation>(saveChargeStationResource);
            await _chargeStationService.UpdateChargeStation(chargeStationId, chargeStation);

            var updatedchargeStation = await _chargeStationService.GetChargeStation(chargeStationId);
            var updatedChargeStationResource = _mapper.Map<ChargeStation, ChargeStationResource>(updatedchargeStation);

            return Ok(updatedChargeStationResource);
        }

        /// <summary>
        /// Removing Charge Station by ChargeStation
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <param name="chargeStationId">Charge Station Id</param>
        /// <returns>No Content</returns>
        [HttpDelete("{chargeStationId}")]
        public async Task<ActionResult> RemoveChargeStation(Guid groupId, Guid chargeStationId)
        {
            await _chargeStationService.RemoveChargeStation(chargeStationId);
            return NoContent();
        }
    }
}
