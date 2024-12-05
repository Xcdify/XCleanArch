namespace XMP.Application.DTOs
{
    public class AxisBankTransactionDto
    {
        public int Id { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? ValueDate { get; set; }
        public string? ChequeNumber { get; set; }
        public string? TransactionDetails { get; set; }
        public decimal AmountTransferred { get; set; }
        public string? TypeOfTransaction { get; set; }
        public decimal BalanceAmount { get; set; }
        public string? BankBranchName { get; set; }
        public string? BankName { get; set; }
        public string? CompanyName { get; set; }
        public string? ReceiverName { get; set; }
        public string? TransactionCategory { get; set; }
        public string? TypeOfTax { get; set; }
        public float TaxPercentage { get; set; }


        // Additional properties for DTO use, if needed
        // public string AdditionalProperty { get; set; }
    }
}
