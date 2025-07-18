using Modeles.Character;

namespace Modeles.Objets;

public class PotionEnergie() : Objet("Potion d'Energie", 7, "Rend {Valeur} point d'action à une cible", false)
{
    public override void Utiliser(Entite cible)
    {
        cible.PointAction += (int)Valeur;
    }

    public override void Utiliser(List<Entite> cibles) { }
}