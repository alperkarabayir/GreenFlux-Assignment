using AutoMapper;
using GreenFluxAssignment.Api.Resources;
using GreenFluxAssignment.Core.Models;
using GreenFluxAssignment.Services.Domain;

namespace GreenFluxAssignment.Api.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // Domain to Resource
            CreateMap<Connector, ConnectorResource>();
            CreateMap<ChargeStation, ChargeStationResource>();
            CreateMap<Group, GroupResource>();

            // Resource to Domain
            CreateMap<ConnectorResource, Connector>();
            CreateMap<ChargeStationResource, ChargeStation>();
            CreateMap<GroupResource, Group>();

            // Request Data
            CreateMap<SaveConnectorResource, Connector>();
            CreateMap<SaveChargeStationResource, ChargeStation>();
            CreateMap<SaveGroupResource, Group>();

            // Resource to Domain
            CreateMap<ConnectorDomain, Connector>();
            CreateMap<ChargeStationDomain, ChargeStation>();
            CreateMap<GroupDomain, Group>();

            // Request Data
            CreateMap<Connector, ConnectorDomain>();
            CreateMap<ChargeStation, ChargeStationDomain>();
            CreateMap<Group, GroupDomain>();
        }
    }
}
