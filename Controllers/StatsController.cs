using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KorogodovMapApp.Controllers;

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
    public IActionResult GetStats()
    {
        var objectBuildingStat = db.SportObjectDetails
            .Where(detail => detail.ActionEndDate.HasValue && !detail.IsReconstruction)
            .Select(detail => detail.ActionEndDate.Value.Year)
            .GroupBy(year => year)
            .Select(g => new { Value = g.Key, Count = g.Count() })
            .OrderBy(x => x.Value);

        var objectReconstructingStat = db.SportObjectDetails
            .Where(detail => detail.ActionEndDate.HasValue && detail.IsReconstruction)
            .Select(detail => detail.ActionEndDate.Value.Year)
            .GroupBy(year => year)
            .Select(g => new { Value = g.Key, Count = g.Count() })
            .OrderBy(x => x.Value);

        var objectTypeStat = db.SportObjectTypes
            .Include(objectType => objectType.SportObjects)
            .Select(objectType => new { Value = objectType.Name, Count = objectType.SportObjects.Count })
            .OrderByDescending(x => x.Count)
            .Take(10);

        var sportTypeStat = db.SportTypes
            .Include(objectType => objectType.SportObjects)
            .Select(objectType => new { Value = objectType.Name, Count = objectType.SportObjects.Count })
            .OrderByDescending(x => x.Count)
            .Take(15);

        return Ok(new
        {
            objectBuildingStat,
            objectReconstructingStat,
            objectTypeStat,
            sportTypeStat
        });
    }
}
