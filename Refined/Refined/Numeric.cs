using System;
using System.Numerics;
using Refined.Constants;
using Refined.Logic;

namespace Refined.Numeric;

public abstract class Add<TLeft, TRight> : IConst<int>
    where TLeft : IConst<int>
    where TRight : IConst<int>
{
    public static int Value => TLeft.Value + TRight.Value;
}

public abstract class NonZero<TValue> : Not<TValue, EqualTo<TValue, _0<TValue>>>
    where TValue : INumber<TValue>;

public abstract class GreaterThan<TValue, TConst> : IRefinement<TValue>
    where TConst : IConst<TValue>
    where TValue : IComparable<TValue>
{
    public static void Refine(TValue value)
    {
        if (value.CompareTo(TConst.Value) <= 0) throw new RefinementException();
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
