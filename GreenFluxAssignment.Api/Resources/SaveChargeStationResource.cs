using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Api.Resources
{
    public class SaveChargeStationResource
    {
        public string Name { get; set; }
        public ICollection<SaveConnectorResource> Connectors { get; set; }
    }
}
