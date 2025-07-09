using Modeles.Character;

namespace Modeles.Capacites;

public class AttaqueZone() : Capacite(5, true, false, 3, 0)
{
    public override void Utiliser(Entite utilisateur, Entite cible) { }

    public override void Utiliser(Entite utilisateur, List<Entite> cibles)
    {
        cibles.ForEach(cible => cible.Blesser((int)Valeur,utilisateur.Attaque,false));
        utilisateur.PointAction -= Cout;
    }
}