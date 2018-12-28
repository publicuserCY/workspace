using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Demo4DotNetCore.AuthorizationServer.Model
{
    public class CustumIdentityContext : DbContext
    {
        public CustumIdentityContext(DbContextOptions<CustumIdentityContext> options)
            : base(options)
        { } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite("Data Source=App_Data/identity.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(p => p.Id).HasColumnName("Id");
                entity.Property(p => p.Password).HasColumnName("Password");
                entity.Property(p => p.UserName).HasColumnName("UserName");
                entity.Property(p => p.Email).HasColumnName("Email");
                entity.Property(p => p.Mobile).HasColumnName("Mobile");
                entity.Property(p => p.Portrait).HasColumnName("Portrait");
                entity.Property(p => p.IsLocked).HasColumnName("IsLocked");
                entity.HasKey(p => p.Id);
            });
        }

        public DbSet<Account> Account { get; set; }
    }
}
