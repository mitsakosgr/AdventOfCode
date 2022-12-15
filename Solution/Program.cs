var res = 0;

var input = @"abaaaaaccccccccccccccccccaaaaaaaaaaaaaccccaaaaaaaccccccccccccccccccccccccccccaaaaaa
abaaaaaaccaaaacccccccccccaaaaaaaaacaaaacaaaaaaaaaacccccccccccccccccccccccccccaaaaaa
abaaaaaacaaaaaccccccccccaaaaaaaaaaaaaaacaaaaaaaaaacccccccccccccaacccccccccccccaaaaa
abaaaaaacaaaaaacccccccccaaaaaaaaaaaaaaccaaacaaaccccccccccccccccaacccccccccccccccaaa
abccaaaccaaaaaacccaaaaccaaaaaaaaaaaaaccccaacaaacccccccccaacaccccacccccccccccccccaaa
abcccccccaaaaaccccaaaacccccaaaaacccaaaccaaaaaaccccccccccaaaaccccccccccccccccccccaac
abcccccccccaaaccccaaaacccccaaaaacccccccccaaaaaccccccccccklllllccccccccccccccccccccc
abcccccccccccccccccaaccccccccaaccccccccaaaaaaaccccccccckklllllllcccccddccccaacccccc
abaccccccccccccccccccccccccccaaccccccccaaaaaaaaccccccckkkklslllllcccddddddaaacccccc
abacccccccccccccccccccccccccccccccaaaccaaaaaaaaccccccckkkssssslllllcddddddddacccccc
abaccccccccccccccccccccccccccccccccaaaaccaaacaccccccckkksssssssslllmmmmmdddddaacccc
abcccccccccccccccaaacccccccccccccaaaaaaccaacccccccccckkkssssusssslmmmmmmmdddddacccc
abcccccccaaccccaaaaacccccccccccccaaaaaccccccaaaaaccckkkrssuuuussssqmmmmmmmmdddccccc
abcccccccaaccccaaaaaacccccccaaccccaaaaacccccaaaaacckkkkrruuuuuussqqqqqqmmmmdddccccc
abccccaaaaaaaacaaaaaacccccccaaaaccaaccaccccaaaaaacjkkkrrruuuxuuusqqqqqqqmmmmeeccccc
abcaaaaaaaaaaacaaaaaccccccaaaaaacccccaaccccaaaaajjjjrrrrruuuxxuvvvvvvvqqqmmmeeccccc
abcaacccaaaaccccaaaaaaacccaaaaacccacaaaccccaaaajjjjrrrrruuuxxxxvvvvvvvqqqmmeeeccccc
abaaaaccaaaaacccccccaaaccccaaaaacaaaaaaaacccaajjjjrrrrtuuuuxxxyvyyyvvvqqqnneeeccccc
abaaaaaaaaaaacccaaaaaaaccccaacaacaaaaaaaacccccjjjrrrttttuxxxxxyyyyyvvvqqnnneeeccccc
abaaaaaaaccaacccaaaaaaaaacccccccccaaaaaaccccccjjjrrrtttxxxxxxxyyyyyvvvqqnnneeeccccc
SbaaaaaacccccccccaaaaaaaaaccccccccaaaaacccccccjjjrrrtttxxxEzzzzyyyvvrrrnnneeecccccc
abaaaaacccccccccccaaaaaaacccccccccaaaaaaccccccjjjqqqtttxxxxxyyyyyvvvrrrnnneeecccccc
abaaacccccccccccaaaaaaaccaaccccccccccaaccaaaaajjjqqqttttxxxxyyyyyyvvrrrnnneeecccccc
abaaacccccccccccaaaaaaaccaaacaaacccccccccaaaaajjjjqqqtttttxxyywyyyywvrrnnnfeecccccc
abcaaacccccccaaaaaaaaaaacaaaaaaaccccccccaaaaaaciiiiqqqqtttxwyywwyywwwrrrnnfffcccccc
abcccccccccccaaaaaaaaaaccaaaaaacccccccccaaaaaacciiiiqqqqttwwywwwwwwwwrrrnnfffcccccc
abccccccccccccaaaaaacccaaaaaaaacccccccccaaaaaaccciiiiqqqttwwwwwswwwwrrrrnnfffcccccc
abccccccccccccaaaaaacccaaaaaaaaacccccccccaaacccccciiiqqqtswwwwssssrrrrrroofffcccccc
abccccccaaaaacaaaaaacccaaaaaaaaaaccccccccccccccccciiiqqqssswsssssssrrrrooofffaccccc
abccccccaaaaacaaccaaccccccaaacaaacccccccccccccccccciiiqqssssssspoorrrooooofffaacccc
abcccccaaaaaacccccccccccccaaacccccccccccccccccccccciiiqppssssspppooooooooffffaacccc
abcccccaaaaaacccccccccccccaacccccccccccccccccccccccciipppppppppppoooooooffffaaccccc
abcccccaaaaaaccccccccccccccccccccccccccccccccccccccciihppppppppgggggggggfffaaaccccc
abccccccaaacccccccccccccccccccccccaccccccccccccccccchhhhpppppphggggggggggfaaaaccccc
abaaaccccccccccccccccccccccaccccaaacccccccccccccccccchhhhhhhhhhgggggggggcaacccccccc
abaaccaaaccaccccccccccccccaaacccaaacaacccaaaaacccccccchhhhhhhhhgaaccccccccccccccccc
abaaacaaacaacccccccccaaaccaaaacaaaaaaaaccaaaaaccccccccchhhhhhaaaaacccccccccccccccca
abaaaccaaaaaccccccccccaaacaaaaaaaacaaaaccaaaaaaccccccccccaaacccaaaacccccccccccaccca
abcccaaaaaaccccccccccaaaaaaaaaaaaacaaaaccaaaaaaccccccccccaaaccccaaaccccccccccaaaaaa
abcccaaaaaaaacccccccaaaaaaaaaaaaaaaaaccccaaaaaacccccccccccccccccccccccccccccccaaaaa
abcccaacaaaaaccccccaaaaaaaaaaaaaaaaaaacccccaacccccccccccccccccccccccccccccccccaaaaa";

var lines = input.Split('\n').Where(i => i.Length > 0).ToList();

int height = lines.Count;
int width = lines[0].Length;

var maze = new int[height, width];
var memoization = new int[height, width];

(int, int) start = (0, 0),
           end = (0, 0);

for (var i = 0; i < height; i++)
{
    for (var j = 0; j < width; ++j)
    {
        memoization[i, j] = int.MaxValue;
        maze[i, j] = lines[i][j];
        switch (maze[i, j])
        {
            case 'S':
                start = (i, j);
                maze[i, j] = 'a';
                break;
            case 'E':
                end = (i, j);
                maze[i, j] = 'z';
                break;
        }
    }
}

memoization[start.Item1, start.Item2] = 0;

var queue = new Queue<(int, int)>();
queue.Enqueue(start);

while (queue.TryDequeue(out var position))
{
    var x = position.Item1;
    var y = position.Item2;

    (int, int) nextPos;
    
    // up
    if (x > 0)
    {
        nextPos = (x - 1, y);
        
        if (maze[nextPos.Item1, nextPos.Item2] - maze[x, y] <= 1)
        {
            if (memoization[nextPos.Item1, nextPos.Item2] > memoization[x, y] + 1)
            {
                memoization[nextPos.Item1, nextPos.Item2] = memoization[x, y] + 1;
                if(!queue.Contains(nextPos))
                    queue.Enqueue(nextPos);
            }
        }
    }
    
    // down
    if (x < height - 1)
    {
        nextPos = (x + 1, y);
        
        if (maze[nextPos.Item1, nextPos.Item2] - maze[x, y] <= 1)
        {
            if (memoization[nextPos.Item1, nextPos.Item2] > memoization[x, y] + 1)
            {
                memoization[nextPos.Item1, nextPos.Item2] = memoization[x, y] + 1;
                if(!queue.Contains(nextPos))
                    queue.Enqueue(nextPos);
            }
        }
    }

    // left
    if (y > 0)
    {
        nextPos = (x, y - 1);
        
        if (maze[nextPos.Item1, nextPos.Item2] - maze[x, y] <= 1)
        {
            if (memoization[nextPos.Item1, nextPos.Item2] > memoization[x, y] + 1)
            {
                memoization[nextPos.Item1, nextPos.Item2] = memoization[x, y] + 1;
                if(!queue.Contains(nextPos))
                    queue.Enqueue(nextPos);
            }
        }
    }
    
    // right
    if (y < width - 1)
    {
        nextPos = (x, y + 1);
        
        if (maze[nextPos.Item1, nextPos.Item2] - maze[x, y] <= 1)
        {
            if (memoization[nextPos.Item1, nextPos.Item2] > memoization[x, y] + 1)
            {
                memoization[nextPos.Item1, nextPos.Item2] = memoization[x, y] + 1;
                if(!queue.Contains(nextPos))
                    queue.Enqueue(nextPos);
            }
        }
    }
}


Console.WriteLine(memoization[end.Item1, end.Item2]);
