using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.Unicode;

namespace AdventOfCode2025;

public class Day6
{
    string[] lines;

    public Day6()
    {
        var input = new StreamReader("inputs/day6.txt")!.ReadToEnd().Replace("\r", "");
        lines = input.Split('\n');
    }

    public long Part1() {
        var nums = lines.SkipLast(1).Select(l => l.Split(" ").Where(s => !s.IsWhiteSpace()).Select(int.Parse).ToArray());
        var ops = lines.Last().Split(" ").Where(s => !s.IsWhiteSpace()).Select(s => s[0]).ToArray();
        var problems = new (int[] n, char op)[ops.Length];
        for (int i = 0; i < ops.Length; i++)
            problems[i] = ([.. nums.Select(l => l[i])], ops[i]);
        return problems.Select(p => p.n.Aggregate(p.op == '*' ? 1L : 0L, (a, b) => p.op == '*' ? a * b : a + b)).Sum();
    }

    public long Part2()
    {
        var cols = new string[lines.SkipLast(1).Max(l => l.Length)];
        for (int i = 0; i < cols.Length; i++)
            cols[i] = new string([.. lines.SkipLast(1).Select(l => i < l.Length ? l[i] : ' ')]);
        var ops = lines.Last().Split(" ").Where(s => !s.IsWhiteSpace()).Select(s => s[0]).Reverse().ToArray();
        long acc = 0;
        int d = 0;
        List<long> b = [];
        for (int i = cols.Length - 1; i >= 0; i--)
        {
            if (cols[i].All(c => c == ' ') && b.Count > 0)
            {
                char op = ops[d++];
                acc += b.Aggregate(op == '*' ? 1L : 0L, (a, b) => op == '*' ? a * b : a + b);
                b.Clear();
            }
            else b.Add(long.Parse(new string([.. cols[i].Where(s => s != ' ')])));
        }
        return acc + b.Aggregate(ops.Last() == '*' ? 1L : 0L, (a, b) => ops.Last() == '*' ? a * b : a + b);
    }
}
