var data = @"on x=-20..26,y=-36..17,z=-47..7
on x=-20..33,y=-21..23,z=-26..28
on x=-22..28,y=-29..23,z=-38..16
on x=-46..7,y=-6..46,z=-50..-1
on x=-49..1,y=-3..46,z=-24..28
on x=2..47,y=-22..22,z=-23..27
on x=-27..23,y=-28..26,z=-21..29
on x=-39..5,y=-6..47,z=-3..44
on x=-30..21,y=-8..43,z=-13..34
on x=-22..26,y=-27..20,z=-29..19
off x=-48..-32,y=26..41,z=-47..-37
on x=-12..35,y=6..50,z=-50..-2
off x=-48..-32,y=-32..-16,z=-15..-5
on x=-18..26,y=-33..15,z=-7..46
off x=-40..-22,y=-38..-28,z=23..41
on x=-16..35,y=-41..10,z=-47..6
off x=-32..-23,y=11..30,z=-14..3
on x=-49..-5,y=-3..45,z=-29..18
off x=18..30,y=-20..-8,z=-3..13
on x=-41..9,y=-7..43,z=-33..15
on x=-54112..-39298,y=-85059..-49293,z=-27449..7877
on x=967..23432,y=45373..81175,z=27513..53682";

var cubes = new Dictionary<int, Dictionary<int, Dictionary<int, bool>>>();

var ranges = data.Split('\n');

foreach (var rangeString in ranges)
{
    var isOn = false;
    var dimensionsPart = rangeString;

    if (rangeString[1] == 'n')
    {
        isOn = true;
        dimensionsPart = dimensionsPart[5..];
    }
    else
    {
        dimensionsPart = dimensionsPart[6..];
    }

    var dimensions = dimensionsPart.Split(',');

    var dimensionX = dimensions[0].Split("..");
    var startX = int.Parse(dimensionX[0]);
    var endX = int.Parse(dimensionX[1]);

    var dimensionY = dimensions[1][2..].Split("..");
    var startY = int.Parse(dimensionY[0]);
    var endY = int.Parse(dimensionY[1]);

    var dimensionZ = dimensions[2][2..].Split("..");
    var startZ = int.Parse(dimensionZ[0]);
    var endZ = int.Parse(dimensionZ[1]);

    for (var x = startX; x <= endX; ++x)
    {
        if (x is < -50 or > 50) continue;

        if (!cubes.ContainsKey(x))
            cubes[x] = new Dictionary<int, Dictionary<int, bool>>();

        for (var y = startY; y <= endY; y++)
        {
            if (y is < -50 or > 50) continue;

            if (!cubes[x].ContainsKey(y))
                cubes[x][y] = new Dictionary<int, bool>();

            for (var z = startZ; z <= endZ; z++)
            {
                if (z is < -50 or > 50) continue;

                cubes[x][y][z] = isOn;
            }
        }
    }
}

var count = cubes
    .Values
    .SelectMany(i => i.Values.SelectMany(j => j.Values))
    .Count(i => i);

Console.WriteLine(count);