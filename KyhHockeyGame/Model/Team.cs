namespace KyhHockeyGame.Model;

public class Team
{
    public Team()
    {
        Players = new List<Player>();
    }
    public string Name { get; set; }
    public string City { get; set; }
    public List<Player> Players { get; set; }
}

public enum PlayerPosition
{
    Goalie,
    Defence,
    Forward
}


public class Player
{
    public int GetAge()
    {
        return Convert.ToInt32((DateTime.Now - BirthDate).TotalDays / 365);
    }
    public PlayerPosition Position { get; set; }
    public DateTime BirthDate { get; set; }
    public int JerseyNumber { get; set; }
    public string Name { get; set; }
    public int OffenceSkills { get; set; }
    public int DefenceSkills { get; set; }
}