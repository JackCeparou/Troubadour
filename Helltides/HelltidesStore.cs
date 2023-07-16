// ReSharper disable InconsistentNaming

namespace T4.Plugins.Troubadour;

public readonly record struct HelltideChest(float X, float Y, string Tag);

public static class HelltidesStore
{
    // https://www.reddit.com/r/diablo4/comments/14cv0t0/world_bosses_helltides_and_legion_events_are_all/

    // fractured peaks
    private static readonly HelltideChest[] FP_G1_1A_2B = { new(-1412, -258, "FP_1A_G1"), new(-1513, 336, "FP_2B_G1"), };
    private static readonly HelltideChest[] FP_G2_1B_2A = { new(-1482, -251, "FP_1B_G2"), new(-1552, 155, "FP_2A_G2"), };

    // scosglen
    private static readonly HelltideChest[] SG_G1_1A_2D = { new(-1790, -1290, "SG_1A_G1"), new(-1304, -784, "SG_2D_G1"), };
    private static readonly HelltideChest[] SG_G2_1D_2A = { new(-1699.5f, -1070, "SG_1D_G2"), new(-1218, -1126, "SG_2A_G2"), };
    private static readonly HelltideChest[] SG_G3_1C_2B = { new(-2072, -989, "SG_1C_G3"), new(-1515, -1020, "SG_2B_G3"), };
    private static readonly HelltideChest[] SG_G4_1B_2C = { new(-2076, -1088, "SG_1B_G4"), new(-1371, -971, "SG_2C_G4"), };

    // dry steppes
    private static readonly HelltideChest[] DS_G1_1A_2B = { new(-1061.5f, -667, "DS_1A_G1"), new(-500, -228, "DS_2B_G1"), };
    private static readonly HelltideChest[] DS_G2_1B_2A = { new(-914, -500, "DS_1B_G2"), new(-285, -625, "DS_2A_G2"), };

    // kehjistan
    private static readonly HelltideChest[] KJ_G1_1A_1B_1C = { new(136, -882, "KJ_1A_G1"), new(58, -670, "KJ_1B_G1"), new(55, -626.5f, "KJ_1C_G1"), };
    private static readonly HelltideChest KJ_G2_2A = new(202.5f, -784.5f, "KJ_2A_G2");
    private static readonly HelltideChest KJ_G2_3A = new(785, -378, "KJ_3A_G2");
    private static readonly HelltideChest KJ_G3_2B = new(376, -618, "KJ_2B_G3");
    private static readonly HelltideChest KJ_G3_3B = new(598, -123, "KJ_3B_G3");

    // hawezar
    private static readonly HelltideChest[] HZ_G1_1A_2B = { new(-718, 235, "HZ_1A_G1"), new(-212, 740, "HZ_2B_G1"), };
    private static readonly HelltideChest[] HZ_G2_1B_2A = { new(-1105, 375, "HZ_1B_G2"), new(-553, 655, "HZ_2A_G2"), };

    public static IEnumerable<HelltideChest> GetActiveMysteriousChests(SubzoneSnoId snoId, int hour)
    {
        switch (snoId)
        {
            case SubzoneSnoId.Frac_Tundra_N:
            case SubzoneSnoId.Frac_Tundra_S:
                // 0AM - 1A, 2B
                // 1AM - 2B, 1A
                // 2AM - repeats, same as 12AM, 1A, 2B,
                // 3AM - same as 1AM
                return (hour % 2) switch
                {
                    1 => FP_G2_1B_2A,
                    _ => FP_G1_1A_2B,
                };

            case SubzoneSnoId.Scos_Coast:
            case SubzoneSnoId.Scos_Deep_Forest:
            case SubzoneSnoId.Scos_ZoneEvent:
                // 0AM - 1A, 2D
                // 1AM - 1D, 2A
                // 2AM - 1C, 2B
                // 3AM - 1B, 2C
                // 4AM - repeats, same as 12AM, 1A, 2D
                return (hour % 4) switch
                {
                    1 => SG_G2_1D_2A,
                    2 => SG_G3_1C_2B,
                    3 => SG_G4_1B_2C,
                    _ => SG_G1_1A_2D,
                };

            case SubzoneSnoId.Step_South:
            case SubzoneSnoId.Step_Central:
            case SubzoneSnoId.Step_TempleOfRot:
            case SubzoneSnoId.Step_ZoneEvent:
                // 0AM - 1A, 2B
                // 1AM - 1B, 2A
                // 2AM - repeats
                return (hour % 2) switch
                {
                    1 => DS_G2_1B_2A,
                    _ => DS_G1_1A_2B,
                };

            case SubzoneSnoId.Kehj_Oasis:
            case SubzoneSnoId.Kehj_LowDesert:
            case SubzoneSnoId.Kehj_HighDesert:
            case SubzoneSnoId.Kehj_ZoneEvent:
                return GetActiveMysteriousChests(hour);

            case SubzoneSnoId.Hawe_Verge:
            case SubzoneSnoId.Hawe_Wetland:
            case SubzoneSnoId.Hawe_ZoneEvent:
                // 0AM - 1A, 2B
                // 1AM - 1B, 2AM
                // 2AM - repeats
                return (hour % 2) switch
                {
                    1 => HZ_G2_1B_2A,
                    _ => HZ_G1_1A_2B,
                };
            default:
                return Array.Empty<HelltideChest>();
        }
    }

    public static IEnumerable<HelltideChest> GetActiveMysteriousChests(int hour)
    {
        // Group 1
        // 0AM - 1B	0AM
        // 1AM - 1C	1AM
        // 2AM - 1A	2AM
        // 3AM - repeats 0AM, 1B
        yield return (hour % 3) switch
        {
            1 => KJ_G1_1A_1B_1C[2],
            2 => KJ_G1_1A_1B_1C[0],
            _ => KJ_G1_1A_1B_1C[1],
        };
        // Group 2/3
        // 0AM - 2B, 3B
        // 1AM - 2A, 3A
        // 2AM - repeats 0AM, 2B, 3B
        // 3AM - repeats 1AM, 2A, 3A
        switch (hour % 2)
        {
            case 1:
                yield return KJ_G2_2A;
                yield return KJ_G2_3A;

                break;
            default:
                yield return KJ_G3_2B;
                yield return KJ_G3_3B;

                break;
        }
    }

    // used for debugging, should be removed
    public static IEnumerable<HelltideChest> GetAllMysteriousChests()
    {
        foreach (var chest in FP_G1_1A_2B)
        {
            yield return chest;
        }

        foreach (var chest in FP_G2_1B_2A)
        {
            yield return chest;
        }

        foreach (var chest in SG_G1_1A_2D)
        {
            yield return chest;
        }

        foreach (var chest in SG_G2_1D_2A)
        {
            yield return chest;
        }

        foreach (var chest in SG_G3_1C_2B)
        {
            yield return chest;
        }

        foreach (var chest in SG_G4_1B_2C)
        {
            yield return chest;
        }

        foreach (var chest in DS_G1_1A_2B)
        {
            yield return chest;
        }

        foreach (var chest in DS_G2_1B_2A)
        {
            yield return chest;
        }

        foreach (var chest in HZ_G1_1A_2B)
        {
            yield return chest;
        }

        foreach (var chest in HZ_G2_1B_2A)
        {
            yield return chest;
        }

        foreach (var chest in KJ_G1_1A_1B_1C)
        {
            yield return chest;
        }

        yield return KJ_G2_2A;
        yield return KJ_G2_3A;
        yield return KJ_G3_2B;
        yield return KJ_G3_3B;
    }
}