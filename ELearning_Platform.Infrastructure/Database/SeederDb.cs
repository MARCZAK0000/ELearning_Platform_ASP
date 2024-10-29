﻿using ELearning_Platform.Domain.Enitities;
using Microsoft.AspNetCore.Identity;
using ELearning_Platform.Domain.Authorization;

namespace ELearning_Platform.Infrastructure.Database
{
    public class SeederDb(RoleManager<Roles> roleManager, PlatformDb platformDb)
    {
        private readonly RoleManager<Roles> _roleManager = roleManager;

        private readonly PlatformDb _platformDb = platformDb;

        private readonly string[] _roles = Enum.GetNames(typeof(AuthorizationRole));
        public async Task GenerateRolesAsync()
        {
            if(await _platformDb.Database.CanConnectAsync())
            {
                if(!_platformDb.Roles.Any())
                {
                    foreach (var item in _roles)
                    {
                        await _roleManager.CreateAsync(new Roles()
                        {
                            Name = item,
                            NormalizedName = item,
                        });
                    }
                    await _platformDb.SaveChangesAsync();
                }
            }
        } 
    }
}
