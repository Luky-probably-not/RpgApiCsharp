namespace Modeles.LabyrintheLogique;

public class Cellule
{
    public int Id { get; set; }
    public bool North { get; set; } = true;
    public bool South { get; set; } = true;
    public bool East { get; set; } = true;
    public bool West { get; set; } = true;
    public string? Type { get; set; } 

    public Cellule(int id, string type = " ")
    {
        Id = id;
        Type = type;
    }

    public Cellule(){}
}