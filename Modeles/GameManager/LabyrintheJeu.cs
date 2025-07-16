using Modeles.LabyrintheLogique;

namespace Modeles.GameManager;

public static class LabyrintheJeu
{
    private static Labyrinthe Laby = new(1);
    private static int PosColonne;
    private static int PosLigne;

    public static bool Deplacement(Labyrinthe laby, out string cell)
    {
        Laby = laby;
        PositionJoueur();
        return Pas(RecupererInput(), out cell);
    }

    private static void PositionJoueur()
    {
        for (var i = 0; i < Laby.Taille; i++)
        {
            for (var f = 0; f < Laby.Taille; f++)
            {
                if (Laby.Laby[i][f].Type != "⚗") continue;
                PosColonne = f;
                PosLigne = i;
                return;
            }
        }
        PosColonne = 0;
        PosLigne = 0;
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
            ConsoleKey.UpArrow => !Laby.Laby[PosLigne][PosColonne].North,
            ConsoleKey.DownArrow => !Laby.Laby[PosLigne][PosColonne].South,
            ConsoleKey.LeftArrow => !Laby.Laby[PosLigne][PosColonne].West,
            ConsoleKey.RightArrow => !Laby.Laby[PosLigne][PosColonne].East,
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
        Laby.Laby[PosLigne][PosColonne].Type = " ";
        var verif = Laby.Laby[PosLigne + NS][PosColonne + WE].Type == "B";
        cell = Laby.Laby[PosLigne + NS][PosColonne + WE].Type!;
        Laby.Laby[PosLigne + NS][PosColonne + WE].Type = "⚗";
        return verif;
    }
}