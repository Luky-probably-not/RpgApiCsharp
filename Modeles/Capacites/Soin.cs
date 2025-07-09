using Modeles.Character;

namespace Modeles.Capacites;

public class Soin() : Capacite(0.3f, false, true, 3, 0)
{
    public override void Utiliser(Entite utilisateur, Entite cible)
    {
        cible.Soigner(Valeur);
        utilisateur.PointAction -= Cout;
    }

    public override void Utiliser(Entite utilisateur, List<Entite> cibles){ }
}