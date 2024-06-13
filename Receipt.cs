using System;
using System.Collections.Generic;

namespace Assesment.Models
{
    public class Receipt
    {
        public int ReceiptId { get; set; }
        public decimal TotalCost { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Remaining { get; set; }
        public ICollection<ReceiptItem> ReceiptItems { get; set; }
    }
}
