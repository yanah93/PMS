using Microsoft.EntityFrameworkCore;
using PMS.Model;

namespace PMS.Data
{
    public class PMScontext :DbContext
    {
        public PMScontext(DbContextOptions<PMScontext>options) : base(options)
        {

        }

        public DbSet<TeamModel> TeamModels { get; set; }
        public DbSet<TeamMemberModel> TeamMemberModels { get; set; }
        public DbSet<RoleModel> RoleModels { get; set; }
        public DbSet<UserAccountModel> UserAccounts { get; set; }
        public DbSet<EmployeeModel> EmployeeModels { get; set; }

        public DbSet<ProjectModel> ProjectModels { get; set; }
        public DbSet<ProjectManagerModel> ProjectManagerModels { get; set; }
        public DbSet<OnProjectModel> OnProjectModels { get; set; }
        public DbSet<ClientPartnerModel> ClientPartnerModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeModel>().ToTable("Employee");
            modelBuilder.Entity<UserAccountModel>().ToTable("UserAccount");
            modelBuilder.Entity<RoleModel>().ToTable("Role");
            modelBuilder.Entity<TeamMemberModel>().ToTable("TeamMember");
            modelBuilder.Entity<TeamModel>().ToTable("Team");

            modelBuilder.Entity<ProjectModel>().ToTable("Project");
            modelBuilder.Entity<ProjectManagerModel>().ToTable("ProjectManager");
            modelBuilder.Entity<OnProjectModel>().ToTable("OnProject");
            modelBuilder.Entity<ClientPartnerModel>().ToTable("ClientPartner");
        }

    }
}
