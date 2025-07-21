using System.Data.SqlTypes;
using System.Drawing;
using static Modeles.Extensions;

namespace Modeles.FonctionsJeu.MiniGames;

public class TimingMiniGame : MiniJeu
{
    public List<StringColorise> Barre;
    public List<string> Joueur;
    public int indexJoueur = 1;
    public bool InputPressed = false;

    public override void Jouer() {}
    public override void Jouer(out Dictionary<string, int> result) { result = []; }

    public override void Jouer(out string result)
    {
        Setup();
        Afficher();
        var direction = true;
        Joueur.Swap(0, 1);

        InputJouer();
        do
        {
            Joueur.Swap(indexJoueur, indexJoueur + (direction ? 1 : -1));
            indexJoueur += direction ? 1 : -1;
            if (indexJoueur + 1 == Joueur.Count || indexJoueur == 0)
                direction = !direction;
            Afficher();
            Thread.Sleep(50);
        } while (!InputPressed);

        result = Resultat();
    }

    public string Resultat()
    {
        return indexJoueur switch
        {
            < 5 => nameof(Color.White),
            < 8 => nameof(Color.Red),
            < 9 => nameof(Color.Yellow),
            < 12 => nameof(Color.Red),
            _ => nameof(Color.White)
        };
    }

    public void InputJouer()
    {
        Task.Factory.StartNew(() =>
        {
            while (Console.ReadKey().Key != ConsoleKey.Spacebar) ;
            InputPressed = true;
        });
    }

    public void Setup()
    {
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
        Barre.ForEach(e => Console.Write(e.Str));
        Console.WriteLine();
        Joueur.ForEach(Console.Write);
        Console.WriteLine();
    }
}