using Microsoft.AspNetCore.Mvc;
using SuFood.Models;
using SuFood.ViewModel;

namespace SuFood.Controllers
{
    public class RetailController : Controller
    {
        private readonly SuFoodDBContext _context;

        public RetailController(SuFoodDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Retail()
        {
            return View();
        }

        [HttpGet]
        public async Task<IEnumerable<VmRetailList>> GetRetial()
        {
            return  _context.Products.Select(p => new VmRetailList
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                Category = p.Category
            });             
        }
        [HttpGet]
        public async Task<IEnumerable<VmRetailList>> GetCategories()
        {
            return _context.Products.Select(p => new VmRetailList { Category = p.Category, });
        }
    }
}
