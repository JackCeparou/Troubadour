namespace T4.Plugins.Troubadour;

/// <summary>
/// - inventory features
///     - enabled
///     - upgraded items
///     - gem items
///     - silent chest keys
///     - sigil items (not yet possible to evaluate)
/// - elixirs
/// - mount cosmetics
/// - aspect hunter
///     - eternal
///     - seasonal
/// - S01: Of the Malignant
///     - invokers
///     - caged hearts (handled by aspect hunter as seasonal affix)
/// </summary>
public static class InventoryGreyOut
{
    public static IFillStyle FillStyle { get; } = Render.GetFillStyle(180, 0, 0, 0);

    private static bool _sorted;
    private static List<(int Order, Func<IItem, bool?> Evaluator)> Rules { get; set; } = new();

    /// <summary>
    ///     Add a grey out rule.
    /// </summary>
    /// <param name="order">will be used to sort rules before first evaluation</param>
    /// <param name="rule">returns either
    /// - null if evaluation should continue
    /// - bool if evaluation should stop
    /// </param>
    public static void RegisterRule(int order, Func<IItem, bool?> rule)
    {
        Rules.Add((order, rule));
    }

    public static bool Evaluate(IItem item)
    {
        if (!_sorted)
        {
            Rules = Rules.OrderBy(r => r.Order).ToList();
            _sorted = true;
        }

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var rule in Rules)
        {
            var result = rule.Evaluator.Invoke(item);
            if (result.HasValue)
                return result.Value;
        }

        return false;
    }
}