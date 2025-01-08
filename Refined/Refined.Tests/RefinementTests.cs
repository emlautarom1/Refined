using FluentAssertions;
using Refined.Constants;
using Refined.Numeric;
using Refined.Collections;
using Refined.Logic;

namespace Refined.Tests;

public record MyRecord(
    Refine<int, NonZero<int>> X,
    Refine<int, GreaterThan<int, _10<int>>> Y,
    Refine<int, And<int, GreaterThan<int, _10<int>>, LessThan<int, _20<int>>>> Z,
    Vector<double, _10<int>> Codes
);

public class RefinementTests
{
    [Test]
    public void MultipleRefinements()
    {
        int x = 10;
        int y = 20;
        int z = 15;
        IReadOnlyList<double> codes = [1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1];
        var record = new MyRecord(
            x.Refine<int, NonZero<int>>(),
            y.Refine<int, GreaterThan<int, _10<int>>>(),
            z.Refine<int, And<int, GreaterThan<int, _10<int>>, LessThan<int, _20<int>>>>(),
            new Vector<double, _10<int>>(codes)
        );

        int unrefX = record.X;
        int unrefY = record.Y.Unrefine;
        int unrefZ = record.Z;
        IReadOnlyList<double> unrefCodes = record.Codes.AsReadOnlyList();

        x.Should().Be(unrefX);
        y.Should().Be(unrefY);
        z.Should().Be(unrefZ);
        codes.Should().BeEquivalentTo(unrefCodes);
    }
}
