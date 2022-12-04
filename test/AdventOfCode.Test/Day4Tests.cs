using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace AdventOfCode.Test;

public class Day4Tests {

    [Fact]
    public void TestParseAssignments() {
        var nooverlap = "2-4,6-8";

        var expectedFirst = new SectionAssignment(2, 4);
        var expectedSecond = new SectionAssignment(6, 8);

        var result = Day4.ParseAssignments(nooverlap);

        result.Item1.Should().Be(expectedFirst);

        result.Item2.Should().Be(expectedSecond);
    }

    [Fact]
    public void TestFullyContains() {
        var a = new SectionAssignment(2, 4);
        var b = new SectionAssignment(2, 5);
        var c = new SectionAssignment(3, 4);
        var d = new SectionAssignment(1, 3);
        var e = new SectionAssignment(3, 9);

        var result1 = a.FullOverlapExistsWith(b);
        var result2 = a.FullOverlapExistsWith(c);
        var result3 = a.FullOverlapExistsWith(d);
        var result4 = a.FullOverlapExistsWith(e);

        result1.Should().BeTrue();

        result2.Should().BeTrue();

        result3.Should().BeFalse();

        result4.Should().BeFalse();
    }
}
