namespace Modeles.LabyrintheLogique;

public class Labyrinthe
{
    public List<List<Cellule>> Laby { get; set; } = [];
    public int Taille { get; set; }

    public Labyrinthe(int taille)
    {
        Taille = taille;
        var id = 0;
        for (var i = 0; i < Taille; i++)
        {
            var list = new List<Cellule>();
            for (var f = 0; f < Taille; f++)
            {
                list.Add(new Cellule(id));
                id++;
            }
            Laby.Add(list);
        }
    }
    #region Display

    public void Display(bool id = false)
    {
        Console.Clear();
        DisplayTop();
        Laby.Take(Taille - 1).ToList().ForEach(line =>
        {
            DisplayLine(line, id);
            DisplaySeparator(Laby.IndexOf(line));
        });
        DisplayLine(Laby[^1], id);
        DisplayBot();
    }

    private static void DisplayLine(List<Cellule> ligne, bool id = false)
    {
        Console.Write("│");
        foreach (var cell in ligne.Take(ligne.Count - 1))
        {
            Console.Write(" {0} ", id ? cell.Id < 10 ? cell.Id + " " : cell.Id : cell.Type + " ");
            Console.Write(cell.East ? "│" : " ");
        }
        Console.WriteLine(" {0} │", id ? ligne.Last().Id < 10 ? ligne.Last().Id + " " : ligne.Last().Id : ligne.Last().Type + " ");
    }

    private void DisplaySeparator(int idLigne)
    {
        var cell = Laby[idLigne][0];
        Console.Write(cell.South ? "├" : "│");
        for (var i = 0; i < Taille - 1; i++)
        {
            cell = Laby[idLigne][i];
            var cellMiroir = Laby[idLigne + 1][i + 1];
            Console.Write(cell.South ? "────" : "    ");
            Console.Write(Croisement(cell, cellMiroir));
        }
        Console.WriteLine(Laby[idLigne].Last().South ? "────┤" : "    │");

    }

    private static string Croisement(Cellule cell, Cellule cellMiroir)
    {
        switch (cell)
        {
            case { South: true, East: true }:
                if (cellMiroir.West)
                    return cellMiroir.North ? "┼" : "┤";
                return cellMiroir.North ? "┴" : "┘";
            case { South: true }:
                if (cellMiroir.West)
                    return cellMiroir.North ? "┬" : "┐";
                return cellMiroir.North ? "─" : "╴";
            case { East: true }:
                if (cellMiroir.West)
                    return cellMiroir.North ? "├" : "│";
                return cellMiroir.North ? "└" : "╵";
            default:
                if (cellMiroir.West)
                    return cellMiroir.North ? "┌" : "╷";
                return cellMiroir.North ? "╶" : " ";
        }
    }

    private void DisplayTop()
    {
        Console.Write("┌");
        for (var i = 0; i < Laby.Count - 1; i++)
        {
            Console.Write("────");
            Console.Write(Laby[0][i].East ? "┬" : "─");
        }
        Console.Write("────");
        Console.WriteLine("┐");
    }

    private void DisplayBot()
    {
        Console.Write("└");
        for (var i = 0; i < Laby.Count - 1; i++)
        {
            Console.Write("────");
            Console.Write(Laby.Last()[i].East ? "┴" : "─");
        }
        Console.Write("────");
        Console.WriteLine("┘");
    }

    #endregion

    #region Generation

    public void Generation()
    {
        var copie = new Labyrinthe(Taille);

        var rand = new Random();
        while (ContinuerGeneration(copie))
        {
            var idLig = rand.Next(Taille);
            var idCol = rand.Next(Taille);
            var choix = rand.Next(4);
            while (!VerifierDirection(idLig, idCol, choix))
                choix = rand.Next(4);
            if (!VerifierId(copie, idLig, idCol, choix))
                continue;
            if (copie.Laby[idLig][idCol].Id == 0)
                continue;

            copie = ConnecterCellule(copie, idLig, idCol, choix);
        }

        Laby = copie.Laby;
        InitalialiserDebut();
        GenererRencontre();
        GenererMagasin();
        GenererMiniJeu();
    }
    
    private void InitalialiserDebut()
    {
        Laby[0][0].Type = "A";
        Laby[^1][^1].Type = "B";
    }

    private static bool ContinuerGeneration(Labyrinthe laby)
    {
        foreach (var ligne in laby.Laby)
        {
            foreach (var cell in ligne)
            {
                if (cell.Id != 0)
                    return true;
            }
        }
        return false;
    }

    private bool VerifierDirection(int idLig, int idCol, int choix)
    {
        if (idLig == 0 && choix == 0)
            return false;
        if (idLig == Taille - 1 && choix == 2)
            return false;
        if (idCol == 0 && choix == 3)
            return false;
        if (idCol == Taille - 1 && choix == 1)
            return false;
        return true;
    }

    private static bool VerifierId(Labyrinthe laby, int idLig, int idCol, int choix)
    {
        Cellule direction = new(0);
        switch (choix)
        {
            case 0:
                direction = laby.Laby[idLig - 1][idCol];
                break;
            case 1:
                direction = laby.Laby[idLig][idCol + 1];
                break;
            case 2:
                direction = laby.Laby[idLig + 1][idCol];
                break;
            case 3:
                direction = laby.Laby[idLig][idCol - 1];
                break;
        }

        return direction.Id != laby.Laby[idLig][idCol].Id;
    }

    private static Labyrinthe ChangementId(Labyrinthe laby, int idPerdu, int idRemplacant)
    {
        foreach (var cell in from ligne in laby.Laby from cell in ligne where cell.Id == idPerdu select cell)
        {
            cell.Id = idRemplacant;
        }

        return laby;
    }

    private static Labyrinthe ConnecterCellule(Labyrinthe laby, int idLig, int idCol, int choix)
    {
        var centre = laby.Laby[idLig][idCol];
        var direction = "";
        var localisation = new Cellule(999);
        var NS = 0;
        var WE = 0;
        switch (choix)
        {
            case 0:
                localisation = laby.Laby[idLig - 1][idCol];
                NS = -1;
                direction = "n";
                break;
            case 1:
                localisation = laby.Laby[idLig][idCol + 1];
                WE = 1;
                direction = "e";
                break;
            case 2:
                localisation = laby.Laby[idLig + 1][idCol];
                NS = 1;
                direction = "s";
                break;
            case 3:
                localisation = laby.Laby[idLig][idCol - 1];
                WE = -1;
                direction = "w";
                break;
        }

        var idMax = Math.Max(localisation.Id, centre.Id);
        var idMin = Math.Min(localisation.Id, centre.Id);
        laby = ChangementId(laby, idMax, idMin);
        switch (direction)
        {
            case "n":
                centre.North = false;
                localisation.South = false;
                break;
            case "s":
                centre.South = false;
                localisation.North = false;
                break;
            case "w":
                centre.West = false;
                localisation.East = false;
                break;
            case "e":
                centre.East = false;
                localisation.West = false;
                break;
        }

        laby.Laby[idLig][idCol] = centre;
        laby.Laby[idLig + NS][idCol + WE] = localisation;
        return laby;
    }

    private void GenererRencontre()
    {
        for (var i = 0; i < Taille/10; i++)
        {
            var rand = new Random();
            var col = rand.Next(Taille);
            var lig = rand.Next(Taille);
            while (Laby[col][lig].Type != " ")
            {
                col = rand.Next(Taille);
                lig = rand.Next(Taille);
            }

            Laby[col][lig].Type = "E";
        }
    }

    private void GenererMagasin()
    {
        for (var i = 0; i < Taille/5; i++)
        {
            var rand = new Random();
            var col = rand.Next(Taille);
            var lig = rand.Next(Taille);
            while (Laby[col][lig].Type != " ")
            {
                col = rand.Next(Taille);
                lig = rand.Next(Taille);
            }

            Laby[col][lig].Type = "S";
        }
    }

    private void GenererMiniJeu()
    {
        for (var i = 0; i < Taille; i++)
        {
            var rand = new Random();
            var col = rand.Next(Taille);
            var lig = rand.Next(Taille);
            while (Laby[col][lig].Type != " ")
            {
                col = rand.Next(Taille);
                lig = rand.Next(Taille);
            }

            Laby[col][lig].Type = "M";
        }
    }

    #endregion
}