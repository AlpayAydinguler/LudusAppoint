using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserManager : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserManager(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDtoForUpdate> GetUserForUpdateAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return _mapper.Map<UserDtoForUpdate>(user);
        }

        public Task<string> IsUserActive(string phoneNumber)
        {
            var user = _userManager.Users.AsNoTracking()
                                         .Select(u => new { u.PhoneNumber, 
                                                            u.IsActive,
                                                            u.LastLogin})
                                         .OrderByDescending(u => u.LastLogin)
                                         .FirstOrDefault(u => u.PhoneNumber == phoneNumber.NormalizePhoneNumber());
            if (user == null)
            {
                return Task.FromResult("NotFound");
            }
            return Task.FromResult(user.IsActive ? "Active" : "Inactive");
        }
    }
}
