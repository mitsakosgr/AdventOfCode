var data = @"..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#

#..#.
#....
##..#
..#..
..###";

var parts = data.Split("\n\n");

var enhancement = parts[0];


var xx = parts[1].Split('\n');
var imageDimensions = xx.Length;
var image = new int[imageDimensions, imageDimensions];

for (int i = 0; i < xx.Length; ++i)
{
    for (int j = 0; j < xx[i].Length; j++)
    {
        image[i, j] = xx[i][j] == '#' ? 1 : 0;
    }
}

var newImageDimensions = 0;
for (var r = 0; r < 50; ++r)
{
    newImageDimensions = imageDimensions + 2;
    var newImage = new int[newImageDimensions, newImageDimensions];

    if (enhancement[0] == '#' && r % 2 == 1)
    {
        for (var i = 0; i < newImageDimensions; ++i)
        {
            for (var j = 0; j < newImageDimensions; ++j)
            {
                newImage[i, j] = 1;
            }
        }
    }

    for (var i = 0; i < imageDimensions; ++i)
    {
        for (var j = 0; j < imageDimensions; ++j)
        {
            newImage[i + 1, j + 1] = image[i, j];
        }
    }

    image = newImage;
    imageDimensions = newImageDimensions;
    newImage = new int[newImageDimensions, newImageDimensions];

    for (var x = 0; x < newImageDimensions; ++x)
    {
        for (var y = 0; y < newImageDimensions; y++)
        {
            int pos = 0;

            for (var i = -1; i <= 1; ++i)
            {
                for (var j = -1; j <= 1; ++j)
                {
                    pos <<= 1;
                    if (x + i < 0 || x + i >= newImageDimensions)
                    {
                        if (enhancement[0] == '#' && r % 2 == 1)
                            pos |= 1;
                        
                        continue;
                    }

                    if (y + j < 0 || y + j >= newImageDimensions)
                    {
                        if (enhancement[0] == '#' && r % 2 == 1)
                            pos |= 1;

                        continue;
                    }

                    pos |= image[x + i, y + j];
                }
            }

            newImage[x, y] = enhancement[pos] == '#' ? 1 : 0;
        }
    }

    image = newImage;
}

PrintImage(image, newImageDimensions);

int count = 0;
for (int i = 0; i < imageDimensions; i++)
{
    for (int j = 0; j < imageDimensions; j++)
    {
        count += image[i, j];
    }
}

Console.WriteLine(count);
return 0;

void PrintImage(int[,] image, int size)
{
    for (int i = 0; i < size; ++i)
    {
        for (int j = 0; j < size; ++j)
        {
            Console.Write(image[i, j] == 1 ? '#' : '.');
        }

        Console.WriteLine();
    }
}