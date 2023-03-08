namespace IMS.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public double InvoiceAmount { get; set; }
        public string InvoiceMonth { get; set; }
        public string InvoiceOwner { get; set; }
        public string InvoiceCreatorId { get; set; }
        public InvoiceStatus Status { get; set; }
    }

    public enum InvoiceStatus
    {
        SUBMITTED,
        APPROVED,
        REJECTED
    }
}