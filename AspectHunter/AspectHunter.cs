using static T4.Plugins.Troubadour.AspectHunterStore;

namespace T4.Plugins.Troubadour;

public sealed class AspectHunter : BasePlugin, IGameUserInterfacePainter, IGameWorldPainter
{
    public Feature OnMap { get; private set; }
    public Feature OnGround { get; private set; }
    public Feature OnlyMyClass { get; private set; }
    public Feature Generic { get; private set; }
    public Feature GenericUnique { get; private set; }
    public Feature Barbarian { get; private set; }
    public Feature BarbarianUnique { get; private set; }
    public Feature Druid { get; private set; }
    public Feature DruidUnique { get; private set; }
    public Feature Necromancer { get; private set; }
    public Feature NecromancerUnique { get; private set; }
    public Feature Rogue { get; private set; }
    public Feature RogueUnique { get; private set; }
    public Feature Sorcerer { get; private set; }
    public Feature SorcererUnique { get; private set; }

    public AspectHunter()
    {
        Order = int.MaxValue;
        EnabledByDefault = true;
    }

    public override string GetDescription()
        => Translation.Translate(this, "Highlight most wanted aspects.");

    public void PaintGameWorld(GameWorldLayer layer)
    {
        switch (layer)
        {
            case GameWorldLayer.Ground when OnGroundEnabled:
                var groundItems = Game.Items.Where(WorldItemPredicate);
                foreach (var item in groundItems)
                {
                    LineStyle.DrawWorldEllipse(WorldCircleSize, -1, item.Coordinate, strokeWidthCorrection: WorldCircleStroke);
                    if (!OnGroundLineEnabled)
                        continue;

                    LineStyle.DrawWorldLine(item.Coordinate, Game.MyPlayerActor.Coordinate, strokeWidthCorrection: WorldCircleStroke);
                }

                break;
            case GameWorldLayer.Map when OnMapEnabled:
                var mapItems = Game.Items.Where(WorldItemPredicate);
                foreach (var item in mapItems)
                {
                    if (!Map.WorldToMapCoordinate(item.Coordinate, out var mapX, out var mapY))
                        continue;

                    MapLineStyle.DrawEllipse(mapX, mapY, MapCircleSize, MapCircleSize, strokeWidthCorrection: MapCircleStroke);
                }

                break;
        }
    }

    public void PaintGameUserInterface(GameUserInterfaceLayer layer)
    {
        // TODO: Add UI on stash & vendors
    }

    public override void Load()
    {
        OnMap = new Feature
        {
            Plugin = this,
            NameOf = nameof(OnMap),
            DisplayName = () => Translation.Translate(this, "items on map"),
            Resources = new List<AbstractFeatureResource>
            {
                new LineStyleFeatureResource { NameOf = nameof(MapLineStyle), DisplayText = this.LineStyle, LineStyle = MapLineStyle },
                new FloatFeatureResource
                {
                    NameOf = nameof(MapCircleSize),
                    DisplayText = this.Radius,
                    Getter = () => MapCircleSize,
                    Setter = newValue => MapCircleSize = newValue,
                    MinValue = 0,
                    MaxValue = 42
                },
                new FloatFeatureResource
                {
                    NameOf = nameof(MapCircleStroke),
                    DisplayText = this.Stroke,
                    Getter = () => MapCircleStroke,
                    Setter = newValue => MapCircleStroke = newValue,
                    MinValue = 0,
                    MaxValue = 10
                },
            }
        }.Register();

        OnGround = new Feature
        {
            Plugin = this,
            NameOf = nameof(OnMap),
            DisplayName = () => Translation.Translate(this, "items on ground"),
            Resources = new List<AbstractFeatureResource>
            {
                new LineStyleFeatureResource { NameOf = nameof(LineStyle), DisplayText = this.LineStyle, LineStyle = LineStyle },
                new FloatFeatureResource
                {
                    NameOf = nameof(WorldCircleSize),
                    DisplayText = this.Radius,
                    Getter = () => WorldCircleSize,
                    Setter = newValue => WorldCircleSize = newValue,
                    MinValue = 0,
                    MaxValue = 42
                },
                new FloatFeatureResource
                {
                    NameOf = nameof(WorldCircleStroke),
                    DisplayText = this.Stroke,
                    Getter = () => WorldCircleStroke,
                    Setter = newValue => WorldCircleStroke = newValue,
                    MinValue = 0,
                    MaxValue = 10
                },
                new BooleanFeatureResource
                {
                    NameOf = nameof(OnGroundLineEnabled),
                    DisplayText = () => Translation.Translate(this, "draw line to item"),
                    Getter = () => OnGroundLineEnabled,
                    Setter = newValue => OnGroundLineEnabled = newValue
                },
            }
        }.Register();

        OnlyMyClass = new Feature
        {
            Plugin = this,
            NameOf = nameof(OnlyMyClass),
            DisplayName = this.Translate("only my class"),
            Resources = new List<AbstractFeatureResource>
            {
                new BooleanFeatureResource
                {
                    NameOf = nameof(OnlyMyCurrentClass),
                    DisplayText = this.Translate("only my class"),
                    Getter = () => OnlyMyCurrentClass,
                    Setter = newValue => OnlyMyCurrentClass = newValue
                },
            }
        }.Register();

        Generic = CreateFeature(nameof(Generic), this.Translate("generic"), AffixSnoIds.Generic);
        GenericUnique = CreateFeature(nameof(GenericUnique), this.TranslateFormat("{0} (unique)", "generic"), AffixSnoIds.GenericUnique);
        Barbarian = CreateFeature(nameof(Barbarian), PlayerClassSnoId.Barbarian, AffixSnoIds.Barbarian);
        BarbarianUnique = CreateFeature(nameof(BarbarianUnique), PlayerClassSnoId.Barbarian, AffixSnoIds.BarbarianUnique, true);
        Druid = CreateFeature(nameof(Druid), PlayerClassSnoId.Druid, AffixSnoIds.Druid);
        DruidUnique = CreateFeature(nameof(DruidUnique), PlayerClassSnoId.Druid, AffixSnoIds.DruidUnique, true);
        Necromancer = CreateFeature(nameof(Necromancer), PlayerClassSnoId.Necromancer, AffixSnoIds.Necromancer);
        NecromancerUnique = CreateFeature(nameof(NecromancerUnique), PlayerClassSnoId.Necromancer, AffixSnoIds.NecromancerUnique, true);
        Rogue = CreateFeature(nameof(Rogue), PlayerClassSnoId.Rogue, AffixSnoIds.Rogue);
        RogueUnique = CreateFeature(nameof(RogueUnique), PlayerClassSnoId.Rogue, AffixSnoIds.RogueUnique, true);
        Sorcerer = CreateFeature(nameof(Sorcerer), PlayerClassSnoId.Sorcerer, AffixSnoIds.Sorcerer);
        SorcererUnique = CreateFeature(nameof(SorcererUnique), PlayerClassSnoId.Sorcerer, AffixSnoIds.SorcererUnique, true);
    }

    private Feature CreateFeature(string name, PlayerClassSnoId playerClassSnoId, List<AffixSnoId> snoIds, bool unique = false)
    {
        var className = playerClassSnoId.ToString().ToLowerInvariant();
        if (unique)
        {
            return CreateFeature(name, this.TranslateFormat("{0} (unique)", className), snoIds);
        }

        return CreateFeature(name, this.Translate(className), snoIds);
    }

    private Feature CreateFeature(string name, Func<string> translation, List<AffixSnoId> snoIds)
    {
        var feature = new Feature { Plugin = this, NameOf = name, DisplayName = translation, Resources = new List<AbstractFeatureResource>() };

        var sortedSnoIds = snoIds
            .Select(x => (snoIds: x, name: x.GetFriendlyName()))
            .OrderBy(x => x.name);
        foreach (var snoId in sortedSnoIds.Select(x => x.snoIds))
        {
            AffixSnoIdEnabled[snoId] = false;
            feature.Resources.Add(new BooleanFeatureResource
            {
                DisplayText = () => snoId.GetFriendlyName(),
                Getter = () => AffixSnoIdEnabled.TryGetValue(snoId, out var enabled) && enabled,
                Setter = newValue => AffixSnoIdEnabled[snoId] = newValue,
                NameOf = snoId.ToString()
            });
        }

        return feature.Register();
    }
}