var data = @"[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]
[[[5,[2,8]],4],[5,[[9,9],0]]]
[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]
[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]
[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]
[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]
[[[[5,4],[7,7]],8],[[8,3],8]]
[[9,3],[[9,9],[6,[4,9]]]]
[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]
[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]";

var lines = data.Split('\n');

var max = Int64.MinValue;
for (int i = 0; i < lines.Length; ++i)
{
    for (int j = 0; j < lines.Length; j++)
    {
        if (i == j)
            continue;

        var root = new TreeNode(null)
        {
            Left = ParseLine(lines[i]),
            Right = ParseLine(lines[j])
        };
        
        root.Left.Parent = root;
        root.Right.Parent = root;
        
        // Console.Write("after addition: ");
        // PrintTree(root);
        // Console.WriteLine();

        while (true)
        {
            bool exploded = false;
            bool splited = false;

            while (PerformExplode(root))
            {
                exploded = true;
                // Console.Write("after explode: ");
                // PrintTree(root);
                // Console.WriteLine();
            }

            splited = PerformSplit(root);
            if (splited)
            {
                // Console.Write("after split: ");
                // PrintTree(root);
                // Console.WriteLine();
            }

            if (!(exploded || splited))
                break;
        }
        
        // PrintTree(root);
        // Console.WriteLine();

        var result = CalculateResult(root);
        if (result > max)
            max = result;
    }
}

Console.WriteLine(max);

return 0;

long CalculateResult(TreeNode start)
{
    if (start.Value != null)
        return start.Value ?? 0;

    return CalculateResult(start.Left) * 3 + CalculateResult(start.Right) * 2;
}

TreeNode ParseLine(string line)
{
    var tree = new TreeNode(null);
    var current = tree;


    foreach (var c in line)
    {
        if (c == '[')
        {
            current.Left = new TreeNode(current);
            current = current.Left;
        }
        else if (c >= '0' && c <= '9')
        {
            current.Value = c - '0';
        }
        else if (c == ',')
        {
            current.Parent.Right = new TreeNode(current.Parent);
            current = current.Parent.Right;
        }
        else if (c == ']')
        {
            current = current.Parent;
        }
    }

    return tree;
}

bool PerformExplode(TreeNode start, int depth = 1)
{
    if (depth > 4 && start.Value == null) // has children
    {
        if (start.Left.Value == null)
            throw new Exception("Left of depth 5 is not value...");

        if (start.Right.Value == null)
            throw new Exception("Right of depth 5 is not value...");

        // find  first left value (if any)
        // climb up if this is left child
        var current = start;
        var parent = start.Parent;
        while (parent != null && parent.Left == current)
        {
            current = parent;
            parent = current.Parent;
        }

        // climb down to rightmost child of left child of parent
        if (parent != null)
        {
            current = parent.Left;

            while (current.Value == null && current.Right != null)
                current = current.Right;

            current.Value += start.Left.Value;
        }


        // find first right value (if any)
        // climb up if this is right child
        current = start;
        parent = start.Parent;
        while (parent != null && parent.Right == current)
        {
            current = parent;
            parent = current.Parent;
        }

        // climb down to leftmost child of right child of parent
        if (parent != null)
        {
            current = parent.Right;

            while (current.Value == null && current.Left != null)
                current = current.Left;

            current.Value += start.Right.Value;
        }

        start.Left = null;
        start.Right = null;
        start.Value = 0;

        return true;
    }
    
    if (start.Left != null)
    {
        if (PerformExplode(start.Left, depth + 1))
            return true;
    }

    if (start.Right != null)
    {
        if (PerformExplode(start.Right, depth + 1))
            return true;
    }

    // action not found, continue traversing tree
    return false;
}

bool PerformSplit(TreeNode start)
{
    // split
    if (start.Value != null && start.Value >= 10)
    {
        start.Left = new TreeNode(start, start.Value / 2);
        start.Right = new TreeNode(start, (start.Value + 1) / 2);
        start.Value = null;

        return true;
    }

    if (start.Left != null)
    {
        if (PerformSplit(start.Left))
            return true;
    }

    if (start.Right != null)
    {
        if (PerformSplit(start.Right))
            return true;
    }

    // action not found, continue traversing tree
    return false;
}

void PrintTree(TreeNode start)
{
    if (start.Value != null)
    {
        Console.Write(start.Value);
        return;
    }

    Console.Write('[');
    PrintTree(start.Left);
    Console.Write(',');
    PrintTree(start.Right);
    Console.Write(']');
}

class TreeNode
{
    public TreeNode(TreeNode? parent)
    {
        Parent = parent;
    }

    public TreeNode(TreeNode? parent, long? value)
    {
        Parent = parent;
        Value = value;
    }

    public long? Value { get; set; } = null;
    public TreeNode? Left { get; set; }

    public TreeNode? Right { get; set; }

    public TreeNode? Parent { get; set; }
}