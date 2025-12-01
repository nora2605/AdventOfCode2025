namespace AdventOfCode2025;
public class Day1
{
    int[] rots;

    public Day1()
    {
        var input = new StreamReader("inputs/day1.txt")!.ReadToEnd();
        rots = [.. input.Split('\n').Select(l => (l[0] == 'R' ? 1 : -1) * int.Parse(l[1..]))];
    }

    public int Part1() {
        int c = 0;
        int rot = 50;
        foreach (int o in rots)
        {
            rot += o;
            rot = (rot + 100) % 100;
            if (rot == 0) c++;
        }
        return c;
    }

    public int Part2()
    {
        int c = 0;
        int rot = 50;
        foreach (int o in rots)
        {
            rot += o;
            if (rot < 0 && o < rot) rot -= 100;
            c += Math.Abs(rot / 100);
            if (rot == 0) c++;
            rot %= 100;
            if (rot < 0) rot += 100;
        }
        return c;
    }
}
