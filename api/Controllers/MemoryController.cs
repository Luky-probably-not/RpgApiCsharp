using Microsoft.AspNetCore.Mvc;
using Modeles.FonctionsJeu.MiniGames;
using Modeles.Objets;

namespace api.Controllers;

[ApiController]
[Route("api/memory")]
public class MemoryController(ILogger<MemoryController> logger) : Controller
{
    private readonly ILogger<MemoryController> _logger = logger;

    [HttpGet("{sommeNiveau:int}")]
    public Memory Get(int sommeNiveau)
    {
        var rand = new Random();

        KeyValuePair<string, int> kvp;
        var quantite = new Dictionary<string, int>()
        {
            { nameof(PotionSoin), 1 },
            { nameof(PotionEnergie), 1 },
            { nameof(AttaqueBoost), 1 },
            { nameof(DefenseBoost), 1 },
            { nameof(PotionDoubleDegats), 1 },
            { nameof(PotionReductionDegats), 1 },
        };
        if (sommeNiveau >= 50) return new Memory(quantite);

        quantite.Remove(nameof(PotionDoubleDegats));
        quantite.Remove(nameof(PotionReductionDegats));
        kvp = quantite.ElementAt(rand.Next(quantite.Count));
        quantite[kvp.Key]++;
        kvp = quantite.ElementAt(rand.Next(quantite.Count));
        quantite[kvp.Key]++;

        if (sommeNiveau >= 20) return new Memory(quantite);

        var total = quantite[nameof(AttaqueBoost)] + quantite[nameof(DefenseBoost)];
        quantite.Remove(nameof(AttaqueBoost));
        quantite.Remove(nameof(DefenseBoost));

        for (var i = 0; i < total; i++)
        {
            kvp = quantite.ElementAt(rand.Next(quantite.Count));
            quantite[kvp.Key]++;
        }
        return new Memory(quantite);
    }

}