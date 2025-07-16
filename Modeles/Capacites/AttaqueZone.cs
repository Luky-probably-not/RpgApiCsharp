using Modeles.Character;

namespace Modeles.Capacites;

public class AttaqueZone() : Capacite("AttaqueZone", 5, true, false, 3, 0, "Inflige {Valeur} dégats tout les ennemies")
{
    public override void Utiliser(Entite utilisateur, Entite cible) { }

    public override void Utiliser(Entite utilisateur, List<Entite> cibles)
    {
        cibles.ForEach(cible => cible.Blesser((int)Valeur,utilisateur,false));
        utilisateur.PointAction -= Cout;
    }
}