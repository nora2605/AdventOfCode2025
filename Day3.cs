using System.Numerics;
using System.Text.Unicode;

namespace AdventOfCode2025;

public class Day3
{
    byte[][] banks;

    public Day3()
    {
        var input = new StreamReader("inputs/day3.txt")!.ReadToEnd();
        banks = [.. input.Replace("\r", "").Split('\n').Select(l => l.ToCharArray().Select(c => (byte)(c - '0')).ToArray())];
    }

    public int Part1()
    {
        int acc = 0;
        foreach (var bank in banks)
        {
            byte maxDigit = 0;
            int si = 0;
            for (int i = 0; i < bank.Length - 1; i++)
            {
                if (bank[i] > maxDigit)
                {
                    maxDigit = bank[i];
                    si = i;
                }
            }
            byte maxL = bank.Skip(si + 1).Max();
            acc += maxDigit * 10 + maxL;
        }
        return acc;
    }

    public long Part2()
    {
        long acc = 0;
        foreach (var bank in banks)
        {
            byte[] md = new byte[12];
            int[] si = new int[11];
            for(int r=12;r>0;r--)for(int i=r==12?0:(1+si[11-r]);i<=bank.Length-r;i++)if(bank[i]>md[12-r]){md[12-r]=bank[i];if(r>1)si[12-r]=i;}
            acc += md.Aggregate((long)0, (a, b) => 10 * a + b);
        }
        return acc;
    }
}
