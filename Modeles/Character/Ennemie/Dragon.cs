using Modeles.MoveSet;

namespace Modeles.Character.Ennemie;

public class Dragon() : Entite("Dragon", 15, 30, 200, 100, [
    "/^=^\\",
    "{o_o}",
    "\\>o</",
    " v v "
]), IAction
{
    public Capacite ChoisirCapacite()
    {
        List<Capacite> choixPossible = Capacites.FindAll(c => c.Cout <= PointAction);
        var rand = new Random();
        return choixPossible[rand.Next(choixPossible.Count)];
    }
}