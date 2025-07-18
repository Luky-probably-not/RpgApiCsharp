using Modeles.Objets;
using System.Collections.Generic;
using System.Drawing;
using static Modeles.Extensions;
namespace Modeles.FonctionsJeu;

public class Magasin
{
    public int Niveau;
    public Dictionary<string, int> Offres;
    public Dictionary<string, int> Stock;
    public List<List<StringColorise>> _ecran = [];
    public int ChoixAction = 0;

    public List<string> Objets =
    [
        nameof(PotionSoin),
        nameof(PotionEnergie),
        nameof(AttaqueBoost),
        nameof(DefenseBoost),
        nameof(PotionDoubleDegats),
        nameof(PotionReductionDegats)
    ];

    private Magasin(int niveau, Dictionary<string, int> offres)
    {
        Niveau = niveau;
        Offres = offres; 
        Stock = new Dictionary<string, int>
        {
            { nameof(PotionSoin), 5 },
            { nameof(PotionEnergie), 5 },
            { nameof(AttaqueBoost), 5 },
            { nameof(DefenseBoost), 5 },
            { nameof(PotionDoubleDegats), 5 },
            { nameof(PotionReductionDegats), 5 },
        };
    }

    public void Afficher()
    {
        Console.Clear();
        _ecran = [[new("┌" + new string('─', 152) + "┐")]];

        for (var a = 0; a < NiveauMagasin(); a++)
        {
            AfficherLigne(ChoixAction / 2 == a, a);
            AjouterListe([
                    new(new string('─', 50)),
                    new("┤"),
                    new(new string(' ', 50)),
                    new("├"),
                    new(new string('─', 50)),
                    new("│")
                ],
                _ecran.Count,
                false
            );
        }

        for (var a = 0; a < 3-NiveauMagasin(); a++)
        {
            AfficherLigneVide(ChoixAction/2 == a);
            AjouterListe([
                    new (new string('─', 50)),
                    new ("┤"),
                    new(new string(' ', 50)),
                    new("├"),
                    new(new string('─', 50)),
                    new ("│")],
                _ecran.Count,
                false
            );
        }


        _ecran.Add([new ("└" + new string('─', 152) + "┘")]);
        _ecran.ForEach(e => Console.WriteLine(e.FormatterLigne()));
        Thread.Sleep(50000);
    }

    private int NiveauMagasin()
    {  
        return Niveau switch
        {
            < 10 => 0,
            < 50 => 1,
            < 100 => 2,
            < 200 => 3,
            _ => 0
        };
    }

    private void AfficherLigne(bool choix,int index)
    {
        List<StringColorise> ligne = [new("│")];
        if (ChoixAction % 2 == 0 && choix)
            ligne.AddRange([new("┌"), new(new string('─', 48)), new("┐")]);
        else
            ligne.AddRange([new(new string(' ', 50))]);

        ligne.AddRange([new("│"), new(new string(' ', 50)), new("│")]);
        if (ChoixAction % 2 != 0 && choix)
            ligne.AddRange([new("┌"), new(new string('─', 48)), new("┐")]);
        else
            ligne.AddRange([new(new string(' ', 50))]);

        _ecran.Add(ligne);

        ligne = AfficherNomObjet(Objets[index], ChoixAction % 2 == 0 && choix);
        ligne.Add(new(new string(' ', 50)));
        ligne.AddRange(AfficherNomObjet(Objets[index+1], ChoixAction % 2 != 0 && choix));
        _ecran.Add(ligne);

        ligne = AfficherCoutObjet(Objets[index],ChoixAction%2 == 0 && choix);
        ligne.Add(new(new string(' ', 50)));
        ligne.AddRange(AfficherCoutObjet(Objets[index + 1], ChoixAction % 2 != 0 && choix));
        _ecran.Add(ligne);
        for (var i = 0; i < 3; i++)
        {
            ligne = [new("│")];
            ligne.AddRange(AfficherDescriptionObjet(Objets[index], ChoixAction % 2 == 0 && choix)[i]);
            ligne.AddRange([new(new string(' ', 50)),new("│")]);
            ligne.AddRange(AfficherDescriptionObjet(Objets[index+1], ChoixAction % 2 != 0 && choix)[i]);
            _ecran.Add(ligne);
        }
        _ecran.Add(ligne);
        ligne = [new("│")];
        if (ChoixAction % 2 == 0 && choix)
            ligne.AddRange([new("└"), new(new string('─', 48)), new("┘")]);
        else
            ligne.AddRange([new(new string(' ', 50))]);

        ligne.AddRange([new("│"), new(new string(' ', 50)), new("│")]);
        if (ChoixAction % 2 != 0 && choix)
            ligne.AddRange([new("└"), new(new string('─', 48)), new("┘")]);
        else
            ligne.AddRange([new(new string(' ', 50))]);

        _ecran.Add(ligne);
    }

    private List<StringColorise> AfficherNomObjet(string obj, bool choix)
    {
        List<StringColorise> ligne = [new("│")];
        if (choix)
            ligne.AddRange([new("│"), new(MettreAuMilieu(Objet.ObjetParNom(obj).Nom, 48)), new("│")]);
        else
            ligne.Add(new(MettreAuMilieu(Objet.ObjetParNom(obj).Nom, 50)));

        ligne.Add(new("│"));
        return ligne;
    }

    private List<StringColorise> AfficherCoutObjet(string obj, bool choix)
    {
        List<StringColorise> ligne = [new("│")];
        if (choix)
            ligne.AddRange([
                new("│"),
                new(new string(' ', 38)),
                CoutObjet(obj),
                new("│")
            ]);
        else
            ligne.AddRange([
                new(new string(' ', 39)),
                CoutObjet(obj),
                new(" ")
            ]);
        ligne.Add(new ("│"));
        return ligne;
    }

    private StringColorise CoutObjet(string obj)
    {
        var cout = Offres[obj];
        var argent = GameManager.Instance.expedition.Pieces;
        return cout switch
        {
            _ when cout > argent => new(MettreAuMilieu($"-{cout}$", 10), Color.Red),
            _ when cout <= argent => new(MettreAuMilieu($"-{cout}$", 10), Color.Yellow),
            _ => new(new string(' ', 10))
        };
    }

    private List<List<StringColorise>> AfficherDescriptionObjet(string obj, bool choix)
    {
        List<List<StringColorise>> result = [[], [], []];
        var lignes = Objet.ObjetParNom(obj).SplitEveryNth(30);
        for (var i = 0; i < lignes.Count; i++)
        {
            result[i].AddRange([
                new(choix ? "│ " : "  "),
                new(lignes[i].FirstOrDefault().Value),
                new(new string(' ', 47 - lignes[i].FirstOrDefault().Key)),
                new(choix ? "│" : " "),
                new("│")
            ]);
        }

        return result;
    }

    private void AfficherLigneVide(bool choix)
    {
        for (var a = 0; a < 7; a++)
        {
            List<StringColorise> ligne = [new("│")];

            if (ChoixAction % 2 == 0 && choix)
            {
                switch (a)
                {
                    case 0:
                        ligne.AddRange([new("┌"), new(new string('─', 48)), new("┐")]);
                        break;
                    case 6:
                        ligne.AddRange([new("└"), new(new string('─', 48)), new("┘")]);
                        break;
                    default:
                        ligne.AddRange([new("│"), new(new string(' ', 48)), new("│")]);
                        break;
                }
            } else
                ligne.AddRange([new(new string(' ', 50))]);

            ligne.AddRange([new("│"), new(new string(' ', 50)), new("│")]);
            if (ChoixAction % 2 != 0 && choix)
            {
                switch (a)
                {
                    case 0:
                        ligne.AddRange([new("┌"), new(new string('─', 48)), new("┐")]);
                        break;
                    case 6:
                        ligne.AddRange([new("└"), new(new string('─', 48)), new("┘")]);
                        break;
                    default:
                        ligne.AddRange([new("│"), new(new string(' ', 48)), new("│")]);
                        break;
                }
            } else
                ligne.AddRange([new(new string(' ', 50))]);
            ligne.Add(new ("│"));
            _ecran.Add(ligne);
        }
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

    public static Magasin Generer(int niveau)
    {
        Dictionary<string, int> offres = new()
        {
            { nameof(PotionSoin), 20 },
            { nameof(PotionEnergie), 20 },
            { nameof(AttaqueBoost), 100 },
            { nameof(DefenseBoost), 100 },
            { nameof(PotionDoubleDegats), 200 },
            { nameof(PotionReductionDegats), 200 }
        };
        return new Magasin(niveau, offres);

        /*
        return niveau switch
        {
            < 10 => new Magasin { Niveau = niveau },
            < 50 => new Magasin { Niveau = niveau,
                Offres =
                {
                    { nameof(PotionSoin), 20 },
                    { nameof(PotionEnergie), 20 }
                },
                Stock = stock
            },
            < 100 => new Magasin
            {
                Niveau = niveau,
                Offres =
                {
                    { nameof(PotionSoin), 20 },
                    { nameof(PotionEnergie), 20 },
                    { nameof(AttaqueBoost), 50},
                    { nameof(DefenseBoost), 50}
                },
                Stock = stock
            },
            _ => new Magasin
            {
                Niveau = niveau,
                Offres =
                {
                    { nameof(PotionSoin), 20 },
                    { nameof(PotionEnergie), 20 },
                    { nameof(AttaqueBoost), 100},
                    { nameof(DefenseBoost), 100},
                    { nameof(PotionDoubleDegats), 200},
                    { nameof(PotionReductionDegats), 200}
                },
                Stock = stock
            },
        };*/
    }

    public void Acheter(string obj)
    {
        if (Stock[obj] < 1)
            return;
        if (GameManager.Instance.expedition.Pieces < Offres[obj])
            return;
        GameManager.Instance.expedition.Pieces -= Offres[obj];
        GameManager.Instance.expedition.Sac[obj]++;
    }

}