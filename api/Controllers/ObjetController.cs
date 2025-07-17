using Microsoft.AspNetCore.Mvc;
using Modeles.Objets;

namespace api.Controllers;

[ApiController]
[Route("api/objets")]
public class ObjetController(ILogger<ObjetController> logger) : Controller
{
    private readonly ILogger<ObjetController> _logger = logger;

    [HttpGet]
    public List<Objet> Get()
    {
        List<Objet> list =
        [
            new PotionDoubleDegats(),
            new PotionEnergie(),
            new PotionSoin(),
            new AttaqueBoost(),
            new DefenseBoost(),
            new PotionReductionDegats()
        ];
        var result = new List<Objet>();
        var rand = new Random();

        for (var i = 0; i < 3; i++)
        {
            
            result.Add(list[rand.Next(list.Count)]);
        }

        return result;
    }

}