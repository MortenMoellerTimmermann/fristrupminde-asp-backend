﻿using System;
using Microsoft.AspNetCore.Identity;

namespace fristrupminde_api.Models
{
    public class ApplicationUser
    {
        public class ApplicationUser : IdentityUser<int>
        {
        }
    }
}