namespace T4.Plugins.Troubadour;

public sealed class HelltideEventsFeature : WorldFeature<ICommonActor>
{
    private HelltideEventsFeature()
    {
        OnGroundEnabled = false;
        MapIconSize = 24f;
        MapIconTexture = Textures.BountyEventActive;
    }

    public override IEnumerable<ICommonActor> GetWorldObjects()
    {
        yield break;
    }

    public static HelltideEventsFeature Create(IPlugin plugin, string nameOf)
    {
        var feature = new HelltideEventsFeature
        {
            Plugin = plugin, NameOf = nameOf, DisplayName = () => Translation.Translate(plugin, "events"), Resources = new List<AbstractFeatureResource>()
        };

        return feature.Register();
    }

    public override void PaintMap()
    {
        if (!Enabled || !OnMapEnabled)
            return;
        var helltide = Game.HelltideEventMarkers.FirstOrDefault(x => x.EndsInMilliseconds > 0);
        if (helltide is null)
            return;

        foreach (var quest in GetZoneEvents(helltide.SubzoneSno.SnoId))
        {
            if (!Map.WorldToMapCoordinate(quest.WorldCoordinate, out var mapX, out var mapY))
                continue;

            MapLineStyle?.DrawEllipse(mapX, mapY, MapCircleSize, MapCircleSize, strokeWidthCorrection: MapCircleStroke);
            MapIconTexture?.Draw(mapX - (MapIconSize / 2), mapY - (MapIconSize / 2), MapIconSize, MapIconSize);
        }
    }

    private static IEnumerable<IQuestSno> GetZoneEvents(SubzoneSnoId snoId)
    {
        switch (snoId)
        {
            case SubzoneSnoId.Frac_Tundra_N:
            case SubzoneSnoId.Frac_Tundra_S:
                return GameData.GetBountyEvents(x => x.SubzoneSno?.SnoId is SubzoneSnoId.Frac_Tundra_N or SubzoneSnoId.Frac_Tundra_S);

            case SubzoneSnoId.Scos_Coast:
            case SubzoneSnoId.Scos_Deep_Forest:
            case SubzoneSnoId.Scos_ZoneEvent:
                return GameData.GetBountyEvents(x => x.SubzoneSno?.SnoId is SubzoneSnoId.Scos_Coast or SubzoneSnoId.Scos_Deep_Forest);

            case SubzoneSnoId.Step_South:
            case SubzoneSnoId.Step_Central:
            case SubzoneSnoId.Step_TempleOfRot:
            case SubzoneSnoId.Step_ZoneEvent:
                return GameData.GetBountyEvents(x => x.SubzoneSno?.SnoId is SubzoneSnoId.Step_South or SubzoneSnoId.Step_Central);

            case SubzoneSnoId.Kehj_Oasis:
            case SubzoneSnoId.Kehj_LowDesert:
            case SubzoneSnoId.Kehj_HighDesert:
            case SubzoneSnoId.Kehj_ZoneEvent:
                return GameData.GetBountyEvents(x => x.SubzoneSno?.SnoId is SubzoneSnoId.Kehj_Oasis or SubzoneSnoId.Kehj_LowDesert or SubzoneSnoId.Kehj_HighDesert);

            case SubzoneSnoId.Hawe_Verge:
            case SubzoneSnoId.Hawe_Wetland:
            case SubzoneSnoId.Hawe_ZoneEvent:
                return GameData.GetBountyEvents(x => x.SubzoneSno?.SnoId is SubzoneSnoId.Hawe_Verge or SubzoneSnoId.Hawe_Wetland);

            default:
                return Array.Empty<IQuestSno>();
        }
    }
}