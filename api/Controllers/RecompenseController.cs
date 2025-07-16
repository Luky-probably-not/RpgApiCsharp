using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/loot")]
public class RecompenseController(ILogger<RecompenseController> logger) : Controller
{
    private readonly ILogger<RecompenseController> _logger = logger;

    [HttpGet("{sommeNiveau}")]
    public int Get(int sommeNiveau)
    {
        return sommeNiveau;
    }
}