using AutoMapper;
using BetFootballLeague.Application.DTOs;
using BetFootballLeague.Domain.Entities;
using BetFootballLeague.Domain.Repositories;
using BetFootballLeague.Shared.Helpers;
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

        public async Task<UserDto?> GetUserByPhone(string input, Guid? currentUserId = null)
        {
            var user = await _userRepository.GetUserByPhoneAsync(input);
            if (user != null)
            {
                if (currentUserId != null && user.Id == currentUserId)
                {
                    return null;
                }
                return _mapper.Map<UserDto>(user);
            }
            return null;
        }

        public async Task<UserDto?> GetUserByEmail(string input, Guid? currentUserId = null)
        {
            var user = await _userRepository.GetUserByEmailAsync(input);
            if (user != null)
            {
                if (currentUserId != null && user.Id == currentUserId)
                {
                    return null;
                }
                return _mapper.Map<UserDto>(user);
            }
            return null;
        }

        public async Task<UserDto?> GetUserByUsername(string input, Guid? currentUserId = null)
        {
            var user = await _userRepository.GetUserByUsernameAsync(input);
            if (user != null)
            {
                if (currentUserId != null && user.Id == currentUserId)
                {
                    return null;
                }
                return _mapper.Map<UserDto>(user);
            }
            return null;
        }

        public async Task AddUser(CreateUserRequestDto userDto)
        {
            userDto.Password = PasswordHelper.HashPasword(userDto.Password);
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
