using System.ComponentModel;
using System.Numerics;
using System.Text.RegularExpressions;

var input = @"Monkey 0:
  Starting items: 99, 67, 92, 61, 83, 64, 98
  Operation: new = old * 17
  Test: divisible by 3
    If true: throw to monkey 4
    If false: throw to monkey 2

Monkey 1:
  Starting items: 78, 74, 88, 89, 50
  Operation: new = old * 11
  Test: divisible by 5
    If true: throw to monkey 3
    If false: throw to monkey 5

Monkey 2:
  Starting items: 98, 91
  Operation: new = old + 4
  Test: divisible by 2
    If true: throw to monkey 6
    If false: throw to monkey 4

Monkey 3:
  Starting items: 59, 72, 94, 91, 79, 88, 94, 51
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 0
    If false: throw to monkey 5

Monkey 4:
  Starting items: 95, 72, 78
  Operation: new = old + 7
  Test: divisible by 11
    If true: throw to monkey 7
    If false: throw to monkey 6

Monkey 5:
  Starting items: 76
  Operation: new = old + 8
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 2

Monkey 6:
  Starting items: 69, 60, 53, 89, 71, 88
  Operation: new = old + 5
  Test: divisible by 19
    If true: throw to monkey 7
    If false: throw to monkey 1

Monkey 7:
  Starting items: 72, 54, 63, 80
  Operation: new = old + 3
  Test: divisible by 7
    If true: throw to monkey 1
    If false: throw to monkey 3
";

var res = 0;

var monkeys = input
    .Split("Monkey")
    .Where(i => !string.IsNullOrWhiteSpace(i))
    .Select(m => new Monkey(m))
    .ToList();

var monkeysCounts = new int[monkeys.Count];

for (var i = 0; i < 20; ++i)
{
    for (var index = 0; index < monkeys.Count; index++)
    {
        var m = monkeys[index];

        monkeysCounts[index] += m.Items.Count;
        while (m.Items.Count > 0)
        {
            var current = m.Items.Dequeue();
            current = m.Operation(current);
            current /= 3;
            if (current % m.Test == 0)
                monkeys[m.TrueMonkey].Items.Enqueue(current);
            else
                monkeys[m.FalseMonkey].Items.Enqueue(current);
        }
    }
}

Console.WriteLine(monkeysCounts.OrderByDescending(i => i).Take(2).Aggregate(1, (acc, val) => acc * val));


internal class Monkey
{
    static readonly Regex StartingRegex = new Regex(@"Starting items: ([\d, ]*)");
    static readonly Regex StartingNumbersRegex = new Regex(@"\d+");
    static readonly Regex OperationRegex = new Regex(@"Operation: new = old ([*+]) (old|\d+)");
    static readonly Regex TestRegex = new Regex(@"Test: divisible by (\d+)");
    static readonly Regex TrueRegex = new Regex(@"If true: throw to monkey (\d+)");
    static readonly Regex FalseRegex = new Regex(@"If false: throw to monkey (\d+)");

    public Monkey(string input)
    {
        Items = new Queue<BigInteger>();
        var startingItems = StartingRegex.Match(input);
        var item = StartingNumbersRegex.Matches(startingItems.Value);
        foreach (Match i in item)
        {
            Items.Enqueue(int.Parse(i.Value));
        }

        Test = int.Parse(TestRegex.Match(input).Groups[1].Value);
        TrueMonkey = int.Parse(TrueRegex.Match(input).Groups[1].Value);
        FalseMonkey = int.Parse(FalseRegex.Match(input).Groups[1].Value);

        var parts = OperationRegex.Match(input);
        Operation = current =>
        {
            var part1 = current;
            var part2 = parts.Groups[2].Value == "old" ? current : int.Parse(parts.Groups[2].Value);

            if (parts.Groups[1].Value == "+")
                return part1 + part2;

            return part1 * part2;
        };
    }

    public Queue<BigInteger> Items { get; }
    public Func<BigInteger, BigInteger> Operation { get; }

    public int Test { get; }
    public int TrueMonkey { get; }
    public int FalseMonkey { get; }
}


// Console.WriteLine(res);