using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KorogodovMapApp.Controllers;

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
    public IActionResult GetAll([FromQuery] string? sportObjectType, [FromQuery] string? sportType)
    {
        var sportObjects = db.SportObjects
            .Include(sportObject => sportObject.SportObjectType)
            .Include(sportObject => sportObject.SportTypes)
            .AsEnumerable();

        if (!string.IsNullOrEmpty(sportObjectType))
        {
            sportObjects = sportObjects
                .Where(sportObject => sportObject.SportObjectType.Name == sportObjectType);
        }

        if (!string.IsNullOrEmpty(sportType))
        {
            sportObjects = sportObjects
                .Where(sportObject => sportObject.SportTypes.Any(type => type.Name == sportType));
        }

        return Ok(sportObjects);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var sportObject = db.SportObjects
            .Include(sportObject => sportObject.SportObjectType)
            .Include(sportObject => sportObject.SportTypes)
            .Include(sportObject => sportObject.SportObjectDetail)
            .Include(sportObject => sportObject.Curator)
            .FirstOrDefault(sportObject => sportObject.Id == id);

        if (sportObject == null)
        {
            return BadRequest();
        }

        return Ok(sportObject);
    }
}
