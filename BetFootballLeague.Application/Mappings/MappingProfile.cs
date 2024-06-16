using AutoMapper;
using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Domain.Entities;

namespace BetFootballLeague.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
