using Modeles.Capacites;
namespace Modeles.Character.Personnage;

public class Barbare : Entite
{
    public Barbare() : base("Barbare", 20, 5, 50, 120, [
        " +−+♃",
        " |⎉| ",
        "  ⧋  ",
        " / \\ "
    ])
    {
        AjouterCapacite(new Frappe());
        AjouterCapacite(new MultiCoup());
        AjouterCapacite(new Sacrifice());
    }
}