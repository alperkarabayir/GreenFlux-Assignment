using GreenFluxAssignment.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Core.Services
{
    public interface IGroupService
    {
        Task<IEnumerable<Group>> GetAllGroups();

        Task<Group> GetGroupById(Guid id);

        Task<Group> CreateGroup(Group newGroup);

        Task<Group> UpdateGroup(Guid groupId, Group updateGroup);

        Task RemoveGroup(Guid id);
    }
}
