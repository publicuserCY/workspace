using Microsoft.EntityFrameworkCore;

namespace Demo4DotNetCore.ResourceServer.Model
{
    public class ResourceContext : DbContext
    {
        public ResourceContext(DbContextOptions<ResourceContext> options)
            : base(options)
        { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Attachment> Attachments { get; set; }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book");
                entity.Property(p => p.Id).HasColumnName("Id");
                entity.Property(p => p.Name).HasColumnName("Name");
                entity.HasKey(p => p.Id);
                entity.HasMany(p => p.Attachments).WithOne();
            });

            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.ToTable("Attachment");
                entity.Property(p => p.Id).HasColumnName("Id");
                entity.Property(p => p.FileName).HasColumnName("FileName");
                entity.Property(p => p.FileSize).HasColumnName("FileSize");
                entity.Property(p => p.FileType).HasColumnName("FileType");
                entity.Property(p => p.FilePath).HasColumnName("FilePath");
                entity.Property(p => p.Category).HasColumnName("Category");
                entity.Property(p => p.AllowAnonymous).HasColumnName("AllowAnonymous");
                entity.Property(p => p.Reference).HasColumnName("Reference");
                entity.Property(p => p.UploadTime).HasColumnName("UploadTime");
                entity.Property(p => p.UploadBy).HasColumnName("UploadBy");
                entity.Property(p => p.Flag).HasColumnName("Flag");
                entity.Property(p => p.MD5).HasColumnName("MD5");
                entity.HasKey(p => p.Id);
            });
        }
    }
}
