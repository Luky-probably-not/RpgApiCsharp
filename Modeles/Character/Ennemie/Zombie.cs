using Modeles.Capacites;

namespace Modeles.Character.Ennemie;

public class Zombie() : Entite("Zombie", 15, 20, 50, 80, [
    "     ",
    "   ☺ ",
    "   #*",
    "  // "
]), IAction
{
    public Capacite ChoisirCapacite()
    {
        List<Capacite> choixPossible = Capacites.FindAll(c => c.Cout <= PointAction);
        var rand = new Random();
        return choixPossible[rand.Next(choixPossible.Count)];
    }
}