using System.Text.Json.Serialization;
using KyhHockeyGame.Model;
using Newtonsoft.Json;

namespace KyhHockeyGame.Services;

public class HockeyRepository
{
    private List<Team> teams = new List<Team>();

    public HockeyRepository()
    {
        if(File.Exists("teams.txt"))
            teams = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText("teams.txt"));

    }
    public List<Team> GetAllTeams()
    {
        return teams;
    }

    public List<Player> GetAllPlayers()
    {
        return teams.SelectMany(e => e.Players).ToList();
    }

    public void AddTeam(Team newTeam)
    {
        teams.Add(newTeam);
    }

    public void Save()
    {
        File.WriteAllText("teams.txt",JsonConvert.SerializeObject(teams));
    }
}