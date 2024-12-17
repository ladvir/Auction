using Auction.Models;
using Auction.Repositories;

namespace Auction.Services;

public class AuctionService(IAuctionRepository repository)
{
    public async Task<bool> PlaceBid(Bid bid)
    {
        var currentItem = await repository.GetCurrentAuctionItemAsync() ?? new AuctionItem();

        if (DateTime.Now < currentItem.AuctionStart || DateTime.Now > currentItem.AuctionEnd)
        {
            throw new InvalidOperationException("Aukce není aktivní");
        }
        
        var currentBids = await repository.GetBidsForItemAsync(currentItem.Id);
        var highestBid = currentBids.Any()? currentBids.Max(b => b.BidAmount): 0;

        if (bid.BidAmount <= highestBid)
        {
            throw new InvalidOperationException("Příhoz musí být vyšší než aktuální nejvyšší nabídka");
        }

        await repository.PlaceBidAsync(bid);
        
        return true;
    }

    public async Task<List<AuctionItem>> GetAuctionItems()
    {
        return await repository.GetCurrentAuctionItemsAsync();
    }

    public async Task Reset(int auctionItemId)
    {
        await repository.Reset(auctionItemId);

    }
}