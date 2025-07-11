using Modeles.Character;
using Modeles.Character.Ennemie;
using Modeles.GameManager;
using Modeles.LabyrintheLogique;
using static Modeles.GameManager.LabyrintheJeu;
namespace Jeu;

public class GameManager : ICombat
{
    private static GameManager? _instance = null;

    private GameManager()
    {
        Equipe = [];
        Ennemies = [];
        OrdreAction = [];
    }

    public static GameManager Instance
    {
        get
        {
            _instance ??= new GameManager();
            return _instance;
        }
    }

    public List<Entite> Equipe { get; set ; }
    public List<Entite> Ennemies { get; set; }
    public List<Entite> OrdreAction { get; set; }

    public void Setup(List<Entite> equipe, List<Entite> ennemies)
    {
        Equipe = equipe;
        Ennemies = ennemies;
        RecupererOrdreAction();
    }

    public async Task Debut(int tailleLaby)
    {
        var laby = await AppelsApi.GetLabyrinthe(tailleLaby);
        var equipe = await AppelsApi.GetEquipe();
        if (laby == null || equipe == null)
            Environment.Exit(1);
        Setup(equipe, [new Zombie(), new Zombie(), new Zombie()]);
        Play(laby);

    }

    public void Play(Labyrinthe laby)
    {
        PlayLabyrinthe(laby);
        AfficherCombat();
    }

    public void RecupererOrdreAction()
    {
        OrdreAction = [.. Equipe, .. Ennemies];
        OrdreAction.Sort((x, y) => x.ValeurAction.CompareTo(y.ValeurAction));
        var valeur = OrdreAction[0].ValeurAction;
        OrdreAction.ForEach(e => e.ValeurAction -= valeur);
    }

    public void AfficherCombat()
    {
        RecupererOrdreAction();
        new Ecran()
        {
            equipe = Equipe,
            ennemies = Ennemies,
            ordre = OrdreAction
        }.Afficher();
    }
}