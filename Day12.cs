using System.Drawing;
using System.Numerics;

namespace AdventOfCode2025;

public class Day12
{
    bool[][,] shapes;
    int[] shapeSizes;
    (int w, int h, int[] a)[] fields;
    public Day12()
    {
        var input = new StreamReader("inputs/day12.txt")!.ReadToEnd().Replace("\r", "");
        var blocks = input.Split("\n\n");
        shapes = [.. blocks[..^1].Select(b => {
            var h = b.Split('\n')[1..].Select(l => l.Select(c => c == '#').ToArray()).ToArray();
            bool[,] shape = new bool[h.Length, h[0].Length];
            for (int i = 0; i < h.Length; i++)
                for (int j = 0; j < h[0].Length; j++)
                    shape[i, j] = h[i][j];
            return shape;
        })];
        shapeSizes = [.. shapes.Select(s => s.Cast<bool>().Count(x => x))];
        fields = [.. blocks[^1].Split('\n').Select(l => {
            var s = l.Split(": ");
            var dims = s[0].Split('x').Select(int.Parse).ToArray();
            int[] a = [.. s[1].Split(' ').Select(int.Parse)];
            return (dims[0], dims[1], a);
        })];
    }

    bool[,] D4(bool[,] shape, int config)
    {
        bool r4 = (config & 1) > 0;
        bool r2 = (config & 2) > 0;
        bool m = (config & 4) > 0;
        int w = shape.GetLength(0);
        int h = shape.GetLength(1);
        if (r4)
            (w, h) = (h, w);
        bool[,] newShape = new bool[w, h];
        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
            {
                int ox = x, oy = y;
                if (r4) (ox, oy) = (oy, w - ox - 1);
                if (r2) (ox, oy) = (w - ox - 1, h - oy - 1);
                if (m) ox = w - ox - 1;
                newShape[x, y] = shape[ox, oy];
            }
        return newShape;
    }

    bool CanPlaceAll(bool[,] currentField, int[] amountsLeft)
    {
        if (amountsLeft.All(a => a == 0)) return true;
        int w = currentField.GetLength(0);
        int h = currentField.GetLength(1);
        int si = amountsLeft.Index().First(e => e.Item > 0).Index;
        bool[,] nextShape = shapes[si];
        int[] newAmountsLeft = (int[])amountsLeft.Clone();
        newAmountsLeft[si]--;
        return Enumerable.Range(0, 8).Any(i =>
        {
            bool[,] config = D4(nextShape, i);
            int sw = config.GetLength(0);
            int sh = config.GetLength(1);
            bool possible = false;
            for (int x = 0; x <= w - sw; x++)
            {
                for (int y = 0; y <= h - sh; y++)
                {
                    bool[,] newField = (bool[,])currentField.Clone();
                    bool canPlace = true;
                    for (int sx = 0; sx < sw; sx++)
                        for (int sy = 0; sy < sh; sy++)
                            if (config[sx, sy] && newField[x + sx, y + sy])
                            {
                                canPlace = false;
                                break;
                            }
                            else newField[x + sx, y + sy] |= config[sx, sy];
                    if (!canPlace) continue;
                    possible = CanPlaceAll(newField, newAmountsLeft);
                    if (possible) break;
                }
                if (possible) break;
            }
            return possible;
        });
    }

    public long Part1()
    {
        int c = 0;
        Parallel.For(0, fields.Length, i =>
        {
            var f = fields[i];
            if (f.w * f.h < f.a.Index().Sum(e => shapeSizes[e.Index] * e.Item)) return;
            //if (CanPlaceAll(new bool[f.w, f.h], f.a))
                Interlocked.Increment(ref c);
        });
        return c;
    }
}
