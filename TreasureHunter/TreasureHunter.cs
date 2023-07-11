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
            DisplayName = this.Translate("on inventory"),
            Resources = new List<AbstractFeatureResource>
            {
                new FillStyleFeatureResource
                {
                    NameOf = nameof(GreyOutFillStyle),
                    DisplayText = this.GreyOut,
                    FillStyle = GreyOutFillStyle,
                },
                new FontFeatureResource
                {
                    NameOf = nameof(MatchedFilterCounterFont),
                    DisplayText = this.MatchedFilterCount,
                    Font = MatchedFilterCounterFont,
                },
            }
        }.Register();

        OnDrop = new Feature
        {
            Plugin = this,
            NameOf = nameof(OnDrop),
            DisplayName = this.Translate("on item drop"),
            Resources = new List<AbstractFeatureResource>
            {
                new BooleanFeatureResource
                {
                    NameOf = nameof(DropNotificationEnabled),
                    DisplayText = this.Translate("notify"),
                    Getter = () => DropNotificationEnabled,
                    Setter = newValue => DropNotificationEnabled = newValue
                },
            }
        }.Register();

        OnMap = new Feature
        {
            Plugin = this,
            NameOf = nameof(OnMap),
            DisplayName = this.Translate("items on map"),
            Resources = new List<AbstractFeatureResource>
            {
                new LineStyleFeatureResource
                {
                    NameOf = nameof(MapLineStyle), DisplayText = this.LineStyle, LineStyle = MapLineStyle
                },
                new FloatFeatureResource
                {
                    NameOf = nameof(MapCircleSize),
                    DisplayText = this.Radius,
                    Getter = () => MapCircleSize,
                    Setter = newValue => MapCircleSize = newValue,
                    MinValue = 0,
                    MaxValue = 20
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
            NameOf = nameof(OnGround),
            DisplayName = this.Translate("items on ground"),
            Resources = new List<AbstractFeatureResource>
            {
                new BooleanFeatureResource
                {
                    NameOf = nameof(ShowFilterNamesOnGround),
                    DisplayText = this.TreasureHunterFilterNames,
                    Getter = () => ShowFilterNamesOnGround,
                    Setter = newValue => ShowFilterNamesOnGround = newValue
                },
                new FillStyleFeatureResource
                {
                    NameOf = nameof(FilterNamesOnGroundBackground),
                    DisplayText = this.BackgroundColor,
                    FillStyle = FilterNamesOnGroundBackground,
                },
                new FontFeatureResource
                {
                    NameOf = nameof(FilterNamesOnGroundFont),
                    DisplayText = this.NormalFont,
                    Font = FilterNamesOnGroundFont,
                },
                new LineStyleFeatureResource
                {
                    NameOf = nameof(LineStyle), DisplayText = this.LineStyle, LineStyle = LineStyle
                },
                new FloatFeatureResource
                {
                    NameOf = nameof(WorldCircleSize),
                    DisplayText = this.Radius,
                    Getter = () => WorldCircleSize,
                    Setter = newValue => WorldCircleSize = newValue,
                    MinValue = 0,
                    MaxValue = 2
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