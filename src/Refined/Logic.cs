namespace Refined;

public abstract class And<TValue, TLeft, TRight> : IRefinement<TValue>
    where TLeft : IRefinement<TValue>
    where TRight : IRefinement<TValue>
{
    public static string Explain => $"{TLeft.Explain} and {TRight.Explain}";

    public static void Refine(TValue value)
    {
        TLeft.Refine(value);
        TRight.Refine(value);
    }
}

public abstract class Or<TValue, TLeft, TRight> : IRefinement<TValue>
    where TLeft : IRefinement<TValue>
    where TRight : IRefinement<TValue>
{
    public static string Explain => $"{TLeft.Explain} or {TRight.Explain}";

    public static void Refine(TValue value)
    {
        try
        {
            TLeft.Refine(value);
        }
        catch (RefinementException)
        {
            TRight.Refine(value);
        }
    }
}

public abstract class Not<TValue, TRefinement> : IRefinement<TValue>
    where TRefinement : IRefinement<TValue>
{
    public static string Explain => $"not {TRefinement.Explain}";

    public static void Refine(TValue value)
    {
        try
        {
            TRefinement.Refine(value);
        }
        catch (RefinementException)
        {
            return;
        }
        throw new RefinementException();
    }
}
