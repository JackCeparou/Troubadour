/*

namespace T4.Plugins.Troubadour;

public sealed class Quartermaster : BasePlugin
{
    public Feature Config { get; set; }
    public bool AspectHunter { get; set; } = true;
    public bool TreasureHunter { get; set; } = true;

    public Quartermaster()
    {
        EnabledByDefault = IsDevSession;
    }

    public override void Load()
    {
        Config = new Feature
        {
            Plugin = this,
            NameOf = nameof(Config),
            DisplayName = () => Translation.Translate(this, nameof(Config)),
            Resources = new List<AbstractFeatureResource>
            {
                new BooleanFeatureResource
                {
                    NameOf = nameof(AspectHunter),
                    DisplayText = () => nameof(AspectHunter),
                    Getter = () => AspectHunter,
                    Setter = value => AspectHunter = value,
                },
                new BooleanFeatureResource
                {
                    NameOf = nameof(TreasureHunter),
                    DisplayText = () => nameof(TreasureHunter),
                    Getter = () => TreasureHunter,
                    Setter = value => TreasureHunter = value,
                },
            }
        }.Register();
    }

    //TODO: translations when ready to enable by default
    public override string GetDescription() => "Display notifications:\nEXPERIMENTAL / IN DEVELOPMENT";
}

//*/