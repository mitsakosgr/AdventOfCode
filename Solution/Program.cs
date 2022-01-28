var data = @"";

int playerTurn = 0;
var players = new int[2] {4, 8};
var scores = new int[2];

int die = 1;
int dieRolls = 0;

while (true)
{
    for (var i = 0; i < 3; ++i)
    {
        players[playerTurn] += die++;
        dieRolls++;
    }

    if (players[playerTurn] > 10)
    {
        players[playerTurn] %= 10;
        if (players[playerTurn] == 0)
            players[playerTurn] = 10;
    }

    var x = players[playerTurn];

    scores[playerTurn] += players[playerTurn];

    var s = scores[playerTurn];

    if (scores[playerTurn] >= 1000)
        break;

    playerTurn = (playerTurn + 1) % players.Length;
}

// loser
playerTurn = (playerTurn + 1) % players.Length;

Console.WriteLine(scores[playerTurn] * dieRolls);

return 0;