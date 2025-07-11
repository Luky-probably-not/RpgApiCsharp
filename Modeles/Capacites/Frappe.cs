using Modeles.Character;

namespace Modeles.Capacites;

public class Frappe() : Capacite("Frappe", 5, false, false, 0, 3, "Inflige {Valeur} dégats un ennemi")
{
    public override void Utiliser(Entite utilisateur, Entite cible)
    {
        cible.Blesser((int)Valeur,utilisateur.Attaque);
        utilisateur.PointAction -= Cout;
    }

    public override void Utiliser(Entite utilisateur, List<Entite> cibles){ }
}