namespace Assesment.Models
{
    public class ReceiptItem
    {
        public int ReceiptItemId { get; set; }
        public int ReceiptId { get; set; }
        public Receipt Receipt { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
