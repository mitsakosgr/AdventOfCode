var data = @"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581";

var lines = data.Split('\n');
int height = lines.Length;
int width = lines[0].Length;

var risks = new int[height * 5, width * 5];
var weight = new int[height * 5, width * 5];
for (int i = 0; i < 5; i++)
{
    for (int j = 0; j < 5; j++)
    {
        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                var posX = x + i * height;
                var posY = y + j * width;

                if (posX == 0 && posY == 0)
                {
                    weight[posX, posY] = 0;
                    risks[posX, posY] = lines[0][0] - '0';
                }
                else
                {
                    weight[posX, posY] = int.MaxValue;
                    risks[posX, posY] = (lines[x][y] - '0' + i + j);
                    if (risks[posX, posY] >= 10)
                        risks[posX, posY] -= 9;
                }
            }
        }
    }
}

width *= 5;
height *= 5;

var queue = new Queue<(int, int)>();

queue.Enqueue((0, 0));

while (queue.TryDequeue(out (int, int) pos))
{
    // top
    if (pos.Item1 > 0 &&
        weight[pos.Item1 - 1, pos.Item2] > weight[pos.Item1, pos.Item2] + risks[pos.Item1 - 1, pos.Item2])
    {
        weight[pos.Item1 - 1, pos.Item2] = weight[pos.Item1, pos.Item2] + risks[pos.Item1 - 1, pos.Item2];
        queue.Enqueue((pos.Item1 - 1, pos.Item2));
    }

    // bottom
    if (pos.Item1 < height - 1 &&
        weight[pos.Item1 + 1, pos.Item2] > weight[pos.Item1, pos.Item2] + risks[pos.Item1 + 1, pos.Item2])
    {
        weight[pos.Item1 + 1, pos.Item2] = weight[pos.Item1, pos.Item2] + risks[pos.Item1 + 1, pos.Item2];
        queue.Enqueue((pos.Item1 + 1, pos.Item2));
    }

    // left
    if (pos.Item2 > 0 &&
        weight[pos.Item1, pos.Item2 - 1] > weight[pos.Item1, pos.Item2] + risks[pos.Item1, pos.Item2 - 1])
    {
        weight[pos.Item1, pos.Item2 - 1] = weight[pos.Item1, pos.Item2] + risks[pos.Item1, pos.Item2 - 1];
        queue.Enqueue((pos.Item1, pos.Item2 - 1));
    }

    // right
    if (pos.Item2 < width - 1 &&
        weight[pos.Item1, pos.Item2 + 1] > weight[pos.Item1, pos.Item2] + risks[pos.Item1, pos.Item2 + 1])
    {
        weight[pos.Item1, pos.Item2 + 1] = weight[pos.Item1, pos.Item2] + risks[pos.Item1, pos.Item2 + 1];
        queue.Enqueue((pos.Item1, pos.Item2 + 1));
    }
}

Console.WriteLine(weight[height - 1, width - 1]);