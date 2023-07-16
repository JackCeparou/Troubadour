namespace T4.Plugins.Troubadour;

public sealed class Helltides : BasePlugin, IGameWorldPainter
{
    public IWorldFeature CinderCaches { get; private set; }
    public IWorldFeature MysteriousChests { get; private set; }
    public Feature Developer { get; private set; }

    public int Hour { get; set; }
    public float OffsetX { get; set; }
    public float OffsetY { get; set; }

    public Helltides()
    {
        Order = -1;
        EnabledByDefault = true;
    }

    public override PluginCategory Category
        => PluginCategory.WorldEvent;

    public override string GetDescription()
        => Translation.Translate(this, "Helltide companion");

    public override void Load()
    {
        CinderCaches = HelltideCindersFeature.Create(this, nameof(CinderCaches));
        MysteriousChests = MysteriousChestsFeature.Create(this, nameof(MysteriousChests));
        return;
        Developer = new Feature
        {
            Plugin = this,
            NameOf = nameof(Developer),
            DisplayName = () => "Developer",
            Resources = new List<AbstractFeatureResource>
            {
                new FloatFeatureResource
                {
                    NameOf = nameof(Hour),
                    DisplayText = () => "Fake Hour",
                    MinValue = 0,
                    MaxValue = 23,
                    Getter = () => Hour,
                    Setter = newValue => Hour = (int)Math.Floor(newValue)
                },
                new FloatFeatureResource
                {
                    NameOf = nameof(OffsetX),
                    DisplayText = () => "Offset X",
                    MinValue = -2000,
                    MaxValue = 2000,
                    Getter = () => OffsetX,
                    Setter = newValue => OffsetX = newValue
                },
                new FloatFeatureResource
                {
                    NameOf = nameof(OffsetY),
                    DisplayText = () => "Offset Y",
                    MinValue = -2000,
                    MaxValue = 2000,
                    Getter = () => OffsetY,
                    Setter = newValue => OffsetY = newValue
                }
            }
        }.Register();
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (!CinderCaches.Enabled && !MysteriousChests.Enabled)
            return;

        switch (layer)
        {
            case GameWorldLayer.Ground:
                CinderCaches.PaintGround();
                MysteriousChests.PaintGround();

                break;
            case GameWorldLayer.Map:
                CinderCaches.PaintMap();
                MysteriousChests.PaintMap();

                break;
        }
    }
}