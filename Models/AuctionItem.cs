namespace Auction.Models;

public class AuctionItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int StartingPrice { get; set; }
    public int CurrentPrice { get; set; }
    public DateTime AuctionStart { get; set; }
    public DateTime AuctionEnd { get; set; }
}