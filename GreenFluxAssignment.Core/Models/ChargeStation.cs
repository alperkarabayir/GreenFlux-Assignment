using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Core.Models
{
    public class ChargeStation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Connector> Connectors { get; set; }
        public Guid GroupId { get; set; }
    }
}
