using Microsoft.AspNetCore.Mvc;

namespace KorogodovMapApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SportTypeController : ControllerBase
{
    ApplicationContext db;

    public SportTypeController(ApplicationContext context)
    {
        db = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var sportTypes = db.SportTypes;
        return Ok(sportTypes);
    }
}
