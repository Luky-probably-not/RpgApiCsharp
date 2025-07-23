using System.Drawing;
using Modeles.Items.Objets;
using static Modeles.Extensions;

namespace Modeles.FonctionsJeu.MiniGames;

public class Memory() : MiniJeu()
{
    
    public Memory(Dictionary<string, int> quantite) : this()
    {
        Compteur = new(){
            { nameof(PotionSoin), 0 },
            { nameof(PotionEnergie), 0 },
            { nameof(AttaqueBoost), 0 },
            { nameof(DefenseBoost), 0 },
            { nameof(PotionDoubleDegats), 0 },
            { nameof(PotionReductionDegats), 0 },
        };
        ActionsRestante = 6;
        Choix = 0;
        Choix = (int)Choix;
        PremierCoup = 0;
        ObjetsLists = [];
        Trouve = [];
        for (var i = 0; i < 3; i++)
        {
            List<string> ligne = [];
            List<bool> bol = [];
            for (var f = 0; f < 4; f++)
            {
                ligne.Add("");
                bol.Add(false);
            }
            ObjetsLists.Add(ligne);
            Trouve.Add(bol);
        }

        Setup(quantite);
    }

    private void Setup(Dictionary<string, int> quantite)
    {
        var rand = new Random();
        foreach (var kvp in quantite)
        {
            for (var i = 0; i < kvp.Value; i++)
            {
                var x = rand.Next(3);
                var x2 = rand.Next(3);
                var y = rand.Next(4);
                var y2 = rand.Next(4);
                while (ObjetsLists![x][y] != "" || ObjetsLists[x2][y2] != "" || x == x2 && y == y2)
                {
                    x = rand.Next(3);
                    y = rand.Next(4);
                    x2 = rand.Next(3);
                    y2 = rand.Next(4);
                }

                ObjetsLists[x][y] = kvp.Key;
                ObjetsLists[x2][y2] = kvp.Key;
            }
        }
    }

    public override void Jouer(out Dictionary<string, int> recompense)
    {
        while (ActionsRestante > 0)
        {
            ChoixAction();
            Retourne();
            PremierCoup = Choix;
            ChoixAction();
            Retourne();
            Afficher();
            Thread.Sleep(1000);
            if (!VerifieCoup())
            {
                Trouve![(int)Choix! / 4][(int)Choix! % 4] = false;
                Trouve[(int)PremierCoup! / 4][(int)PremierCoup! % 4] = false;
            }
            else
                Compteur![ObjetsLists![(int)Choix! / 4][(int)Choix! % 4]]++;
            ActionsRestante--;
        }

        AfficherSolution();
        recompense = Compteur!;
    }

    public void Afficher()
    {
        Console.Clear();
        var x = 0;
        
        foreach (var list in ObjetsLists!)
        {
            var y = 0;
            foreach (var obj in list)
            {
                Console.Write(
                    Trouve![x][y] 
                        ? x*4+y == Choix 
                            ? new StringColorise("▉",Color.White).Str + SpriteObjet(obj).Str + " "
                            : " " + SpriteObjet(obj).Str + " " 
                        : new StringColorise("▉▉▉",x*4+y == Choix 
                            ? Color.White 
                            : Color.Yellow).Str);
                //Console.Write(" " + obj.Str + " ");
                y++;
            }

            x++;
            Console.Write("\n");
        }
        Console.WriteLine("Actions restantes : {0}", ActionsRestante);
    }

    public void AfficherSolution()
    {
        Console.Clear();

        foreach (var list in ObjetsLists!)
        {
            foreach (var obj in list)
            {
                
                Console.Write(" " + SpriteObjet(obj).Str + " ");
            }
            Console.Write("\n");
        }
    }

    public bool VerifieCoup()
    {
        return ObjetsLists![(int)Choix! / 4][(int)Choix! % 4] == ObjetsLists[(int)PremierCoup! / 4][(int)PremierCoup! % 4];
    }

    public void Retourne()
    {
        Trouve![(int)Choix! / 4][(int)Choix! % 4] = true;
    }

    public void ChoixAction()
    {
        var choix = Choix;
        List<ConsoleKey> toucheValide =
        [
            ConsoleKey.LeftArrow,
            ConsoleKey.RightArrow,
            ConsoleKey.UpArrow,
            ConsoleKey.DownArrow,
            ConsoleKey.Spacebar
        ];
        var touche = ConsoleKey.A;
        var caseValide = false;
        while (touche != ConsoleKey.Spacebar || !caseValide)
        {
            Afficher();
            touche = Console.ReadKey().Key;
            if (!toucheValide.Contains(touche)) continue;
            choix += touche switch
            {
                ConsoleKey.RightArrow => 1,
                ConsoleKey.UpArrow => -4,
                ConsoleKey.DownArrow => 4,
                ConsoleKey.LeftArrow => -1,
                _ => 0
            };
            if (choix > 11) choix -= 12;
            if (choix < 0) choix += 12;
            Choix = choix;
            caseValide = !Trouve![(int)Choix! / 4][(int)Choix! % 4];
        }
    }

    public static StringColorise SpriteObjet(string obj)
    {
        return obj switch
        {
            nameof(PotionSoin) => new("⚱", Color.Red),
            nameof(PotionEnergie) => new("⚱", Color.Blue),
            nameof(AttaqueBoost) => new ("⛊", Color.Red),
            nameof(DefenseBoost) => new ("⛊", Color.Green),
            nameof(PotionDoubleDegats) => new ("✧", Color.Red),
            nameof(PotionReductionDegats) => new ("✧", Color.Green),
            _ => new("")
        };
    }

    public override void Jouer() { }
    public override void Jouer(out string result) { result = ""; }
    public override void Jouer(out int result) { result = 0; }
}