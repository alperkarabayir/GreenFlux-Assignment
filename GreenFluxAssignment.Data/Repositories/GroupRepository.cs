using GreenFluxAssignment.Core.Models;
using GreenFluxAssignment.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Data.Repositories
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(GreenFluxDbContext context)
            : base(context)
        { }

        public async Task<IEnumerable<Group>> GetAllWithGroupsAsync()
        {
            return await GreenFluxDbContext.Groups
                .Include(a => a.ChargeStations)
                .ToListAsync();
        }

        public Task<Group> GetWithGroupsByIdAsync(Guid id)
        {
            return GreenFluxDbContext.Groups
                .Include(a => a.ChargeStations)
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        private GreenFluxDbContext GreenFluxDbContext
        {
            get { return Context as GreenFluxDbContext; }
        }
    }
}
