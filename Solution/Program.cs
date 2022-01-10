var data = @"2199943210
3987894921
9856789892
8767896789
9899965678";

int sum = 0;

var lines = data.Split('\n');
var length = lines[0].Length;

for (int i = 0; i < lines.Length; ++i)
{
    for (int j = 0; j < length; j++)
    {
        int num = lines[i][j] - '0';
        
        int top = int.MaxValue;
        int bot = int.MaxValue;
        int left = int.MaxValue;
        int right = int.MaxValue;

        if (i > 0)
        {
            top = lines[i - 1][j] - '0';
            
            if(top <= num)
                continue;
        }

        if (i < lines.Length - 1)
        {
            bot = lines[i + 1][j] - '0';
            
            if(bot <= num)
                continue;
        }

        if (j > 0)
        {
            left = lines[i][j - 1] - '0';
            
            if(left <= num)
                continue;
        }

        if (j < length - 1)
        {
            right = lines[i][j + 1] - '0';
            
            if(right <= num)
                continue;
        }
        
        Console.WriteLine($"({i}, {j}) = {num}");

        sum += num + 1;
    }
}

Console.WriteLine(sum);
