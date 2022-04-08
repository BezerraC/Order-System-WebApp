using System;
using Microsoft.AspNetCore.Identity;

namespace ArkdBarV1.Models
{
    public class AppUser : IdentityUser
    {
        public string Nome { get; set; }

       


    }
}
