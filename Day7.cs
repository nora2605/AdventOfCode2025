using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Unicode;

namespace AdventOfCode2025;

public class Day7
{
    bool[][] manifold;
    int startPos;

    public Day7()
    {
        var input = new StreamReader("inputs/day7.txt")!.ReadToEnd().Replace("\r", "");
        startPos = input.IndexOf('S');
        manifold = [..input.Split('\n').Select(l => l.Select(c => c == '^').ToArray())];
    }

    public int Part1()
    {
        int c = 0;
        bool[] row = new bool[manifold[0].Length];
        row[startPos] = true;
        for (int i = 1; i < manifold.Length; i++)
        {
            for (int j = 0; j < manifold[0].Length; j++)
            {
                if (manifold[i][j] && row[j])
                {
                    c++;
                    row[j] = false;
                    if (j > 0) row[j - 1] = true;
                    if (j < manifold[0].Length - 1) row[j + 1] = true;
                }
            }
        }
        return c;
    }

    Dictionary<(int, int), long> memo = [];
    public long Splits(int initPos, int line)
    {
        if (initPos < 0 || initPos >= manifold[0].Length) return 0;
        if (line == manifold.Length) return 1;
        if (memo.TryGetValue((initPos, line), out long s))
            return s;

        if (!manifold[line][initPos])
        {
            var m = Splits(initPos, line + 1);
            memo.Add((initPos, line), m);
            return m;
        }
        else
        {
            var m1 = Splits(initPos - 1, line + 1);
            var m2 = Splits(initPos + 1, line + 1);
            memo.Add((initPos, line), m1 + m2);
            return m1 + m2;
        }
    }

    public long Part2()
    {
        return Splits(startPos, 0);
    }
}
