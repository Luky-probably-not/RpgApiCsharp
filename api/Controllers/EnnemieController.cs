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

    [HttpGet("{sommeNiveau}")]
    public List<Entite> Get(int sommeNiveau)
    {
        List<string> enemiesPossible = [nameof(Zombie), nameof(Gobelin), nameof(Maudit), nameof(Slime)];
        var rand = new Random();
        var niveau = Niveau(sommeNiveau);
        var result = new List<Entite>();
        for (var i = 0; i < 3; i++)
        {
            var entite = Entite.EntiteParNom(enemiesPossible[rand.Next(enemiesPossible.Count)]);
            entite.Niveau = niveau[i];
            entite.Capacites = [
                new Frappe(), new AttaqueZone()
            ];
            entite.PointAction = 2;
            result.Add(entite);
        }
        return result;
    }

    private static List<int> Niveau(int niveau)
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
        return [niveau - list.Max() - list.Min(), list.Max(), list.Min()];
    }
}