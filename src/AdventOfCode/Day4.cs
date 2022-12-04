using Funk;

namespace AdventOfCode;

public static class Day4 {

    public static void RunPart1() {
        var result =
            File.ReadAllLines(@"../../../../../../advent-of-code-2022/input/day4.txt")
                .Select(Day4.Compute)
                .Sum();

        Console.WriteLine(result);
    }

    public static void RunPart2() {
        var result =
            File.ReadAllLines(@"../../../../../../advent-of-code-2022/input/day4.txt")
                .Select(Day4.Compute2)
                .Sum();

        Console.WriteLine(result);
    }


    public static Func<string, int> Compute =
        new Func<string, (SectionAssignment, SectionAssignment)>(Day4.ParseAssignments)
            .AndThen(Fn.Tupled<SectionAssignment, SectionAssignment, bool>(SectionAssignmentOps.FullOverlapExistsWith))
            .AndThen((it) => it == true ? 1 : 0);


    public static Func<string, int> Compute2 =
        new Func<string, (SectionAssignment, SectionAssignment)>(Day4.ParseAssignments)
            .AndThen(Fn.Tupled<SectionAssignment, SectionAssignment, bool>(SectionAssignmentOps.OverlapExistsWith))
            .AndThen((it) => it == true ? 1 : 0);


    public static (SectionAssignment, SectionAssignment) ParseAssignments(string input) {
        var split = input.Split(',');
        return (SectionAssignmentOps.Parse(split[0]), SectionAssignmentOps.Parse(split[1]));
    }
}

public record SectionAssignment(int Start, int End);

public static class SectionAssignmentOps {

    public static SectionAssignment Parse(string input) {
        var split = input.Split('-');
        return new SectionAssignment(int.Parse(split[0]), int.Parse(split[1]));
    }

    public static bool FullOverlapExistsWith(this SectionAssignment @this, SectionAssignment other) {
        if (@this.Start == other.Start) {
            return true;
        } else if (@this.Start < other.Start) {
            return @this.End >= other.End;
        } 

        return other.End >= @this.End;
    }

    public static bool OverlapExistsWith(this SectionAssignment @this, SectionAssignment other) {
        if (@this.Start == other.Start || @this.End == other.End) {
            return true;
        } else if (@this.Start < other.Start) {
            return @this.End >= other.Start;
        } 

        return other.End >= @this.Start;
    }
}
