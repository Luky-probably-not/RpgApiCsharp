using Modeles.Capacites;

namespace Modeles.Character.Personnage;

public class Sorcier : Entite
{
    public Sorcier() : base("Sorcier", 5, 8, 50, 125, [
        " ⚗  ᛉ",
        "⧶║╰-╡",
        "/ \\ │",
        "⎸ ⎹ ⤓"
    ])
    {
        AjouterCapacite(new Soin());
        AjouterCapacite(new Frappe());
        AjouterCapacite(new Energie());
        ReinitialiserValeurAction();
    }
}