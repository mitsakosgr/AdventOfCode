var data = @"";

var nodes = new List<Node>()
{
    new Node(), // 0
    new Node(), // 1
    new Node(), // 2
    new Node(), // 3
    new Node(), // 4
    new Node(), // 5
    new Node(), // 6
    new Node('A'), // 7 
    new Node('A'), // 8
    new Node('A'), // 9
    new Node('A'), // 10
    new Node('B'), // 11
    new Node('B'), // 12
    new Node('B'), // 13
    new Node('B'), // 14
    new Node('C'), // 15
    new Node('C'), // 16
    new Node('C'), // 17
    new Node('C'), // 18
    new Node('D'), // 19
    new Node('D'), // 20
    new Node('D'), // 21
    new Node('D'), // 22
};

Amphipod.Bases = new Node[4] { nodes[10], nodes[14], nodes[18], nodes[22] };

nodes[0].AddNeighbor(nodes[1], 1);
nodes[1].AddNeighbor(nodes[2], 2);
nodes[2].AddNeighbor(nodes[3], 2);
nodes[3].AddNeighbor(nodes[4], 2);
nodes[4].AddNeighbor(nodes[5], 2);
nodes[5].AddNeighbor(nodes[6], 1);

nodes[7].AddNeighbor(nodes[1], 2);
nodes[7].AddNeighbor(nodes[2], 2);
nodes[7].AddNeighbor(nodes[8], 1);
nodes[8].AddNeighbor(nodes[9], 1);
nodes[9].AddNeighbor(nodes[10], 1);

nodes[11].AddNeighbor(nodes[2], 2);
nodes[11].AddNeighbor(nodes[3], 2);
nodes[11].AddNeighbor(nodes[12], 1);
nodes[12].AddNeighbor(nodes[13], 1);
nodes[13].AddNeighbor(nodes[14], 1);

nodes[15].AddNeighbor(nodes[3], 2);
nodes[15].AddNeighbor(nodes[4], 2);
nodes[15].AddNeighbor(nodes[16], 1);
nodes[16].AddNeighbor(nodes[17], 1);
nodes[17].AddNeighbor(nodes[18], 1);

nodes[19].AddNeighbor(nodes[4], 2);
nodes[19].AddNeighbor(nodes[5], 2);
nodes[19].AddNeighbor(nodes[20], 1);
nodes[20].AddNeighbor(nodes[21], 1);
nodes[21].AddNeighbor(nodes[22], 1);

// 7 - B
// 8 - D
// 9 - D
// 10 - A

// 11 - C
// 12 - C
// 13 - B
// 14 - D

// 15 - B
// 16 - B
// 17 - A
// 18 - C

// 19 - D
// 20 - A
// 21 - C
// 22 - A


var amphipods = new List<Amphipod>
{
    new Amphipod('B', nodes[7]),
    new Amphipod('D', nodes[8]),
    new Amphipod('D', nodes[9]),
    new Amphipod('C', nodes[10]),
    new Amphipod('D', nodes[11]),
    new Amphipod('C', nodes[12]),
    new Amphipod('B', nodes[13]),
    new Amphipod('D', nodes[14]),
    new Amphipod('C', nodes[15]),
    new Amphipod('B', nodes[16]),
    new Amphipod('A', nodes[17]),
    new Amphipod('B', nodes[18]),
    new Amphipod('A', nodes[19]),
    new Amphipod('A', nodes[20]),
    new Amphipod('C', nodes[21]),
    new Amphipod('A', nodes[22]),
};

PrintMaze();

var minScore = long.MaxValue;
var move = amphipods.Where(i => i.InPlace == false).ToList();
Move(move);

Console.WriteLine(minScore);

return 0;

void Move(List<Amphipod> currentAmphipods, long currentScore = 0)
{
    if (currentAmphipods.Count == 0)
        return;

    // everything we add is gonna go over min
    if (currentScore >= minScore)
        return;

    foreach (var a in currentAmphipods)
    {
        // // if already something moved or this is not the last item try not moving it
        // if (currentAmphipods.Count > 1)
        // {
        //     var remainingAmphipods = currentAmphipods.Where(i => i != a).ToList();
        //     Move(remainingAmphipods, currentScore);
        // }

        // if it can move
        if (TryGetMovements(a, out var movements))
        {
            foreach (var m in movements)
            {
                currentScore += m.Item2 * a.Weight;
                var prevPos = a.Position;

                a.Position = m.Item1;

                // Console.WriteLine("move");
                // PrintMaze();

                if (amphipods.All(i => i.InPlace))
                {
                    // if (currentScore != long.MaxValue)
                    //     Console.WriteLine($"Found solution with score {currentScore}");

                    if (currentScore < minScore)
                    {
                        minScore = currentScore;
                        Console.WriteLine($"Found new best solution with score {currentScore}");
                    }
                }
                else
                {
                    Move(amphipods.Where(i => i.InPlace == false).ToList(), currentScore);
                }

                a.Position = prevPos;
                currentScore -= m.Item2 * a.Weight;

                // Console.WriteLine("rollback");
                // PrintMaze();
            }
        }
    }
    return;
}

void PrintMaze()
{
    Console.WriteLine("#############");

    Console.Write("#");
    Console.Write(nodes[0].Contains?.Type ?? '.');
    Console.Write(nodes[1].Contains?.Type ?? '.');
    Console.Write('.');
    Console.Write(nodes[2].Contains?.Type ?? '.');
    Console.Write('.');
    Console.Write(nodes[3].Contains?.Type ?? '.');
    Console.Write('.');
    Console.Write(nodes[4].Contains?.Type ?? '.');
    Console.Write('.');
    Console.Write(nodes[5].Contains?.Type ?? '.');
    Console.Write(nodes[6].Contains?.Type ?? '.');
    Console.WriteLine("#");

    Console.Write("###");
    Console.Write(nodes[7].Contains?.Type ?? '.');
    Console.Write('#');
    Console.Write(nodes[11].Contains?.Type ?? '.');
    Console.Write('#');
    Console.Write(nodes[15].Contains?.Type ?? '.');
    Console.Write('#');
    Console.Write(nodes[19].Contains?.Type ?? '.');
    Console.WriteLine("###");

    Console.Write("  #");
    Console.Write(nodes[8].Contains?.Type ?? '.');
    Console.Write('#');
    Console.Write(nodes[12].Contains?.Type ?? '.');
    Console.Write('#');
    Console.Write(nodes[16].Contains?.Type ?? '.');
    Console.Write('#');
    Console.Write(nodes[20].Contains?.Type ?? '.');
    Console.WriteLine("#  ");

    Console.Write("  #");
    Console.Write(nodes[9].Contains?.Type ?? '.');
    Console.Write('#');
    Console.Write(nodes[13].Contains?.Type ?? '.');
    Console.Write('#');
    Console.Write(nodes[17].Contains?.Type ?? '.');
    Console.Write('#');
    Console.Write(nodes[21].Contains?.Type ?? '.');
    Console.WriteLine("#  ");

    Console.Write("  #");
    Console.Write(nodes[10].Contains?.Type ?? '.');
    Console.Write('#');
    Console.Write(nodes[14].Contains?.Type ?? '.');
    Console.Write('#');
    Console.Write(nodes[18].Contains?.Type ?? '.');
    Console.Write('#');
    Console.Write(nodes[22].Contains?.Type ?? '.');
    Console.WriteLine("#  ");

    Console.WriteLine("  #########  ");
    Console.WriteLine();
}

bool TryGetMovements(Amphipod a, out List<(Node, int)> movements)
{
    movements = new List<(Node, int)>();
    // already in place
    if (a.InPlace)
        return false;

    // try to find a route home
    var targetNodes = nodes.Where(i => i.HomeFor == a.Type && i.Contains == null).ToList();
    Node target = null;

    if (targetNodes.Count > 0)
    {
        var inPlace = amphipods.Count(i => i.Type == a.Type && i.InPlace);
        if (targetNodes.Count + inPlace == 4)
            target = targetNodes.Last();
    }

    if (target != null) // target found
    {
        // it can go home, ignore everything else
        if (a.Position.TryFindWeightToNode(target, out var weight))
        {
            movements.Add((target, weight));
            return true;
        }
    }

    // if it is not in corridor try to get there
    if (a.Position.HomeFor != null)
    {
        var emptyCorridor = nodes.Where(i => i.HomeFor == null && i.Contains == null).ToList();
        foreach (var n in emptyCorridor)
        {
            if (a.Position.TryFindWeightToNode(n, out var weight))
            {
                movements.Add((n, weight));
            }
        }
    }

    return movements.Any();
}

internal class Amphipod
{
    public static Node[] Bases { get; set; } = { };

    private static int _counter = 0;

    public int Id { get; }

    private Node _position;
    public Amphipod(char type, Node position)
    {
        Id = _counter++;
        Type = type;
        Position = position;
    }

    public char Type { get; }
    public bool InPlace { get; set; }

    public int Weight
    {
        get
        {
            return Type switch
            {
                'A' => 1,
                'B' => 10,
                'C' => 100,
                'D' => 1000,
                _ => throw new Exception($"Unknown TYPE {Type}")
            };
        }
    }

    public Node Position
    {
        get => _position;
        set
        {
            if (_position != null)
                _position.Contains = null;

            InPlace = false;

            _position = value;
            value.Contains = this;

            if (_position.HomeFor == null || _position.HomeFor != Type)
                return;

            if (_position.HomeFor == Type)
            {
                var b = Bases[Type - 'A'];
                var visited = new List<Node>();
                while (b.Contains?.Type == b.HomeFor)
                {
                    b.Contains.InPlace = true;
                    visited.Add(b);

                    // Console.WriteLine($"Amphipod {Type} is in place");
                    b = b.Neighbors.Where(i => i.Key.HomeFor == Type && !visited.Any(j => j == i.Key)).FirstOrDefault().Key;
                    if (b == null)
                        break;
                }
            }
        }

    }
}

internal class Node
{
    private static int _counter = 0;
    public int Id { get; }
    public Node(char? homeFor = null)
    {
        HomeFor = homeFor;
        Id = _counter++;
    }

    public char? HomeFor { get; }

    public Amphipod? Contains { get; set; }

    public Dictionary<Node, int> Neighbors { get; } = new();

    public void AddNeighbor(Node n, int weight)
    {
        Neighbors[n] = weight;
        n.Neighbors[this] = weight;
    }

    public bool TryFindWeightToNode(Node target, out int weight)
    {
        weight = 0;
        var nodes = new Queue<Node>();
        nodes.Enqueue(this);

        var weights = new Dictionary<Node, int>();
        weights[this] = 0;

        while (nodes.TryDequeue(out var current))
        {
            var availableNeighbors = current.Neighbors.Where(i => i.Key.Contains == null).ToList();
            foreach (var (neighbor, w) in availableNeighbors)
            {
                if (!weights.ContainsKey(neighbor))
                {
                    weights[neighbor] = weights[current] + w;
                    nodes.Enqueue(neighbor);
                }
                else
                {
                    if (weights[current] + w < weights[neighbor])
                    {
                        weights[neighbor] = weights[current] + w;
                        nodes.Enqueue(neighbor);
                    }
                }
            }
        }

        if (!weights.ContainsKey(target))
            return false;

        weight = weights[target];
        return true;
    }
}
