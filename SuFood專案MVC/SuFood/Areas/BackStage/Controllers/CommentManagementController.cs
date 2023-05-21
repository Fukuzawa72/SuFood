using Microsoft.AspNetCore.Mvc;
using SuFood.Models;

namespace SuFood.Areas.BackStage.Controllers
{
	[Area("BackStage")]
	public class CommentManagementController : Controller
	{
		private readonly SuFoodDBContext _context;
		public CommentManagementController(SuFoodDBContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult CommentManagementPage()
		{
			return View();
		}
	}
}
