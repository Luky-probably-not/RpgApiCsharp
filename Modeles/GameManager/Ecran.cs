using Modeles.Character;
using System.Drawing;
using Modeles.Capacites;
using static Modeles.Extensions;

namespace Modeles.GameManager;

public class Ecran
{
    private List<List<StringColorise>> _ecran = [[new StringColorise("┌" + string.Concat(Enumerable.Repeat("─", 153)) + "┐")]];

    public required List<Entite> equipe;
    public required List<Entite> ennemies;
    public required List<Entite> ordre;
    public void Afficher()
    {
        Console.Clear();
        _ecran = [[new StringColorise("┌" + new string('─', 152) + "┐")]];
        
        AfficherOrdre();
        AfficherCote(ennemies,2);
        AfficherCote(equipe,14);
        AfficherCapacite();
        _ecran.ForEach(e => Console.WriteLine(FormatterLigne(e)));
    }

    #region ValeurEcran

    private void AfficherCapacite()
    {
        if (!equipe.Contains(ordre[0]))
            return;
        _ecran.Add([new ("├"), new (new string('─', 50)), new ("┬"), new(new string('─', 50)), new ("┬"), new(new string('─', 50)), new ("┤")]);
        LigneVide();
        List<StringColorise> result = [];
        foreach (Capacite cap in ordre[0].Capacites)
        {
            result.Add(new(MettreAuMilieu(cap.Nom, 49)));
            if (ordre[0].Capacites.IndexOf(cap) != 2)
                result.Add(new("│ "));
        }
        AjouterListe(result, _ecran.Count);
        var length = _ecran.Count;
        foreach (Capacite cap in ordre[0].Capacites)
        {
            result =
            [
                new(new string(' ', 40)),
                AffichagePointActionCapacite(cap)
            ];
            if (ordre[0].Capacites.IndexOf(cap) != 2)
                result.Add(new("│ "));
            AjouterListe(result, length);
        }

        LigneVide();
        _ecran.Add([new("└"), new(new string('─', 50)), new("┴"), new(new string('─', 50)), new("┴"), new(new string('─', 50)), new("┘")]);


    }

    private StringColorise AffichagePointActionCapacite(Capacite cap)
    {
        if (cap.Cout == 0)
            return new(MettreAuMilieu($"+{cap.Gain}⭍",9),Color.Blue);
        var perso = ordre[0].PointAction;
        if (cap.Cout > perso)
            return new(MettreAuMilieu($"-{cap.Cout}⭍", 9), Color.Red);
        else
            return new(MettreAuMilieu($"-{cap.Cout}⭍", 9), Color.Green);
    }

    private void LigneVide(int count = 1)
    {
        for (int a = 0; a < count ; a++)
            _ecran.Add([new ("│"), new(new string(' ', 50)), new("│"), new(new string(' ', 50)), new("│"), new(new string(' ', 50)), new ("│")]);
    }

    private void AfficherOrdre()
    {
        var index = 1;
        foreach (Entite entite in ordre)
        {
            var box = Entourage(new StringColorise(MettreAuMilieu(entite.Nom, 20)), 20, out _, equipe.Contains(entite) ? Color.Blue : Color.Red);
            AjouterString(box[0], index++);
            AjouterListe([box[1], box[2], box[3]], index++);
            AjouterString(box[^1], index++);
        }
    }
    private void AfficherCote(List<Entite> cote, int debut)
    {
        for (var f = 0; f < 4; f++)
        {
            List<StringColorise> result = [];
            foreach (var entite in cote)
            {
                result.Add(new(entite.Sprite[f]));
                var pv = Entourage(BarPv(entite, out var taille), taille, out var tailleStr);
                result.Add(new(" "));
                switch (f)
                {
                    case 0:
                        result.Add(new StringColorise(MettreAuMilieu(entite.Nom, tailleStr)));
                        break;
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
                        result.Add(pv[6]);
                        break;

                }
                result.Add(new("    "));
            }
            AjouterListe(result, debut + f);

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

    private void AjouterListe(List<StringColorise> liste, int index)
    {
        if (index >= _ecran.Count)
            _ecran.Add([new StringColorise("│ ")]);
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