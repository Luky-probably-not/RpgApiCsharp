using Microsoft.AspNetCore.Mvc;
using Modeles.FonctionsJeu.FonctionsJeu;

namespace api.Controllers;

[ApiController]
[Route("api/magasin")]
public class MagasinController(ILogger<MagasinController> logger) : Controller
{
    private readonly ILogger<MagasinController> _logger = logger;

    [HttpGet("{sommeNiveau:int}")]
    public Magasin Get(int sommeNiveau)
    {
        return Magasin.Generer(sommeNiveau);
    }
}