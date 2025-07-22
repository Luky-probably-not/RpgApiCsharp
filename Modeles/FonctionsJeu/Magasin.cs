using System.Drawing;
using Modeles.Items;
using Modeles.Items.Objets;
using static Modeles.Extensions;
namespace Modeles.FonctionsJeu;

public class Magasin
{
    public int Niveau { get; set; }
    public Dictionary<string, int> Offres { get; set; }
    public Dictionary<string, int> Stock { get; set; }
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

    public Magasin()
    {
        Offres = [];
        Stock = [];
    }

    private Magasin(int niveau, Dictionary<string, int> offres)
    {
        Niveau = niveau;
        Offres = offres; 
        Stock = [];
        foreach (var kvp in Offres)
            Stock.Add(kvp.Key,5);
    }

    public static Magasin Generer(int niveau)
    {
        return niveau switch
        {
            < 10 => new Magasin(niveau, []),
            < 50 => new Magasin(niveau,new Dictionary<string, int> {
                    { nameof(PotionSoin), 20 },
                    { nameof(PotionEnergie), 20 }
                }
            ),
            < 100 => new Magasin(niveau, new Dictionary<string, int> {
                    { nameof(PotionSoin), 20 },
                    { nameof(PotionEnergie), 20 },
                    { nameof(AttaqueBoost), 50},
                    { nameof(DefenseBoost), 50}
                }
            ),
            _ => new Magasin(niveau, new Dictionary<string, int> {
                    { nameof(PotionSoin), 20 },
                    { nameof(PotionEnergie), 20 },
                    { nameof(AttaqueBoost), 50},
                    { nameof(DefenseBoost), 50},
                    { nameof(PotionDoubleDegats), 200},
                    { nameof(PotionReductionDegats), 200}
                }
            )
        };
    }

    public void Afficher(int choix)
    {
        ChoixAction = choix;
        Console.Clear();
        _ecran = [];
        _ecran.Add([
            new("┌" + new string('─', 50) + "┬"),
            new (new string('─', 50) + "┬"),
            new (new string('─', 50) + "┐"),

        ]);

        for (var a = 0; a < NiveauMagasin(); a++)
        {
            AfficherLigne(ChoixAction / 2 == a, a*2);
            _ecran.Add([
                    new ("├"),
                    a == 2 ? new(new string('─', 25) + "┬" + new string('─',24)) : new(new string('─', 50)),
                    a == 2 ? new("┼") : new ("┤"),
                    a == 2 ? new(new string('─', 25) + "┬" + new string('─',24)) :  new(new string(' ', 50)),
                    a == 2 ? new("┼") : new ("├"),
                    a == 2 ? new(new string('─', 25) + "┬" + new string('─',24)) : new(new string('─', 50)),
                    new("┤")
                ]);
            
        }

        for (var a = 0; a < 3-NiveauMagasin(); a++)
        {
            AfficherLigneVide(ChoixAction/2 == a+NiveauMagasin(),a == 2 - NiveauMagasin());
            _ecran.Add([
                new ("├"),
                a == 2 - NiveauMagasin() ? new(new string('─', 25) + "┬" + new string('─',24)) : new(new string('─', 50)),
                a == 2 - NiveauMagasin() ? new("┼") : new("┤"),
                a == 2 - NiveauMagasin() ? new(new string('─', 25) + "┬" + new string('─',24)) : new(new string(' ', 50)),
                a == 2 - NiveauMagasin() ? new("┼") : new("├"),
                a == 2 - NiveauMagasin() ? new(new string('─', 25) + "┬" + new string('─',24)) : new(new string('─', 50)),
                new("┤")
            ]);

        }

        AfficherObjetsExpedition();
        
        _ecran.Add([
            new ("└" + new string('─', 25) + "┴"),
            new (new string('─', 24) + "┴"),
            new (new string('─', 25) + "┴"),
            new (new string('─', 24) + "┴"),
            new (new string('─', 25) + "┴"),
            new (new string('─', 24) + "┘"),
        ]);
        _ecran.ForEach(e => Console.WriteLine(e.FormatterLigne()));
    }

    private int NiveauMagasin()
    {  
        return Niveau switch
        {
            < 10 => 0,
            < 50 => 1,
            < 100 => 2,
            _ => 3
        };
    }

    private void AfficherObjetsExpedition()
    {
        for (var a = 0; a < 3; a++)
        {
            List<StringColorise> ligne = [new("│")];
            foreach (var obj in Objets)
            {
                switch (a)
                {
                    case 0:
                        ligne.Add(new (MettreAuMilieu(Objet.ObjetParNom(obj).Nom,Objets.IndexOf(obj)%2 == 1 ? 24 : 25)));
                        break;
                    case 2:
                        ligne.Add(new (new string(' ', Objets.IndexOf(obj) % 2 == 1 ? 24 : 25)));
                        break;
                    case 1:
                        ligne.Add(new (new string(' ', Objets.IndexOf(obj) % 2 == 1 ? 14 : 15)));
                        ligne.Add(AfficherQuantiteObjet(obj));
                        break;
                }
                ligne.Add(new("│"));
            }
            _ecran.Add(ligne);
        }
    }

    private static StringColorise AfficherQuantiteObjet(string obj)
    {
        var quantite = GameManager.Instance.Expedition.Sac[obj];
        return new(MettreAuMilieu($"x{quantite}", 10), quantite == 0 ? Color.Red : Color.Green);
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

        ligne.Add(new ("│"));
        _ecran.Add(ligne);

        ligne = AfficherNomObjet(Objets[index], ChoixAction % 2 == 0 && choix);
        ligne.Add(new(new string(' ', 50)));
        ligne.AddRange(AfficherNomObjet(Objets[index+1], ChoixAction % 2 != 0 && choix));
        _ecran.Add(ligne);

        ligne = AfficherCoutObjet(Objets[index],ChoixAction%2 == 0 && choix);
        ligne.Add(new(new string(' ', 50)));
        ligne.AddRange(AfficherCoutObjet(Objets[index + 1], ChoixAction % 2 != 0 && choix));
        _ecran.Add(ligne);
        for (var i = 0; i < 2; i++)
        {
            ligne = [new("│")];
            ligne.AddRange(AfficherDescriptionObjet(Objets[index], ChoixAction % 2 == 0 && choix)[i]);
            ligne.AddRange([new(new string(' ', 50)),new("│")]);
            ligne.AddRange(AfficherDescriptionObjet(Objets[index+1], ChoixAction % 2 != 0 && choix)[i]);
            _ecran.Add(ligne);
        }
        ligne = [new("│")];
        if (ChoixAction % 2 == 0 && choix)
            ligne.AddRange([new("└"), new(new string('─', 48)), new("┘")]);
        else
            ligne.AddRange([new(new string(' ', 50))]);

        ligne.AddRange([new("│"), index/2 != 2 ? new(new string(' ', 50)) : GameManager.Instance.Expedition.QuantitePieces(), new("│")]);
        if (ChoixAction % 2 != 0 && choix)
            ligne.AddRange([new("└"), new(new string('─', 48)), new("┘")]);
        else
            ligne.AddRange([new(new string(' ', 50))]);
        ligne.Add(new("│"));
        _ecran.Add(ligne);
    }

    private static List<StringColorise> AfficherNomObjet(string obj, bool choix)
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
                QuantiteObjet(obj),
                new(new string(' ', 28)),
                CoutObjet(obj),
                new("│")
            ]);
        else
            ligne.AddRange([
                new (" "),
                QuantiteObjet(obj),
                new(new string(' ', 28)),
                CoutObjet(obj),
                new(" ")
            ]);
        ligne.Add(new ("│"));
        return ligne;
    }

    private StringColorise CoutObjet(string obj)
    {
        var cout = Offres[obj];
        var argent = GameManager.Instance.Expedition.Pieces;
        return cout switch
        {
            _ when cout > argent => new(MettreAuMilieu($"-{cout}$", 10), Color.Red),
            _ when cout <= argent => new(MettreAuMilieu($"-{cout}$", 10), Color.Yellow),
            _ => new(new string(' ', 10))
        };
    }

    private StringColorise QuantiteObjet(string obj)
    {
        var stock = Stock[obj];
        return stock switch
        {
            0 => new(MettreAuMilieu($"x{stock}", 10), Color.Red),
            > 0 => new(MettreAuMilieu($"x{stock}", 10), Color.Green),
            _ => new(new string(' ', 10))
        };
    }

    private static List<List<StringColorise>> AfficherDescriptionObjet(string obj, bool choix)
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

    private void AfficherLigneVide(bool choix, bool index)
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
                ligne.Add(new(new string(' ', 50)));

            ligne.AddRange([
                new("│"),
                index && a == 6 ? GameManager.Instance.Expedition.QuantitePieces() : new(new string(' ', 50)),
                new("│")
            ]);
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



}