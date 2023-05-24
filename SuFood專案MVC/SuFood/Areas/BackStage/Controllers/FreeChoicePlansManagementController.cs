using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;

namespace SuFood.Areas.BackStage.Controllers
{
    [Area("BackStage")]
    public class FreeChoicePlansManagementController : Controller
    {
        private readonly SuFoodDBContext _context;

        public FreeChoicePlansManagementController(SuFoodDBContext context)
        {
            _context = context;
        }

        //GET: /BackStage/FreeChoicePlansManagement/GetPlansAndProducts 取得方案與方案內的商品 !!有三張表的寫法
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
                Products = group.Select(p => new
                {
                    ProductId = p.Product.ProductId,
                    ProductName = p.Product.ProductName,
                    Price = p.Product.Price,
                })
            });
        }

        //GET: /BackStage/FreeChoicePlansManagement/GetAllProducts 取得所有商品
        [HttpGet]
        public async Task<IEnumerable<object>> GetAllProducts()
        {
            return _context.Products.Select(pro => new
            {
                ProductId = pro.ProductId,
                ProductName = pro.ProductName,
                Price = pro.Price,
            });
        }

        //DELETE: /FreeChoicePlansManagement/DeleteProductsOfPlans 刪除方案中的產品(單筆)
        [HttpDelete]
        public async Task<string> DeleteProductsOfPlans(int PlanId, int ProductId)
        {
            if (_context.ProductsOfPlans == null)
            {
                return "資料不存在";
            }

            var product = await _context.ProductsOfPlans.FindAsync(PlanId, ProductId);
            if (product != null)
            {
                _context.ProductsOfPlans.Remove(product);
            }
            await _context.SaveChangesAsync();
            return "刪除成功";
        }

        [HttpPost]
        public async Task<string> CreatePlans([FromBody] VmFreeChoicePlans vmParameters)
        {
            FreeChoicePlans fcp = new FreeChoicePlans
            {
                PlanId= vmParameters.PlanId,
                PlanName= vmParameters.PlanName,
                PlanDescription= vmParameters.PlanDescription,
                PlanPrice= vmParameters.PlanPrice,
                PlanCanChoiceCount= vmParameters.PlanCanChoiceCount,
                PlanTotalCount= vmParameters.PlanTotalCount,
                PlanStatus= vmParameters.PlanStatus,
                ProductsOfPlans= vmParameters.ProductsOfPlans,
            };

            _context.FreeChoicePlans.Add(fcp);
            await _context.SaveChangesAsync();
            return "新增方案成功";
        }

        //新增ProdcutOfPlans資料表
        [HttpPost]
        public async Task<string> CreateProductsOfPlan([FromBody] List<VmProductsOfPlans> vmParameters)
        {
            foreach (var vmParameter in vmParameters)
            {
                if (!_context.FreeChoicePlans.Any(p => p.PlanId == vmParameter.PlanId))
                {
                    return "不存在該方案";
                }

                // 檢查是否已存在相同的記錄
                if (_context.ProductsOfPlans.Any(pop => pop.PlanId == vmParameter.PlanId && pop.ProductId == vmParameter.ProductId))
                {
                    return $"重複的主鍵：PlanId = {vmParameter.PlanId}, ProductId = {vmParameter.ProductId}";
                }

                ProductsOfPlans pop = new ProductsOfPlans
                {
                    ProductId = vmParameter.ProductId,
                    PlanId = vmParameter.PlanId,
                };

                _context.ProductsOfPlans.Add(pop);
            }

            await _context.SaveChangesAsync();

            return "新增成功";
        }
    }
}
