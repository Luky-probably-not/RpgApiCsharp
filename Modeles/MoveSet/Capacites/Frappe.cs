using Modeles.Character;

namespace Modeles.MoveSet.Capacites;

public class Frappe() : Capacite("Frappe", 5, false, false, 0, 3, "Inflige {Valeur} dégats un ennemi")
{
    public override void Utiliser(Entite utilisateur, Entite cible)
    {
        cible.Blesser((int)Valeur,utilisateur);
        utilisateur.PointAction -= Cout;
        utilisateur.PointAction += Gain;
    }

    public override void Utiliser(Entite utilisateur, List<Entite> cibles){ }
    public override void Utiliser(Entite utilisateur, List<Entite> equipe, List<Entite> ennemies) { }
}