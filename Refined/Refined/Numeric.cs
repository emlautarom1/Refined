using System;
using System.Numerics;
using Refined.Constants;

namespace Refined.Numeric;

public abstract class Add<TLeft, TRight> : IConst<int>
    where TLeft : IConst<int>
    where TRight : IConst<int>
{
    public static int Value => TLeft.Value + TRight.Value;
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

public abstract class EqualTo<TValue, TConst> : IRefinement<TValue>
    where TConst : IConst<TValue>
    where TValue : IComparable<TValue>
{
    public static void Refine(TValue value)
    {
        if (value.CompareTo(TConst.Value) != 0) throw new RefinementException();
    }
}
