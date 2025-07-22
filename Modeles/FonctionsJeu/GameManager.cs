using Modeles.LabyrintheLogique;
using static Modeles.FonctionsJeu.Helper.LabyHelper;
using static Modeles.FonctionsJeu.Helper.MagasinHelper;
using static Modeles.FonctionsJeu.Helper.MiniJeuHelper;
using static Modeles.FonctionsJeu.Helper.CombatHelper;
using Modeles.FonctionsJeu.Helper;
namespace Modeles.FonctionsJeu;

public class GameManager
{
    private static GameManager? _instance = null;

    public Expedition Expedition;
    private int Niveau = 10;

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
                    Combat(Niveau).GetAwaiter().GetResult();
                    Niveau += 5;
                    break;
                case "S":
                    Expedition.Pieces += 500;
                    Magasin(Niveau).GetAwaiter().GetResult();
                    break;
                case "M":
                    MiniJeu(Niveau).GetAwaiter().GetResult();
                    break;
            }
        }
        LabyAffichage();
    }
}

