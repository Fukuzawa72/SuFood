using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            return _context.Products.Select(p => new VmRetailList
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                Category = p.Category,
                Img = p.Img
            });
        }

        [HttpGet]
        public async Task<IEnumerable<VmRetailList>> SortProducts(string category)
        {
            var categoryProducts = await _context.Products
                .Where(p => p.Category == category)
                .Select(p => new VmRetailList
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Img = p.Img,                                      
                })
                .ToListAsync();

            return categoryProducts;
        }



        [HttpGet]
        public async Task<IEnumerable<VmRetailList>> GetCategories()
        {
            var GroupCategories = _context.Products
                .GroupBy(p => p.Category)
                .Select(p => new VmRetailList { Category = p.Key, CountCategory = p.Count() });
            return GroupCategories;
        }
        
    }
}
