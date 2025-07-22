using Microsoft.AspNetCore.Mvc;

using Modeles.Character;
using Modeles.Character.Personnage;
using Modeles.FonctionsJeu.FonctionsJeu;
using Modeles.MoveSet.Capacites;

namespace api.Controllers;

[ApiController]
[Route("api/equipe")]
public class EquipeController(ILogger<EquipeController> logger) : Controller
{
    private readonly ILogger<EquipeController> _logger = logger;
 
    [HttpGet]
    public Expedition Get()
    {
        List<Entite> equipe =
        [
            new Chevalier
            {
                Capacites = { new Frappe(), new AttaqueZone(), new VolPv() }
            }, 
            new Sorcier
            {
                Capacites = { new Soin(), new Frappe(), new Energie() }
            },
            new Barbare
            {
                Capacites = { new Frappe(), new MultiCoup(), new Sacrifice() }
            }
        ];
        return new Expedition
        {
            Equipe = equipe
        };
    }
}