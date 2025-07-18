using Modeles.Character;

namespace Modeles.Objets;

public class PotionReductionDegats() : Objet("Potion de Résistance", 0.5f, "La prochaine attaque subit inflige -{ValeurPourcent} de dégats", false)
{
    public override void Utiliser(Entite cible)
    {
        cible.Bonus.Add(new ("DiviseDegats", 0));
    }

    public override void Utiliser(List<Entite> cibles) { }
}