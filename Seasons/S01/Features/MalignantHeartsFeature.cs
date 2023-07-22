namespace T4.Plugins.Troubadour;

public sealed class MalignantHeartsFeature : WorldFeature<ICommonActor>
{
    private MalignantHeartsFeature()
    {
        LineStyle = Render.GetLineStyle(200, 255, 255, 0);
        MapLineStyle = Render.GetLineStyle(200, 255, 255, 0);
        SnoIdsSet = _snoIds.ToHashSet();
    }

    public override IEnumerable<ICommonActor> GetWorldObjects()
    {
        return Game.GizmoActors.Where(item =>
        {
            if (!OnGroundEnabled && !OnMapEnabled)
                return false;

            return SnoIdsSet.Contains(item.ActorSno.SnoId);
        });
    }

    public static MalignantHeartsFeature Create(IPlugin plugin, string nameOf)
    {
        var feature = new MalignantHeartsFeature
        {
            Plugin = plugin,
            NameOf = nameOf,
            DisplayName = () => Translation.Translate(plugin, "malignant hearts"),
            Resources = new List<AbstractFeatureResource>()
        };

        feature.AddDefaultGroundResources();
        feature.AddDefaultMapResources();

        return feature.Register();
    }

    private HashSet<ActorSnoId> SnoIdsSet { get; }
    private readonly IEnumerable<ActorSnoId> _snoIds = new[]
    {
        ActorSnoId.S01_MalignantHeart_CaptureSequence_Main_Pink_Dyn,
        ActorSnoId.S01_MalignantHeart_CaptureSequence_Main_Blue_Dyn,
        ActorSnoId.S01_MalignantHeart_CaptureSequence_Main_Black_Dyn,
        ActorSnoId.S01_MalignantHeart_CaptureSequence_Main_Orange_Dyn,
        // ActorSnoId.S01_MalignantHeart_CaptureSequence_Main_Dyn, // <-- not sure about this one.
    };
}