using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Services.Domain
{
    public class ConnectorDomain
    {
        public int Id { get; set; }
        public decimal MaxCurrent { get; set; }
        public Guid ChargeStationId { get; set; }

        public void ChangeMaxCurrent(decimal current)
        {
            if (current <= 0)
            {
                throw new Exception("Current cannot be equal or below 0");
            }
            MaxCurrent = current;
        }
    }
}
