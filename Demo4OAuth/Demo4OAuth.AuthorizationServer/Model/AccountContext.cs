using System.Data.Entity;

namespace Demo4OAuth.AuthorizationServer.Model
{
    public class AccountContext : DbContext
    {
        public AccountContext()
            : base("AccountContext")
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("idn_user");
            modelBuilder.Entity<User>().Property(p => p.Id).HasColumnName("Id");
            modelBuilder.Entity<User>().Property(p => p.Password).HasColumnName("Password");
            modelBuilder.Entity<User>().Property(p => p.UserName).HasColumnName("UserName");
            modelBuilder.Entity<User>().Property(p => p.Email).HasColumnName("Email");
            modelBuilder.Entity<User>().Property(p => p.UserName).HasColumnName("UserName");
            modelBuilder.Entity<User>().Property(p => p.PhoneNumber).HasColumnName("PhoneNumber");
            modelBuilder.Entity<User>().Property(p => p.Portrait).HasColumnName("Portrait");
        }

        //public static AccountContext Create()
        //{
        //    return new AccountContext();
        //}
    }
}
