using Modeles.Character;

namespace Modeles.Objets;

public class PotionDoubleDegats() : Objet("Potion de Dégats", 1f, "La prochaine attaque inflige +{ValeurPourcent} de dégats supplémentaire", false)
{
    public override void Utiliser(Entite cible)
    {
        cible.Bonus.Add(new ("DoubleDegats",0));
    }

    public override void Utiliser(List<Entite> cibles) { }
}