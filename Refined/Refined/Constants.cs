using System.Numerics;

namespace Refined.Constants;

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

