using System;
using System.Collections.Generic;

namespace Refined;

public abstract class CountIs<TElem, TRefinement> : IRefinement<IReadOnlyCollection<TElem>>
    where TRefinement : IRefinement<int>
{
    public static string Explain => $"Collection.Count should be {TRefinement.Explain}";

    public static void Refine(IReadOnlyCollection<TElem> value)
    {
        TRefinement.Refine(value.Count);
    }
}

public abstract class ForAll<TColl, TElem, TRefinement> : IRefinement<TColl>
    where TColl : IReadOnlyCollection<TElem>
    where TRefinement : IRefinement<TElem>
{
    public static string Explain => $"All elements in Collection should be {TRefinement.Explain}";

    public static void Refine(TColl value)
    {
        foreach (var elem in value)
        {
            TRefinement.Refine(elem);
        }
    }
}

public readonly struct Vector<TElem, TLength>(IReadOnlyList<TElem> elements)
    where TLength : IConst<int>
{
    private readonly Refine<IReadOnlyList<TElem>, CountIs<TElem, EqualTo<int, TLength>>> _refined =
        elements.Refine<IReadOnlyList<TElem>, CountIs<TElem, EqualTo<int, TLength>>>();

    public IReadOnlyList<TElem> AsReadOnlyList() => _refined.Unrefine;
}

public readonly struct Bytes<TLength>(Memory<byte> memory)
    where TLength : IConst<int>
{
    private abstract class LengthIs<TRefinement> : IRefinement<Memory<byte>>
    where TRefinement : IRefinement<int>
    {
        public static string Explain => $"Memory.Length should be {TRefinement.Explain}";

        public static void Refine(Memory<byte> value)
        {
            TRefinement.Refine(value.Length);
        }
    }

    private readonly Refine<Memory<byte>, LengthIs<EqualTo<int, TLength>>> _refined =
        memory.Refine<Memory<byte>, LengthIs<EqualTo<int, TLength>>>();

    public Memory<byte> AsMemory() => _refined.Unrefine;
}
