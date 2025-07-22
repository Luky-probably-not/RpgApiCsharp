using Modeles.Character;

namespace Modeles.MoveSet.Capacites;

public class Buff : Capacite
{
    public Buff() : base("Buff", 0.3f, false, true, 5, 0,
        "Augmente l'attaque d'un allié de {ValeurPourcent} de son attaque de base") { }

    public override void Utiliser(Entite utilisateur, Entite cible)
    {
        cible.Bonus.Add(new ("Attaque", 1 + Valeur));
        utilisateur.PointAction -= Cout;
    }

    public override void Utiliser(Entite utilisateur, List<Entite> cibles) { }
    public override void Utiliser(Entite utilisateur, List<Entite> equipe, List<Entite> ennemies) { }
}