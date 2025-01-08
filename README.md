# Refined

Refinement types for C# using generics and structs for minimal overhead.

Inspired by Haskell's [refined](https://hackage.haskell.org/package/refined).

## Features

- Extensible and composable refinements.
- Implicit conversions.
- Minimal overhead through the usage of Generics.
- Logical operators like `And`, `Or` and `Not`.
- Numerical operators like `<`, `>` and `=`.
- Literal constants for `string`, `int`, etc.

## Examples

```C#
// Define refinements as `abstract class` implementing `IRefinement` for any type.
public abstract class NonZero<TValue> : IRefinement<TValue> where TValue : INumber<TValue>
{
    public static void Refine(TValue value)
    {
        if (value < TValue.Zero) throw new RefinementException();
    }
}

public record Player(Refine<int, NonZero<int>> Age);

// Take any value as input.
int x = 10;

// Use `Refine` to refine a value.
// Throws `RefinementException` if the operation fails.
var player = new Player(x.Refine<int, NonZero<int>>());

// Implicit conversions are supported.
int unrefX = player.Age;
```

## Tests

```shell
dotnet test Refined
```
