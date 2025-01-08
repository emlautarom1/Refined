namespace Refined.Logic;

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
