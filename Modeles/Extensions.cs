namespace Modeles;

public static class Extensions
{
    public static string Display(this Dictionary<string, string> valeurs)
    {
        var result = "";
        foreach (KeyValuePair<string, string> kvp in valeurs)
        {
            result += $"{kvp.Key} : {kvp.Value};\n";
        }
        return result;
    }

    public static float FormuleDegats(float valeurAttaque, float attaque, float defense)
    {
        return Math.Clamp(valeurAttaque * attaque / defense, 1, 9999);
    }
}