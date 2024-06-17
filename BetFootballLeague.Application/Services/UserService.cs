using AutoMapper;
using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Domain.Entities;
using BetFootballLeague.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BetFootballLeague.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<List<UserDto>> GetActiveNormalUsers()
        {
            var users = await _userRepository.GetActiveNormalUsersAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserById(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        public async Task<UserDto?> GetUserByPhoneOrEmail(string input)
        {
            var user = await _userRepository.GetUserByPhoneOrEmailAsync(input);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        public async Task AddUser(UserDto userDto)
        {
            await _userRepository.AddUserAsync(_mapper.Map<User>(userDto));
        }

        public async Task UpdateUser(UserDto userDto)
        {
            await _userRepository.UpdateUserAsync(_mapper.Map<User>(userDto));
        }

        public async Task DeleteUser(UserDto userDto)
        {
            await _userRepository.DeleteUserAsync(_mapper.Map<User>(userDto));
        }
    }
}
