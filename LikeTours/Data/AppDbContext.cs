using LikeTours.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LikeTours.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<TourType> Types { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<About> AboutUs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("CS");
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Package>()
               .HasOne(p => p.Place)
               .WithMany(pl => pl.Packages)
               .HasForeignKey(p => p.PlaceId);

            modelBuilder.Entity<Package>()
                .HasOne(p => p.TourType)
                .WithMany(t => t.Packages)
                .HasForeignKey(p => p.TourTypeId);

            modelBuilder.Entity<Package>()
                .HasMany(p => p.Images)
                .WithOne(i => i.Package)
                .HasForeignKey(i => i.PackageId);

            modelBuilder.Entity<Package>()
             .HasMany(p => p.Sections)
             .WithOne(s => s.Package)
             .HasForeignKey(s => s.PackageId);





            modelBuilder.Entity<Place>()
                .HasOne(p => p.MainPlace)
                .WithMany(p => p.RefferencePlaces)
                .HasForeignKey(p => p.PlaceId);

            modelBuilder.Entity<TourType>()
                .HasOne(p => p.MainTourType)
                .WithMany(p => p.RefferenceTypes)
                .HasForeignKey(p => p.TourTypeId);

            modelBuilder.Entity<Package>()
               .HasOne(p => p.MainPackage)
               .WithMany(p => p.RefferencePackages)
               .HasForeignKey(p => p.PackageId);

            modelBuilder.Entity<Questions>()
               .HasOne(p => p.MainQuetion)
               .WithMany(p => p.RefferenceQuestion)
               .HasForeignKey(p => p.QuestionId);

            modelBuilder.Entity<Review>()
               .HasOne(p => p.MainReview)
               .WithMany(p => p.RefferenceReview)
               .HasForeignKey(p => p.ReviewId);

            modelBuilder.Entity<Payment>()
           .HasOne(p => p.MainPayment)
           .WithMany(p => p.RefferencePayments)
           .HasForeignKey(p => p.PaymentId);




            modelBuilder.Entity<Place>()
              .HasIndex(p => new { p.Lang, p.PlaceId })
              .IsUnique();

            modelBuilder.Entity<Place>()
               .HasIndex(p => p.Name)
               .IsUnique();

            modelBuilder.Entity<TourType>()
                .HasIndex(p => new { p.Lang, p.TourTypeId })
                .IsUnique();

            modelBuilder.Entity<TourType>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<Package>()
                .HasIndex(p => new { p.Lang, p.PackageId })
                .IsUnique();

            modelBuilder.Entity<Questions>()
             .HasIndex(p => new { p.Lang, p.QuestionId })
             .IsUnique();

                modelBuilder.Entity<Questions>()
           .Property(q => q.Main)
           .HasDefaultValue(false);



            modelBuilder.Entity<Review>()
          .HasIndex(p => new { p.Lang, p.ReviewId })
          .IsUnique();

          
            modelBuilder.Entity<Payment>()
            .HasIndex(p => new { p.Lang, p.PaymentId })
            .IsUnique();




            base.OnModelCreating(modelBuilder);
        }
    }
}
