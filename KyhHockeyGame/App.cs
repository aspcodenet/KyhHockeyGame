using System.Transactions;
using KyhHockeyGame.Model;
using KyhHockeyGame.Services;
using Spectre.Console;

namespace KyhHockeyGame
{
    public class App
    {
        public void Run()
        {
            AnsiConsole.Write(
                new FigletText("KYH HOCKEY MANAGER")
                    .LeftAligned()
                    .Color(Color.Red));

            var image = new CanvasImage("hockey.png");
            image.MaxWidth(20);
            AnsiConsole.Write(image);

            if (!AnsiConsole.Confirm("Ready to start ??")) return;

            GenerateSeedData();
            ShowTeamsMenu();
        }

        private void ShowTeamsMenu()
        {
            while (true)
            {
                AnsiConsole.Write(
                    new FigletText("Teams")
                        .LeftAligned()
                        .Color(Color.Red));
                var grid = new Grid();

                // Add columns 
                grid.AddColumn();
                grid.AddColumn();
                grid.AddColumn();


                // Add header row 
                grid.AddRow("Number", "Name", "Number of players");

                var repository = new HockeyRepository();
                var num = 1;
                foreach (var team in repository.GetAllTeams())
                {
                    grid.AddRow(num.ToString(), team.Name, team.Players.Count().ToString());
                    num++;
                }


                // Write to Console
                AnsiConsole.Write(grid);

                int sel = AnsiConsole.Prompt(
                    new TextPrompt<int>("Ange  [green]nummer[/] för lag du vill titta på?")
                        .PromptStyle("green")
                        .ValidationErrorMessage("[red]Ogiltigt nummer[/]")
                        .Validate(age =>
                        {
                            if(age < 0 ) return ValidationResult.Error("Inte mindre än 0");
                            if (age > num) return ValidationResult.Error("Så många lag finns inte");
                            return ValidationResult.Success();
                            
                            }));
                ShowTeam(repository.GetAllTeams()[sel]);

            }
        }

        private void ShowTeam(Team team)
        {
            AnsiConsole.Write(
                new FigletText(team.Name)
                    .LeftAligned()
                    .Color(Color.Red));
            var grid = new Grid();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();


            // Add header row 
            grid.AddRow("Position" , "Jersey", "Name", "Age", "Offence skills", "Defence skills");
            foreach (var player in team.Players)
            {
                grid.AddRow(player.Position.ToString(), player.JerseyNumber.ToString(), player.Name.ToString(),
                    player.GetAge().ToString(),
                    player.OffenceSkills.ToString(), player.DefenceSkills.ToString());

            }
            AnsiConsole.Write(grid);

            if (!AnsiConsole.Confirm("Go back? ??")) return;

        }

        private void GenerateSeedData()
        {
            var repository = new HockeyRepository();
            var teamGenerator = new TeamGenerator();
            while (repository.GetAllTeams().Count < 20)
            {
                var newTeam = teamGenerator.Generate(repository);
                repository.AddTeam(newTeam);
                repository.Save();
            }
        }
    }
}

