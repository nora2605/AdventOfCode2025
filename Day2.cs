using System.Numerics;
using System.Text.Unicode;

namespace AdventOfCode2025;

public class Day2
{
    (BigInteger, BigInteger)[] ranges;

    public Day2()
    {
        var input = new StreamReader("inputs/day2.txt")!.ReadToEnd();
        ranges = [.. input.Split(',').Select(r => { var s = r.Split('-').Select(BigInteger.Parse).ToArray(); return (s[0], s[1]); })];
    }

    public BigInteger Part1()
    {
        BigInteger acc = 0;
        foreach (var r in ranges)
        {
            for (BigInteger i = r.Item1; i <= r.Item2; i++)
            {
                string j = i.ToString();
                if (j.Length % 2 == 1) continue;
                if (j[..(j.Length / 2)] == j[(j.Length / 2)..])
                    acc += i;
            }
        }
        return acc;
    }

    public BigInteger Part2()
    {
        BigInteger acc = 0;
        foreach (var r in ranges)
        {
            for (BigInteger i = r.Item1; i <= r.Item2; i++)
            {
                string j = i.ToString();
                string pat = j[..1];
                for (int m = 1; m <= j.Length / 2; m++)
                {
                    if (
                        j.Length % pat.Length == 0 &&
                        j[m..].Zip(pat).All(t => t.First == t.Second) &&
                        string.Join("", Enumerable.Repeat(pat, j.Length / pat.Length)) == j
                    ) {
                        acc += i;
                        break;
                    }
                    else pat += j[m];
                }
            }
        }
        return acc;
    }
}
