using Modeles.MoveSet;

namespace Modeles.Character.Ennemie;

public class Maudit() : Entite("Maudit", 40, 15, 80, 50, [
    " {~~}",
    " (YY)",
    " ~**~",
    " )--("
]), IAction
{
    public Capacite ChoisirCapacite()
    {
        List<Capacite> choixPossible = Capacites.FindAll(c => c.Cout <= PointAction);
        var rand = new Random();
        return choixPossible[rand.Next(choixPossible.Count)];
    }
}