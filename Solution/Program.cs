using System.Globalization;

var input = @"v...>>.vv>
.vv>>.vv..
>>.>v>...v
>>v>>.>.v.
v>v.vv.v..
>.>>..v...
.vv..>.>v.
v.v..>>v.v
....v..v.>";

var width = 0;
var height = 0;
var lines = input.Split('\n');
height = lines.Length;

var grid = new Node[0, 0];

var bottomFacing = new List<Node>();
var rightFacing = new List<Node>();

for (var i = 0; i < lines.Length; i++)
{
    for (var j = 0; j < lines[i].Length; j++)
    {
        if (width == 0)
        {
            width = lines[i].Length;
            grid = new Node[height, width];
        }

        grid[i, j] = new Node();

        switch (lines[i][j])
        {
            case '>':
                rightFacing.Add(grid[i, j]);
                grid[i, j].Occupied = true;
                break;
            case 'v':
                bottomFacing.Add(grid[i, j]);
                grid[i, j].Occupied = true;
                break;
            default:
                grid[i, j].Occupied = false;
                break;
        }
    }
}

for (var i = 0; i < height; ++i)
{
    for (int j = 0; j < width; j++)
    {
        grid[i, j].Bottom = grid[(i + 1) % height, j];
        grid[i, j].Right = grid[i, (j + 1) % width];
    }
}

// PrintGrid();

var counter = 1;
while (true)
{
    var moved = false;

    var newRight = new List<Node>(rightFacing.Count);
    foreach (var r in rightFacing)
    {
        if (r.Right.Occupied)
        {
            newRight.Add(r);
            continue;
        }

        moved = true;
        newRight.Add(r.Right);
    }

    foreach (var n in rightFacing)
        n.Occupied = false;
    foreach (var n in newRight)
        n.Occupied = true;
    rightFacing = newRight;

    var newBottom = new List<Node>(bottomFacing.Count);
    foreach (var r in bottomFacing)
    {
        if (r.Bottom.Occupied)
        {
            newBottom.Add(r);
            continue;
        }

        moved = true;
        newBottom.Add(r.Bottom);
    }
    foreach (var n in bottomFacing)
        n.Occupied = false;
    foreach (var n in newBottom)
        n.Occupied = true;
    bottomFacing = newBottom;

    // PrintGrid();

    if (!moved) break;
    ++counter;
}

Console.WriteLine(counter);

void PrintGrid()
{
    for (var i = 0; i < height; ++i)
    {
        for (var j = 0; j < width; ++j)
        {
            if (grid[i, j].Occupied)
            {
                if(rightFacing.Contains(grid[i,j]))
                    Console.Write('>');
                else
                    Console.Write('v');
            }
            else
            {
                Console.Write('.');
            }
        }
        Console.WriteLine();
    }
    Console.WriteLine();
    Console.WriteLine();
}


internal class Node
{
    public Node Bottom { get; set; }
    public Node Right { get; set; }

    public bool Occupied { get; set; } = false;

    public override string ToString()
    {
        return !Occupied ? "." : "#";

    }
}
