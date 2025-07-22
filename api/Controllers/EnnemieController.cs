using Microsoft.AspNetCore.Mvc;
using Modeles.Character;
using Modeles.Character.Ennemie;
using Modeles.MoveSet.Capacites;

namespace api.Controllers;

[ApiController]
[Route("api/ennemie")]
public class EnnemieController(ILogger<EnnemieController> logger) : Controller
{
    private readonly ILogger<EnnemieController> _logger = logger;

    [HttpGet("{niveau}")]
    public List<Entite> Get(int niveau)
    {
        int nv1;
        int nv2;
        int nv3;
        while (true)
        {
            nv1 = new Random().Next(niveau / 4, niveau / 2);
            nv2 = new Random().Next(niveau / 4, niveau / 2);
            nv3 = niveau - nv1 - nv2;
            if (nv3 >= niveau / 4 && nv3 <= niveau / 2)
                break;
        }
        var list = new[] { nv1, nv2, nv3 };
        var max = list.Max();
        var min = list.Min();
        var last = niveau - max - min;
        List<Entite> ennemies =
        [
            new Zombie
            {
                Niveau = last,
                Capacites = { new Frappe(), new Soin() }
            },
            new Zombie
            {
                Niveau = max,
                Capacites = { new Frappe(), new Soin() }
            },
            new Zombie
            {
                Niveau = min,
                Capacites = { new Frappe(), new Soin() }
            }
        ];
        return ennemies;
    }
}