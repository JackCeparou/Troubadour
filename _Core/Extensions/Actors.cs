namespace T4.Plugins.Troubadour;

public static partial class Actors
{
    // this is awful, but till the hitbox radius is found in game data, this is the only way
    public static float GetWorldRadius(this ICommonActor actor)
    {
        return actor.ActorSno.SnoId switch
        {
            // spikes 1.3f
            ActorSnoId.DRLG_Trap_Spikes_2x2 => 1.3f,
            ActorSnoId.DRLG_Trap_Spikes_3x3 => 1.3f,
            // barrels 0.5f
            ActorSnoId.DRLG_Trap_Barrel_Cold_AOE => 0.5f,
            ActorSnoId.DRLG_Trap_Barrel_Fire_AOE => 0.5f,
            ActorSnoId.DRLG_Trap_Barrel_Lightning_AOE => 0.5f,
            ActorSnoId.DRLG_Trap_Barrel_Poison_AOE => 0.5f,
            ActorSnoId.DRLG_Trap_Barrel_Shadow_AOE => 0.5f,
            // traps 1f
            // ActorSnoId.DRLG_Trap_BearTrap,
            ActorSnoId.DRLG_Trap_BearTrap_DestroyableObject => 1f,
            ActorSnoId.DRLG_Trap_Moonseed => 1f,
            // mines 1f
            ActorSnoId.DRLG_Trap_Mine_Cold => 1f,
            ActorSnoId.DRLG_Trap_Mine_Fire => 1f,
            ActorSnoId.DRLG_Trap_Mine_Lightning => 1f,
            ActorSnoId.DRLG_Trap_Mine_Poison => 1f,
            ActorSnoId.DRLG_Trap_Mine_Shadow => 1f,
            // falls 1f
            ActorSnoId.DRLG_Trap_FallingRocks_FallingRocks => 1f,
            ActorSnoId.DRLG_Trap_FallingRocks_FallingRocksBrickDark => 1f,
            ActorSnoId.DRLG_Trap_FallingRocks_FallingRocksBrickDarkAndWood => 1f,
            ActorSnoId.DRLG_Trap_FallingRocks_FallingRocksBrickLight => 1f,
            ActorSnoId.DRLG_Trap_FallingRocks_FallingRocksSand => 1f,
            // dungeon affixes
            ActorSnoId.Dungeon_Affix_Orb => 1f,
            ActorSnoId.dungeon_affix_driftingShade_payload_burst_coasters => 1f,
            ActorSnoId.dungeon_affix_driftingShade_payload_persistent_ringMesh => 1f,
            ActorSnoId.dungeon_affix_driftingShade_projectile => 1f,
            ActorSnoId.dungeon_affix_driftingShade_projectile_coreMesh => 1f,
            ActorSnoId.dungeon_affix_driftingShade_projectile_parent => 1f,
            ActorSnoId.dungeon_affix_driftingShade_warning_coasters => 1f,
            //
            ActorSnoId.DRLG_Trap_Demonic => 1f,
            _ => 1f
        };
    }
}