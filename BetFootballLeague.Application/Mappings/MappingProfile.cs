﻿using AutoMapper;
using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Domain.Entities;

namespace BetFootballLeague.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CreateUserRequestDto, User>();
            CreateMap<UpdateUserRequestDto, User>();

            CreateMap<LeagueGroup, GroupDto>().ReverseMap();
            CreateMap<CreateGroupRequestDto, LeagueGroup>();
            CreateMap<UpdateGroupRequestDto, LeagueGroup>();

            CreateMap<Round, RoundDto>().ReverseMap();
            CreateMap<CreateRoundRequestDto, Round>();
            CreateMap<UpdateGroupRequestDto, Round>();

            CreateMap<Team, TeamDto>().ReverseMap();
            CreateMap<CreateTeamRequestDto, Team>();
            CreateMap<UpdateTeamRequestDto, Team>();
        }
    }
}
