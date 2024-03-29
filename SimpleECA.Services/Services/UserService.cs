﻿using SimpleECA.Models;
using SimpleECA.Repos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleECA.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepo _userRepo;
        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        public async Task<bool> CreateUser(UserDetailsViewModel user)
        {
            return await _userRepo.CreateUser(user);
        }
        public async Task<bool> CreateUserAddress(UserAddressViewModel model)
        {
            return await _userRepo.CreateUserAddress(model);
        }
        public async Task<List<UserAddressViewModel>> GetUserAddressList(int userid)
        {
            return await _userRepo.GetUserAddressList(userid);
        }
    }
}
