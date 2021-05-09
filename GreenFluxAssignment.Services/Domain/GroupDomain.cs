using GreenFluxAssignment.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Services.Domain
{
    public class GroupDomain
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public decimal Capacity { get; private set; }
        public ICollection<ChargeStationDomain> ChargeStations { get; set; }

        public GroupDomain(Guid id, string name, decimal capacity)
        {
            Id = id;
            Name = name;
            Capacity = capacity;
        }

        public GroupDomain(string name, decimal capacity)
        {
            Id = Guid.NewGuid();
            UpdateCapacity(capacity);
            UpdateName(name);
        }

        public void UpdateName(string name)
        {
            Name = name;
        }

        public void UpdateCapacity(decimal capacity)
        {
            Capacity = capacity;
        }

        public void AddChargeStation(ChargeStationDomain chargeStation)
        {
            ChargeStations.Add(chargeStation);
        }

        public void RemoveChargeStation(Guid chargeStationId)
        {
            var chargeStation = ChargeStations.FirstOrDefault(f => f.Id == chargeStationId);
            ChargeStations.Remove(chargeStation);
        }
    }
}
