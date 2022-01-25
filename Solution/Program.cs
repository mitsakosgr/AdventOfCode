var data = @"target area: x=20..30, y=-10..-5";

data = data.Substring(15);
var parts = data.Split(", ");
var xRange = parts[0].Split("..");
var fromX = Convert.ToInt32(xRange[0]);
var toX = Convert.ToInt32(xRange[1]);

var yRange = parts[1].Substring(2).Split("..");
var toY = Convert.ToInt32(yRange[0]);
var fromY = Convert.ToInt32(yRange[1]);

// available initial speeds is bound by toX (cant be greater) and drag
var acceptableX = new List<int>();
// find initial x speed 
for (int i = 1; i <= toX; i++)
{
    int pos = 0;

    for (int velocity = i; velocity > 0; velocity--)
    {
        pos += velocity;

        // we went outside toX
        if (pos > toX)
            break;

        if (pos >= fromX && pos <= toX)
        {
            acceptableX.Add(i);
            break;
        }
    }
}

var acceptable = new List<(int, int)>();
foreach (var x in acceptableX)
{
    // toY negative
    for (int i = -toY; i >= toY; --i)
    {
        int velocity = x;
        int height = i;

        int posX = 0;
        int posY = 0;

        for (; height >= toY; velocity--, height--)
        {
            if (velocity < 0)
                velocity = 0;

            posX += velocity;
            posY += height;

            if (posY < toY)
                break;

            if (posX > toX)
                break;

            if (posX >= fromX && posX <= toX && posY >= toY && posY <= fromY)
            {
                acceptable.Add((x, i));
                break;
            }
        }
    }
}

Console.WriteLine(acceptable.Count);

int max = acceptable.MaxBy(i => i.Item2).Item2;

int count = 0;
for (int i = 1; i <= max; ++i)
    count += i;

Console.WriteLine(count);