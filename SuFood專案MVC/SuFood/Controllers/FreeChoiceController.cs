﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;
using System.Numerics;

namespace SuFood.Controllers
{
    public class FreeChoiceController : Controller
    {
        private readonly SuFoodDBContext _context;

        public FreeChoiceController(SuFoodDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult FreeChoice()
        {
            return View();
        }

        //GET: /FreeChoice/GetProducts (所有商品的檢視)
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






        //這裡開始是方案與方案產品的新刪修查。

        //GET: /FreeChoice/GetPlansAndProducts (自由選方案與每個方案內的商品檢視) !!互相關聯的寫法
        //public object GetPlansAndProducts()
        //{
        //    return _context.FreeChoicePlans.Include(plan => plan.ProductsOfPlans).Select(p => new
        //    {
        //        plans = new VmFreeChoicePlans
        //        {
        //            PlanId = p.PlanId,
        //            PlanName = p.PlanName,
        //            PlanDescription = p.PlanDescription,
        //            PlanPrice = p.PlanPrice,
        //            PlanCanChoiceCount = p.PlanCanChoiceCount,
        //            PlanStatus = p.PlanStatus,
        //            PlanTotalCount = p.PlanTotalCount
        //        },
        //        products = p.ProductsOfPlans.Select(product => new VmProductForFront
        //        {
        //            ProductId = product.ProductId,
        //            ProductName = product,
        //            Price = product.Price,
        //            Img = product.Img,
        //            Quantity = 0
        //        })
        //    });
        //}

        //GET: /FreeChoice/GetPlansAndProducts 取得方案與方案內的商品 !!有三張表的寫法
        public Object GetPlansAndProducts()
        {
            return _context.ProductsOfPlans.GroupBy(p => p.PlanId).Select(group => new
            {
                plans = group.Select(p => new VmFreeChoicePlans
                {
                    PlanId = p.Plan.PlanId,
                    PlanName = p.Plan.PlanName,
                    PlanDescription = p.Plan.PlanDescription,
                    PlanPrice = p.Plan.PlanPrice,
                    PlanTotalCount = p.Plan.PlanTotalCount,
                    PlanCanChoiceCount = p.Plan.PlanCanChoiceCount,
                    PlanStatus = p.Plan.PlanStatus,
                }).SingleOrDefault(),
                Products = group.Select(p => new VmProductForFront
                {
                    ProductId = p.Product.ProductId,
                    ProductName = p.Product.ProductName,
                    Price = p.Product.Price,
                    Img = p.Product.Img,
                    Quantity = 0
                })
            });
        }

        //POST: /FreeChoice/DeleteProductsOfPlans 刪除方案中的產品(單筆)
        [HttpDelete]
        public async Task<string> DeleteProductsOfPlans(int PlanId, int ProductId)
        {
            if(_context.ProductsOfPlans == null)
            {
                return "資料不存在";
            }

            var product = await _context.ProductsOfPlans.FindAsync(PlanId, ProductId);
            if(product != null)
            {
                _context.ProductsOfPlans.Remove(product);
            }
            await _context.SaveChangesAsync();
            return "刪除成功";
        }

        //POST: /FreeChoice/CreateProductsOfPlan 新增多筆方案中的產品
        [HttpPost]
        public async Task<string> CreateProductsOfPlan(List<ProductsOfPlans> products)
        {
            foreach (var product in products)
            {
                if (!_context.FreeChoicePlans.Any(p => p.PlanId == product.PlanId))
                {
                    return "不存在該方案";
                }

                ProductsOfPlans insert = new ProductsOfPlans
                {
                    PlanId = product.PlanId,
                    ProductId = product.ProductId
                };

                _context.ProductsOfPlans.Add(insert);
            }

            await _context.SaveChangesAsync();
            return "新增成功";
        }

        //[HttpDelete]
        //public string test(int id)
        //{
        //    var delete = _context.ProductsOfPlans.Where(x => x.PlanId == id).Include(c => c.Product).SingleOrDefault();
        //    _context.ProductsOfPlans.Remove(delete);
        //    _context.SaveChanges();
        //}
    }
}
