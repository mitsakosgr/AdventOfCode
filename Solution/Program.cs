var data = @"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526";

var lines = data.Split('\n');

var height = lines.Length;
var width = lines[0].Length;

var grid = new Octopus[height, width];

for (int h = 0; h < height; h++)
{
    for (int w = 0; w < width; w++)
    {
        grid[h, w] = new Octopus(lines[h][w] - '0');
    }
}

for (int h = 0; h < height; h++)
{
    for (int w = 0; w < width; w++)
    {
        if (h > 0)
            grid[h, w].AddAdjacent(grid[h - 1, w]);

        if (w > 0)
            grid[h, w].AddAdjacent(grid[h, w - 1]);

        if (h > 0 && w > 0)
            grid[h, w].AddAdjacent(grid[h - 1, w - 1]);

        if (h < height - 1)
            grid[h, w].AddAdjacent(grid[h + 1, w]);

        if (w < width - 1)
            grid[h, w].AddAdjacent(grid[h, w + 1]);

        if (h < height - 1 && w < width - 1)
            grid[h, w].AddAdjacent(grid[h + 1, w + 1]);

        if (h > 0 && w < width - 1)
            grid[h, w].AddAdjacent(grid[h - 1, w + 1]);

        if (w > 0 && h < height - 1)
            grid[h, w].AddAdjacent(grid[h + 1, w - 1]);
    }
}

int count = 0;
for (int i = 0; i < 100; i++)
{
    for (int h = 0; h < height; h++)
    {
        for (int w = 0; w < width; w++)
        {
            count += grid[h, w].Step();
        }
    }

    for (int h = 0; h < height; h++)
    {
        for (int w = 0; w < width; w++)
        {
            // if (grid[h, w].Flashed)
            // {
            //     Console.BackgroundColor = ConsoleColor.White;
            //     Console.ForegroundColor = ConsoleColor.Black;
            // }
            // else
            //     Console.ResetColor();

            // Console.Write(grid[h, w].Value);
            grid[h, w].Reset();
        }

        // Console.WriteLine();
    }
    // Console.WriteLine();
}

Console.WriteLine(count);
return 0;


internal class Octopus
{
    public int Value { get; private set; }
    public bool Flashed { get; private set; }
    public Octopus(int value)
    {
        Value = value;
    }

    private List<Octopus> _adjacent = new();

    public void AddAdjacent(Octopus octopus)
    {
        _adjacent.Add(octopus);
    }

    public int Step()
    {
        if (Flashed)
            return 0;

        int count = 0;

        Value += 1;

        if (Value == 10)
        {
            count = 1;

            Value = 0;
            Flashed = true;

            foreach (var o in _adjacent)
                count += o.Step();
        }

        return count;
    }

    public void Reset()
    {
        Flashed = false;
    }
}
