using System.Text.RegularExpressions;

var input = @"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20";

var res = 0;

var posHead = (0, 0);
var tails = new List<(int, int)>(9);
for (var i = 0; i < 9; ++i)
{
    tails.Add(new(0, 0));
}

var positions = new HashSet<(int, int)>();
positions.Add((0, 0));

foreach (var l in input.Split('\n'))
{
    var movement = l.Split(' ');

    var steps = int.Parse(movement[1]);

    for (var i = 0; i < steps; ++i)
    {
        switch (movement[0])
        {
            case "R":
                posHead.Item2 += 1;
                break;
            case "L":
                posHead.Item2 -= 1;
                break;
            case "U":
                posHead.Item1 -= 1;
                break;
            case "D":
                posHead.Item1 += 1;
                break;
        }

        tails[0] = Reposition(posHead, tails[0]);

        for (int j = 1; j < 9; j++)
        {
            tails[j] = Reposition(tails[j - 1], tails[j]);

            if (j == 8)
            {
                positions.Add(tails[j]);
                // Console.WriteLine(tails[j]);
            }
        }
    }
}

(int, int) Reposition((int, int) head, (int, int) current)
{
    if (Math.Abs(head.Item1 - current.Item1) <= 1 && Math.Abs(head.Item2 - current.Item2) <= 1)
        return current;

    if (head.Item1 == current.Item1)
        current.Item2 += head.Item2 > current.Item2 ? 1 : -1;
    else if (head.Item2 == current.Item2)
        current.Item1 += head.Item1 > current.Item1 ? 1 : -1;
    else
    {
        current.Item1 += head.Item1 > current.Item1 ? 1 : -1;
        current.Item2 += head.Item2 > current.Item2 ? 1 : -1;
    }

    return current;
}




Console.WriteLine(positions.Count);