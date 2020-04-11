using System;
using System.ComponentModel.DataAnnotations;

namespace fristrupminde_api.Models.Inputs.Authentication
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
