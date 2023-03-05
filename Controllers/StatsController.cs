using Microsoft.AspNetCore.Mvc;
using KorogodovMapApp.Models;

namespace KorogodovMapApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : ControllerBase
    {
        ApplicationContext db;

        public StatsController(ApplicationContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var sportObjects = db.SportObjects;
            return Ok(sportObjects);
        }
    }
}
