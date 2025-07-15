using Microsoft.AspNetCore.Mvc;
using Modeles;
using Modeles.Capacites;
using Modeles.Character;
using Modeles.Character.Personnage;

namespace api.Controllers;

[ApiController]
[Route("api/equipe")]
public class EquipeController(ILogger<EquipeController> logger) : Controller
{
    private readonly ILogger<EquipeController> _logger = logger;

    [HttpGet]
    public List<Entite> Get()
    {
        List<Entite> equipe = [new Chevalier(), new Sorcier(), new Barbare()];
        return equipe;
    }

    [HttpGet("id")]
    public List<Extensions.StringColorise> Get(string id)
    {
        var frappe = new Sacrifice();
        return frappe.Description.SplitEveryNth(10);
    }
}