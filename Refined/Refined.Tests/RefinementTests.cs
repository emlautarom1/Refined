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
    public void Record()
    {
        int x = 10;
        int y = 20;
        int z = 15;
        IReadOnlyList<double> codes = [1, 2, 3, 4, 5, 6, 7, 8, 9, 0];
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

    [Test]
    public void RefineAndUnrefine()
    {
        int x = 10;
        var refined = x.Refine<int, NonZero<int>>();

        int unrefined = refined;
        int unrefinedExplicit = refined.Unrefine;

        x.Should().Be(unrefined);
        x.Should().Be(unrefinedExplicit);
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(10)]
    [TestCase(1337)]
    [TestCase(int.MaxValue)]
    public void NumericNonZero(int value)
    {
        var refined = value.Refine<int, NonZero<int>>();

        refined.Unrefine.Should().Be(value);
    }

    [Test]
    public void NonZeroThrowsOnZero()
    {
        Action tryRefine = () => 0.Refine<int, NonZero<int>>();

        tryRefine.Should().Throw<RefinementException>();
    }

    [Test]
    public void GreaterThan()
    {
        var value = 11;
        var refined = value.Refine<int, GreaterThan<int, _10<int>>>();

        refined.Unrefine.Should().Be(value);
    }

    [Test]
    public void GreaterThanThrowsOnEqual()
    {
        Action tryRefine = () => 10.Refine<int, GreaterThan<int, _10<int>>>();

        tryRefine.Should().Throw<RefinementException>();
    }

    [Test]
    public void GreaterThanThrowsOnLessThan()
    {
        Action tryRefine = () => 9.Refine<int, GreaterThan<int, _10<int>>>();

        tryRefine.Should().Throw<RefinementException>();
    }


    [Test]
    public void LessThan()
    {
        var value = 9;
        var refined = value.Refine<int, LessThan<int, _10<int>>>();

        refined.Unrefine.Should().Be(value);
    }

    [Test]
    public void LessThanThrowsOnEqual()
    {
        Action tryRefine = () => 10.Refine<int, LessThan<int, _10<int>>>();

        tryRefine.Should().Throw<RefinementException>();
    }

    [Test]
    public void LessThanThrowsOnGreaterThan()
    {
        Action tryRefine = () => 11.Refine<int, LessThan<int, _10<int>>>();

        tryRefine.Should().Throw<RefinementException>();
    }

    [Test]
    public void AndRange()
    {
        var value = 15;
        var refined = value.Refine<int, And<int, GreaterThan<int, _10<int>>, LessThan<int, _20<int>>>>();

        refined.Unrefine.Should().Be(value);
    }

    [Test]
    public void AndThrowsOnLeft()
    {
        Action tryRefine = () => 0.Refine<int, And<int, GreaterThan<int, _10<int>>, LessThan<int, _20<int>>>>();

        tryRefine.Should().Throw<RefinementException>();
    }

    [Test]
    public void AndThrowsOnRight()
    {
        Action tryRefine = () => 21.Refine<int, And<int, GreaterThan<int, _10<int>>, LessThan<int, _20<int>>>>();

        tryRefine.Should().Throw<RefinementException>();
    }

    [Test]
    public void OrAcceptsLeft()
    {
        var value = 0;
        var refined = value.Refine<int, Or<int, EqualTo<int, _0<int>>, GreaterThan<int, _10<int>>>>();

        refined.Unrefine.Should().Be(value);
    }

    [Test]
    public void OrAcceptsRight()
    {
        var value = 0;
        var refined = value.Refine<int, Or<int, GreaterThan<int, _10<int>>, EqualTo<int, _0<int>>>>();

        refined.Unrefine.Should().Be(value);
    }

    [Test]
    public void OrThrowsOnNeither()
    {
        Action tryRefine = () => 0.Refine<int, Or<int, EqualTo<int, _10<int>>, EqualTo<int, _20<int>>>>();

        tryRefine.Should().Throw<RefinementException>();
    }

    [Test]
    public void Vector()
    {
        IReadOnlyList<double> codes = [1, 2, 3, 4, 5, 6, 7, 8, 9, 0];
        var vector = new Vector<double, _10<int>>(codes);

        var unrefCodes = vector.AsReadOnlyList();

        codes.Should().BeEquivalentTo(unrefCodes);
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(9)]
    [TestCase(11)]
    public void VectorThrowsOnInvalidLength(int length)
    {
        IReadOnlyList<double> codes = Enumerable.Repeat(0.0, length).ToList();
        Func<Vector<double, _10<int>>> tryRefine = () => new(codes);

        tryRefine.Should().Throw<RefinementException>();
    }

    [Test]
    public void ForAll()
    {
        IReadOnlyCollection<int> collection = [1, 2, 3];
        var refined = collection.Refine<IReadOnlyCollection<int>, ForAll<IReadOnlyCollection<int>, int, NonZero<int>>>();

        refined.Unrefine.Should().BeEquivalentTo(collection);
    }
}
