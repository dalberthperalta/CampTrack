using AutoMapper;
using CampTrack.Application.DTOs;
using CampTrack.Domain.Entities;

namespace CampTrack.Application.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<Categoria, CategoriaDto>();
        CreateMap<CreateCategoriaDto, Categoria>();

        CreateMap<Equipo, EquipoDto>()
            .ForMember(dest => dest.CategoriaNombre, opt => opt.MapFrom(src => src.Categoria.Nombre));
        CreateMap<CreateEquipoDto, Equipo>();
        CreateMap<UpdateEquipoDto, Equipo>();

        CreateMap<Prestamo, PrestamoDto>()
            .ForMember(dest => dest.EquipoNombre, opt => opt.MapFrom(src => src.Equipo.Nombre));
        CreateMap<CreatePrestamoDto, Prestamo>();

        CreateMap<Revision, RevisionDto>()
            .ForMember(dest => dest.EquipoNombre, opt => opt.MapFrom(src => src.Equipo.Nombre));
        CreateMap<CreateRevisionDto, Revision>();
    }
}
