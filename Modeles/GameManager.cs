using Modeles.LabyrintheLogique;

namespace Modeles;

public class GameManager
{
    private static GameManager? _instance = null;

    public Labyrinthe Laby;

    private int PosLigne;
    private int PosColonne;
    private GameManager(){}

    public static GameManager Instance
    {
        get
        {
            _instance ??= new GameManager();
            return _instance;
        }
    }

    public void Play()
    {
        var arrive = false;
        var score = 0;
        while (!arrive)
        {
            Laby.Display();
            PositionJoueur();
            arrive = Deplacement(RecupererInput());
            score++;
        }
        Laby.Display();
        Console.WriteLine($"score : {score}");

    }

    private void PositionJoueur()
    {
        for (var i = 0; i < Laby.Taille; i++)
        {
            for (var f = 0; f < Laby.Taille; f++)
            {
                if (Laby.Laby[i][f].Type != "P") continue;
                PosColonne = f;
                PosLigne = i;
                return;
            }
        }
        PosColonne = 0;
        PosLigne = 0;
    }

    private ConsoleKey RecupererInput()
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

    private bool VerifierInput(ConsoleKey touche)
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

    private bool Deplacement(ConsoleKey touche)
    {
        var NS = touche switch
        {
            ConsoleKey.UpArrow => -1,
            ConsoleKey.DownArrow => 1,
            _ => 0
        } ;
        var WE = touche switch
        {
            ConsoleKey.LeftArrow => -1,
            ConsoleKey.RightArrow => 1,
            _ => 0
        };
        Laby.Laby[PosLigne][PosColonne].Type = " ";
        bool verif = Laby.Laby[PosLigne + NS][PosColonne + WE].Type == "B";
        Laby.Laby[PosLigne + NS][PosColonne + WE].Type = "P";
        return verif;
    }
}