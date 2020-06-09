using Microsoft.AspNetCore.Identity;

namespace HdMovies.Models
{
    public enum Role
    {
        SuperAdmin,
        Admin,
        Moderator,
        Editor
    }

    public class UserRole : IdentityRole
    {
        public UserRole(Role role) : base(role.ToString())
        {
            Role = role;
        }

        public Role Role { get; set; }
    }
}
