using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Demo4OAuth.ResourceServer.Model
{
    public class ResourceContext : DbContext
    {
        public ResourceContext()
            : base("ResourceContext")
        {
            ((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized += (sender, args) =>
            {
                var entity = args.Entity as BaseModel;
                if (entity != null)
                {
                    entity.EntityState = EntityState.Unchanged;
                }
            };
        }

        public void ApplyChanges<TEntity>(TEntity root) where TEntity : BaseModel
        {
            var entitiesWithoutState = from p in ChangeTracker.Entries() where !(p.Entity is BaseModel) select p;
            if (entitiesWithoutState.Any())
            {
                throw new NotSupportedException("所有实体必须继承自BaseModel");
            }
            Set<TEntity>().Add(root);
            foreach (var entry in ChangeTracker.Entries<BaseModel>())
            {
                entry.State = entry.Entity.EntityState;
            }
            SaveChanges();
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new BookConfiguration());
            modelBuilder.Configurations.Add(new AttachmentConfiguration());
        }

        public static ResourceContext Create()
        {
            return new ResourceContext();
        }

        class BookConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Book>
        {
            public BookConfiguration()
            {
                ToTable("Book");
                Property(p => p.Id).HasColumnName("Id");
                Property(p => p.Name).HasColumnName("Name");
                HasMany(p => p.Attachments).WithOptional().HasForeignKey(p => p.FK);
            }
        }

        class AttachmentConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Attachment>
        {
            public AttachmentConfiguration()
            {
                ToTable("Attachment");
                Property(p => p.Id).HasColumnName("Id");
                Property(p => p.FileName).HasColumnName("FileName");
                Property(p => p.FileSize).HasColumnName("FileSize");
                Property(p => p.FileType).HasColumnName("FileType");
                Property(p => p.FilePath).HasColumnName("FilePath");                
                Property(p => p.Category).HasColumnName("Category");
                Property(p => p.AllowAnonymous).HasColumnName("AllowAnonymous");
                Property(p => p.FK).HasColumnName("FK");
                Property(p => p.UploadTime).HasColumnName("UploadTime");
                Property(p => p.UploadBy).HasColumnName("UploadBy");
                Property(p => p.Flag).HasColumnName("Flag");
                Property(p => p.MD5).HasColumnName("MD5");
            }
        }
    }
}
