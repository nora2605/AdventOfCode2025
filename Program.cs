using AdventOfCode2025;

LabelledTimer t = new();

Console.WriteLine("Solutions:");

// Day 1

t.Start();
Day1 d1 = new();
t.Round("Day 1 - Init");

Console.WriteLine(t.RoundWith("Day 1 - Star 1", d1.Part1));
Console.WriteLine(t.RoundWith("Day 1 - Star 2", d1.Part2));

// Timing

Console.WriteLine("\nTimes:");
Console.WriteLine(t);