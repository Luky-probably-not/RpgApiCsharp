using Modeles.LabyrintheLogique;

namespace Modeles.FonctionsJeu.Helper;

public static class LabyHelper
{
    public static Labyrinthe Laby { get; set; }= new(1);
    private static int _posColonne;
    private static int _posLigne;

    public static bool LabyDeplacement(out string cell)
    {
        PositionJoueur();
        return Pas(RecupererInput(), out cell);
    }

    public static void LabyAffichage()
    {
        Laby.Display();
    }


    private static void PositionJoueur()
    {
        for (var i = 0; i < Laby.Taille; i++)
        {
            for (var f = 0; f < Laby.Taille; f++)
            {
                if (Laby.Laby[i][f].Type != "⚗") continue;
                _posColonne = f;
                _posLigne = i;
                return;
            }
        }
        _posColonne = 0;
        _posLigne = 0;
    }

    private static ConsoleKey RecupererInput()
    {
        List<ConsoleKey> toucheValide =
        [
            ConsoleKey.UpArrow,
            ConsoleKey.DownArrow,
            ConsoleKey.LeftArrow,
            ConsoleKey.RightArrow
        ];

        var touche = Console.ReadKey().Key;
        while (!toucheValide.Contains(touche) || !VerifierInput(touche))
        {
            Console.WriteLine("Bad input");
            touche = Console.ReadKey().Key;
        }

        return touche;
    }

    private static bool VerifierInput(ConsoleKey touche)
    {
        return touche switch
        {
            ConsoleKey.UpArrow => !Laby.Laby[_posLigne][_posColonne].North,
            ConsoleKey.DownArrow => !Laby.Laby[_posLigne][_posColonne].South,
            ConsoleKey.LeftArrow => !Laby.Laby[_posLigne][_posColonne].West,
            ConsoleKey.RightArrow => !Laby.Laby[_posLigne][_posColonne].East,
            _ => false
        };
    }

    private static bool Pas(ConsoleKey touche, out string cell)
    {
        var NS = touche switch
        {
            ConsoleKey.UpArrow => -1,
            ConsoleKey.DownArrow => 1,
            _ => 0
        };
        var WE = touche switch
        {
            ConsoleKey.LeftArrow => -1,
            ConsoleKey.RightArrow => 1,
            _ => 0
        };
        Laby.Laby[_posLigne][_posColonne].Type = " ";
        var verif = Laby.Laby[_posLigne + NS][_posColonne + WE].Type == "B";
        cell = Laby.Laby[_posLigne + NS][_posColonne + WE].Type!;
        Laby.Laby[_posLigne + NS][_posColonne + WE].Type = "⚗";
        return verif;
    }
}