using Modeles.Character;
using Modeles.Resources;
using static Modeles.Resources.Resource;

namespace Modeles.Capacites;

public class Buff() : Capacite("Buff", 0.5f, false, true, 5, 0, "Augmente l'attaque d'un allié de {ValeurPourcent} de son attaque de base")
{
    public override void Utiliser(Entite utilisateur, Entite cible)
    {
        cible.Bonus.Add(new ("Attaque", 1 + Valeur));
        utilisateur.PointAction -= Cout;
    }

    public override void Utiliser(Entite utilisateur, List<Entite> cibles) { }
}