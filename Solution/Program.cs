var players = new [] {4 - 1, 8 - 1}; // 9, 3

// [playerTurn][pos1][pos2][s1][s2][w1,w2]
var memoization = new long[2][][][][][];
for (int m = 0; m < 2; ++m)
{
    memoization[m] = new long[10][][][][];
    for (int i = 0; i < 10; ++i)
    {
        memoization[m][i] = new long[10][][][];
        for (int j = 0; j < 10; ++j)
        {
            memoization[m][i][j] = new long[21][][];
            for (int k = 0; k < 21; ++k)
            {
                memoization[m][i][j][k] = new long[21][];
                for (int l = 0; l < 21; ++l)
                {
                    memoization[m][i][j][k][l] = new long[2] {-1, -1};
                }
            }
        }
    }
}

var wins = Play(0, new [] {0, 0}, players);

if(wins[0] > wins[1])
    Console.WriteLine($"Player 1 wins at {wins[0]} universes over {wins[1]}");
else
    Console.WriteLine($"Player 2 wins at {wins[1]} universes over {wins[0]}");

return 0;

long[] Play(int player, int[] scores, int[] positions)
{
    var wins = new long[2];

    // already known result for current position
    if (memoization[player][positions[0]][positions[1]][scores[0]][scores[1]][0] != -1)
    {
        wins[0] = memoization[player][positions[0]][positions[1]][scores[0]][scores[1]][0];
        wins[1] = memoization[player][positions[0]][positions[1]][scores[0]][scores[1]][1];
        return wins;
    }

    var initialPosition = positions[player];
    var initialScore = scores[player];

    for (int i = 1; i <= 3; ++i)
    {
        for (int j = 1; j <= 3; ++j)
        {
            for (int k = 1; k <= 3; ++k)
            {
                positions[player] = (positions[player] + i + j + k) % 10;
                scores[player] += positions[player] + 1;

                if (scores[player] >= 21)
                {
                    wins[player] += 1;
                }
                else
                {
                    var w = Play((player + 1) % 2, scores, positions);
                    wins[0] += w[0];
                    wins[1] += w[1];
                }

                positions[player] = initialPosition;
                scores[player] = initialScore;
            }
        }
    }


    memoization[player][positions[0]][positions[1]][scores[0]][scores[1]][0] = wins[0];
    memoization[player][positions[0]][positions[1]][scores[0]][scores[1]][1] = wins[1];

    return wins;
}