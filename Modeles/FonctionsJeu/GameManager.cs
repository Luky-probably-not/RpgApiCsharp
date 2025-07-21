using Modeles.Capacites;
using Modeles.Character;
using Modeles.Character.Ennemie;
using Modeles.FonctionsJeu.MiniGames;
using Modeles.LabyrintheLogique;
using Modeles.Objets;
using static Modeles.FonctionsJeu.LabyrintheJeu;
namespace Modeles.FonctionsJeu;

public class GameManager
{
    private static GameManager? _instance = null;

    public Expedition expedition;
    public Ecran Ecran;
    private int DernierNiveau = 10;

    private GameManager()
    {
        expedition = new() { Equipe = [] };
        Ennemies = [];
        OrdreAction = [];
        Ecran = new Ecran();
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

    public void Setup(Expedition expe)
    {
        expedition = expe;
    }

    public async Task Debut(int tailleLaby)
    {
        var laby = await AppelsApi.GetLabyrinthe(tailleLaby);
        var expe = await AppelsApi.GetExpedition();
        if (laby == null || expe == null)
            Environment.Exit(1);
        Setup(expe);
        Play(laby);

    }

    public void Play(Labyrinthe laby)
    {
        var arrive = false;
        while (!arrive)
        {
            laby.Display();
            arrive = Deplacement(laby, out var cell);
            switch (cell)
            {
                case "E":
                    Combat().GetAwaiter().GetResult();
                    break;
                case "S":
                    expedition.Pieces += 500;
                    Achats();
                    break;
                case "M":
                    var rand = new Random();
                    Dictionary<string, int> recompense;
                    if (rand.NextDouble() > 1)
                    {
                        AppelsApi.GetMemory(DernierNiveau).GetAwaiter().GetResult().Jouer(out recompense);
                    }
                    else
                    {
                        var jeu = new TimingMiniGame();
                        jeu.Jouer(out string res);
                        recompense = AppelsApi.GetRecompenseTiming(DernierNiveau, res).GetAwaiter().GetResult();
                    }
                    foreach (var kvp in recompense.Where(kvp => kvp.Value != 0))
                    {
                        Console.WriteLine("Butin : {0} x{1}", Objet.ObjetParNom(kvp.Key).Nom, kvp.Value);
                    }
                    expedition.Recompense(recompense);
                    Console.ReadKey();
                    break;
            }
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

    public void Achats()
    {
        var magasin = AppelsApi.GetMagasin(DernierNiveau*500).GetAwaiter().GetResult();
        var choix = 0;
        while (true)
        {
            magasin.Afficher(choix);
            choix = ChoixMagasin(choix, out var touche);
            if (touche == ConsoleKey.Escape)
                break;
            if (touche != ConsoleKey.Spacebar)
                continue;

            var objet = magasin.Objets[choix];
            var achatValide = magasin.Offres.TryGetValue(objet, out var cout);
            if (!achatValide)
                continue;
            if (cout > expedition.Pieces)
                continue;

            achatValide = magasin.Stock[objet] > 0;
            if (!achatValide)
                continue;
            expedition.Pieces -= magasin.Offres[objet];
            expedition.Sac[objet]++;
            magasin.Stock[objet] -= 1;
        }
    }

    public static int ChoixMagasin(int choix, out ConsoleKey touche)
    {
        touche = Console.ReadKey().Key;
        List<ConsoleKey> toucheValide =
        [
            ConsoleKey.LeftArrow,
            ConsoleKey.RightArrow,
            ConsoleKey.UpArrow,
            ConsoleKey.DownArrow,
            ConsoleKey.Spacebar,
            ConsoleKey.Escape
        ];
        while (!toucheValide.Contains(touche)) 
            touche = Console.ReadKey().Key;
        choix += touche switch
        {
            ConsoleKey.RightArrow => 1,
            ConsoleKey.UpArrow => -2,
            ConsoleKey.DownArrow => 2,
            ConsoleKey.LeftArrow => -1,
            _ => 0
        };
        if (choix < 0) choix += 6;
        if (choix > 5) choix -= 6;
        return choix;
    }

    public async Task Combat()
    {
        Ennemies = (await AppelsApi.GetEnnemies(DernierNiveau))!;
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
            Ecran.ChoixAction = "";
            Ecran.ChoixCapacite = 0;
            Ecran.Afficher();
            TourEnnemie();

            Agir();
            
        }
        MajEcran();
        Ecran.Afficher();
        expedition.Equipe.ForEach(e => e.FinCombat(Ennemies.Sum(f => f.Niveau)*500));
        DernierNiveau += 5;
        expedition.Recompense(await AppelsApi.GetRecompenses(Ennemies.Sum(e => e.Niveau)));
    }

    public void Agir()
    {
        while (expedition.Equipe.Contains(OrdreAction[0]))
        {
            var choix = ChoixAction();
            List<Entite> cibles;
            switch (choix)
            {
                case 0:
                    MajEcran();
                    Ecran.ChoixAction = nameof(Capacite);
                    Ecran.ChoixCapacite = 0;
                    Ecran.Afficher();
                    var cap = ChoixCapacite();
                    if (cap == null)
                        continue;
                    cibles = ChoixCible(cap);
                    var doublexp = false;
                    if (cap.Zone)
                    {
                        cap.Utiliser(expedition.Equipe.Find(e => e == OrdreAction[0])!, cibles);
                        var vivant = cibles.Where(e => e.Vivant);
                        if (vivant.Count() != cibles.Count)
                            doublexp = true;

                    }
                    else
                    {
                        cap.Utiliser(expedition.Equipe.Find(e => e == OrdreAction[0])!, cibles[0]);

                        doublexp = !cibles[0].Vivant;
                    }
                    expedition.Equipe.Find(e => e == OrdreAction[0])!.FinTour(doublexp);
                    return; 

                case 1:
                    MajEcran();
                    Ecran.ChoixAction = nameof(Objet);
                    Ecran.ChoixObjet = 0;
                    Ecran.Afficher();
                    var obj = ChoixObjet();
                    if (obj == null)
                        continue;
                    cibles = ChoixCible(obj);
                    if (obj.Zone)
                        obj.Utiliser(cibles);
                    else
                        obj.Utiliser(cibles[0]);
                    expedition.Equipe.Find(e => e == OrdreAction[0])!.FinTour(false);
                    return;
                case 2:
                    expedition.Equipe.Find(e => e == OrdreAction[0])!.FinTour(false);
                    return;
            }
        }
    }

    private bool VerifierVivant()
    {
        var vivantEnnemie = Ennemies.Where(e => e.Vivant);
        var vivantEquipe = expedition.Equipe.Where(e => e.Vivant);
        return vivantEquipe.Any() && vivantEnnemie.Any();
    }

    private void TourEnnemie()
    {
        if (!Ennemies.Contains(OrdreAction[0])) return;
        Thread.Sleep(1000);
        var entite = OrdreAction[0];
        var cap = (entite as IAction)!.ChoisirCapacite();
        var rand = new Random();
        List<Entite> cibleValide;
        switch (cap)
        {
            case { Zone: true }:
                cap.Utiliser(entite, expedition.Equipe);
                break;
            case { Zone: false, Allie: true }:
                cibleValide = [.. Ennemies.Where(e => e.Vivant)];
                cap.Utiliser(entite, cibleValide[rand.Next(cibleValide.Count)]);
                break;
            case { Zone: false, Allie: false }:
                cibleValide = [.. expedition.Equipe.Where(e => e.Vivant)];
                cap.Utiliser(entite, cibleValide[rand.Next(cibleValide.Count)]);
                break;
        }
        Thread.Sleep(1000);
        Ennemies.Find(e => e == OrdreAction[0])!.FinTour(false);
    }

    public int ChoixAction()
    {
        var choix = 0;
        List<ConsoleKey> toucheValide =
        [
            ConsoleKey.LeftArrow,
            ConsoleKey.RightArrow,
            ConsoleKey.Spacebar
        ];
        ConsoleKey touche = ConsoleKey.A;
        MajEcran();
        Ecran.ChoixAction = "";
        Ecran.Afficher();
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
            Ecran.ChoixCapacite = choix;
            Ecran.Afficher();
        }
        return choix;
    }

    public Objet? ChoixObjet()
    {
        var choix = 0;
        List<ConsoleKey> toucheValide =
        [
            ConsoleKey.LeftArrow,
            ConsoleKey.RightArrow,
            ConsoleKey.Spacebar,
            ConsoleKey.Escape
        ];
        var touche = ConsoleKey.A;
        var objetPossede = false;
        while ((touche != ConsoleKey.Spacebar && touche != ConsoleKey.Escape) || !objetPossede)
        {
            touche = Console.ReadKey().Key;
            if (!toucheValide.Contains(touche)) continue;
            choix += touche switch
            {
                ConsoleKey.RightArrow => 1,
                ConsoleKey.LeftArrow => -1,
                _ => 0
            };
            if (choix > 5) choix -= 6;
            if (choix < 0) choix += 6;
            MajEcran();
            Ecran.ChoixObjet = choix;
            Ecran.ChoixAction = nameof(Objet);
            objetPossede = expedition.Sac[Expedition.IndexObjet[choix]] > 0;
            if (touche == ConsoleKey.Escape)
                break;
            Ecran.Afficher();
        }

        return touche == ConsoleKey.Escape ? null : Expedition.IndexObjet[choix] switch
        {
            nameof(PotionSoin) => new PotionSoin(),
            nameof(PotionEnergie) => new PotionEnergie(),
            nameof(AttaqueBoost) => new AttaqueBoost(),
            nameof(DefenseBoost) => new DefenseBoost(),
            nameof(PotionDoubleDegats) => new PotionDoubleDegats(),
            nameof(PotionReductionDegats) => new PotionReductionDegats(),
            _ => throw new NotImplementedException()
        };
    }

    public Capacite? ChoixCapacite()
    {
        var choix = 0;
        List<ConsoleKey> toucheValide =
        [
            ConsoleKey.LeftArrow,
            ConsoleKey.RightArrow,
            ConsoleKey.Spacebar,
            ConsoleKey.Escape
        ];
        var touche = ConsoleKey.A;
        var energie = false;
        while ((touche != ConsoleKey.Spacebar && touche != ConsoleKey.Escape) || !energie )
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
            Ecran.ChoixCapacite = choix;
            Ecran.ChoixAction = nameof(Capacite);
            energie = OrdreAction[0].PointAction - OrdreAction[0].Capacites[choix].Cout >= 0;
            Ecran.Afficher();
        }
        return touche == ConsoleKey.Escape ? null : OrdreAction[0].Capacites[choix];
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
        var touche = ConsoleKey.A;
        var cibleVivante = false;
        while (touche != ConsoleKey.Spacebar || !cibleVivante)
        {
            if (choix > 2) choix -= 3;
            if (choix < 0) choix += 3;
            MajEcran();
            Ecran.Cibles = cap switch
            {
                { Allie: true, Zone: true } => expedition.Equipe,
                { Allie: true, Zone: false } => [expedition.Equipe[choix]],
                { Allie: false, Zone: true } => Ennemies,
                { Allie: false, Zone: false } => [Ennemies[choix]],
                _ => Ecran.Cibles
            };
            cibleVivante = Ecran.Cibles.Any(e => e.Vivant);
            Ecran.ChoixAction = nameof(Capacite);
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

    public List<Entite> ChoixCible(Objet obj)
    {
        var choix = 0;
        List<ConsoleKey> toucheValide =
        [
            ConsoleKey.LeftArrow,
            ConsoleKey.RightArrow,
            ConsoleKey.Spacebar
        ];
        var touche = ConsoleKey.A;
        while (touche != ConsoleKey.Spacebar)
        {
            if (choix > 2) choix -= 3;
            if (choix < 0) choix += 3;
            MajEcran();
            Ecran.Cibles = obj.Zone ? expedition.Equipe : [expedition.Equipe[choix]];
            Ecran.ChoixAction = nameof(Objet);
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

    public void MajEcran()
    {
        Ecran =  new Ecran()
        {
            Expedition = expedition,
            Ennemies = Ennemies,
            Ordre = OrdreAction,
        };
    }


}

