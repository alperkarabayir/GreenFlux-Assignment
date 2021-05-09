using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Api.Resources
{
    public class ChargeStationResource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<ConnectorResource> Connectors { get; set; }
        public Guid GroupId { get; set; }
    }

}
