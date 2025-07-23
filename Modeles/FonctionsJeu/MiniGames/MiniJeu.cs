using System.Drawing;
using static Modeles.Extensions;

namespace Modeles.FonctionsJeu.MiniGames;

public abstract class MiniJeu()
{

    public abstract void Jouer();

    public abstract void Jouer(out Dictionary<string, int> result);

    public abstract void Jouer(out string result);

    public abstract void Jouer(out int result);

    //timing

    public List<StringColorise>? Barre { get; set; }
    public List<string>? Joueur { get; set; }
    public int? IndexJoueur { get; set; }
    public bool? InputPressed { get; set; }

    //memory

    public List<List<string>>? ObjetsLists { get; set; }
    public List<List<bool>>? Trouve { get; set; }
    public Dictionary<string, int>? Compteur { get; set; }
    public int? ActionsRestante { get; set; } 
    public int? Choix { get; set; } 
    public int? PremierCoup { get; set; }

    //esquive
    public List<List<StringColorise>>? Plateau { get; set; }
    public List<(bool x, int index)>? Attaques { get; set; }
    public (int x, int y) PosJoueur { get; set; }
    public Color? CouleurCaseJoueur{ get; set; }
    public bool? Vivant { get; set; }
    public int? Vague { get; set; }
    public int? Difficulte { get; set; }
}