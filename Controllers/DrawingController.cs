using Microsoft.AspNetCore.Mvc;
using CDB.Data;
using Microsoft.EntityFrameworkCore;
namespace CDB.Controllers
{
    public class DrawingController : Controller
    {
        private readonly DrawingContext _dbContext;

        public DrawingController(DrawingContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult>List()
        {
            var drawingList = await _dbContext.Drawings.ToListAsync();
            if (drawingList != null)
            {
                return View(drawingList);

            }
            else
            {
                return View();
            }
        }

    }
}
