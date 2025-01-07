using System;
using System.Collections.Generic;
using System.Numerics;
using FluentAssertions;

namespace Refined;

public interface IRefinement<in TValue>
{
    static abstract void Refine(TValue value);
}

public class RefinementException : Exception;

public readonly struct Refined<TValue, TRefinement> where TRefinement : IRefinement<TValue>
{
    private readonly TValue _value;

    public TValue Unrefine => _value;
    public static implicit operator TValue(Refined<TValue, TRefinement> value) => value.Unrefine;

    public Refined()
    {
        throw new RefinementException();
    }

    public Refined(TValue value)
    {
        TRefinement.Refine(value);
        _value = value;
    }
}

public static class RefinedExtensions
{
    public static Refined<TValue, TRefinement> Refine<TValue, TRefinement>(this TValue value)
        where TRefinement : IRefinement<TValue> => new(value);
}

public interface IConst<out T>
{
    static abstract T Value { get; }
}

public abstract class _10<T> : IConst<T> where T : INumber<T>
{
    public static T Value => T.CreateChecked(10);
}

public abstract class _20<T> : IConst<T> where T : INumber<T>
{
    public static T Value => T.CreateChecked(20);
}

public abstract class Add<TLeft, TRight> : IConst<int>
    where TLeft : IConst<int>
    where TRight : IConst<int>
{
    public static int Value => TLeft.Value + TRight.Value;
}

public abstract class And<TValue, TLeft, TRight> : IRefinement<TValue>
    where TLeft : IRefinement<TValue>
    where TRight : IRefinement<TValue>
{
    public static void Refine(TValue value)
    {
        TLeft.Refine(value);
        TRight.Refine(value);
    }
}

public abstract class NonZero<TValue> : IRefinement<TValue> where TValue : INumber<TValue>
{
    public static void Refine(TValue value)
    {
        if (value < TValue.Zero) throw new RefinementException();
    }
}

public abstract class GreaterThan<TValue, TConst> : IRefinement<TValue>
    where TConst : IConst<TValue>
    where TValue : IComparable<TValue>
{
    public static void Refine(TValue value)
    {
        if (value.CompareTo(TConst.Value) < 0) throw new RefinementException();
    }
}

public abstract class LessThan<TValue, TConst> : IRefinement<TValue>
    where TConst : IConst<TValue>
    where TValue : IComparable<TValue>
{
    public static void Refine(TValue value)
    {
        if (value.CompareTo(TConst.Value) >= 0) throw new RefinementException();
    }
}

public abstract class CountIs<TElem, TRefinement> : IRefinement<IReadOnlyCollection<TElem>>
    where TRefinement : IRefinement<int>
{
    public static void Refine(IReadOnlyCollection<TElem> value)
    {
        TRefinement.Refine(value.Count);
    }
}

public readonly struct Vector<TElem, TLength>(IReadOnlyList<TElem> elements)
    where TLength : IConst<int>
{
    private readonly Refined<IReadOnlyList<TElem>, CountIs<TElem, GreaterThan<int, TLength>>> _refined =
        elements.Refine<IReadOnlyList<TElem>, CountIs<TElem, GreaterThan<int, TLength>>>();

    public IReadOnlyList<TElem> AsReadOnlyList() => _refined.Unrefine;
}

public record MyRecord(
    Refined<int, NonZero<int>> X,
    Refined<int, GreaterThan<int, _10<int>>> Y,
    Refined<int, And<int, GreaterThan<int, _10<int>>, LessThan<int, _20<int>>>> Z,
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
