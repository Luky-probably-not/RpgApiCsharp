using Modeles.Character;

namespace Modeles.Objets;

public class PotionDoubleDegats() : Objet("Potion de Dégats", 0, "Double les dégats de la prochaine attaque")
{
    public override void Utiliser(Entite cible)
    {
        cible.Bonus.Add(new ("DoubleDegats",0));
    }

    public override void Utiliser(List<Entite> cibles) { }
}