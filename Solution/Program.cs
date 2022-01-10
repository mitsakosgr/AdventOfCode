var data = @"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]";

var lines = data.Split('\n');
var queue = new Stack<char>();

var scores = new List<long>();
foreach (var l in lines)
{
    queue.Clear();
    foreach (var c in l)
    {
        if ("([{<".Contains(c))
        {
            queue.Push(c);
            continue;
        }

        char last = queue.Pop();

        if (c == ')' && last != '(')
        {
            queue.Clear();
            break;
        }

        if (c == ']' && last != '[')
        {
            queue.Clear();
            break;
        }

        if (c == '}' && last != '{')
        {
            queue.Clear();
            break;
        }

        if (c == '>' && last != '<')
        {
            queue.Clear();
            break;
        }
    }

    long sum = 0;
    while (queue.TryPop(out char c))
    {
        sum *= 5;

        if (c == '(')
            sum += 1;

        if (c == '[')
            sum += 2;

        if (c == '{')
            sum += 3;

        if (c == '<')
            sum += 4;
    }

    if (sum > 0)
    {
        scores.Add(sum);
    }
}

Console.WriteLine(scores.OrderBy(i => i).ToList()[scores.Count / 2]);