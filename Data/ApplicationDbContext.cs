using Microsoft.EntityFrameworkCore;
using PartnerControlAPI.Models;

namespace PartnerControlAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Partner> Partners { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ItemDetail> ItemDetails { get; set; }
        public DbSet<DiscountSetting> DiscountSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Partner>()
                .HasIndex(p => p.PartnerNo)
                .IsUnique();

            modelBuilder.Entity<Partner>()
                .HasIndex(p => p.PartnerKey)
                .IsUnique();

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => new { t.PartnerKey, t.PartnerRefNo })
                .IsUnique();

            modelBuilder.Entity<Partner>().HasData(
                new Partner
                {
                    Id = 1,
                    PartnerNo = "FG-00001",
                    PartnerKey = "FAKEGOOGLE",
                    Password = "FAKEPASSWORD1234",
                    IsActive = true
                },
                new Partner
                {
                    Id = 2,
                    PartnerNo = "FG-00002",
                    PartnerKey = "FAKEPEOPLE",
                    Password = "FAKEPASSWORD4578",
                    IsActive = true
                }
            );

            modelBuilder.Entity<DiscountSetting>().HasData(
                new DiscountSetting
                {
                    Id = 1,
                    Type = DiscountType.Base,
                    MinAmount = 0,
                    MaxAmount = 199,
                    DiscountPercentage = 0,
                    IsActive = true,
                    Description = "No discount for amounts less than MYR 200",
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new DiscountSetting
                {
                    Id = 2,
                    Type = DiscountType.Base,
                    MinAmount = 200,
                    MaxAmount = 500,
                    DiscountPercentage = 5,
                    IsActive = true,
                    Description = "5% discount for amounts between MYR 200 and MYR 500",
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new DiscountSetting
                {
                    Id = 3,
                    Type = DiscountType.Base,
                    MinAmount = 501,
                    MaxAmount = 800,
                    DiscountPercentage = 7,
                    IsActive = true,
                    Description = "7% discount for amounts between MYR 501 and MYR 800",
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new DiscountSetting
                {
                    Id = 4,
                    Type = DiscountType.Base,
                    MinAmount = 801,
                    MaxAmount = 1200,
                    DiscountPercentage = 10,
                    IsActive = true,
                    Description = "10% discount for amounts between MYR 801 and MYR 1200",
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new DiscountSetting
                {
                    Id = 5,
                    Type = DiscountType.Base,
                    MinAmount = 1201,
                    MaxAmount = long.MaxValue,
                    DiscountPercentage = 15,
                    IsActive = true,
                    Description = "15% discount for amounts above MYR 1200",
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new DiscountSetting
                {
                    Id = 6,
                    Type = DiscountType.PrimeNumber,
                    MinAmount = 501,
                    MaxAmount = long.MaxValue,
                    DiscountPercentage = 8,
                    IsActive = true,
                    Description = "Additional 8% discount for prime numbers above MYR 500",
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new DiscountSetting
                {
                    Id = 7,
                    Type = DiscountType.EndsWith5,
                    MinAmount = 901,
                    MaxAmount = long.MaxValue,
                    DiscountPercentage = 10,
                    IsActive = true,
                    Description = "Additional 10% discount for amounts ending in 5 above MYR 900",
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );
        }
    }
} 