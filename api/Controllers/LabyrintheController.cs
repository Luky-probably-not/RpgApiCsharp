using Microsoft.AspNetCore.Mvc;
using Modeles.LabyrintheLogique;

namespace api.Controllers;

[ApiController]
[Route("api/labyrinthe")]
public class LabyrintheController(ILogger<LabyrintheController> logger) : Controller
{
    private readonly ILogger<LabyrintheController> _logger = logger;

    [HttpGet("{taille}")]
    public Labyrinthe Get(string taille)
    {
        var laby = new Labyrinthe(int.Parse(taille));
        laby.Generation();
        return laby;
    }
}