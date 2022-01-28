using Microsoft.AspNetCore.Identity;
using System;

namespace Core.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Active { get; set; }
    }
}
