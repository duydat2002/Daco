namespace Daco.Domain.Wallets.Aggregates
{
    public class BuyerTopupRequest : AggregateRoot
    {
        public Guid        UserId               { get; private set; }
        public Guid        WalletId             { get; private set; }
        public string      TopupCode            { get; private set; }
        public decimal     Amount               { get; private set; }
        public decimal     Fee                  { get; private set; }
        public decimal     NetAmount            { get; private set; }
        public TopupMethod Method               { get; private set; }
        public string?     PaymentGateway       { get; private set; }
        public string?     GatewayTransactionId { get; private set; }
        public string?     GatewayOrderId       { get; private set; }
        public string?     GatewayResponse      { get; private set; }
        public string?     GatewayCallbackData  { get; private set; }
        public string?     BankName             { get; private set; }
        public string?     BankAccountNumber    { get; private set; }
        public string?     TransferReference    { get; private set; }
        public string?     TransferProofUrl     { get; private set; }
        public TopupStatus Status               { get; private set; }
        public DateTime    ExpiresAt            { get; private set; }
        public DateTime?   CreatedAt            { get; private set; }
        public DateTime?   UpdatedAt            { get; private set; }
        public DateTime?   CompletedAt          { get; private set; }
        public DateTime?   FailedAt             { get; private set; }
        public string?     FailureReason        { get; private set; }
        public string?     UserNote             { get; private set; }
        public string?     AdminNote            { get; private set; }

        protected BuyerTopupRequest() { }
    }
}
