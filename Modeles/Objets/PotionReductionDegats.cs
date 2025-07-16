using Modeles.Character;

namespace Modeles.Objets;

public class PotionReductionDegats() : Objet("Potion de Résistance", 0, "Divise par 2 les prochains dégats recus")
{
    public override void Utiliser(Entite cible)
    {
        cible.Bonus.Add(new ("DiviseDegats", 0));
    }

    public override void Utiliser(List<Entite> cibles) { }
}