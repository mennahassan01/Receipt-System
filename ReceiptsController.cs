using Assesment.Data;
using Assesment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    public class ReceiptsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReceiptsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Receipts/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Items = await _context.Items.ToListAsync();
            return View(new Receipt());
        }

        // POST: Receipts/AddItem
        [HttpPost]
        public async Task<IActionResult> AddItem(int itemId, int quantity)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item == null)
            {
                return NotFound();
            }

            if (quantity > item.Quantity)
            {
                TempData["ErrorMessage"] = "The selected quantity is not available.";
                return RedirectToAction(nameof(Create));
            }

            var receipt = await GetCurrentReceipt();
            if (receipt == null)
            {
                receipt = new Receipt();
                _context.Receipts.Add(receipt);
                await _context.SaveChangesAsync();
            }

            var receiptItem = new ReceiptItem
            {
                Item = item,
                Quantity = quantity,
                Price = item.Price
            };

            receipt.ReceiptItems.Add(receiptItem);
            receipt.TotalCost += receiptItem.Price * quantity;

            _context.Update(receipt);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Item added successfully.";
            return RedirectToAction(nameof(Create));
        }

        // POST: Receipts/Pay
        [HttpPost]
        public async Task<IActionResult> Pay(decimal paidAmount)
        {
            var receipt = await GetCurrentReceipt();
            if (receipt == null)
            {
                return NotFound();
            }

            if (paidAmount < receipt.TotalCost)
            {
                TempData["ErrorMessage"] = "Transaction Failed. The paid amount is less than the total cost.";
                return RedirectToAction(nameof(Create));
            }

            receipt.PaidAmount = paidAmount;
            receipt.Remaining = paidAmount - receipt.TotalCost;

            _context.Update(receipt);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Payment successfully completed!";
            return RedirectToAction(nameof(Create));
        }

        private async Task<Receipt> GetCurrentReceipt()
        {
            var receiptId = HttpContext.Session.GetInt32("ReceiptId");
            if (receiptId.HasValue)
            {
                return await _context.Receipts
                    .Include(r => r.ReceiptItems)
                    .ThenInclude(ri => ri.Item)
                    .FirstOrDefaultAsync(r => r.ReceiptId == receiptId);
            }
            else
            {
                return null;
            }
        }
    }
}
