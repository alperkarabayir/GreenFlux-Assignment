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
    [Route("api/Groups")]
    [ApiController]
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;
        public GroupController(IGroupService groupService, IMapper mapper)
        {
            this._groupService = groupService;
            this._mapper = mapper;
        }

        /// <summary>
        /// Getting All Groups
        /// </summary>
        /// <returns>List of Group</returns>
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<GroupResource>>> GetAllGroups()
        {
            var groups = await _groupService.GetAllGroups();
            var groupsResources = _mapper.Map<IEnumerable<Group>, IEnumerable<GroupResource>>(groups);
            return Ok(groupsResources);
        }

        /// <summary>
        /// Getting Group by GroupId
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <returns>Group</returns>
        [HttpGet("{groupId}")]
        public async Task<ActionResult<GroupResource>> GetGroup(Guid groupId)
        {
            try
            {
                var group = await _groupService.GetGroupById(groupId);
                if (group != null)
                {
                    var groupResource = _mapper.Map<Group, GroupResource>(group);

                    return Ok(groupResource);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Creating a New Group
        /// </summary>
        /// <param name="saveGroupResource">New Group Item</param>
        /// <returns>Created Group</returns>
        [HttpPost("")]
        public async Task<ActionResult<GroupResource>> CreateGroup([FromBody] SaveGroupResource saveGroupResource)
        {
            var validator = new SaveGroupResourceValidator();
            var validationResult = await validator.ValidateAsync(saveGroupResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var groupToCreate = _mapper.Map<SaveGroupResource, Group>(saveGroupResource);
            var newGroup = await _groupService.CreateGroup(groupToCreate);

            var groupResource = _mapper.Map<Group, GroupResource>(newGroup);

            return Ok(groupResource);
        }

        /// <summary>
        /// Updating a Group
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <param name="saveGroupResource">Group Update Item</param>
        /// <returns>Updated Group</returns>
        [HttpPut("{groupId}")]
        public async Task<ActionResult<GroupResource>> UpdateGroup(Guid groupId, [FromBody] SaveGroupResource saveGroupResource)
        {
            var validator = new SaveGroupResourceValidator();
            var validationResult = await validator.ValidateAsync(saveGroupResource);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var connectorToBeUpdated = await _groupService.GetGroupById(groupId);
            if (connectorToBeUpdated == null)
                return NotFound();

            var group = _mapper.Map<SaveGroupResource, Group>(saveGroupResource);

            var updatedGroup = await _groupService.UpdateGroup(groupId, group);
            var updatedGroupResource = _mapper.Map<Group, GroupResource>(updatedGroup);

            return Ok(updatedGroupResource);
        }

        /// <summary>
        /// Removing a Group
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <returns>No Content</returns>
        [HttpDelete("{groupId}")]
        public async Task<ActionResult> RemoveGroup(Guid groupId)
        {
            await _groupService.RemoveGroup(groupId);
            return NoContent();
        }
    }
}
