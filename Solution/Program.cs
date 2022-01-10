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

var errors = 0;
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
            errors += 3;
            break;
        }

        if (c == ']' && last != '[')
        {
            errors += 57;
            break;
        }

        if (c == '}' && last != '{')
        {
            errors += 1197;
            break;
        }

        if (c == '>' && last != '<')
        {
            errors += 25137;
            break;
        }
    }
}

Console.WriteLine(errors);