using Microsoft.AspNetCore.Mvc;
using Assesment.Data;
using Assesment.Models;
using System.Threading.Tasks;


namespace YourNamespace.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display form to add items
        public IActionResult Index()
        {
            return View();
        }

        // Add item to database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }
    }
}
