using Auction.Database;
using Auction.Models;
using Microsoft.EntityFrameworkCore;

namespace Auction.Repositories;

public interface IAuctionRepository
{
    Task<AuctionItem?> GetCurrentAuctionItemAsync();
    
    Task<List<AuctionItem>> GetCurrentAuctionItemsAsync();
    Task<List<Bid>> GetBidsForItemAsync(int itemId);
    Task PlaceBidAsync(Bid bid);
    Task<bool> ValidateBidAsync(Bid bid);

    Task Reset(int auctionItemId);
}


    public class AuctionRepository(IDbContextFactory<AuctionDbContext> dbContextFactory) : IAuctionRepository
    {
        public async Task<AuctionItem?> GetCurrentAuctionItemAsync()
        {
            await using var context = await dbContextFactory.CreateDbContextAsync();
            return await context.AuctionItems
                .Where(item =>
                    DateTime.Now >= item.AuctionStart && 
                    DateTime.Now <= item.AuctionEnd)
                .FirstOrDefaultAsync();
        }

        public async Task<List<AuctionItem>> GetCurrentAuctionItemsAsync()
        {
            await using var context = await dbContextFactory.CreateDbContextAsync();
            return await context.AuctionItems
                .Where(item =>
                    DateTime.Now >= item.AuctionStart && 
                    DateTime.Now <= item.AuctionEnd)
                .ToListAsync();
        }

        public async Task<List<Bid>> GetBidsForItemAsync(int itemId)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync();
            return await context.Bids
                .Where(b => b.AuctionItemId == itemId)
                .OrderByDescending(b => b.BidAmount)
                .ToListAsync();
        }

        public async Task PlaceBidAsync(Bid bid)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync();
            
            bid.BidTime = DateTime.Now;

            context.Bids.Add(bid);

            
            var auctionItem = await context.AuctionItems
                .FindAsync(bid.AuctionItemId);
        
            if (auctionItem != null)
            {
                auctionItem.CurrentPrice = bid.BidAmount;
            }
            
            await context.SaveChangesAsync();
        }

        public async Task<bool> ValidateBidAsync(Bid bid)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync();
            var currentItem = await GetCurrentAuctionItemAsync();

            if (currentItem==null) return true;
            
            var highestBid = await context.Bids
                .Where(b => b.AuctionItemId == currentItem.Id)
                .MaxAsync(b => (decimal?)b.BidAmount) ?? currentItem.StartingPrice;

            return bid.BidAmount > highestBid;
        }
        
        public async Task Reset(int auctionItemId)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync();
            
            var auctionItem = await context.AuctionItems
                .FindAsync(auctionItemId);
        
            if (auctionItem != null)
            {
                auctionItem.CurrentPrice = 0;
            }
            
            var bids = await GetBidsForItemAsync(auctionItemId);

            context.Bids.RemoveRange(bids);
            
            await context.SaveChangesAsync();
        }
    }

