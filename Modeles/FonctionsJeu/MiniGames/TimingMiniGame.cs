using System.Drawing;

namespace Modeles.FonctionsJeu.MiniGames;

public class TimingMiniGame() : MiniJeu()
{
    public override void Jouer(out string result)
    {
        Setup();
        Afficher();
        var direction = true;
        Joueur!.Swap(0, 1);

        InputJoueur();
        do
        {
            Joueur!.Swap((int)IndexJoueur!, (int)IndexJoueur! + (direction ? 1 : -1));
            IndexJoueur += direction ? 1 : -1;
            if (IndexJoueur + 1 == Joueur!.Count || IndexJoueur == 0)
                direction = !direction;
            Afficher();
            Thread.Sleep(50);
        } while (!(bool)InputPressed!);

        result = Resultat();
    }

    public string Resultat()
    {
        return IndexJoueur switch
        {
            < 5 => nameof(Color.White),
            < 8 => nameof(Color.Red),
            < 9 => nameof(Color.Yellow),
            < 12 => nameof(Color.Red),
            _ => nameof(Color.White)
        };
    }

    public void InputJoueur()
    {
        Task.Factory.StartNew(() =>
        {
            while (Console.ReadKey().Key != ConsoleKey.Spacebar) ;
            InputPressed = true;
        });
    }

    public void Setup()
    {
        IndexJoueur = 1;
        InputPressed = false;
        Barre =
        [
            new(new('▉', 5), Color.White),
            new(new('▉', 3), Color.Red),
            new(new('▉', 1), Color.Yellow),
            new(new('▉', 3), Color.Red),
            new(new('▉', 5), Color.White),
        ];
        Joueur = ["│"];
        for (var i = 0; i < 16; i++)
        {
            Joueur.Add(" ");
        }
    }

    public void Afficher()
    {
        Console.Clear();
        Barre!.ForEach(e => Console.Write(e.Str));
        Console.WriteLine();
        Joueur!.ForEach(Console.Write);
        Console.WriteLine();
    }
    public override void Jouer() { }
    public override void Jouer(out Dictionary<string, int> result) { result = []; }
    public override void Jouer(out int result) { result = 0; }
}