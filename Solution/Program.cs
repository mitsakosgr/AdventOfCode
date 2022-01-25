var data = @"A0016C880162017C3686B18A3D4780";

var binary = String.Join("", data.Select(x => Convert.ToString(Convert.ToInt32(x+"", 16), 2).PadLeft(4,'0')));

var sumOfVersions = 0;
ParsePacket(binary);
Console.WriteLine(sumOfVersions);

int ParsePacket(string binary)
{
    int pos = 0;
    
    var version = GetVersion(binary);
    pos += 3;
    sumOfVersions += version;
    
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

        var finalNumber = Convert.ToInt64(final, 2);
    }
    else // operator type
    {
        if (binary[pos++] == '0')
        {
            var length = Convert.ToInt32(binary.Substring(pos, 15), 2);
            pos += 15;

            int sum = 0;
            while (sum < length)
            {
                var l = ParsePacket(binary.Substring(pos));
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
                pos += ParsePacket(binary.Substring(pos));
            }
        }
    }
    
    return pos;
}

int GetVersion(string data)
{
    return Convert.ToInt32(data.Substring(0, 3), 2);
}

int GetPacketId(string data)
{
    return Convert.ToInt32(data.Substring(3, 3), 2);
}

