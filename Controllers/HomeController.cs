using CDB.Data;
using CDB.Models;
using CDB.Models.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CDB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DrawingContext _dbContext;

        public HomeController(ILogger<HomeController> logger, DrawingContext dbContext)
        {
            _logger = logger;
            _dbContext= dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _dbContext.Drawings.ToListAsync();
            ViewBag.drawingList= data;
            return View();
        }
    }
}
