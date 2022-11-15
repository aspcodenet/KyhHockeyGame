using Bogus.DataSets;
using KyhHockeyGame.Model;

namespace KyhHockeyGame.Services;

public class TeamGenerator
{
    private string []TeamExtraNames = {"Hockey","HC", "BK", "IF", "Tigers", "Fighters", "" };
    Bogus.Randomizer random = new Bogus.Randomizer();
    public Team Generate(HockeyRepository repository)
    {

        var address = new Bogus.DataSets.Address("sv");
        var person = new Bogus.DataSets.Name("sv");
        var team = new Team();
        team.City = address.City();
        team.Name = team.City + " " + random.ArrayElement(TeamExtraNames);
        
        while (true)
        {
            var player = new Player();
            player.Name = person.FirstName(Name.Gender.Male) + " " + person.LastName();
            player.DefenceSkills = random.Number(0, 100);
            player.OffenceSkills = random.Number(0, 100);

            var positionsToFill = new List<PlayerPosition>();
            var finnsredanGoalies = team.Players.Count(tp => tp.Position == PlayerPosition.Goalie);
            if(finnsredanGoalies < 3)
                positionsToFill.Add(PlayerPosition.Goalie);
            var finnsredanDefenece = team.Players.Count(tp => tp.Position == PlayerPosition.Defence);
            if (finnsredanDefenece < 8)
                positionsToFill.Add(PlayerPosition.Defence);
            var finnsredanForward= team.Players.Count(tp => tp.Position == PlayerPosition.Forward);
            if (finnsredanForward < 14)
                positionsToFill.Add(PlayerPosition.Forward);
            if (positionsToFill.Count == 0)
                break;
            player.Position = random.ListItem(positionsToFill);
            player.BirthDate = DateTime.Now.AddYears(-35)
                .AddDays( random.Number(0, 18*365) );

            player.JerseyNumber = GetFreeJersey(player.Position, team.Players);
            team.Players.Add(player);

        }

        return team;
    }

    private int GetFreeJersey(PlayerPosition playerPosition, List<Player> teamPlayers)
    {
        if (playerPosition == PlayerPosition.Goalie)
        {
            int []numbers = { 1, 30, 29, 20, 35, 36 };
            numbers = random.Shuffle(numbers).ToArray();
            foreach(var a in numbers)
                if (!teamPlayers.Any(e => e.JerseyNumber == a))
                    return a;
        }
        else if (playerPosition == PlayerPosition.Defence)
        {
            int[] numbers = { 2,3,4,5,6,7 };
            numbers = random.Shuffle(numbers).ToArray();
            foreach (var a in numbers)
                if (!teamPlayers.Any(e => e.JerseyNumber == a))
                    return a;
        }
        else if (playerPosition == PlayerPosition.Forward)
        {
            int[] numbers = {9,10,11,19,20,15,16,17 };
            numbers = random.Shuffle(numbers).ToArray();
            foreach (var a in numbers)
                if (!teamPlayers.Any(e => e.JerseyNumber == a))
                    return a;
        }
        foreach(var a in Enumerable.Range(2, 99))
            if (!teamPlayers.Any(e => e.JerseyNumber == a))
                return a;
        return 0;
    }
}