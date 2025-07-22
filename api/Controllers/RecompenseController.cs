using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Modeles.Items;
using Modeles.Items.Objets;

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

    [HttpGet("{sommeNiveau:int},{couleur}")]
    public Dictionary<string, int> Get(int sommeNiveau, string couleur)
    {
        Dictionary<string, int> recompense = [];
        return sommeNiveau switch
        {
            < 10  when couleur == nameof(Color.White)  => [],
            < 10  when couleur == nameof(Color.Red)    => new() { { nameof(PotionSoin), 1 } },
            < 10  when couleur == nameof(Color.Yellow) => new() { { nameof(PotionSoin), 1 }, { nameof(PotionEnergie), 1 } },
            < 50  when couleur == nameof(Color.White)  => new() { { nameof(PotionSoin), 1 }, { nameof(PotionEnergie), 1 } },
            < 50  when couleur == nameof(Color.Red)    => new() { { nameof(PotionSoin), 2 }, { nameof(PotionEnergie), 2 } },
            < 50  when couleur == nameof(Color.Yellow) => new() { { nameof(PotionSoin), 2 }, { nameof(PotionEnergie), 2 }, { nameof(AttaqueBoost), 1 }, { nameof(DefenseBoost), 1 } },
            < 100 when couleur == nameof(Color.White)  => new() { { nameof(PotionSoin), 2 }, { nameof(PotionEnergie), 2 }, { nameof(AttaqueBoost), 1 }, { nameof(DefenseBoost), 1 } },
            < 100 when couleur == nameof(Color.Red)    => new() { { nameof(PotionSoin), 3 }, { nameof(PotionEnergie), 3 }, { nameof(AttaqueBoost), 2 }, { nameof(DefenseBoost), 2 }, { nameof(PotionReductionDegats), 1 } },
            < 100 when couleur == nameof(Color.Yellow) => new() { { nameof(PotionSoin), 3 }, { nameof(PotionEnergie), 3 }, { nameof(AttaqueBoost), 2 }, { nameof(DefenseBoost), 2 }, { nameof(PotionReductionDegats), 1 }, { nameof(PotionDoubleDegats), 1 } },
            _     when couleur == nameof(Color.White)  => new() { { nameof(PotionSoin), 4 }, { nameof(PotionEnergie), 4 }, { nameof(AttaqueBoost), 3 }, { nameof(DefenseBoost), 3 }, { nameof(PotionReductionDegats), 1 }, { nameof(PotionDoubleDegats), 1 } },
            _     when couleur == nameof(Color.Red)    => new() { { nameof(PotionSoin), 5 }, { nameof(PotionEnergie), 5 }, { nameof(AttaqueBoost), 3 }, { nameof(DefenseBoost), 3 }, { nameof(PotionReductionDegats), 2 }, { nameof(PotionDoubleDegats), 2 } },
            _     when couleur == nameof(Color.Yellow) => new() { { nameof(PotionSoin), 6 }, { nameof(PotionEnergie), 6 }, { nameof(AttaqueBoost), 4 }, { nameof(DefenseBoost), 4 }, { nameof(PotionReductionDegats), 3 }, { nameof(PotionDoubleDegats), 3 } },
            _ => []
        };
    }
}
