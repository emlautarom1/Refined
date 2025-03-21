using System;

namespace Refined;

public interface IRefinement<in TValue>
{
    static abstract void Refine(TValue value);
    static abstract string Explain { get; }
}

public class RefinementException : Exception
{
    public RefinementException()
    {
    }

    public RefinementException(string? message, RefinementException inner) : base(message, inner)
    {
    }
}


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
        try
        {
            TRefinement.Refine(value);
            _value = value;
        }
        catch (RefinementException e)
        {
            throw new RefinementException($$"""

                The following contraint was not satisfied:
                    '{{TRefinement.Explain}}'
                by value:
                    '{{value}}'
            """, e);
        }
    }
}

public static class RefinedExtensions
{
    public static Refine<TValue, TRefinement> Refine<TValue, TRefinement>(this TValue value)
        where TRefinement : IRefinement<TValue> => new(value);
}
