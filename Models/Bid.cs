using System.ComponentModel.DataAnnotations;

namespace Auction.Models;

public class Bid
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Jméno je povinné")]
    [StringLength(100, ErrorMessage = "Jméno nesmí být delší než 100 znaků")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Příjmení je povinné")]
    [StringLength(100, ErrorMessage = "Příjmení nesmí být delší než 100 znaků")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Email je povinný")]
    [EmailAddress(ErrorMessage = "Zadejte platný email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Telefon je povinný")]
    [Phone(ErrorMessage = "Zadejte platné telefonní číslo")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Částka příhozu je povinná")]
    [Range(1, int.MaxValue, ErrorMessage = "Částka příhozu musí být větší než 0")]
    public int BidAmount { get; set; }

    public int AuctionItemId { get; set; }
    public DateTime BidTime { get; set; }
}