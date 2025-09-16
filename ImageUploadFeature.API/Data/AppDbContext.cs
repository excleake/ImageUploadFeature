using ImageUploadFeature.API.Entities;

using Microsoft.EntityFrameworkCore;

namespace ImageUploadFeature.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Profile> Profiles { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Lead> Leads { get; set; } = null!;
        public DbSet<Image> Images { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profile>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Customer>("Customer")
                .HasValue<Lead>("Lead");

            modelBuilder.Entity<Profile>(b =>
            {
                b.HasKey(p => p.Id);
                b.HasMany(p => p.Images)
                 .WithOne(i => i.Profile)
                 .HasForeignKey(i => i.ProfileId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Image>(b =>
            {
                b.HasKey(i => i.Id);
                b.Property(i => i.Base64).IsRequired();
            });
        }
    }
}
