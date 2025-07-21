using Microsoft.AspNetCore.Mvc;
using Modeles.LabyrintheLogique;

namespace api.Controllers;

[ApiController]
[Route("api/labyrinthe")]
public class LabyrintheController(ILogger<LabyrintheController> logger) : Controller
{
    private readonly ILogger<LabyrintheController> _logger = logger;

    [HttpGet("{taille:int}")]
    public Labyrinthe Get(int taille)
    {
        var laby = new Labyrinthe(taille);
        laby.Generation();
        return laby;
    }
}