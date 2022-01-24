var data = @"NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C";

var parts = data.Split("\n\n");

var template = parts[0];

var polymers = new Dictionary<string, int>();

var alters = new Dictionary<string, char>();

parts[1].Split('\n').ToList().ForEach(i =>
{
    var a = i.Split(" -> ");
    polymers[a[0]] = 0;
    alters[a[0]] = a[1][0];
});

for (int i = 0; i < template.Length - 1; ++i)
{
    polymers[template[i..(i + 2)]] += 1;
}
int steps = 10;

for (int i = 0; i < steps; ++i)
{
    var d = polymers.Where(kv => kv.Value > 0).ToList();
    foreach (var (key, value) in d)
    {
        var add = alters[key];
        
        polymers[key] -= value;
        polymers[key[0].ToString() + add] += value;
        polymers[add + key[1].ToString()] += value;
    }
}

var counts = new Dictionary<char, int>();
foreach (var (key, value) in polymers)
{
    if (!counts.ContainsKey(key[0]))
        counts[key[0]] = 0;

    if (!counts.ContainsKey(key[1]))
        counts[key[1]] = 0;

    counts[key[0]] += value;
    counts[key[1]] += value;
}

var max = counts.MaxBy(i => i.Value);
var min = counts.MinBy(i => i.Value);

// fix rounding error
var res = (max.Value + 1) / 2 - (min.Value + 1) / 2;

Console.WriteLine(res);
