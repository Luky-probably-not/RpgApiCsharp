using Microsoft.AspNetCore.Mvc;
using Modeles.FonctionsJeu.MiniGames;
using Modeles.Objets;

namespace api.Controllers;

[ApiController]
[Route("/api/minijeu")]
public class MiniJeuController(ILogger<MiniJeuController> logger) : Controller
{
    private readonly ILogger<MiniJeuController> _logger = logger;

    [HttpGet("{sommeNiveau:int}")]
    public MiniJeu? Get(int sommeNiveau)
    {
        List<string> jeux = ["memory", "timing"];
        var rand = new Random();

        return jeux[rand.Next(jeux.Count)] switch
        {
            "memory" => GetMemory(sommeNiveau),
            "timing" => GetTiming(),
            _ => null,
        };
    }

    private static Memory GetMemory(int sommeNiveau)
    {
        var rand = new Random();

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
        var kvp = quantite.ElementAt(rand.Next(quantite.Count));
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

    private static TimingMiniGame GetTiming()
    {
        return new TimingMiniGame();
    }
}
