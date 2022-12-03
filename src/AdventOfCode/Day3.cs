using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NullableExtensions;

namespace AdventOfCode;

public static class Day3 {

    public static void RunPart1() {
        var result =
            File.ReadAllLines(@"../../../../../../advent-of-code-2022/input/day3.txt")
                .Select(Day3.ComputePriority)
                .Sum();


        Console.WriteLine(result);
    }


    public static void RunPart2() {
        var result =
            File.ReadAllLines(@"../../../../../../advent-of-code-2022/input/day3.txt")
                .Select(RucksackOps.Parse)
                .Chunk(3)
                .Select(Day3.ComputePriority2)
                .Sum();

        Console.WriteLine(result);
    }


    public static Dictionary<char, int> Priorities =>
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
            .Zip(Enumerable.Range(1, 52))
            .ToDictionary(
                keySelector: it => it.First,
                elementSelector: it => it.Second);


    public static int ComputePriority(string items) {
        var c = 
            RucksackOps.Parse(items)
                .FindDuplicate()
                .OrElseThrow(new Exception("Couldn't find a duplicate"));

        return Priorities
            .GetValueOrDefault(c);
    }


    public static int ComputePriority2(Rucksack[] items) =>
        Priorities
            .GetValueOrDefault(Day3.FindBadge(items));


    public static char FindBadge(Rucksack[] rucksacks) {

        var first = rucksacks[0];
        return first.CombineCompartments()
            .FirstOrDefault(it =>
                rucksacks[1]
                    .CombineCompartments()
                    .Contains(it) &&
                rucksacks[2]
                    .CombineCompartments()
                    .Contains(it));
    }
}


public record Rucksack(string FirstCompartment, string SecondCompartment);


public static class RucksackOps {

    public static Rucksack Parse(string input) {
        var half = input.Length / 2;
        var firstCompartment = input.Substring(0, half);
        var secondCompartment = input.Substring(half);

        return new Rucksack(firstCompartment, secondCompartment);
    }


    public static char? FindDuplicate(this Rucksack @this) =>
        @this
            .FirstCompartment
            .FirstOrDefault(@this.SecondCompartment.Contains);


    public static string CombineCompartments(this Rucksack @this) => 
        @this.FirstCompartment + @this.SecondCompartment;
}

