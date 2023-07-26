namespace T4.Plugins.Troubadour;

public sealed class HelltideChestIconsFeature : WorldFeature<ICommonActor>
{

    public ITexture BackgroundTexture { get; }
    private HelltideChestIconsFeature()
    {
        Enabled = false;

        LineStyle = Render.GetLineStyle(200, 255, 255, 0);
        MapLineStyle = Render.GetLineStyle(200, 255, 255, 0);
        MapIconSize = 32f;
        WorldIconTexture = Textures.HelltideChest;
        WorldIconSize = 64f;
        MapIconTexture = Textures.HelltideChest;
        BackgroundTexture = Textures.CircleBackground;

        _uberChests = _uberChestsTextures.Keys.ToHashSet();
    }

    public override IEnumerable<ICommonActor> GetWorldActors()
    {
        return Game.GizmoActors.Where(x => _uberChests.Contains(x.ActorSno.SnoId));
    }

    public static HelltideChestIconsFeature Create(IPlugin plugin, string nameOf)
    {
        var feature = new HelltideChestIconsFeature
        {
            Plugin = plugin,
            NameOf = nameOf,
            DisplayName = () => Translation.Translate(plugin, "icon on chests"),
            Resources = new List<AbstractFeatureResource>()
        };
        // feature.AddDefaultGroundResources();
        // feature.AddDefaultMapResources();

        plugin.Features.Add(feature);
        return feature;
    }

    public override void PaintGround()
    {
        if (!Enabled || !OnGroundEnabled)
            return;

        foreach (var item in GetWorldActors())
        {
            _uberChestsTextures.TryGetValue(item.ActorSno.SnoId, out var texture);
            if (texture is null) 
                continue;

            var size = WorldIconSize;
            var x = item.Coordinate.ScreenX - (size / 2);
            var y = item.Coordinate.ScreenY - (size / 2);
            BackgroundTexture.Draw(x, y, size, size);
            texture.Draw(x, y, size, size);
        }
    }

    public override void PaintMap()
    {
        if (!Enabled || !OnMapEnabled)
            return;

        foreach (var item in GetWorldActors())
        {
            if (!item.Coordinate.IsOnMap)
                continue;

            _uberChestsTextures.TryGetValue(item.ActorSno.SnoId, out var texture);
            if (texture is null) 
                continue;

            var size = MapIconSize;
            var x = item.Coordinate.MapX - (size / 2);
            var y = item.Coordinate.MapY - (size / 2);
            BackgroundTexture.Draw(x, y, size, size);
            texture.Draw(x, y, size, size);
        }
    }

    private readonly HashSet<ActorSnoId> _uberChests;

    private readonly Dictionary<ActorSnoId, ITexture> _uberChestsTextures = new()
    {
        // [ActorSnoId.usz_rewardGizmo_1H] = Textures.Sword_TBN,
        // [ActorSnoId.usz_rewardGizmo_2H] = Textures.CrossedSwordsOnShield,
        // [ActorSnoId.usz_rewardGizmo_Amulet] = Textures.Amulet,
        // [ActorSnoId.usz_rewardGizmo_Boots] = Textures.Boots,
        // [ActorSnoId.usz_rewardGizmo_ChestArmor] = Textures.Sword_TBN,
        // [ActorSnoId.usz_rewardGizmo_Gloves] = Textures.Gloves,
        // [ActorSnoId.usz_rewardGizmo_Helm] = Textures.Helm,
        // [ActorSnoId.usz_rewardGizmo_Legs] = Textures.Pants,
        // [ActorSnoId.usz_rewardGizmo_Rings] = Textures.Ring,
        // [ActorSnoId.usz_rewardGizmo_Uber] = Textures.TreasureBag,
    };
}