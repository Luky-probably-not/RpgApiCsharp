using Modeles.Character;

namespace Modeles.Capacites;

public class Energie() : Capacite("Energie", 3, true, true, 5, 0, "Restaure {Valeur} Points d'Actions a toute l'équipe")
{
    public override void Utiliser(Entite utilisateur, Entite cible) { }

    public override void Utiliser(Entite utilisateur, List<Entite> cibles)
    {
        cibles.ForEach(cible => cible.PointAction += (int)Valeur);
        utilisateur.PointAction -= Cout;
    }
}