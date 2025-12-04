using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.Unicode;

namespace AdventOfCode2025;

public class Day4
{
    bool[,] field;
    int w, h;

    public Day4()
    {
        var input = new StreamReader("inputs/day4.txt")!.ReadToEnd();
        var lines = input.Replace("\r", "").Split('\n');
        w = lines[0].Length;
        h = lines.Length;
        field = new bool[w, h];
        for (int y=0;y<h;y++)for(int x=0;x<w;x++)field[x,y]=lines[y][x]=='@';
    }

    public int Part1()
    {
        int c = 0;
        for (int y = 1; y < h - 1; y++)
        {
            for (int x = 1; x < w - 1; x++)
            {
                if (!field[x, y]) continue;
                bool[] neighbors = [
                    field[x-1, y-1],
                    field[x, y-1],
                    field[x+1, y-1],
                    field[x-1, y],
                    field[x+1, y],
                    field[x-1, y+1],
                    field[x, y+1],
                    field[x+1, y+1]
                ];
                if (neighbors.Where(n => n).Count() < 4) c++;
            }
        }
        for (int x = 1; x < w - 1; x++)
        {
            if (
                field[x, 0] && ((bool[])[
                    field[x - 1, 0],
                    field[x + 1, 0],
                    field[x - 1, 1],
                    field[x, 1],
                    field[x + 1, 1]
                ]).Where(n => n).Count() < 4
            ) c++;
            if (
                field[x, h-1] && ((bool[])[
                    field[x - 1, h - 1],
                    field[x + 1, h - 1],
                    field[x - 1, h - 2],
                    field[x, h - 2],
                    field[x + 1, h - 2]
                ]).Where(n => n).Count() < 4
            ) c++;
        }
        for (int y = 1; y < h - 1; y++)
        {
            if (
                field[0, y] && ((bool[])[
                    field[0, y - 1],
                    field[0, y + 1],
                    field[1, y - 1],
                    field[1, y],
                    field[1, y + 1]
                ]).Where(n => n).Count() < 4
            ) c++;
            if (
                field[w-1, y] && ((bool[])[
                    field[w - 1, y - 1],
                    field[w - 1, y + 1],
                    field[w - 2, y - 1],
                    field[w - 2, y],
                    field[w - 2, y + 1]
                ]).Where(n => n).Count() < 4
            ) c++;
        }
        if (field[0, 0]) c++;
        if (field[0, h - 1]) c++;
        if (field[w - 1, h - 1]) c++;
        if (field[w - 1, 0]) c++;
        return c;
    }

    public int Part2()
    {
        int tc, ic = 0;
        if (field[0, 0])
        {
            ic++;
            field[0, 0] = false;
        }
        if (field[0, h - 1])
        {
            ic++;
            field[0, h - 1] = false;
        }
        if (field[w - 1, h - 1])
        {
            ic++;
            field[w - 1, h - 1] = false;
        }
        if (field[w - 1, 0])
        {
            ic++;
            field[w - 1, 0] = false;
        }
        tc = ic;
        do
        {
            ic = 0;
            for (int y = 1; y < h - 1; y++)
            {
                for (int x = 1; x < w - 1; x++)
                {
                    if (!field[x, y]) continue;
                    bool[] neighbors = [
                        field[x-1, y-1],
                        field[x, y-1],
                        field[x+1, y-1],
                        field[x-1, y],
                        field[x+1, y],
                        field[x-1, y+1],
                        field[x, y+1],
                        field[x+1, y+1]
                    ];
                    if (neighbors.Where(n => n).Count() < 4)
                    {
                        ic++;
                        field[x, y] = false;
                    }
                }
            }
            for (int x = 1; x < w - 1; x++)
            {
                if (
                    field[x, 0] && ((bool[])[
                        field[x - 1, 0],
                    field[x + 1, 0],
                    field[x - 1, 1],
                    field[x, 1],
                    field[x + 1, 1]
                    ]).Where(n => n).Count() < 4
                )
                {
                    ic++;
                    field[x, 0] = false;
                }
                if (
                    field[x, h - 1] && ((bool[])[
                        field[x - 1, h - 1],
                    field[x + 1, h - 1],
                    field[x - 1, h - 2],
                    field[x, h - 2],
                    field[x + 1, h - 2]
                    ]).Where(n => n).Count() < 4
                )
                {
                    ic++;
                    field[x, h - 1] = false;
                }
            }
            for (int y = 1; y < h - 1; y++)
            {
                if (
                    field[0, y] && ((bool[])[
                        field[0, y - 1],
                    field[0, y + 1],
                    field[1, y - 1],
                    field[1, y],
                    field[1, y + 1]
                    ]).Where(n => n).Count() < 4
                )
                {
                    ic++;
                    field[0, y] = false;
                }
                if (
                    field[w - 1, y] && ((bool[])[
                        field[w - 1, y - 1],
                    field[w - 1, y + 1],
                    field[w - 2, y - 1],
                    field[w - 2, y],
                    field[w - 2, y + 1]
                    ]).Where(n => n).Count() < 4
                )
                {
                    ic++;
                    field[w - 1, y] = false;
                }
            }
            tc += ic;
        } while (ic > 0);
        return tc;
    }
}
