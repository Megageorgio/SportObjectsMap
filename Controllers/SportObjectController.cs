using Microsoft.AspNetCore.Mvc;
using KorogodovMapApp.Models;
using Microsoft.EntityFrameworkCore;

namespace KorogodovMapApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SportObjectController : ControllerBase
    {
        ApplicationContext db;

        public SportObjectController(ApplicationContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var sportObjects = db.SportObjects
                .Include(sportObject => sportObject.SportObjectType)
                .Include(sportObject => sportObject.SportTypes);
            return Ok(sportObjects);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var sportObject = db.SportObjects.Find(id);
            if (sportObject == null)
            {
                return BadRequest();
            }

            return Ok(sportObject);
        }
    }
}
