using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Billing.Entities
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string PersonName { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(20), MinLength(6)]
        public string MobileNo { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(50), MinLength(6)]
        public string NationalId { get; set; }
        public MaritialStatus MaritialStatus { get; set; }
        public Sex Gender { get; set; }
        [Required]
        public UserRole UserRoles { get; set; }
        public UserStatus UserStatus { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("DisplayName", PersonName));
            return userIdentity;
        }
    }
}
