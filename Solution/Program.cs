var data = @"start-A
start-b
A-c
A-b
b-d
A-end
b-end";

var lines = data.Split('\n');

var nodes = new Dictionary<string, Node>();

foreach (var l in lines)
{
    var t = l.Split('-');

    if (!nodes.ContainsKey(t[0]))
        nodes[t[0]] = new Node(t[0]);

    if (!nodes.ContainsKey(t[1]))
        nodes[t[1]] = new Node(t[1]);

    nodes[t[0]].Neighbors.Add(nodes[t[1]]);
    nodes[t[1]].Neighbors.Add(nodes[t[0]]);
}

var start = nodes["start"];
start.Visited = true;

var paths = new HashSet<string>();
foreach (var neighbor in start.Neighbors)
{
    VisitNode(neighbor, false, new Queue<Node>());
}

Console.WriteLine(paths.Count);

void VisitNode(Node n, bool visitedTwice, Queue<Node> visited)
{
    if (n.Name == "end")
    {
        var path = string.Join(',', visited);
        paths.Add(path);
        return;
    }

    if (n.Visited)
        return;

    visited.Enqueue(n);

    if (n.Cave)
    {
        foreach (var neighbor in n.Neighbors)
            VisitNode(neighbor, visitedTwice, visited);
    }
    else
    {
        if (visitedTwice == false)
        {
            // visit neighbors but allow this to pass again
            foreach (var neighbor in n.Neighbors)
                VisitNode(neighbor, true, visited);
        }

        // visit as normal
        n.Visited = true;
        foreach (var neighbor in n.Neighbors)
            VisitNode(neighbor, visitedTwice, visited);
    }

    visited.Dequeue();

    n.Visited = false;
}

internal class Node
{
    public Node(string name)
    {
        Name = name;
        if (name.ToUpper() == name)
            Cave = true;
    }

    public bool Visited { get; set; }
    public bool Cave { get; }
    public string Name { get; }
    public List<Node> Neighbors { get; } = new();

    public override string ToString()
    {
        return Name;
    }
}
