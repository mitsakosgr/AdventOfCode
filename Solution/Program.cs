var data = @"dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc";

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

int count = 0;
foreach (var neighbor in start.Neighbors)
{
    VisitNode(neighbor);
}

Console.WriteLine(count);

void VisitNode(Node n)
{
    if (n.Name == "end")
    {
        ++count;
        return;
    }

    if (n.Visited)
        return;

    if (!n.Cave)
        n.Visited = true;

    foreach (var neighbor in n.Neighbors)
        VisitNode(neighbor);

    n.Visited = false;
}

class Node
{
    public Node(string name)
    {
        Name = name;
        if (name.ToUpper() == name)
            Cave = true;
    }

    public bool Visited { get; set; } = false;
    public bool Cave { get; set; } = false;
    public string Name { get; set; }
    public List<Node> Neighbors { get; set; } = new();
}
