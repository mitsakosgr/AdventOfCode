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
    new Node('B'), // 9
    new Node('B'), // 10
    new Node('C'), // 11
    new Node('C'), // 12
    new Node('D'), // 13
    new Node('D'), // 14
};

nodes[0].AddNeighbor(nodes[1], 1);
nodes[1].AddNeighbor(nodes[2], 2);
nodes[2].AddNeighbor(nodes[3], 2);
nodes[3].AddNeighbor(nodes[4], 2);
nodes[4].AddNeighbor(nodes[5], 2);
nodes[5].AddNeighbor(nodes[6], 1);

nodes[7].AddNeighbor(nodes[1], 2);
nodes[7].AddNeighbor(nodes[2], 2);
nodes[7].AddNeighbor(nodes[8], 1);

nodes[9].AddNeighbor(nodes[2], 2);
nodes[9].AddNeighbor(nodes[3], 2);
nodes[9].AddNeighbor(nodes[10], 1);

nodes[11].AddNeighbor(nodes[3], 2);
nodes[11].AddNeighbor(nodes[4], 2);
nodes[11].AddNeighbor(nodes[12], 1);

nodes[13].AddNeighbor(nodes[4], 2);
nodes[13].AddNeighbor(nodes[5], 2);
nodes[13].AddNeighbor(nodes[14], 1);


var amphipods = new List<Amphipod>
{
    new Amphipod('A', nodes[13]),
    new Amphipod('A', nodes[14]),
    new Amphipod('B', nodes[7]),
    new Amphipod('B', nodes[12]),
    new Amphipod('C', nodes[8]),
    new Amphipod('C', nodes[11]),
    new Amphipod('D', nodes[9]),
    new Amphipod('D', nodes[10]),
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
                        // Console.WriteLine($"Found new best solution with score {currentScore}");
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
    Console.Write(nodes[9].Contains?.Type ?? '.');
    Console.Write('#');
    Console.Write(nodes[11].Contains?.Type ?? '.');
    Console.Write('#');
    Console.Write(nodes[13].Contains?.Type ?? '.');
    Console.WriteLine("###");

    Console.Write("  #");
    Console.Write(nodes[8].Contains?.Type ?? '.');
    Console.Write('#');
    Console.Write(nodes[10].Contains?.Type ?? '.');
    Console.Write('#');
    Console.Write(nodes[12].Contains?.Type ?? '.');
    Console.Write('#');
    Console.Write(nodes[14].Contains?.Type ?? '.');
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

    var current = a.Position;

    // try to find a route home
    var targetNodes = nodes.Where(i => i.HomeFor == a.Type && i.Contains == null).ToList();
    Node target = null;

    if (targetNodes.Count == 2)
    {
        target = targetNodes.Single(i => i.Neighbors.Count == 1);
    }
    else if (targetNodes.Count == 1) // got top home
    {
        var other = targetNodes[0].Neighbors.Single(i => i.Key.HomeFor != null).Key;
        if (other.Contains?.InPlace == true)
            target = targetNodes[0];
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
    private Node _position;
    public Amphipod(char type, Node position)
    {
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

            switch (_position.Neighbors.Count)
            {
                case 1:
                {
                    InPlace = true;
                    // Console.WriteLine($"Amphipod {Type} is home...");

                    var other = _position.Neighbors.First().Key;
                    if (other.Contains == null || other.Contains.Type != Type)
                        return;

                    other.Contains.InPlace = true;
                    // Console.WriteLine($"Amphipods {Type} are both home...");
                    break;
                }
                case 3:
                {
                    var other = _position.Neighbors.Single(i => i.Key.HomeFor == Type).Key;
                    if (other.Contains == null || other.Contains.Type != Type)
                        return;

                    other.Contains.InPlace = true;
                    InPlace = true;

                    // Console.WriteLine($"Amphipods {Type} are both home...");
                    break;
                }
            }
        }

    }
}

internal class Node
{
    public Node(char? homeFor = null)
    {
        HomeFor = homeFor;
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
