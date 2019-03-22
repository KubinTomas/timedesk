using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TImeDesk.Models.Database;

namespace TImeDesk.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {

        public DbSet<WorkSpace> WorkSpaces { get; set; }
        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<ToDoNotes> ToDoNoteses { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Billings> Billingses { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<UserSettings> UserSettingses { get; set; }
        public DbSet<NoteColorTheme> NoteColorThemes { get; set; }
        public DbSet<UserWorkspace> UserWorkspace { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<UserWorkspaceRoles> UserWorkspaceRoles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TimeEntryTag> TimeEntryTags { get; set; }
        public DbSet<UserWorkspaceBilling> UserWorkspaceBillings { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}