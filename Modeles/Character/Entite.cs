namespace Modeles.Character;

public class Entite(string nom, int att, int def, int pv, int vitesse, List<string> sprite)
{
    public string Nom { get; protected set; } = nom;

    public int AttaqueDeBase {get; protected set;} = att;
    public int Attaque {get; set;} = att;
    public int DefenseDeBase {get; protected set;} = def;
    public int Defense {get; set;} = def;
    public int PointDeVie {get; set;} = pv;
    public int PointDeVieMax {get; protected set;} = pv;
    public int Vitesse {get; set;} = vitesse;
    public bool Vivant { get; protected set; } = true;

    private int _valeurAction;
    public int ValeurAction
    {
        get => _valeurAction; 
        set => _valeurAction = Math.Max(0, ValeurAction - value);
    }
    public int PointAction { get; set; }
    public List<string> Sprite { get; protected set; } = sprite;
    public Dictionary<string, float> Bonus = new();
    public Dictionary<string, float> Malus = new();

    public void Blesser(int puissanceAttaque, int attaqueAssaillant, bool ignoreDefense = false)
    {
        var degats = puissanceAttaque * attaqueAssaillant;
        if (!ignoreDefense)
            degats /= (Defense + 100) / 100;
        PointDeVie = Math.Max(0, PointDeVie - degats);
        if (PointDeVie == 0)
            Vivant = false;
    }

    public void Soigner(float puissanceSoin)
    {
        PointDeVie = Math.Min(PointDeVieMax, PointDeVie + (int)(PointDeVieMax * puissanceSoin));
    }

    public void Ressuciter(float puissanceSoin)
    {
        if (!Vivant)
            Vivant = true;
        PointDeVie = Math.Min(PointDeVieMax, PointDeVie + (int)(PointDeVieMax * puissanceSoin));
    }

    public void ReinitialiserValeurAction()
    {
        ValeurAction = (int)Math.Round(10000f / Vitesse);
    }
}