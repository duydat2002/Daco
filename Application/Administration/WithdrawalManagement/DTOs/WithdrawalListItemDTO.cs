namespace Daco.Application.Administration.WithdrawalManagement.DTOs
{
    public class WithdrawalListItemDTO
    {
        public Guid      Id                { get; set; }
        public Guid      SellerId          { get; set; }
        public string    SellerUsername    { get; set; } = null!;
        public string?   SellerEmail       { get; set; }
        public decimal   Amount            { get; set; }
        public decimal   Fee               { get; set; }
        public decimal   NetAmount         { get; set; }
        public string    BankName          { get; set; } = null!;
        public string    BankAccountNumber { get; set; } = null!;
        public string    BankAccountName   { get; set; } = null!;
        public string    Status            { get; set; } = null!;
        public string?   TransactionCode   { get; set; }
        public string?   SellerNote        { get; set; }
        public string?   AdminNote         { get; set; }
        public DateTime? ApprovedAt        { get; set; }
        public DateTime? CompletedAt       { get; set; }
        public DateTime  CreatedAt         { get; set; }
    }
}
