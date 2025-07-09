using Modeles.Character;

namespace Modeles.Capacites;

public class MultHit() : Capacite(3, false, false, 3, 0)

{
    public override void Utiliser(Entite utilisateur, Entite cible)
    {
        for (var i = 0; i < 3; i++)
        {
            cible.Blesser((int)Valeur,utilisateur.Attaque,true);
        }
        utilisateur.PointAction -= Cout;
    }

    public override void Utiliser(Entite utilisateur, List<Entite> cibles) { }
}