using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;

namespace SuFood.Controllers
{
    public class HelpChoiceController : Controller
    {
        private readonly SuFoodDBContext _context;

        public HelpChoiceController(SuFoodDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult HelpChoice()
        {
            return View();
        }

        [HttpGet]
        public async Task<IEnumerable<VmProductForFront>> GetProducts()
        {
            return _context.Products.Select(x => new VmProductForFront
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Price = x.Price,
                Img = x.Img,
                Quantity = 0
            });
        }

    }
}