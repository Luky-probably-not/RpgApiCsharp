using System.Drawing;
using System.Runtime.InteropServices.JavaScript;
using static Modeles.Extensions;
namespace Modeles.FonctionsJeu.MiniGames;

public class Esquive() : MiniJeu()
{
    public override void Jouer(out int result)
    {
        
        Setup();
        InputJoueur();
        PreparerAttaque();
        while ((bool)Vivant!)
        {
            Affichage();
            Thread.Sleep(100);
        }
        Affichage();
        Console.WriteLine(Vague);
        result = (int)Vague!;
    }

    private void PreparerAttaque()
    {
        Task.Factory.StartNew(() =>
        {
            Attaques!.Add((false,0));
            while ((bool)Vivant!)
            {
                Vague++;
                var start = DateTime.Now;
                while (DateTime.Now < start.AddSeconds(2))
                {
                    Thread.Sleep(100);
                    CleanEcran();
                }
                var rand = new Random();
                Attaques = [.. Attaques.Select(e =>
                {
                    e = e with { index = rand.Next(5) };
                    e = e with { x = rand.NextDouble() > 0.5  };
                    return e;
                })];
                start = DateTime.Now;
                while (DateTime.Now < start.AddSeconds(2))
                {
                    Thread.Sleep(100);
                    DisplayAttaque(Color.Orange);
                }
                start = DateTime.Now;
                while (DateTime.Now < start.AddSeconds(1))
                {
                    Thread.Sleep(100);
                    DisplayAttaque(Color.Red);
                    VerifierAttaque();
                }

                if ((Vague < 5 * Difficulte)) continue;
                Difficulte++;
                PreparerAttaque();
            }
        });
    }

    private void VerifierAttaque()
    {
        foreach (var (b, index) in Attaques!)
        {
            for (var i = 0; i < 5; i++)
            {
                var x = b ? index : i;
                var y = b ? i : index;
                if (PosJoueur == (x, y))
                    Vivant = false;
            }
        }

    }

    private void DisplayAttaque(Color couleur)
    {
        foreach (var (x, index) in Attaques!)
        {
            for (var i = 0; i < 5; i++)
            {
                Plateau![x ? index : i][!x ? index : i]
                    = new StringColorise("■", couleur);
                if ((x ? index : i) == PosJoueur.x && (!x ? index : i) == PosJoueur.y)
                    Plateau[PosJoueur.x][PosJoueur.y] = new StringColorise("■", Color.Green);
            }
        }
    }

    private void CleanEcran()
    {
        Plateau = [.. Plateau!.Select(e => e.Select(f => new StringColorise("■", Color.White)).ToList())];

        Plateau[PosJoueur.x][PosJoueur.y] = new StringColorise("■", Color.Green);
    }

    private void InputJoueur()
    {
        List<ConsoleKey> toucheValide =
        [
            ConsoleKey.LeftArrow,
            ConsoleKey.RightArrow,
            ConsoleKey.UpArrow,
            ConsoleKey.DownArrow,
        ];
        Task.Factory.StartNew(() =>
        {
            while ((bool)Vivant!)
            {
                var touche = Console.ReadKey().Key;
                if (toucheValide.Contains(touche))
                    DeplacerJoueur(touche);
            }
        });
    }

    private void Affichage()
    {
        Console.Clear();
        foreach (var ligne in Plateau!)
        {
            ligne.ForEach(e => Console.Write($" {e.Str} "));
            Console.Write("\n");
        }
    }

    private void DeplacerJoueur(ConsoleKey touche)
    {
        if (!VerifierDirection(touche)) return;

        switch (touche)
        {
            case ConsoleKey.UpArrow:
                PlacerJoueur(PosJoueur.x-1,PosJoueur.y);
                break;
            case ConsoleKey.DownArrow:
                PlacerJoueur(PosJoueur.x + 1, PosJoueur.y);
                break;
            case ConsoleKey.LeftArrow:
                PlacerJoueur(PosJoueur.x, PosJoueur.y-1);
                break;
            case ConsoleKey.RightArrow:
                PlacerJoueur(PosJoueur.x, PosJoueur.y+1);
                break;
        }

    }

    private bool VerifierDirection(ConsoleKey touche)
    {
        return touche switch
        {
            ConsoleKey.UpArrow when PosJoueur.x == 0 => false,
            ConsoleKey.DownArrow when PosJoueur.x == 4 => false,
            ConsoleKey.LeftArrow when PosJoueur.y == 0 => false,
            ConsoleKey.RightArrow when PosJoueur.y == 4 => false,
            _ => true,
        };
    }

    private void PlacerJoueur(int x, int y)
    {
        Plateau![PosJoueur.x][PosJoueur.y] = new StringColorise("■", CouleurCaseJoueur);
        CouleurCaseJoueur = (Color)Plateau[x][y].Couleur!;
        Plateau[x][y] = new StringColorise("■", Color.Green);
        PosJoueur = (x, y);

    }

    private void Setup()
    {
        Vivant = true;
        CouleurCaseJoueur = Color.White;
        Vague = 0;
        Difficulte = 1;
        Plateau = [];
        Attaques = [];
        for (var i = 0; i < 5; i++)
        {
            var ligne = new List<StringColorise>();
            for (var f = 0; f < 5; f++)
            {
                ligne.Add(new("■", Color.White));
            }
            Plateau!.Add(ligne);
        }

        PosJoueur = (2, 2);
    }


    public override void Jouer() {}
    public override void Jouer(out Dictionary<string, int> result) { result = []; }

    public override void Jouer(out string result) { result = ""; }
}