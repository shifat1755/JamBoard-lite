using Microsoft.AspNetCore.Mvc;
using CDB.Data;
using CDB.Models.Drawing;
using CDB.Services;
namespace CDB.Controllers
{
    public class DrawingController : Controller
    {
        private readonly IDrawingService _drawingServcie;
        private readonly DrawingContext _dbContext;

        public DrawingController(DrawingContext dbContext, IDrawingService drawingService)
        {
            _drawingServcie= drawingService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string name)
        {
            var data = await _drawingServcie.GetDrawing(name);
            ViewData["prevData"]= data;
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            var response=await _drawingServcie.Create(model);

            return RedirectToAction("Index","Home");
        }

    }
}
