using Microsoft.AspNetCore.Mvc;
using Modeles.Objets;

namespace api.Controllers;

[ApiController]
[Route("api/loot")]
public class RecompenseController(ILogger<RecompenseController> logger) : Controller
{
    private readonly ILogger<RecompenseController> _logger = logger;

    [HttpGet("{sommeNiveau:int}")]
    public Dictionary<string, int> Get(int sommeNiveau)
    {

        var result = new Dictionary<string, int>
        {
            { "Pieces", sommeNiveau },
            { nameof(PotionSoin), sommeNiveau < 10 ? 0 : Objet.RandomAmount(sommeNiveau)},
            { nameof(PotionEnergie), sommeNiveau < 10 ? 0 : Objet.RandomAmount(sommeNiveau)},
            { nameof(AttaqueBoost), sommeNiveau < 50 ? 0 : Objet.RandomAmount(sommeNiveau-50)},
            { nameof(DefenseBoost), sommeNiveau < 50 ? 0 : Objet.RandomAmount(sommeNiveau-50)},
            { nameof(PotionDoubleDegats), sommeNiveau < 100 ? 0 : Objet.RandomAmount(sommeNiveau-100)},
            { nameof(PotionReductionDegats), sommeNiveau < 100 ? 0 : Objet.RandomAmount(sommeNiveau-100)},
        };
        return result;
    }
}
