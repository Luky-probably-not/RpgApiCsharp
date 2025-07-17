using Modeles.Capacites;
using Modeles.Character;
using Modeles.Character.Ennemie;
using Modeles.LabyrintheLogique;
using static Modeles.FonctionsJeu.LabyrintheJeu;
namespace Modeles.FonctionsJeu;

public class GameManager
{
    private static GameManager? _instance = null;

    private Expedition expedition;
    private Ecran Ecran;

    private GameManager()
    {
        Ennemies = [];
        OrdreAction = [];
        Ecran = new Ecran
        {
            equipe = [],
            ennemies = [],
            ordre = []
        };
    }

    public static GameManager Instance
    {
        get
        {
            _instance ??= new GameManager();
            return _instance;
        }
    }

    public List<Entite> Ennemies { get; set; }
    public List<Entite> OrdreAction { get; set; }

    public void Setup(List<Entite> equipe, List<Entite> ennemies)
    {
        expedition = new()
        {
            Equipe = equipe,
        };
        Ennemies = ennemies;
        RecupererOrdreAction();
    }

    public async Task Debut(int tailleLaby)
    {
        var laby = await AppelsApi.GetLabyrinthe(tailleLaby);
        var equipe = await AppelsApi.GetEquipe();
        var ennemies = await AppelsApi.GetEnnemies();
        if (laby == null || equipe == null || ennemies == null)
            Environment.Exit(1);
        Setup(equipe, ennemies);
        Play(laby);

    }

    public void Play(Labyrinthe laby)
    {
        var arrive = false;
        while (!arrive)
        {
            laby.Display();
            Console.WriteLine(expedition.Pieces + " pieces");
            arrive = Deplacement(laby, out var cell);
            if (cell == "E")
                Combat().GetAwaiter().GetResult();
        }
        laby.Display();
    }

    public void RecupererOrdreAction()
    {
        OrdreAction = [.. expedition.Equipe, .. Ennemies];
        OrdreAction.Sort((x, y) => x.ValeurAction.CompareTo(y.ValeurAction));
        var valeur = OrdreAction[0].ValeurAction;
        OrdreAction.ForEach(e => e.ValeurAction -= valeur);
    }

    public async Task Combat()
    {
        Ennemies = (await AppelsApi.GetEnnemies())!;
        while (VerifierVivant())
        {
            RecupererOrdreAction();
            OrdreAction[0].PointAction += 1;
            if (!OrdreAction[0].Vivant)
            {
                if (expedition.Equipe.Contains(OrdreAction[0]))
                    expedition.Equipe.Find(e => e == OrdreAction[0])!.ReinitialiserValeurAction();
                else
                    Ennemies.Find(e => e == OrdreAction[0])!.ReinitialiserValeurAction();
                continue;
            }
            MajEcran();
            Ecran.Afficher();
            if (Ennemies.Contains(OrdreAction[0]))
            {
                Thread.Sleep(1000);
                TourEnnemie();
                Ennemies.Find(e => e == OrdreAction[0])!.FinTour();
                Thread.Sleep(1000);
                continue;
            }
            var cap = ChoixCapacite();
            var cibles = ChoixCible(cap);
            if (cap.Zone)
                cap.Utiliser(expedition.Equipe.Find(e => e == OrdreAction[0])!, cibles);
            else
                cap.Utiliser(expedition.Equipe.Find(e => e == OrdreAction[0])!, cibles[0]);
            expedition.Equipe.Find(e => e == OrdreAction[0])!.FinTour();
        }
        MajEcran();
        Ecran.Afficher();
        expedition.Equipe.ForEach(e => e.FinCombat());
        expedition.Pieces += await AppelsApi.GetRecompenses(Ennemies.Sum(e => e.Niveau));
    }

    private bool VerifierVivant()
    {
        var vivantEnnemie = Ennemies.Where(e => e.Vivant);
        var vivantEquipe = expedition.Equipe.Where(e => e.Vivant);
        return vivantEquipe.Any() && vivantEnnemie.Any();
    }

    private void TourEnnemie()
    {
        var entite = OrdreAction[0];
        var cap = (entite as IAction)!.ChoisirCapacite();
        var rand = new Random();
        switch (cap)
        {
            case { Zone: true }:
                cap.Utiliser(entite, expedition.Equipe);
                break;
            case { Zone: false, Allie: true }:
                cap.Utiliser(entite, Ennemies[rand.Next(Ennemies.Count)]);
                break;
            case { Zone: false, Allie: false }:
                cap.Utiliser(entite, expedition.Equipe[rand.Next(expedition.Equipe.Count)]);
                break;
        }
    }

    public Capacite ChoixCapacite()
    {
        var choix = 0;
        List<ConsoleKey> toucheValide =
        [
            ConsoleKey.LeftArrow,
            ConsoleKey.RightArrow,
            ConsoleKey.Spacebar
        ];
        ConsoleKey touche = ConsoleKey.A;
        while (touche != ConsoleKey.Spacebar)
        {
            touche = Console.ReadKey().Key;
            if (!toucheValide.Contains(touche)) continue;
            choix += touche switch
            {
                ConsoleKey.RightArrow => 1,
                ConsoleKey.LeftArrow => -1,
                _ => 0
            };
            if (choix > 2) choix -= 3;
            if (choix < 0) choix += 3;
            MajEcran();
            Ecran.ChoixAction = choix;
            Ecran.Afficher();
        }
        return OrdreAction[0].Capacites[choix];
    }

    public List<Entite> ChoixCible(Capacite cap)
    {
        var choix = 0;
        List<ConsoleKey> toucheValide =
        [
            ConsoleKey.LeftArrow,
            ConsoleKey.RightArrow,
            ConsoleKey.Spacebar
        ];
        ConsoleKey touche = ConsoleKey.A;
        while (touche != ConsoleKey.Spacebar)
        {
            if (choix > 2) choix -= 3;
            if (choix < 0) choix += 3;
            MajEcran();
            switch (cap)
            {
                case { Allie: true, Zone: true }:
                    Ecran.Cibles = expedition.Equipe;
                    break;
                case { Allie: true, Zone: false }:
                    Ecran.Cibles = [expedition.Equipe[choix]];
                    break;
                case { Allie: false, Zone: true }:
                    Ecran.Cibles = Ennemies;
                    break;
                case { Allie: false, Zone: false }:
                    Ecran.Cibles = [Ennemies[choix]];
                    break;
            }
            Ecran.Afficher();
            touche = Console.ReadKey().Key;
            if (!toucheValide.Contains(touche)) continue;
            choix += touche switch
            {
                ConsoleKey.RightArrow => 1,
                ConsoleKey.LeftArrow => -1,
                _ => 0
            };
        }
        return Ecran.Cibles;
    }

    private void MajEcran()
    {
        Ecran =  new Ecran()
        {
            equipe = expedition.Equipe,
            ennemies = Ennemies,
            ordre = OrdreAction
        };
    }


}

