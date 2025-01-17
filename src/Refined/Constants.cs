using System.Numerics;

namespace Refined;

public interface IConst<out T>
{
    static abstract T Value { get; }
}

public abstract class _0<T> : IConst<T> where T : INumber<T>
{
    public static T Value => T.CreateChecked(0);
}

public abstract class _1<T> : IConst<T> where T : INumber<T>
{
    public static T Value => T.CreateChecked(1);
}

public abstract class _10<T> : IConst<T> where T : INumber<T>
{
    public static T Value => T.CreateChecked(10);
}

public abstract class _20<T> : IConst<T> where T : INumber<T>
{
    public static T Value => T.CreateChecked(20);
}
