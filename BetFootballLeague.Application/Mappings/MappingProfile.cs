using AutoMapper;
using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Domain.Entities;
using System.Globalization;

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

            CreateMap<LeagueMatch, MatchDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time.ToString("HH:mm")))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.DateTime.ToString("HH:mm dd/MM/yyyy")));
            CreateMap<CreateMatchRequestDto, LeagueMatch>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.ParseExact(src.Date, "dd/MM/yyyy")))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => TimeOnly.ParseExact(src.Time, "HH:mm")))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => DateTime.ParseExact($"{src.Date} {src.Time}", "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture)));
            CreateMap<UpdateMatchRequestDto, LeagueMatch>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.ParseExact(src.Date, "dd/MM/yyyy")))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => TimeOnly.ParseExact(src.Time, "HH:mm")))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => DateTime.ParseExact($"{src.Date} {src.Time}", "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture)));
            CreateMap<MatchDto, LeagueMatch>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.ParseExact(src.Date, "dd/MM/yyyy")))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => TimeOnly.ParseExact(src.Time, "HH:mm")))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => DateTime.ParseExact($"{src.Date} {src.Time}", "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture)));

            CreateMap<UserBet, UserBetDto>()
                .ForMember(dest => dest.CreatedAtStr, opt => opt.MapFrom(src => src.CreatedAt.ToString("HH:mm dd/MM/yyyy")))
                .ForMember(dest => dest.UpdatedAtStr, opt => opt.MapFrom(src => src.UpdatedAt));
            CreateMap<UserBetDto, UserBet>();
            CreateMap<MatchDto, MatchBetDto>();
            CreateMap<BetMatchRequestDto, CreateUserBetRequestDto>();
            CreateMap<BetMatchRequestDto, UpdateUserBetRequestDto>();
            CreateMap<CreateUserBetRequestDto, UserBet>();
            CreateMap<UpdateUserBetRequestDto, UserBet>();

            CreateMap<DateTime?, string?>()
                .ConvertUsing(new NullableDateTimeToStringConverter());
        }
    }
}
