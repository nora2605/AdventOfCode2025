using System.Drawing;
using System.Numerics;

namespace AdventOfCode2025;

public class Day11
{
    Dictionary<ushort, ushort[]> graph = [];

    static ushort L2I(string l) => l.ToCharArray().Select(c => (ushort)(c - 'a')).Aggregate((a, b) => (ushort)(a * 26 + b));
    static string I2L(ushort i)
    {
        var s = "";
        for (int c = 0; c < 3; c++)
        {
            s = (char)('a' + (i % 26)) + s;
            i /= 26;
        }
        return s;
    }

    public Day11()
    {
        var input = new StreamReader("inputs/day11.txt")!.ReadToEnd().Replace("\r", "");
        foreach (var line in input.Split('\n'))
        {
            var s = line.Split(": ");
            ushort label = L2I(s[0]);
            ushort[] connections = [.. s[1].Split(' ').Select(L2I)];
            graph.Add(label, connections);
        }
    }

    static ushort @dac = L2I("dac");
    static ushort @svr = L2I("svr");
    static ushort @fft = L2I("fft");
    static ushort @you = L2I("you");
    static ushort @out = L2I("out");

    long NumPaths(ushort from, ushort to, Dictionary<ushort, long> memo)
    {
        if (from == to) return 1;
        if (memo.TryGetValue(from, out var v)) return v;
        if (graph.TryGetValue(from, out var c)) {
            var r = c.Sum(n => NumPaths(n, to, memo));
            memo[from] = r;
            return r;
        }
        memo[from] = 0;
        return 0;
    }

    public long Part1() => NumPaths(you, @out, []);

    public long Part2() => NumPaths(svr, dac, []) * NumPaths(dac, fft, []) * NumPaths(fft, @out, []) +
        NumPaths(svr, fft, []) * NumPaths(fft, dac, []) * NumPaths(dac, @out, []);
}
