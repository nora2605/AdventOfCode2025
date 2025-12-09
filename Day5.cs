using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.Unicode;

namespace AdventOfCode2025;

public class Day5
{
    (long start, long end)[] ranges;
    long[] inputs;

    public Day5()
    {
        var input = new StreamReader("inputs/day5.txt")!.ReadToEnd().Replace("\r", "");
        var s = input.Split("\n\n");
        ranges = [.. s[0].Split('\n').Select(l => { var ls = l.Split('-').Select(long.Parse).ToArray(); return (ls[0], ls[1]); })];
        inputs = [.. s[1].Split('\n').Select(long.Parse)];
    }

    public int Part1()
    {
        return inputs.Where(x => ranges.Any(t => t.start <= x && x <= t.end)).Count();
    }

    public long Part2()
    {
        List<(long s, long e)> disjunct = [];
        static bool overlaps((long s, long e) u, (long s, long e) v) => (u.e >= v.s) && (u.s <= v.e);
        static (long s, long e) merge((long s, long e) u, (long s, long e) v) => (Math.Min(u.s, v.s), Math.Max(u.e, v.e)); 
        foreach (var (s, e) in ranges)
        {
            var bubble = (s, e);
            for (int i = 0; i < disjunct.Count; i++)
            {
                if (overlaps(disjunct[i], bubble))
                    bubble = merge(disjunct[i], bubble);
            }
            disjunct.RemoveAll(r => overlaps(r, bubble));
            disjunct.Add(bubble);
        }
        return disjunct.Select(r => r.e - r.s + 1).Sum();
    }
}
