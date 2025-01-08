using System;

namespace Refined;

public interface IRefinement<in TValue>
{
    static abstract void Refine(TValue value);
}

public class RefinementException : Exception;

public readonly struct Refine<TValue, TRefinement> where TRefinement : IRefinement<TValue>
{
    private readonly TValue _value;

    public TValue Unrefine => _value;
    public static implicit operator TValue(Refine<TValue, TRefinement> value) => value.Unrefine;

    public Refine()
    {
        throw new RefinementException();
    }

    public Refine(TValue value)
    {
        TRefinement.Refine(value);
        _value = value;
    }
}

public static class RefinedExtensions
{
    public static Refine<TValue, TRefinement> Refine<TValue, TRefinement>(this TValue value)
        where TRefinement : IRefinement<TValue> => new(value);
}
