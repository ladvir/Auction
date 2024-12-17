using Auction.Models;
using Microsoft.EntityFrameworkCore;

namespace Auction.Database;
    public class AuctionDbContext(DbContextOptions<AuctionDbContext> options) : DbContext(options)
    {
        public DbSet<AuctionItem> AuctionItems { get; set; }
        public DbSet<Bid> Bids { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuctionItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.StartingPrice);
                entity.Property(e => e.CurrentPrice);
            });

            modelBuilder.Entity<Bid>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                entity.Property(e => e.BidAmount);

                entity.HasOne<AuctionItem>()
                    .WithMany()
                    .HasForeignKey(b => b.AuctionItemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
