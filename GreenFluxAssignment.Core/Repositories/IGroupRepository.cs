using GreenFluxAssignment.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Core.Repositories
{
    public interface IGroupRepository : IRepository<Group>
    {
        Task<IEnumerable<Group>> GetAllWithGroupsAsync();
        Task<Group> GetWithGroupsByIdAsync(Guid id);
    }
}
