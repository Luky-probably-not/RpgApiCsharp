
using System.Drawing;
using Modeles.Character;
using Pastel;
using static Modeles.Extensions;

namespace Modeles.GameManager;
public interface ICombat
{
    public List<Entite> Equipe { get; set; }
    public List<Entite> Ennemies{ get; set; }
    public List<Entite> OrdreAction{ get; set; }

    void RecupererOrdreAction();

}
