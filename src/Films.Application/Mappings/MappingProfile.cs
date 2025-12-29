using AutoMapper;
using Films.Application.DTOs;
using Films.Domain.Entities;

namespace Films.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Film, FilmDto>();
        CreateMap<FilmCreateDto, Film>();
        CreateMap<FilmUpdateDto, Film>();

        CreateMap<Actor, ActorDto>()
            .ForMember(dest => dest.SexName, opt => opt.MapFrom(src => src.Sex != null ? src.Sex.Name : null));
        CreateMap<ActorCreateDto, Actor>();
        CreateMap<ActorUpdateDto, Actor>();

        CreateMap<DirectedBy, DirectedByDto>();
        CreateMap<DirectedByCreateDto, DirectedBy>();
        CreateMap<DirectedByUpdateDto, DirectedBy>();

        CreateMap<User, UserDto>()
            .ForMember(dest => dest.TypeUserName, opt => opt.MapFrom(src => src.TypeUser != null ? src.TypeUser.Name : null));
        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>();
    }
}
