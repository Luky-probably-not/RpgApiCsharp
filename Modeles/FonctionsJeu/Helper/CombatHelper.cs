using Modeles.Character;
using Modeles.Character.Ennemie;
using Modeles.FonctionsJeu.FonctionsJeu;
using Modeles.Items;
using Modeles.MoveSet;

namespace Modeles.FonctionsJeu.Helper;

public static class CombatHelper
{
    private static Expedition? _expedition;

    private static List<Entite> _ennemies = [];

    private static List<Entite> _ordreAction = [];

    public static Ecran Ecran { get; private set; }= new();


    public static async Task Combat(int niveau)
    {
        _expedition = GameManager.Instance.Expedition;
        _ennemies = (await AppelsApi.GetEnnemies(niveau))!;
        while (VerifierVivant())
        {
            RecupererOrdreAction();
            _ordreAction[0].PointAction++;
            if (!_ordreAction[0].Vivant)
            {
                if (_expedition.Equipe.Contains(_ordreAction[0]))
                    _expedition.Equipe.Find(e => e == _ordreAction[0])!.ReinitialiserValeurAction();
                else
                    _ennemies.Find(e => e == _ordreAction[0])!.ReinitialiserValeurAction();
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
        _expedition.Equipe.ForEach(e => e.FinCombat(_ennemies.Sum(f => f.Niveau)*30));
        _expedition.Recompense(await AppelsApi.GetRecompenses(_ennemies.Sum(e => e.Niveau)));
        GameManager.Instance.Expedition = _expedition;
    }

    public static void MajEcran()
    {
        Ecran = new Ecran()
        {
            Expedition = _expedition!,
            Ennemies = _ennemies,
            Ordre = _ordreAction,
        };
    }

    private static bool VerifierVivant()
    {
        var vivantEnnemie = _ennemies.Where(e => e.Vivant);
        var vivantEquipe = _expedition!.Equipe.Where(e => e.Vivant);
        return vivantEquipe.Any() && vivantEnnemie.Any();
    }

    private static void RecupererOrdreAction()
    {
        _ordreAction = [.. _expedition!.Equipe, .. _ennemies];
        _ordreAction.Sort((x, y) => x.ValeurAction.CompareTo(y.ValeurAction));
        var valeur = _ordreAction[0].ValeurAction;
        _ordreAction.ForEach(e => e.ValeurAction -= valeur);
    }

    private static void TourEnnemie()
    {
        if (!_ennemies.Contains(_ordreAction[0])) return;
        Thread.Sleep(1000);
        var entite = _ordreAction[0];
        var cap = (entite as IAction)!.ChoisirCapacite();
        var rand = new Random();
        List<Entite> cibleValide;
        switch (cap)
        {
            case { Zone: true }:
                cap.Utiliser(entite, _expedition!.Equipe);
                break;
            case { Zone: false, Allie: true }:
                cibleValide = [.. _ennemies.Where(e => e.Vivant)];
                cap.Utiliser(entite, cibleValide[rand.Next(cibleValide.Count)]);
                break;
            case { Zone: false, Allie: false }:
                cibleValide = [.. _expedition!.Equipe.Where(e => e.Vivant)];
                cap.Utiliser(entite, cibleValide[rand.Next(cibleValide.Count)]);
                break;
        }
        Thread.Sleep(1000);
        _ennemies.Find(e => e == _ordreAction[0])!.FinTour(false);
    }

    private static void Agir()
    {
        Capacite? cap = null;
        Objet? obj = null;
        var choix = -1;
        while (_expedition!.Equipe.Contains(_ordreAction[0]))
        {
            choix =  choix == -1 ? ChoixAction(cap != null, obj != null) : choix;
            List<Entite>? cibles;
            switch (choix)
            {
                case 0:
                    MajEcran();
                    Ecran.ChoixAction = nameof(Capacite);
                    Ecran.ChoixCapacite = 0;
                    Ecran.Afficher();
                    cap ??= ChoixCapacite();
                    if (cap == null)
                    {
                        choix = -1;
                        continue;
                    }

                    if (cap.ToutLeMonde)
                    {
                        cap.Utiliser(_expedition.Equipe.Find(e => e == _ordreAction[0])!, _expedition.Equipe, _ennemies);
                        _expedition.Equipe.Find(e => e == _ordreAction[0])!.FinTour(false);
                        return;

                    }
                    cibles = ChoixCible(cap);
                    if (cibles == null)
                    {
                        cap = null;
                        continue;
                    }
                    var doublexp = false;
                    if (cap.Zone)
                    {
                        cap.Utiliser(_expedition.Equipe.Find(e => e == _ordreAction[0])!, cibles);
                        var vivant = cibles.Where(e => e.Vivant);
                        if (vivant.Count() != cibles.Count)
                            doublexp = true;

                    }
                    else
                    {
                        cap.Utiliser(_expedition.Equipe.Find(e => e == _ordreAction[0])!, cibles[0]);

                        doublexp = !cibles[0].Vivant;
                    }
                    _expedition.Equipe.Find(e => e == _ordreAction[0])!.FinTour(doublexp);
                    return;

                case 1:
                    MajEcran();
                    Ecran.ChoixAction = nameof(Objet);
                    Ecran.ChoixObjet = 0;
                    Ecran.Afficher();
                    obj ??= ChoixObjet();
                    if (obj == null)
                    {
                        choix = -1;
                        continue;
                    }

                    cibles = ChoixCible(obj);
                    if (cibles == null)
                    {
                        obj = null;
                        continue;
                    }
                    if (obj.Zone)
                        obj.Utiliser(cibles);
                    else
                        obj.Utiliser(cibles[0]);
                    _expedition.Equipe.Find(e => e == _ordreAction[0])!.FinTour(false);
                    return;
                case 2:
                    _expedition.Equipe.Find(e => e == _ordreAction[0])!.FinTour(false);
                    return;
            }
        }
    }

    private static int ChoixAction(bool cap, bool obj)
    {
        var choix = 0;
        List<ConsoleKey> toucheValide =
        [
            ConsoleKey.LeftArrow,
            ConsoleKey.RightArrow,
            ConsoleKey.Spacebar
        ];
        var touche = ConsoleKey.A;
        MajEcran();
        Ecran.ChoixAction = cap ? nameof(Capacite) : obj ? nameof(Objet) : "";
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

    private static Capacite? ChoixCapacite()
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
        while (touche != ConsoleKey.Spacebar && touche != ConsoleKey.Escape || !energie)
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
            energie = _ordreAction[0].PointAction - _ordreAction[0].Capacites[choix].Cout >= 0;
            Ecran.Afficher();
        }
        return touche == ConsoleKey.Escape ? null : _ordreAction[0].Capacites[choix];
    }

    private static Objet? ChoixObjet()
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
        while (touche != ConsoleKey.Spacebar && touche != ConsoleKey.Escape || !objetPossede)
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
            objetPossede = _expedition!.Sac[Expedition.IndexObjet[choix]] > 0;
            if (touche == ConsoleKey.Escape)
                break;
            Ecran.Afficher();
        }

        return touche == ConsoleKey.Escape ? null : Objet.ObjetParNom(Expedition.IndexObjet[choix]);
    }

    private static List<Entite>? ChoixCible(Capacite cap)
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
        var cibleVivante = false;
        while ((touche != ConsoleKey.Spacebar && touche != ConsoleKey.Escape) || !cibleVivante)
        {
            MajEcran();
            Ecran.Cibles = cap switch
            {
                { Allie: true, Zone: true } => _expedition!.Equipe,
                { Allie: true, Zone: false } => [_expedition!.Equipe[choix]],
                { Allie: false, Zone: true } => _ennemies,
                { Allie: false, Zone: false } => [_ennemies[choix]],
                _ => Ecran.Cibles
            };
            cibleVivante = Ecran.Cibles.Any(e => e.Vivant);
            Ecran.ChoixAction = nameof(Capacite);
            Ecran.Afficher();
            
            touche = Console.ReadKey().Key;
            if (!toucheValide.Contains(touche)) continue;
            if (touche == ConsoleKey.Escape) break;

            choix += touche switch
            {
                ConsoleKey.RightArrow => 1,
                ConsoleKey.LeftArrow => -1,
                _ => 0
            };

            if (choix > 2) choix -= 3;
            if (choix < 0) choix += 3;

        }
        return touche == ConsoleKey.Escape ? null : Ecran.Cibles;
    }

    private static List<Entite>? ChoixCible(Objet obj)
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
        while (touche != ConsoleKey.Spacebar && touche != ConsoleKey.Escape)
        {
            MajEcran();
            Ecran.Cibles = obj.Zone ? _expedition!.Equipe : [_expedition!.Equipe[choix]];
            Ecran.ChoixAction = nameof(Objet);
            Ecran.Afficher();

            touche = Console.ReadKey().Key;
            if (!toucheValide.Contains(touche)) continue;
            if (touche == ConsoleKey.Escape) break;

            choix += touche switch
            {
                ConsoleKey.RightArrow => 1,
                ConsoleKey.LeftArrow => -1,
                _ => 0
            };

            if (choix > 2) choix -= 3;
            if (choix < 0) choix += 3;

        }

        return touche == ConsoleKey.Escape ? null : Ecran.Cibles;
    }
}