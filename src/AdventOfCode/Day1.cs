using NullableExtensions;

using System.Collections.Immutable;


namespace AdventOfCode;

public static class Day1 {

    public static void RunPart1() {
        var result =
            File.ReadAllLines(@"../../../../../../advent-of-code-2022/input/day1.txt")
                .Select(Day1.ParseInt)
                .StackUntil(it => it == 0)
                .Select(Enumerable.Sum)
                .Max();

        Console.WriteLine(result);
    }

    public static void RunPart2() {
        var result =
            File.ReadAllLines(@"../../../../../../advent-of-code-2022/input/day1.txt")
                .Select(Day1.ParseInt)
                .StackUntil(it => it == 0)
                .Select(Enumerable.Sum)
                .Aggregate(new int[] { 0, 0, 0 }, Day1.ThreeMaxes)
                .Sum();

        Console.WriteLine(result);
    }

    public static int ParseInt(string it) =>
        it == ""
            ? 0
            : int.Parse(it);

    public static int[] ThreeMaxes(int[] maxes, int next) {
        var low = maxes[0];
        var mid = maxes[1];
        var high = maxes[2];

        if (next > high) {
            return new int[] { mid, high, next };
        }

        if (next > mid) {
            return new int[] { mid, next, high };
        }

        if (next > low) {
            return new int[] { next, mid, high };
        }

        return maxes;
    }
}


public static class IEnumerableExt {

    public static IEnumerable<ImmutableList<A>> StackUntil<A>(
        this IEnumerable<A> @this,
        Func<A, bool> predicate
    ) {
        var stack = ImmutableList.Create<A>();

        foreach (var item in @this) {

            if (predicate(item)) {
                yield return stack;
                stack = ImmutableList.Create<A>();
            } else {
                stack = stack.Add(item);
            }
        }
    }
}



