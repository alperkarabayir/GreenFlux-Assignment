using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GreenFluxAssignment.Core;
using GreenFluxAssignment.Core.Models;
using GreenFluxAssignment.Core.Services;
using GreenFluxAssignment.Services.Domain;

namespace GreenFluxAssignment.Services
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GroupService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<Group> CreateGroup(Group newGroup)
        {
            //Creating a New Group Domain - Mapping to Group
            GroupDomain groupDomain = new GroupDomain(newGroup.Name, newGroup.Capacity);
            var group = _mapper.Map<GroupDomain, Group>(groupDomain);

            await _unitOfWork.Groups.AddAsync(group);
            await _unitOfWork.CommitAsync();

            return group;
        }

        public async Task<IEnumerable<Group>> GetAllGroups()
        {
            var groups = await _unitOfWork.Groups.GetAllWithGroupsAsync();
            if (groups != null)
            {
                return groups;
            }
            throw new Exception("Groups can not found");
        }

        public async Task<Group> GetGroupById(Guid id)
        {
            var group = await _unitOfWork.Groups.GetWithGroupsByIdAsync(id);
            if (group != null)
            {
                return group;
            }
            throw new Exception("Group can not found");
        }

        public async Task RemoveGroup(Guid id)
        {
            var group = await _unitOfWork.Groups.GetByIdAsync(id);
            if (group != null)
            {
                if(group.ChargeStations != null)
                {
                    foreach (var item in group.ChargeStations)
                    {
                        _unitOfWork.ChargeStations.Remove(item);
                    }
                }

                _unitOfWork.Groups.Remove(group);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new Exception($"Group not found with id: {id}");
            }
            
        }

        public async Task<Group> UpdateGroup(Guid groupId, Group updateGroup)
        {
            //Getting Group Data and Mapping to Group Domain
            var group = await _unitOfWork.Groups.GetByIdAsync(groupId);
            var groupDomain = _mapper.Map<Group, GroupDomain>(group);

            //Update Values
            if (!string.IsNullOrEmpty(updateGroup.Name))
            {
                groupDomain.UpdateName(updateGroup.Name);
            }

            if (updateGroup.Capacity > 0)
            {
                groupDomain.UpdateCapacity(updateGroup.Capacity);
            }

            group.Name = groupDomain.Name;
            group.Capacity = groupDomain.Capacity;
            
            await _unitOfWork.CommitAsync();
            return group;
        }
    }
}
