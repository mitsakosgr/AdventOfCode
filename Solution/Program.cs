var data = @"9C0141080250320F1802104A08";

var binary = String.Join("", data.Select(x => Convert.ToString(Convert.ToInt32(x+"", 16), 2).PadLeft(4,'0')));

var value = ParsePacket(binary, out int _);
Console.WriteLine(value);

long ParsePacket(string binary, out int pos)
{
    var ret = 0L;
    pos = 0;
    
    var version = GetVersion(binary);
    pos += 3;
    
    var packetId = GetPacketId(binary);
    pos += 3;

    // literal packet type
    if (packetId == 4)
    {
        Queue<string> numberQueue = new Queue<string>();
        
        while (true)
        {
            var num = binary.Substring(pos, 5);
            numberQueue.Enqueue(num);
            pos += 5;
            if (num[0] == '0')
                break;
        }

        string final = "";
        while (numberQueue.TryDequeue(out string s))
        {
            final += s.Substring(1);
        }

        ret = Convert.ToInt64(final, 2);
    }
    else // operator type
    {
        var packets = new List<long>();
        if (binary[pos++] == '0')
        {
            var length = Convert.ToInt32(binary.Substring(pos, 15), 2);
            pos += 15;

            int sum = 0;
            while (sum < length)
            {
                packets.Add(ParsePacket(binary.Substring(pos), out int l));
                sum += l;
                pos += l;
            }
        }
        else
        {
            var numberOfSub = Convert.ToInt32(binary.Substring(pos, 11), 2);
            pos += 11;

            for (int i = 0; i < numberOfSub; ++i)
            {
                packets.Add(ParsePacket(binary.Substring(pos), out int l));
                pos += l;
            }
        }

        ret = packetId switch
        {
            0 => packets.Sum(),
            1 => packets.Aggregate(1L, (acc, val) => acc * val),
            2 => packets.Min(),
            3 => packets.Max(),
            5 => packets[0] > packets[1] ? 1 : 0,
            6 => packets[0] < packets[1] ? 1 : 0,
            7 => packets[0] == packets[1] ? 1 : 0,
            _ => ret
        };
    }

    return ret;
}

int GetVersion(string data)
{
    return Convert.ToInt32(data.Substring(0, 3), 2);
}

int GetPacketId(string data)
{
    return Convert.ToInt32(data.Substring(3, 3), 2);
}

