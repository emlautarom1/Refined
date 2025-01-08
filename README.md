# Refined

Refinement types for C# using generics and structs for minimal overhead.

## Features

- Extensible refinements.
- Implicit conversions.
- Minimal overhead through the usage of Generics.
- Composability through `And`, `Or`, operators.
- Literal constants (ex. `string`, `int`, etc.).

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
