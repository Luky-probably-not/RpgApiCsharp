using Modeles.MoveSet;

namespace Modeles.Character.Ennemie;

public class Gobelin() : Entite("Gobelin", 15, 5, 30, 120, [
    "┌^─^┐",
    "(o_o)",
    "/|_|\\",
    "/   \\"
]), IAction
{
    public Capacite ChoisirCapacite()
    {
        List<Capacite> choixPossible = Capacites.FindAll(c => c.Cout <= PointAction);
        var rand = new Random();
        return choixPossible[rand.Next(choixPossible.Count)];
    }
}