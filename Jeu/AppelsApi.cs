using Modeles.Character;
using Modeles.LabyrintheLogique;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using static Modeles.Extensions;

namespace Jeu;

public class AppelsApi
{
    private static readonly HttpClient Client = SetupClient("http://localhost:5019");
    
    public static async Task<Labyrinthe?> GetLabyrinthe(int taille)
    {
        HttpResponseMessage response = await Client.GetAsync($"/api/labyrinthe/{taille}");
        var laby = await response.Content.ReadFromJsonAsync<Labyrinthe>();
        return laby;
    }

    public static async Task<List<Entite>?> GetEquipe()
    {
        HttpResponseMessage response = await Client.GetAsync($"/api/equipe");
        var equipe = await response.Content.ReadFromJsonAsync<List<Entite>>(OptionsJson);
        return equipe;
    }

    private static HttpClient SetupClient(string adresseApi)
    {
        HttpClient client = new()
        {
            BaseAddress = new Uri(adresseApi)
        };

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        return client;
    }
}