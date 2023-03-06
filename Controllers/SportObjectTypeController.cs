using Microsoft.AspNetCore.Mvc;

namespace KorogodovMapApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SportObjectTypeController : ControllerBase
{
    ApplicationContext db;

    public SportObjectTypeController(ApplicationContext context)
    {
        db = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var sportObjectTypes = db.SportObjectTypes;
        return Ok(sportObjectTypes);
    }
}
