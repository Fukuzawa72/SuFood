using Microsoft.AspNetCore.Mvc;
using SuFood.Models;
using SuFood.ViewModel;

namespace SuFood.Controllers
{
	public class CouponController : Controller
	{
		private readonly SuFoodDBContext _context;

		public CouponController(SuFoodDBContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Coupon()
		{
			return View();
		}

		[HttpGet]
		public async Task<IEnumerable<VmCouponForFront>> GetCoupons()
		{
			return _context.Coupon.Select(x => new VmCouponForFront
			{
				CouponId = x.CouponId,
				CouponName = x.CouponName,
				CouponDescription = x.CouponDescription,
				CouponEndDate = x.CouponEndDate,
			});
		}
	}
}
