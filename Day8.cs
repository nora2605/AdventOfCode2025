using System.Numerics;

namespace AdventOfCode2025;

public class Day8
{
    Vector3[] junctions;

    public Day8()
    {
        var input = new StreamReader("inputs/day8.txt")!.ReadToEnd().Replace("\r", "");
        junctions = [.. input.Split('\n').Select(l => { var s = l.Split(',').Select(float.Parse).ToArray(); return new Vector3(s[0], s[1], s[2]); })];
    }

    PriorityQueue<(int, int), float> pairs = new();
    List<List<int>> circuits = [];

    void Connect((int, int) p)
    {
        var (i, j) = p;
        List<int>? ci = circuits.Find(c => c.Contains(i));
        List<int>? cj = circuits.Find(c => c.Contains(j));
        if (ci != null && cj != null)
        {
            if (ci == cj) return;
            ci.AddRange(cj);
            circuits.Remove(cj);
        }
        else if (ci != null) ci.Add(j);
        else if (cj != null) cj.Add(i);
        else circuits.Add([i, j]);
    }

    public int Part1()
    {
        for (int i = 0; i < junctions.Length - 1; i++)
            for (int j = i + 1; j < junctions.Length; j++)
                pairs.Enqueue((i, j), (junctions[j] - junctions[i]).LengthSquared());
        int c = 1000;
        while (c-- > 0 && pairs.TryDequeue(out var p, out var d)) Connect(p);
        return circuits.Select(c => c.Count).Order().TakeLast(3).Aggregate((a, b) => a * b);
    }

    public long Part2()
    {
        var pp = (-1, -1);
        while (pairs.TryDequeue(out var p, out var d))
        {
            Connect(pp = p);
            if (circuits[0].Count == junctions.Length) break;
        }
        return (long)junctions[pp.Item1].X * (long)junctions[pp.Item2].X;
    }
}
