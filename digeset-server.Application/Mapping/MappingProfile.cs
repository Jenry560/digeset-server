
using AutoMapper;
using digeset_server.Core.dtos;
using digeset_server.Core.entities;

namespace digeset_server.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Concepto, ConceptoDto>().ReverseMap();
            CreateMap<Multa, MultaDto>().ReverseMap();
            CreateMap<Agente, AgenteDto>().ReverseMap();
        }
    }
}
