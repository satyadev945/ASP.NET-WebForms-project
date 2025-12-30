using AutoMapper;
using Films.Application.DTOs;
using Films.Domain.Entities;

namespace Films.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Actor mappings
        CreateMap<Actor, ActorDto>();
        CreateMap<ActorCreateDto, Actor>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"));
        CreateMap<ActorUpdateDto, Actor>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => "System"));

        // Film mappings
        CreateMap<Film, FilmDto>();
        CreateMap<FilmCreateDto, Film>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"));
        CreateMap<FilmUpdateDto, Film>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => "System"));

        // User mappings
        CreateMap<User, UserDto>();
        CreateMap<UserCreateDto, User>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Password hashing handled separately
        CreateMap<UserUpdateDto, User>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => "System"));

        // DirectedBy mappings
        CreateMap<DirectedBy, DirectedBy>();

        // TypeUser mappings
        CreateMap<TypeUser, TypeUser>();

        // Sex mappings
        CreateMap<Sex, Sex>();

        // Right mappings
        CreateMap<Right, Right>();

        // Reference mappings
        CreateMap<RefAF, RefAF>();
        CreateMap<RefDAF, RefDAF>();
        CreateMap<UserRight, UserRight>();
    }
}