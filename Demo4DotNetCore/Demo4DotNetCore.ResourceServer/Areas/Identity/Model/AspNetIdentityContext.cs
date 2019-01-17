using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Demo4DotNetCore.ResourceServer.Identity.Model
{
    public class AspNetIdentityContext : IdentityDbContext<ApplicationUser>
    {
        public AspNetIdentityContext(DbContextOptions<AspNetIdentityContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
