using System.Collections.Generic;
using Refined.Constants;
using Refined.Numeric;

namespace Refined.Collections;

public abstract class CountIs<TElem, TRefinement> : IRefinement<IReadOnlyCollection<TElem>>
    where TRefinement : IRefinement<int>
{
    public static void Refine(IReadOnlyCollection<TElem> value)
    {
        TRefinement.Refine(value.Count);
    }
}

public readonly struct Vector<TElem, TLength>(IReadOnlyList<TElem> elements)
    where TLength : IConst<int>
{
    private readonly Refine<IReadOnlyList<TElem>, CountIs<TElem, EqualTo<int, TLength>>> _refined =
        elements.Refine<IReadOnlyList<TElem>, CountIs<TElem, EqualTo<int, TLength>>>();

    public IReadOnlyList<TElem> AsReadOnlyList() => _refined.Unrefine;
}
