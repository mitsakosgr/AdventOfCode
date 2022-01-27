var data = @"--- scanner 0 ---
404,-588,-901
528,-643,409
-838,591,734
390,-675,-793
-537,-823,-458
-485,-357,347
-345,-311,381
-661,-816,-575
-876,649,763
-618,-824,-621
553,345,-567
474,580,667
-447,-329,318
-584,868,-557
544,-627,-890
564,392,-477
455,729,728
-892,524,684
-689,845,-530
423,-701,434
7,-33,-71
630,319,-379
443,580,662
-789,900,-551
459,-707,401

--- scanner 1 ---
686,422,578
605,423,415
515,917,-361
-336,658,858
95,138,22
-476,619,847
-340,-569,-846
567,-361,727
-460,603,-452
669,-402,600
729,430,532
-500,-761,534
-322,571,750
-466,-666,-811
-429,-592,574
-355,545,-477
703,-491,-529
-328,-685,520
413,935,-424
-391,539,-444
586,-435,557
-364,-763,-893
807,-499,-711
755,-354,-619
553,889,-390

--- scanner 2 ---
649,640,665
682,-795,504
-784,533,-524
-644,584,-595
-588,-843,648
-30,6,44
-674,560,763
500,723,-460
609,671,-379
-555,-800,653
-675,-892,-343
697,-426,-610
578,704,681
493,664,-388
-671,-858,530
-667,343,800
571,-461,-707
-138,-166,112
-889,563,-600
646,-828,498
640,759,510
-630,509,768
-681,-892,-333
673,-379,-804
-742,-814,-386
577,-820,562

--- scanner 3 ---
-589,542,597
605,-692,669
-500,565,-823
-660,373,557
-458,-679,-417
-488,449,543
-626,468,-788
338,-750,-386
528,-832,-391
562,-778,733
-938,-730,414
543,643,-506
-524,371,-870
407,773,750
-104,29,83
378,-903,-323
-778,-728,485
426,699,580
-438,-605,-362
-469,-447,-387
509,732,623
647,635,-688
-868,-804,481
614,-800,639
595,780,-596

--- scanner 4 ---
727,592,562
-293,-554,779
441,611,-461
-714,465,-776
-743,427,-804
-660,-479,-426
832,-632,460
927,-485,-438
408,393,-506
466,436,-512
110,16,151
-258,-428,682
-393,719,612
-211,-452,876
808,-476,-593
-575,615,604
-485,667,467
-680,325,-822
-627,-443,-432
872,-547,-609
833,512,582
807,604,487
839,-516,451
891,-625,532
-652,-548,-490
30,-46,-14";

var dataScanners = data.Split("\n\n");

var scanners = new Dictionary<string, List<Probe>>();

var probesList = new List<List<Probe>>();

var finalProbes = new List<Probe>();

foreach (var s in dataScanners)
{
    var probes = new List<Probe>();
    var probeLines = s.Split('\n');

    for (int i = 1; i < probeLines.Length; i++)
        probes.Add(new Probe(probeLines[i]));

    scanners[probeLines[0]] = probes;
    probesList.Add(probes);
}

finalProbes.AddRange(probesList[0]);

probesList.RemoveAt(0);

var scannersList = new List<(int, int, int)> {(0, 0, 0)};

var pos = 0;
while (probesList.Count > 0)
{
    bool matched = false;

    // apply transformation
    // 24 transformations
    for (int t = 0; t < 24 && !matched; ++t)
    {
        var newProbes = Transform(probesList[pos], t);

        // try to match every probe with every final probe
        foreach (var probe in newProbes)
        {
            foreach (var finalProbe in finalProbes)
            {
                // get sensor pos
                // sensor pos = (p1.x - p2.x, p1.y - p2.y, p1.z - p2.z)
                var sensorPos = (finalProbe.X - probe.X, finalProbe.Y - probe.Y, finalProbe.Z - probe.Z);

                // offset all probes
                // p1_pos + sensor_pos = p2_pos
                var offset = Offset(newProbes, sensorPos);

                // check how many match
                var matches = offset.Count(p => finalProbes.Any(f => f.X == p.X && f.Y == p.Y && f.Z == p.Z));

                if (matches >= 12)
                {
                    Console.WriteLine(matches);
                    finalProbes.AddRange(
                        offset.Where(p => finalProbes.All(f => f.X != p.X || f.Y != p.Y || f.Z != p.Z)));

                    scannersList.Add(sensorPos);
                    probesList.RemoveAt(pos);
                    matched = true;
                    break;
                }
            }

            if (matched)
                break;
        }
    }

    if (matched == false)
    {
        pos++;
    }

    if (pos >= probesList.Count)
        pos = 0;
}

Console.WriteLine(finalProbes.Count);

int maxDistance = 0;
for (int i = 0; i < scannersList.Count; ++i)
{
    for (int j = 0; j < scannersList.Count; ++j)
    {
        var distance = Math.Abs(scannersList[i].Item1 - scannersList[j].Item1)
                       + Math.Abs(scannersList[i].Item2 - scannersList[j].Item2)
                       + Math.Abs(scannersList[i].Item3 - scannersList[j].Item3);

        if (distance > maxDistance)
            maxDistance = distance;
    }
}

Console.WriteLine(maxDistance);

return 0;


List<Probe> Transform(List<Probe> list, int transformation)
{
    return list.Select(i => i.Transform(transformation)).ToList();
}

List<Probe> Offset(IEnumerable<Probe> list, (int, int, int) offset)
{
    return list.Select(i => i.Offset(offset)).ToList();
}

struct Probe
{
    private static int _counter = 0;

    public int Id { get; }
    public int X { get; }
    public int Y { get; }
    public int Z { get; }

    private Probe(int x, int y, int z, int id)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.Id = id;
    }

    public Probe(string dimensions)
    {
        var parts = dimensions.Split(',');
        Id = _counter++;
        X = int.Parse(parts[0]);
        Y = int.Parse(parts[1]);
        Z = int.Parse(parts[2]);
    }

    public Probe Transform(int transformation)
    {
        return transformation switch
        {
            //
            0 => new Probe(X, Y, Z, Id),
            1 => new Probe(-Y, X, Z, Id),
            2 => new Probe(-X, -Y, Z, Id),
            3 => new Probe(Y, -X, Z, Id),
            //
            4 => new Probe(X, -Z, Y, Id),
            5 => new Probe(Z, X, Y, Id),
            6 => new Probe(-X, Z, Y, Id),
            7 => new Probe(-Z, -X, Y, Id),
            //
            8 => new Probe(X, -Y, -Z, Id),
            9 => new Probe(Y, X, -Z, Id),
            10 => new Probe(-X, Y, -Z, Id),
            11 => new Probe(-Y, -X, -Z, Id),
            //
            12 => new Probe(X, Z, -Y, Id),
            13 => new Probe(-Z, X, -Y, Id),
            14 => new Probe(-X, -Z, -Y, Id),
            15 => new Probe(Z, -X, -Y, Id),
            //
            16 => new Probe(Z, Y, -X, Id),
            17 => new Probe(-Y, Z, -X, Id),
            18 => new Probe(-Z, -Y, -X, Id),
            19 => new Probe(Y, -Z, -X, Id),
            //
            20 => new Probe(-Z, Y, X, Id),
            21 => new Probe(-Y, -Z, X, Id),
            22 => new Probe(Z, -Y, X, Id),
            23 => new Probe(Y, Z, X, Id),
            _ => throw new Exception("Unknown transformation")
        };
    }

    public Probe Offset((int, int, int) offset)
    {
        return new Probe(X + offset.Item1, Y + offset.Item2, Z + offset.Item3, Id);
    }

    public double Distance(Probe other)
    {
        return Math.Round(Math.Sqrt(Math.Pow(other.X - X, 2) + Math.Pow(other.Y - Y, 2) + Math.Pow(other.Z - Z, 2)), 6);
    }
}