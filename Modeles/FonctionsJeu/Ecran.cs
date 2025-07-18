using System.Drawing;
using Modeles.Capacites;
using Modeles.Character;
using Modeles.Objets;
using static Modeles.Extensions;

namespace Modeles.FonctionsJeu;

public class Ecran
{
    private List<List<StringColorise>> _ecran = [[new StringColorise("┌" + string.Concat(Enumerable.Repeat("─", 153)) + "┐")]];

    public Expedition Expedition = new(){Equipe = [] };
    public List<Entite> Ennemies = [];
    public List<Entite> Ordre = [];
    public int ChoixCapacite;
    public int ChoixObjet;
    public string ChoixAction ="";
    public List<Entite> Cibles = [];
    public List<String> Actions = ["Capacites", "Objets", "Passer le tour"];

    public void Afficher()
    {
        Console.Clear();
        _ecran = [[new StringColorise("┌" + new string('─', 152) + "┐")]];
        
        AfficherOrdre();
        AfficherCote(Ennemies,2);
        AfficherCote(Expedition.Equipe,14);
        switch (ChoixAction)
        {
            case nameof(Capacite):
                AfficherCapacite();
                break;
            case nameof(Objet):
                AfficherObjet();
                break;
            case "":
                AfficherAction();
                break;
        }
        _ecran.ForEach(e => Console.WriteLine(FormatterLigne(e)));
    }

    #region ValeurEcran

    private void AfficherAction()
    {
        _ecran.Add([
            new("├"), new(new string('─', 50)), new("┬"), new(new string('─', 50)), new("┬"), new(new string('─', 50)),
            new("┤")
        ]);
        List<StringColorise> ligne = [new("│")];
        for (var a = 0; a < 3; a++)
        {
            if (a == ChoixCapacite)
                ligne.AddRange([new("┌"), new(new string('─', 48)), new("┐")]);
            else
                ligne.AddRange([new(new string(' ', 50))]);
            ligne.Add(new("│"));
        }
        _ecran.Add(ligne);

        ligne = [];
        foreach (var action in Actions)
        {
            if (Actions.IndexOf(action) == ChoixCapacite)
                ligne.AddRange([new("│"), new(MettreAuMilieu(action, 48)), new("│")]);
            else
                ligne.AddRange([new(MettreAuMilieu(action, 50))]);
            ligne.Add(new ("│"));
        }
        AjouterListe(ligne, _ecran.Count, false);
        var length = _ecran.Count;
        for (var b = 0; b < 4; b++)
        {
            ligne = [new("│")];
            for (var a = 0; a < 3; a++)
            {
                if (a == ChoixCapacite)
                    ligne.AddRange([new("│"), new(new string(' ', 48)), new("│")]);
                else
                    ligne.AddRange([new(new string(' ', 50))]);
                ligne.Add(new("│"));
            }
            _ecran.Add(ligne);
        }
        ligne = [new("│")];
        for (var a = 0; a < 3; a++)
        {
            if (a == ChoixCapacite)
                ligne.AddRange([new("└"), new(new string('─', 48)), new("┘")]);
            else
                ligne.AddRange([new(new string(' ', 50))]);
            ligne.Add(new("│"));
        }
        _ecran.Add(ligne);
        _ecran.Add([new("└"), new(new string('─', 50)), new("┴"), new(new string('─', 50)), new("┴"), new(new string('─', 50)), new("┘")]);
    }

    private void AfficherCapacite()
    {
        if (!Expedition.Equipe.Contains(Ordre[0]))
            return;
        _ecran.Add([
            new("├"), new(new string('─', 50)), new("┬"), new(new string('─', 50)), new("┬"), new(new string('─', 50)),
            new("┤")
        ]);
        List<StringColorise> ligne = [new("│")];
        for (var a = 0; a < 3; a++)
        {
            if (a == ChoixCapacite)
                ligne.AddRange([new ("┌"), new (new string('─',48)), new ("┐")]);
            else
                ligne.AddRange([new (new string(' ', 50))]);
            ligne.Add(new ("│"));
        }
        _ecran.Add(ligne);

        List<StringColorise> result = [];
        foreach (var cap in Ordre[0].Capacites)
        {
            if (Ordre[0].Capacites.IndexOf(cap) == ChoixCapacite)
                result.AddRange([new ("│"),new(MettreAuMilieu(cap.Nom, 48)), new ("│")]);
            else
                result.Add(new(MettreAuMilieu(cap.Nom, 50)));
            result.Add(new("│"));
        }
        AjouterListe(result, _ecran.Count, false );
        var length = _ecran.Count;
        foreach (var cap in Ordre[0].Capacites)
        {
            result = [];
            if (Ordre[0].Capacites.IndexOf(cap) == ChoixCapacite)
                result.AddRange([
                    new ("│"),
                    new(new string(' ', 38)),
                    AffichagePointActionCapacite(cap),
                    new ("│")
                    ]);
            else
                result.AddRange([
                    new(new string(' ', 39)),
                    AffichagePointActionCapacite(cap),
                    new (" ")
                    ]);
            result.Add(new("│"));
            AjouterListe(result, length, false);
        }
        AffichageDescriptionCapacite().ForEach(l => AjouterListe(l,_ecran.Count,false));

        ligne = [new("│")];
        for (var a = 0; a < 3; a++)
        {
            if (a == ChoixCapacite)
                ligne.AddRange([new("└"), new(new string('─', 48)), new("┘")]);
            else
                ligne.AddRange([new(new string(' ', 50))]);
            ligne.Add(new("│"));
        }
        _ecran.Add(ligne);
        _ecran.Add([new("└"), new(new string('─', 50)), new("┴"), new(new string('─', 50)), new("┴"), new(new string('─', 50)), new("┘")]);
    }

    private void AfficherObjet()
    {
        if (!Expedition.Equipe.Contains(Ordre[0]))
            return;
        _ecran.Add([
            new("├"), new(new string('─', 50)), new("┬"), new(new string('─', 50)), new("┬"), new(new string('─', 50)),
            new("┤")
        ]);
        List<StringColorise> ligne = [new("│")];
        for (var a = 0; a < 3; a++)
        {
            if (a == (ChoixObjet > 2 ? ChoixObjet - 3 : ChoixObjet))
                ligne.AddRange([new("┌"), new(new string('─', 48)), new("┐")]);
            else
                ligne.Add(new(new string(' ', 50)));
            ligne.Add(new("│"));
        }

        _ecran.Add(ligne);

        ligne = [];
        foreach (var obj in Expedition.IndexObjet.GetRange(ChoixObjet > 2 ? 3 : 0, 3))
        {
            if (Expedition.IndexObjet[ChoixObjet] == obj)
                ligne.AddRange([
                    new("│"),
                    new(MettreAuMilieu(Objet.ObjetParNom(obj).Nom, 48)),
                    new("│")
                ]);
            else
                ligne.Add(new(MettreAuMilieu(Objet.ObjetParNom(obj).Nom, 50)));
            ligne.Add(new("│"));
        }

        AjouterListe(ligne, _ecran.Count, false);
        var length = _ecran.Count;
        foreach (var obj in Expedition.IndexObjet.GetRange(ChoixObjet > 2 ? 3 : 0, 3))
        {
            ligne = [];
            if (Expedition.IndexObjet[ChoixObjet] == obj)
                ligne.AddRange([
                    new("│"),
                    new(new string(' ', 38)),
                    AffichageQuantiteObjet(obj),
                    new("│"),
                ]);
            else
                ligne.AddRange([
                    new(new string(' ', 39)),
                    AffichageQuantiteObjet(obj),
                    new(" ")
                ]);
            ligne.Add(new("│"));
            AjouterListe(ligne, length, false);
        }
        AffichageDescriptionObjet().ForEach(l => AjouterListe(l, _ecran.Count, false));

        ligne = [new("│")];
        for (var a = 0; a < 3; a++)
        {
            if (a == (ChoixObjet > 2 ? ChoixObjet - 3 : ChoixObjet))
                ligne.AddRange([new("└"), new(new string('─', 48)), new("┘")]);
            else
                ligne.Add(new(new string(' ', 50)));
            ligne.Add(new("│"));
        }

        _ecran.Add(ligne);
        _ecran.Add([new("└"), new(new string('─', 50)), new("┴"), new(new string('─', 50)), new("┴"), new(new string('─', 50)), new("┘")]);
    }

    private List<List<StringColorise>> AffichageDescriptionCapacite()
    {
        List<List<StringColorise>> result = [[], [], []];
        foreach (var cap in Ordre[0].Capacites)
        {
            var lignes = cap.SplitEveryNth(30);
            for (var l = 0; l < lignes.Count; l++)
            {
                result[l].AddRange([new (Ordre[0].Capacites.IndexOf(cap) == ChoixCapacite ? "│ " : "  "),new (lignes[l].FirstOrDefault().Value)]);
                result[l].AddRange([new StringColorise(new string(' ', 47 - lignes[l].FirstOrDefault().Key)), new(Ordre[0].Capacites.IndexOf(cap) == ChoixCapacite ? "│" : " "), new  (Ordre[0].Capacites.IndexOf(cap) != 2 ? "│" : " ")]);
            }
        }

        return result;
    }

    private List<List<StringColorise>> AffichageDescriptionObjet()
    {
        List<List<StringColorise>> result = [[], [], []];
        foreach (var obj in Expedition.IndexObjet.GetRange(ChoixObjet > 2 ? 3 : 0, 3))
        {
            var lignes = Objet.ObjetParNom(obj).SplitEveryNth(30);
            for (var l = 0; l < lignes.Count; l++)
            {
                result[l].AddRange([
                    new (Expedition.IndexObjet[ChoixObjet] == obj ? "│ " : "  "),
                    new (lignes[l].FirstOrDefault().Value)
                    ]);
                result[l].AddRange([
                    new (new string(' ', 47 - lignes[l].FirstOrDefault().Key)),
                    new (Expedition.IndexObjet[ChoixObjet] == obj ? "│": " "),
                    new (ChoixObjet != 2 && ChoixObjet != 5 ? "│" : " ")
                    ]);
            }
        }

        return result;
    }

    private StringColorise AffichagePointActionCapacite(Capacite cap)
    {
        if (cap.Cout == 0)
            return new(MettreAuMilieu($"+{cap.Gain}⭍",10),Color.Blue);
        var perso = Ordre[0].PointAction;
        if (cap.Cout > perso)
            return new(MettreAuMilieu($"-{cap.Cout}⭍", 10), Color.Red);
        else
            return new(MettreAuMilieu($"-{cap.Cout}⭍", 10), Color.Green);
    }

    private StringColorise AffichageQuantiteObjet(string obj)
    {
        var quantite = Expedition.Sac[obj];
        return quantite switch
        {
            0 => new(MettreAuMilieu($"x{quantite}", 10), Color.Red),
            > 0 => new(MettreAuMilieu($"x{quantite}", 10), Color.Blue),
            _ => new(new string(' ', 10))
        };
    }


    private void AfficherOrdre()
    {
        var index = 1;
        foreach (var entite in Ordre)
        {
            var box = Entourage(new StringColorise(MettreAuMilieu(entite.Nom, 20)), 20, out _, entite.Vivant ? Expedition.Equipe.Contains(entite) ? Color.Blue : Color.Red : Color.Black);
            AjouterString(box[0], index++);
            AjouterListe([box[1], box[2], box[3]], index++);
            AjouterString(box[^1], index++);
        }
    }
    private void AfficherCote(List<Entite> cote, int debut)
    {
        List<StringColorise> cibles = [];
        foreach (var e in cote)
        {
            if (Cibles.Contains(e))
                cibles.Add(new (MettreAuMilieu(Expedition.Equipe.Contains(Cibles[0]) ? "˅" : "˄", 42)));
            else
                cibles.Add(new (new string(' ', 42)));
        }

        if (Cibles.Count != 0)
        {
            if (Expedition.Equipe.Contains(Cibles[0]))
            {
                AjouterListe(cibles, debut-1);
            }
        }
            
        var index = 0;
        for (var f = 0; f < 5; f++)
        {
            
            List<StringColorise> result = [];
            foreach (var entite in cote)
            {
                List<StringColorise> barrePointAction = [];
                if (f != 0)
                    result.Add(new(entite.Sprite[f-1]));
                var pv = Entourage(BarPv(entite, out var taille), taille, out var tailleStr);
                if (Expedition.Equipe.Contains(entite))
                    barrePointAction.AddRange(BarPointAction(entite, out taille));
                result.Add(new(" "));
                switch (f)
                {
                    case 0:
                        result.Add(new StringColorise(MettreAuMilieu(entite.Nom, 41)));
                        continue;
                    case 1:
                        result.Add(pv[0]);
                        break;
                    case 2:
                        result.Add(pv[1]);
                        result.Add(pv[2]);
                        result.Add(pv[3]);
                        result.Add(pv[4]);
                        result.Add(pv[5]);
                        break;
                    case 3:
                        if (Ennemies.Contains(entite))
                        {
                            result.AddRange([new ("│"),new (new string(' ', tailleStr-2)), new ("│")]);
                            result.Add(new("    "));
                            continue;
                        }
                        result.AddRange(barrePointAction);
                        break;

                    case 4:
                        result.Add(pv[6]);
                        break;
                }
                result.Add(new("    "));
            }

            AjouterListe(result, debut + f + index);
        }

        if (Cibles.Count == 0)
        {
            AjouterListe([new("")], debut + 5);
            return;
        }
        ;
        if (Ennemies.Contains(Cibles[0]))
        {
            AjouterListe(cibles, debut +5);
        }
    }

    private static string MettreAuMilieu(string str, int count)
    {
        var reste = (count - str.Length) % 2 == 1 ? " " : "";
        var vide = new string(' ', (int)Math.Ceiling((count - str.Length -reste.Length) / 2f));
        return vide + reste + str + vide;
    }

    private static List<StringColorise> Entourage(StringColorise str, int count, out int taille, Color? couleur = null)
    {
        var topBar = new StringColorise("┌" + new string('─', count+2) + "┐  ", couleur);
        taille = topBar.Length;
        var botBar = new StringColorise("└" + new string('─', count + 2) + "┘  ", couleur);

        return [topBar, new("│ ", couleur), str, new(" │  ", couleur), botBar];
    }
    private static List<StringColorise> Entourage(List<StringColorise> listeStr, int count, out int taille, Color? couleur = null)
    {
        var topBar = new StringColorise("┌" + new string('─', count + 2) + "┐", couleur);
        taille = topBar.Length;
        var botBar = new StringColorise("└" + new string('─', count + 2) + "┘", couleur);
        List<StringColorise> result = [topBar, new("│ ", couleur)];
        listeStr.ForEach(e => result.Add(e));
        result.Add(new(" │", couleur));
        result.Add(botBar);
        return result;
    }

    private static List<StringColorise> BarPv(Entite entite, out int taille)
    {
        var pv = entite.PointDeVie;
        var pvmax = entite.PointDeVieMax;
        var pourcentagePv = pv * 100 / pvmax;
        var pvRestant = new StringColorise(new 
                string('▉', (int)Math.Ceiling(pourcentagePv * 0.2)),
                Color.Green); 
        var pvManquant = new StringColorise(new 
                string( '▉', (int)Math.Floor((100 - pourcentagePv) * 0.2)),
                Color.Red);

        var pvFormate = pv < 10 ? $" {pv} " : pv < 100 ? $" {pv}" : pv.ToString();
        var pvmaxFormate = pvmax < 10 ? $" {pvmax} " : pvmax < 100 ? $" {pvmax}" : pvmax.ToString();
        var nombrepv = new StringColorise($" {pvFormate}/{pvmaxFormate}");
        taille = $"{pvRestant.BaseStr + pvManquant.BaseStr} {pvFormate}/{pvmaxFormate}".Length;

        return [pvRestant, pvManquant, nombrepv];
    }

    private static List<StringColorise> BarPointAction(Entite entite, out int taille)
    {
        var action = entite.PointAction;
        var pa = new StringColorise(new 
            string('▉', action*2),
            Color.RoyalBlue);
        var paMax = new StringColorise(new
                string('▉', 20 - action*2),
            Color.SkyBlue);
        var paFormate = new StringColorise(
            action < 10 ? $" {action}⭍ " : action + "⭍ ",
            Color.Blue);
        var pamaxFormate = new StringColorise(
            "10⭍ ",
            Color.Blue);
        taille = $"{pa.BaseStr + paMax.BaseStr} {paFormate.BaseStr}/ 10⭍ ".Length;
        return [new ("│ "),pa, paMax, paFormate, new ("/"),pamaxFormate, new("│")];


    }

    #endregion
    
    #region Ecran
    private void AjouterString(StringColorise str, int index)
    {
        if (index >= _ecran.Count)
        {
            _ecran.Add([new StringColorise("│ ")]);
        }
        _ecran[index].Add(str);
    }

    private void AjouterListe(List<StringColorise> liste, int index, bool space = true)
    {
        if (index >= _ecran.Count)
        {
            if (space)
                _ecran.Add([new StringColorise("│ ")]);
            else
                _ecran.Add([new StringColorise("│")]);
        }
        liste.ForEach(e => _ecran[index].Add(e));
    }

    private static string FormatterLigne(List<StringColorise> liste)
    {
        var result = string.Join("", liste.Select(f => f.Str));
        var length = 0;
        liste.ForEach(e => length += e.Length);
        var lastElement = result[^1].ToString();
        var check = lastElement == "┐" || lastElement == "┘" || lastElement == "│" || lastElement == "┤";
        for (var i = 0; i < 154 - length; i++)
        {
            result += " ";
        }

        return check ? result : result[..^1] + "│";

    }

    #endregion
}