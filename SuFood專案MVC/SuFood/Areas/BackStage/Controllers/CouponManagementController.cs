using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;

namespace SuFood.Areas.BackStage.Controllers
{
	[Area("BackStage")]
	public class CouponManagementController : Controller
	{
		private readonly SuFoodDBContext _context;

		public CouponManagementController(SuFoodDBContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}
		public IActionResult CouponManagement()
		{
			return View();
		}

		//Get
		[HttpGet]
		public async Task<IEnumerable<VmCoupon>> GetCoupons()
		{
			return _context.Coupon.Select(c => new VmCoupon
			{
				CouponId = c.CouponId,
				CouponName = c.CouponName,
				CouponDescription = c.CouponDescription,
				CouponMinusCost = (int?)c.CouponMinusCost,
				MinimumPurchasingAmount = c.MinimumPurchasingAmount,
				Couponstartdate2String = c.CouponStartDate.ToString().Substring(0,10),
				Couponenddate2String = c.CouponEndDate.ToString().Substring(0,10),
			});
		}

		public async Task<JsonResult> GetCouponItem()
		{
			return Json(_context.Coupon);
		}

		//Create
		[HttpPost]
		//[Bind("CouponId,CouponName,CouponDescription,CouponMinusCost,MinimumPurchasingAmount,CouponStartDate,CouponEndDate")]
		public async Task<string> Create([FromBody] Coupon coupons)
		{
			if(ModelState.IsValid)
			{
				_context.Add(coupons);
				await _context.SaveChangesAsync();
				return "新增成功";
			}
			//var errorMessages = ModelState
			//	.Where(abc => abc.Value.ValidationState == ModelValidationState.Invalid)
			//	.SelectMany(abc => abc.Value.Errors)
			//	.Select(e => e.ErrorMessage).ToList();
			return "新增失敗";
		}

		//Edit
		[HttpPut]
		public async Task<string> Edit([FromBody] Coupon coupons)
		{
			if(ModelState.IsValid)
			{
				try
				{
					_context.Update(coupons);
					await _context.SaveChangesAsync();
				}
				catch(DbUpdateConcurrencyException)
				{
					if (!CouponsExists(coupons.CouponId))
					{
						return "修改資料庫失敗";
					}
					else
					{
						throw;
					}
				}
				return "修改成功";
			}
			return "修改失敗";
		}
		private bool CouponsExists(int id)
		{
			return (_context.Coupon?.Any(c => c.CouponId == id)).GetValueOrDefault();
		}
	}
}
