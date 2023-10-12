using AutoMapper;
using DataAccess.Models;
using Model.DataTransfer;
using Model.Domain;

namespace Api.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            AddMapping();
        }

        public void AddMapping()
        {
            CreateMap<LineagePathFindingRequest, LineagePathFindingModel>()
                .ForMember(dest => dest.SourcePoint, opt => opt.Ignore())
                .ForMember(dest => dest.Destinations, opt => opt.Ignore());
            CreateMap<RouteModel, PathModelResponse>()
                .ForMember(dest => dest.Source, src => src.MapFrom(x => x.Source.Name))
                .ForMember(dest => dest.Destination, src => src.MapFrom(x => x.Destination.Name))
                .ForMember(dest => dest.MoveType, src => src.MapFrom(x => x.MoveType));

            CreateMap<NodeRequest, NodeModel>();
            CreateMap<NodeModel, NodeResponse>();

            CreateMap<Model.Domain.NodeType, DataAccess.Models.NodeType>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => (int)x))
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.ToString()));
            CreateMap<Model.Domain.MoveType, DataAccess.Models.MoveType>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => (int)x))
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.ToString()));


            CreateMap<DataAccess.Models.NodeType, Model.Domain.NodeType>()
                .ConvertUsing(src => (Model.Domain.NodeType)src.Id);
                //.ForPath(dest => dest., src => src.MapFrom(x => (Model.Domain.NodeType)x.Id));
            CreateMap<DataAccess.Models.MoveType, Model.Domain.MoveType>()
                .ConvertUsing(src => (Model.Domain.MoveType)src.Id);

            CreateMap<NodeModel, Node>().ReverseMap();
            CreateMap<RouteModel, Route>().ReverseMap();
        }
    }
}
