using AutoMapper;
using Films.Domain.DTOs;
using Films.Domain.Entities;

namespace Films.Application.Mappings;

/// <summary>
/// AutoMapper configuration profile for DTOs
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Film mappings
        CreateMap<Film, FilmDto>();
        CreateMap<FilmCreateDto, Film>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.RefAFs, opt => opt.Ignore())
            .ForMember(dest => dest.RefDAFs, opt => opt.Ignore());

        CreateMap<FilmUpdateDto, Film>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.RefAFs, opt => opt.Ignore())
            .ForMember(dest => dest.RefDAFs, opt => opt.Ignore());

        // Actor mappings
        CreateMap<Actor, ActorDto>();
        CreateMap<ActorCreateDto, Actor>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Sex, opt => opt.Ignore())
            .ForMember(dest => dest.RefAFs, opt => opt.Ignore());

        CreateMap<ActorUpdateDto, Actor>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.Sex, opt => opt.Ignore())
            .ForMember(dest => dest.RefAFs, opt => opt.Ignore());
    }
}