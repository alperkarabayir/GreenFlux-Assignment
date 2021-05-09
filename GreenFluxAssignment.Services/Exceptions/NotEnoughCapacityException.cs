using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Services.Exceptions
{
    public class NotEnoughCapacityException : Exception
    {
        public decimal Capacity { get; }
        public decimal Difference { get; }

        public NotEnoughCapacityException(decimal connectorCapacity, decimal differenceCurrent)
            : base($"Connector can not add; connector capacity: {connectorCapacity}, difference : {differenceCurrent} ")
        {
            Capacity = connectorCapacity;
            Difference = differenceCurrent;
        }
    }
}
