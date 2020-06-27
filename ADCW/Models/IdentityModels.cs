using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ADCW.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<ADCW.Models.Studio> Studios { get; set; }

        public System.Data.Entity.DbSet<ADCW.Models.Producer> Producers { get; set; }

        public System.Data.Entity.DbSet<ADCW.Models.VideoCategory> VideoCategories { get; set; }

        public System.Data.Entity.DbSet<ADCW.Models.Video> Videos { get; set; }

        public System.Data.Entity.DbSet<ADCW.Models.DVD> DVDs { get; set; }

        public System.Data.Entity.DbSet<ADCW.Models.Member> Members { get; set; }

        public System.Data.Entity.DbSet<ADCW.Models.MemberCategory> MemberCategories { get; set; }

        public System.Data.Entity.DbSet<ADCW.Models.LoanType> LoanTypes { get; set; }

        public System.Data.Entity.DbSet<ADCW.Models.Actor> Actors { get; set; }

        public System.Data.Entity.DbSet<ADCW.Models.CastMember> CastMembers { get; set; }

        public System.Data.Entity.DbSet<ADCW.Models.Loan> Loans { get; set; }

    }
}