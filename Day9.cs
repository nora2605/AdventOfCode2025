using System.Drawing;
using System.Numerics;

namespace AdventOfCode2025;

public class Day9
{
    (int x, int y)[] reds;

    public Day9()
    {
        var input = new StreamReader("inputs/day9.txt")!.ReadToEnd();
        reds = [.. input.Split('\n').Select(l => { var s = l.Split(',').Select(int.Parse).ToArray(); return (s[0], s[1]); })];
    }

    public long Part1()
    {
        long max = 0;
        for (int i = 0; i < reds.Length - 1; i++)
        {
            for (int j = i + 1; j < reds.Length; j++)
            {
                long dx = Math.Abs(reds[i].x - reds[j].x) + 1;
                long dy = Math.Abs(reds[i].y - reds[j].y) + 1;
                if (dx * dy > max) max = dx * dy;
            }
        }
        return max;
    }

    public long Part2()
    {
        List<(int x1, int y1, int x2, int y2)> lines = [];
        for (int i = 0; i < reds.Length - 1; i++)
            lines.Add((reds[i].x, reds[i].y, reds[i+1].x, reds[i+1].y));
        lines.Add((reds[^1].x, reds[^1].y, reds[0].x, reds[0].y));
        long max = 0;
        for (int i = 0; i < reds.Length - 1; i++)
        {
            for (int j = i + 2; j < reds.Length; j++)
            {
                var (x1, x2) = reds[i].x < reds[j].x ? (reds[i].x, reds[j].x) : (reds[j].x, reds[i].x);
                var (y1, y2) = reds[i].y < reds[j].y ? (reds[i].y, reds[j].y) : (reds[j].y, reds[i].y);
                int w = x2 - x1 + 1;
                int h = y2 - y1 + 1;
                long a = (long)w * h;
                if (a <= max) continue;
                if (!lines.Any(l => l.x2 > x1 && x2 > l.x1 && l.y2 > y1 && y2 > l.y1))
                    max = a;
            }
        }
        return max;
    }
}
