
    public class PrintPaymentResponse
    {
        public string BillNumber { get; set; } = string.Empty;
        public string BillMonth { get; set; }= string.Empty;
        public string BillAmount { get; set; }= string.Empty;
        public string Surcharge { get; set; }= string.Empty;
        public string PartialPayment { get; set; }= string.Empty;
        public string WithholdingTaxes { get; set; }= string.Empty;
        public string TotalAmount { get; set; }= string.Empty;
        public string Interest { get; set; }= string.Empty;
        public string Vat { get; set; }= string.Empty;
        public string WithholdingTax5 { get; set; }= string.Empty;
        public string WithholdingTax2 { get; set; }= string.Empty;
        public string BillMonthInWords { get; set; }= string.Empty;
    }

    public class PrintResponse
    {
        public string AccountNumberWithDash { get; set; }= string.Empty;
        public string AccountNumber { get; set; }= string.Empty;
        public string ConsumerName { get; set; }= string.Empty;
        public string ConsumerAddress { get; set; }= string.Empty;
        public string MeterNumber { get; set; }= string.Empty;
        public string ConsumerType { get; set; }= string.Empty;
        public string ServerDate { get; set; }= string.Empty;
        public string ServerTime { get; set; }= string.Empty;
        public string TotalVat { get; set; }= string.Empty;
        public string TotalWithholdingTax { get; set; }= string.Empty;
        public string TotalAmount { get; set; }= string.Empty;
        public string TotalDiscount { get; set; }= string.Empty;
        public string TotalPartialPayment { get; set; }= string.Empty;
        public string TotalSurcharge { get; set; }= string.Empty;
        public string TotalInterest { get; set; }= string.Empty;
        public string TotalBillsAmount { get; set; }= string.Empty;
        public string TotalBottomBillAmount { get; set; }= string.Empty;
        public List<PrintPaymentResponse> Payments { get; set; } = null!;
    }