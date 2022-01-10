var data = @"2199943210
3987894921
9856789892
8767896789
9899965678";

var lines = data.Split('\n');

var height = lines.Length;
var width = lines[0].Length;

var area = new int[height, width];
var visited = new int[height, width];

for (int h = 0; h < height; ++h)
{
    for (int w = 0; w < width; w++)
    {
        area[h, w] = lines[h][w] - '0';
        visited[h, w] = 0;
    }
}

var basins = new List<int>();

for (int h = 0; h < height; h++)
{
    for (int w = 0; w < width; w++)
    {
        int basin = RecursiveVisit(h, w);
        if (basin > 0)
            basins.Add(basin);
    }
}

var result = basins.OrderByDescending(i => i).Take(3).Aggregate((current, next) => current * next);
Console.WriteLine(result);

int RecursiveVisit(int h, int w)
{
    if (area[h, w] == 9)
        return 0;
    
    if (visited[h, w] == 1)
        return 0;
    
    visited[h, w] = 1;

    int sum = 1;
    
    if (h > 0)
        sum += RecursiveVisit(h - 1, w);

    if (h < height - 1)
        sum += RecursiveVisit(h + 1, w);

    if (w > 0)
        sum += RecursiveVisit(h, w - 1);

    if (w < width - 1)
        sum += RecursiveVisit(h, w + 1);

    return sum;
}
