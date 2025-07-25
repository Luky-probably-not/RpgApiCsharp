using Modeles.Character;

namespace Modeles.MoveSet.Capacites;

public class Nerf() : Capacite("Nerf", 0.5f, false, false, 5, 0,
    "Diminue la défense d'un ennemie de {ValeurPourcent}")
{
    public override void Utiliser(Entite utilisateur, Entite cible)
    {
        cible.Malus.Add(new ("Defense", 1 - Valeur));
        utilisateur.PointAction -= Cout;
    }

    public override void Utiliser(Entite utilisateur, List<Entite> cibles) { }
    public override void Utiliser(Entite utilisateur, List<Entite> equipe, List<Entite> ennemies) { }
}