using GreenFluxAssignment.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Services.Domain
{
    public class ChargeStationDomain
    {
        private const int MinConnectors = 1;
        private const int MaxConnectors = 5;

        internal decimal TotalCurrent
        {
            get
            {
                return Connectors?.Sum(s => s.MaxCurrent) ?? 0;
            }
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid GroupId { get; set; }
        public ICollection<ConnectorDomain> Connectors { get; set; }
            
        public ChargeStationDomain() { }
        public ChargeStationDomain(GroupDomain group, string name, ICollection<ConnectorDomain> connectors)
        {
            Id = Guid.NewGuid();
            Name = name;
            GroupId = group.Id;
            foreach (var connector in connectors)
            {
                AddConnector(group, connector);
            }
        }

        public void UpdateStationName(string name)
        {
            Name = name;
        }

        public void AddConnector(GroupDomain group, ConnectorDomain connector)
        {
            group.ChargeStations = group.ChargeStations == null ? new List<ChargeStationDomain>() : group.ChargeStations;
            var groupCurrentCapacity = group.ChargeStations.Sum(a => a.TotalCurrent);
            if (groupCurrentCapacity + connector.MaxCurrent > group.Capacity)
            {
                var diff = (groupCurrentCapacity + connector.MaxCurrent) - group.Capacity;
                throw new NotEnoughCapacityException(connector.MaxCurrent, diff);
            }
            
            var nextConnectorId = GetNextAvailableConnectorId();
            if (nextConnectorId > MaxConnectors)
            {
                throw new DomainException($"You can not add more than {MaxConnectors} Connectors to the ChargeStation");
            }
            
            if(Connectors == null)
            {
                Connectors = new List<ConnectorDomain>();
            }
            Connectors.Add(new ConnectorDomain
            {
                Id = nextConnectorId,
                MaxCurrent = connector.MaxCurrent,
                ChargeStationId = Id
            });
        }

        public void RemoveConnector(int id)
        {
            var ConnectorDomain = Connectors.FirstOrDefault(f => f.Id == id);
            Connectors.Remove(ConnectorDomain);
        }

        private int GetNextAvailableConnectorId()
        {
            if (Connectors == null)
            {
                return 1;
            }

            int i = MinConnectors;
            for (; i <= MaxConnectors; i++)
            {
                if (Connectors.Any(x => x.Id == i))
                {
                    continue;
                }

                return i++;
            }

            return i++;
        }
    }
}
