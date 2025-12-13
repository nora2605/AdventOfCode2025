using System.Drawing;
using System.Numerics;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Xml;

namespace AdventOfCode2025;

public class Day10
{
    (bool[,] bMatrix, short[] joltages)[] machines;

    public Day10()
    {
        var input = new StreamReader("inputs/day10.txt")!.ReadToEnd().Replace("\r", "");
        machines = [..input.Split('\n').Select(l =>
        {
            var s = l.Split(' ');
            bool[] lights = [.. s[0][1..^1].Select(c => c == '#')];
            IEnumerable<byte>[] buttons = [.. s[1..^1].Select(h => h[1..^1].Split(',').Select(byte.Parse))];
            bool[,] buttonMatrix = new bool[lights.Length, buttons.Length + 1];
            for (int bi = 0; bi < buttons.Length; bi++)
                foreach (var i in buttons[bi])
                    buttonMatrix[i, bi] = true;
            for (int i = 0; i < lights.Length; i++)
                buttonMatrix[i, buttons.Length] = lights[i];
            short[] joltages = [.. s[^1][1..^1].Split(',').Select(short.Parse)];
            return (buttonMatrix, joltages);
        })];
    }

    public static int MWSGF2(bool[,] aug)
    {
        int m = aug.GetLength(0);
        int n = aug.GetLength(1) - 1;
        ulong[] pm = new ulong[m];
        for (int i = 0; i < m; i++)
            for (int j = 0; j <= n; j++)
                if (aug[i, j])
                    pm[i] |= 1UL << j;
        int pr = 0;
        int[] pc = new int[Math.Min(m, n)];
        for (int c = 0; c < n && pr < m; c++)
        {
            int s = -1;
            for (int r = pr; r < m; r++)
                if ((pm[r] & (1UL << c)) > 0) { s = r; break; }
            if (s == -1) continue;
            (pm[s], pm[pr]) = (pm[pr], pm[s]);
            for (int r = 0; r < m; r++)
                if (r != pr && (pm[r] & (1UL<<c)) > 0)
                    pm[r] ^= pm[pr];
            pc[pr++] = c;
        }
        ulong x = 0UL;
        for (int r = 0; r < pr; r++)
            x |= ((pm[r] & (1UL << n)) >> n) << pc[r];
        bool[] ip = new bool[n];
        for (int i = 0; i < pr; i++) ip[pc[i]] = true;
        int[] fc = [..Enumerable.Range(0, n).Where(c => !ip[c])];
        ulong[] b = new ulong[fc.Length];
        for (int i = 0; i < fc.Length; i++)
        {
            ulong v = 1UL << fc[i];
            for (int r = 0; r < pr; r++)
                v |= ((pm[r] & (1UL << fc[i])) >> fc[i]) << pc[r];
            b[i] = v;
        }
        int bw = BitOperations.PopCount(x);
        int cw = bw;
        for (int i = 1; i < 1 << fc.Length; i++)
        {
            x ^= b[BitOperations.TrailingZeroCount((uint)i)];
            cw = BitOperations.PopCount(x);
            if (cw < bw) bw = cw;
        }
        return bw;
    }

    public int Part1() => machines.Sum(m => MWSGF2(m.bMatrix));

    int M1NSN(bool[,] A, short[] b, int idx)
    {
        int m = A.GetLength(0);
        int n = A.GetLength(1) - 1;
        int[,] M = new int[m, n + 1];
        for (int i = 0; i < m; i++)
            for (int j = 0; j < n; j++)
                if (A[i, j]) M[i, j] = 1;
        for (int i = 0; i < m; i++)
            M[i, n] = b[i];
        int[] pc = new int[Math.Min(m, n)];
        int r = 0;
        for (int i = 0; i < n && r < m; i++)
        {
            int s = -1;
            for (int k = r; k < m; k++)
                if (M[k, i] != 0) s = k;
            if (s == -1) continue;
            if (s != r) for (int k = 0; k <= n; k++)
                (M[r, k], M[s, k]) = (M[s, k], M[r, k]);
            for (int k = r + 1; k < m; k++) if (M[k, i] != 0)
            {
                int f = M[k, i];
                int p = M[r, i];
                for (int j = 0; j <= n; j++)
                    M[k, j] = p * M[k, j] - f * M[r, j];
            }
            pc[r++] = i;
        }
        bool[] ip = new bool[n];
        for (int i = 0; i < r; i++) ip[pc[i]] = true;
        int[] fc = [.. Enumerable.Range(0, n).Where(c => !ip[c])];
        int mm = Enumerable.Range(0, m).Max(i => M[i, n]);
        int[] cu = new int[n];
        int best = int.MaxValue;

        void dfs(int d)
        {
            if (d == fc.Length)
            {
                int[] x = new int[n];
                for (int i = 0; i < fc.Length; i++)
                    x[fc[i]] = cu[i];
                for (int i = r - 1; i >= 0; i--)
                {
                    var t = M[i, n];
                    for (var j = pc[i] + 1; j < n; j++)
                        t -= M[i, j] * x[j];
                    var (q, rem) = Math.DivRem(t, M[i, pc[i]]);
                    if (rem != 0) return;
                    if (q < 0) return;
                    x[pc[i]] = q;
                }
                var z = x.Sum();
                if (z < best) best = z;
                return;
            }
            var ps = cu.Take(d).Sum();
            for (int i = 0; i < mm; i++)
            {
                if (ps + i >= best) break;
                cu[d] = i;
                dfs(d + 1);
            }
        }
        dfs(0);
        return best;
    }

    public long Part2()
    {
        long sum = 0;
        Parallel.For(0, machines.Length, i =>
        {
            (bool[,] bMatrix, short[] joltages) = machines[i];
            var s = M1NSN(bMatrix, joltages, i);
            Interlocked.Add(ref sum, s);
        });
        return sum;
    }
}
