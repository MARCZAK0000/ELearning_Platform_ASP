﻿using Microsoft.AspNetCore.Identity;

namespace ELearning_Platform.Domain.Database.Enitities
{
    public class Account : IdentityUser
    {
        public User User { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
