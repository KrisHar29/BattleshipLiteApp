using BattleshipLiteLibrary;
using BattleshipLiteLibrary.Models;

WelcomeMessage();

PlayerInfoModel activePlayer = CreatePlayer("Player 1");
PlayerInfoModel opponent = CreatePlayer("Player 2");
PlayerInfoModel winner = null;

do
{
    // display grid from player 1 on where they fired
    DisplayShotGrid(activePlayer);
    Console.WriteLine();
    Console.WriteLine();
    //ask player 1 for a shot
    // determine if shot is valid
    // detemine shot result
    RecordPlayerShot(activePlayer, opponent);

    // determine if game should continue
    bool doesGameContinue = GameLogic.PlayerStillActive(opponent);
    // if over set player 1 as winner
    // else swap positions
    if (doesGameContinue == true)
    {
        //use tuple swap positions
        (activePlayer, opponent) = (opponent, activePlayer);

    }
    else { winner = activePlayer; }

} while (winner == null);

IdentifyWinner(winner);
Console.ReadLine();

void IdentifyWinner(PlayerInfoModel winner)
{
    Console.WriteLine($"Congratulations to {winner.UsersName} for winning!");
    Console.WriteLine($"{winner.UsersName} took {GameLogic.GetShotCount(winner)} shots.");
}

void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
{
    bool isValidShot = false;
    string row = "";
    int column = 0;
    do
    {
        string shot = AskForShot();
        (row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
        isValidShot = GameLogic.ValidateShot(activePlayer, row, column);

        if (isValidShot == false)
        {
            Console.WriteLine("Invalid shot location. Please try again.");
        }
    } while (!isValidShot);
    // asks for a shot (b2)
    // determine what row and column
    // determine if valid shot
    // go backt o beginning if not valid


    // determine shot result
    bool isAHit = GameLogic.IdentidyShotResult(opponent, row, column);
    // record results
    GameLogic.MarkShotResult(activePlayer, row, column, isAHit);

}

string AskForShot()
{
    Console.Write("Please enter your shot selection.");
    string output = Console.ReadLine();

    return output;
}

void DisplayShotGrid(PlayerInfoModel activePlayer)
{
    string currentRow = activePlayer.ShotGrid[0].SpotLetter;

    foreach (var gridSpot in activePlayer.ShotGrid)
    {
        if (gridSpot.SpotLetter != currentRow)
        {
            Console.WriteLine();
            currentRow = gridSpot.SpotLetter;
        }

        if (gridSpot.Status == GridSpotStatus.Empty)
        {
            Console.Write($" { gridSpot.SpotLetter }{ gridSpot.SpotNumber }");
        }
        else if (gridSpot.Status == GridSpotStatus.Hit)
        {
            Console.Write(" X ");
        }
        else if (gridSpot.Status == GridSpotStatus.Miss)
        {
            Console.Write(" O ");
        }
        else
        {
            Console.Write(" ? ");
        }
    }
}

static void WelcomeMessage()
{
    Console.WriteLine("Welcome to Battleship Lite");
    Console.WriteLine("Created by Kris");
    Console.WriteLine();
}

static PlayerInfoModel CreatePlayer(string playerTitle)
{
    PlayerInfoModel output = new PlayerInfoModel();
    Console.WriteLine($"Player information for {playerTitle}");
    //ask user name
    output.UsersName = AskForUsersName();
    // load up shot grid
    GameLogic.InitializeGrid(output);
    // ask user for ship placements
    PlaceShips(output);
    //clear
    Console.Clear();

    return output;
}

static string AskForUsersName()
{
    Console.Write("What is your name: ");
    string output = Console.ReadLine();
    return output;
}


static void PlaceShips(PlayerInfoModel model)
{
    do
    {
        Console.Write($"Where do you want to place ship number {model.ShipLocations.Count +1}: ");
        string location = Console.ReadLine();

        bool isValidLocation = GameLogic.PlaceShip(model, location);

        if (isValidLocation == false)
        {
            Console.WriteLine("That was not a valid location. Please try again.");
        }

    } while (model.ShipLocations.Count < 5);
}