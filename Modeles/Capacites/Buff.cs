using Modeles.Character;
using Modeles.Resources;
using static Modeles.Resources.Resource;

namespace Modeles.Capacites;

public class Buff() : Capacite(0, false, true, 5, 0)
{
    public override void Utiliser(Entite utilisateur, Entite cible)
    {
        cible.Bonus.TryAdd(DoubleDegats, 1);
        utilisateur.PointAction -= Cout;
    }

    public override void Utiliser(Entite utilisateur, List<Entite> cibles) { }
}