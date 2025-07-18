using Microsoft.AspNetCore.Mvc;
using Modeles.Capacites;
using Modeles.Character;
using Modeles.Character.Ennemie;

namespace api.Controllers;

[ApiController]
[Route("api/ennemie")]
public class EnnemieController(ILogger<EnnemieController> logger) : Controller
{
    private readonly ILogger<EnnemieController> _logger = logger;

    [HttpGet]
    public List<Entite> Get()
    {
        List<Entite> ennemies =
        [
            new Zombie()
            {
                Niveau = 5,
                Capacites = { new Frappe(), new Soin() }
            },
            new Zombie()
            {
                Niveau = 5,
                Capacites = { new Frappe(), new Soin() }
            },
            new Zombie()
            {
                Niveau = 5,
                Capacites = { new Frappe(), new Soin() }
            }
        ];
        return ennemies;
    }
}