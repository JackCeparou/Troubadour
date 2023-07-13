using static T4.Plugins.Troubadour.TreasureHunterStore;

namespace T4.Plugins.Troubadour;

public sealed class TreasureHunter : BasePlugin, IGameWorldPainter, IItemDetector
{
    public Feature OnInventory { get; private set; }
    public Feature OnDrop { get; private set; }
    public Feature OnMap { get; private set; }
    public Feature OnGround { get; private set; }

    public TreasureHunter()
    {
        EnabledByDefault = true;
        Order = int.MaxValue;
    }

    public override string GetDescription()
        => Translation.Translate(this, "Highlight most wanted items.\nClose this window and press F3 to configure rules.");

    public void PaintGameWorld(GameWorldLayer layer)
    {
        switch (layer)
        {
            case GameWorldLayer.Ground when OnGround.Enabled:
                var groundItems = Game.Items.Where(WorldItemPredicate);
                foreach (var item in groundItems)
                {
                    LineStyle.DrawWorldEllipse(WorldCircleSize, -1, item.Coordinate, strokeWidthCorrection: WorldCircleStroke);
                    if (OnGroundLineEnabled)
                        LineStyle.DrawWorldLine(item.Coordinate, Game.MyPlayerActor.Coordinate, strokeWidthCorrection: WorldCircleStroke);
                    if (!ShowFilterNamesOnGround || item.MatchingFilterNames.Length == 0)
                        continue;

                    var names = string.Join(Environment.NewLine, item.MatchingFilterNames);
                    var tl = FilterNamesOnGroundFont.GetTextLayout(names);
                    var x = item.Coordinate.ScreenX - (tl.Width / 2);
                    var y = item.Coordinate.ScreenY - (tl.Height / 2);
                    const float padding = 2f;
                    FilterNamesOnGroundBackground.FillRectangle(x - padding, y - padding, tl.Width + (padding * 2), tl.Height + (padding * 2));
                    tl.DrawText(x, y);
                }

                break;
            case GameWorldLayer.Map when OnMap.Enabled:
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

    public override void Load()
    {
        OnInventory = new Feature
        {
            Plugin = this,
            NameOf = nameof(OnInventory),
            DisplayName = () => Translation.Translate(this, "on inventory"),
            Resources = new List<AbstractFeatureResource>
            {
                new FillStyleFeatureResource
                {
                    NameOf = nameof(GreyOutFillStyle),
                    DisplayText = () => Translation.Translate(this, "grey out"),
                    FillStyle = GreyOutFillStyle,
                },
                new FontFeatureResource
                {
                    NameOf = nameof(MatchedFilterCounterFont),
                    DisplayText = () => Translation.Translate(this, "matched filter count"),
                    Font = MatchedFilterCounterFont,
                },
            }
        }.Register();

        OnDrop = new Feature
        {
            Plugin = this,
            NameOf = nameof(OnDrop),
            DisplayName = () => Translation.Translate(this, "on item drop"),
            Resources = new List<AbstractFeatureResource>
            {
                new BooleanFeatureResource
                {
                    NameOf = nameof(DropNotificationEnabled),
                    DisplayText = () => Translation.Translate(this, "notify"),
                    Getter = () => DropNotificationEnabled,
                    Setter = newValue => DropNotificationEnabled = newValue
                },
            }
        }.Register();

        OnMap = new Feature
        {
            Plugin = this,
            NameOf = nameof(OnMap),
            DisplayName = () => Translation.Translate(this, "items on map"),
            Resources = new List<AbstractFeatureResource>
            {
                new LineStyleFeatureResource
                {
                    NameOf = nameof(MapLineStyle), DisplayText = () => Translation.Translate(this, "line style"), LineStyle = MapLineStyle
                },
                new FloatFeatureResource
                {
                    NameOf = nameof(MapCircleSize),
                    DisplayText = () => Translation.Translate(this, "radius"),
                    Getter = () => MapCircleSize,
                    Setter = newValue => MapCircleSize = newValue,
                    MinValue = 0,
                    MaxValue = 20
                },
                new FloatFeatureResource
                {
                    NameOf = nameof(MapCircleStroke),
                    DisplayText = () => Translation.Translate(this, "stroke"),
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
            NameOf = nameof(OnGround),
            DisplayName = () => Translation.Translate(this, "items on ground"),
            Resources = new List<AbstractFeatureResource>
            {
                new BooleanFeatureResource
                {
                    NameOf = nameof(ShowFilterNamesOnGround),
                    DisplayText = () =>  Translation.Translate(this, "matched filter names"),
                    Getter = () => ShowFilterNamesOnGround,
                    Setter = newValue => ShowFilterNamesOnGround = newValue
                },
                new FillStyleFeatureResource
                {
                    NameOf = nameof(FilterNamesOnGroundBackground),
                    DisplayText = () => Translation.Translate(this, "background color"),
                    FillStyle = FilterNamesOnGroundBackground,
                },
                new FontFeatureResource
                {
                    NameOf = nameof(FilterNamesOnGroundFont),
                    DisplayText = () => Translation.Translate(this, "normal font"),
                    Font = FilterNamesOnGroundFont,
                },
                new LineStyleFeatureResource
                {
                    NameOf = nameof(LineStyle), DisplayText = () => Translation.Translate(this, "line style"), LineStyle = LineStyle
                },
                new FloatFeatureResource
                {
                    NameOf = nameof(WorldCircleSize),
                    DisplayText = () => Translation.Translate(this, "radius"),
                    Getter = () => WorldCircleSize,
                    Setter = newValue => WorldCircleSize = newValue,
                    MinValue = 0,
                    MaxValue = 2
                },
                new FloatFeatureResource
                {
                    NameOf = nameof(WorldCircleStroke),
                    DisplayText = () => Translation.Translate(this, "stroke"),
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
    }

    public void OnItemDetected(IItem item)
    {
        if (!OnDrop.Enabled || !DropNotificationEnabled)
            return;
        if (item.Location != ItemLocation.None)
            return;
        if (item.MatchingFilterNames.Length == 0)
            return;

        item.NotifyMatchedFilters(this);
    }
}