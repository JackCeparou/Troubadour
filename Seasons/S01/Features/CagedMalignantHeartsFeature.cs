using static T4.Plugins.Troubadour.SeasonOfTheMalignantStore;

namespace T4.Plugins.Troubadour;

public sealed class CagedMalignantHeartsFeature : WorldFeature<ICommonActor>
{
    private CagedMalignantHeartsFeature()
    {
        LineStyle = Render.GetLineStyle(255, 178, 0, 255);
        MapLineStyle = Render.GetLineStyle(255, 178, 0, 255);
        WorldCircleSize = 0.5f;
        WorldCircleStroke = 6f;
        MapCircleSize = 8f;
        MapCircleStroke = 4f;
    }

    public override IEnumerable<ICommonActor> GetWorldObjects()
    {
        return Game.Items.Where(item =>
        {
            if (!OnGroundEnabled && !OnMapEnabled)
                return false;
            if (item.Location != ItemLocation.None)
                return false;

            var affix = item.CurrentAffixes.FirstOrDefault(x => x.SnoId.IsMalignantHeartAffix());
            if (affix is not null)
            {
                return MalignantHeartAffixSnoIdEnabled.TryGetValue(affix.SnoId, out var enabled) && enabled;
            }

            return false;
        });
    }

    public static CagedMalignantHeartsFeature Create(IPlugin plugin, string nameOf)
    {
        var feature = new CagedMalignantHeartsFeature
        {
            Plugin = plugin,
            NameOf = nameOf,
            DisplayName = () => Translation.Translate(plugin, "caged malignant hearts"),
            Resources = new List<AbstractFeatureResource>()
        };

        feature.AddDefaultGroundResources();
        feature.AddDefaultMapResources();
        foreach (var snoId in MalignantHeartAffixesSnoIds)
        {
            MalignantHeartAffixSnoIdEnabled[snoId] = false;
            feature.Resources.Add(new BooleanFeatureResource()
            {
                NameOf = snoId.ToString(),
                DisplayText = () => GameData.GetAffixSno(snoId)?.GetFriendlyName() ?? snoId.ToString(),
                Getter = () => MalignantHeartAffixSnoIdEnabled.TryGetValue(snoId, out var enabled) && enabled,
                Setter = newValue => MalignantHeartAffixSnoIdEnabled[snoId] = newValue
            });
        }

        return feature.Register();
    }
}