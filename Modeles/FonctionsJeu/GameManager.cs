using static Modeles.FonctionsJeu.Helper.LabyHelper;
using static Modeles.FonctionsJeu.Helper.MagasinHelper;
using static Modeles.FonctionsJeu.Helper.MiniJeuHelper;
using static Modeles.FonctionsJeu.Helper.CombatHelper;
using Modeles.FonctionsJeu.FonctionsJeu;
namespace Modeles.FonctionsJeu;

public class GameManager
{
    private static GameManager? _instance = null;

    public Expedition Expedition;
    private int _niveau = 4;

    private GameManager()
    {
        Expedition = new() { Equipe = [] };
    }

    public static GameManager Instance
    {
        get
        {
            _instance ??= new GameManager();
            return _instance;
        }
    }

    public async Task Debut(int tailleLaby)
    {
        var laby = await AppelsApi.GetLabyrinthe(tailleLaby);
        var expe = await AppelsApi.GetExpedition();
        if (laby == null || expe == null)
            Environment.Exit(1);
        Laby = laby;
        Expedition = expe;
        Expedition.Equipe.ForEach(e => e.PointDeVie -= 40);
        Play();

    }

    public void Play()
    {
        var arrive = false;
        while (!arrive)
        {
            LabyAffichage();
            arrive = LabyDeplacement(out var cell);
            switch (cell)
            {
                case "E":
                    Combat(_niveau).GetAwaiter().GetResult();
                    _niveau += 5;
                    break;
                case "S":
                    Magasin(_niveau).GetAwaiter().GetResult();
                    break;
                case "M":
                    MiniJeu(_niveau).GetAwaiter().GetResult();
                    break;
            }
        }
        LabyAffichage();
    }
}

