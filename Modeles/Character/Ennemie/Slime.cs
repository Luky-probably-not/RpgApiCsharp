using Modeles.MoveSet;

namespace Modeles.Character.Ennemie;

public class Slime() : Entite("Slime", 15, 10, 50, 100, [
    "  __ ",
    " (oo)",
    " |O \\",
    " \\__/"
]), IAction
{
    public Capacite ChoisirCapacite()
    {
        List<Capacite> choixPossible = Capacites.FindAll(c => c.Cout <= PointAction);
        var rand = new Random();
        return choixPossible[rand.Next(choixPossible.Count)];
    }
}