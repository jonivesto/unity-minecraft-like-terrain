public class Config
{
    // List of items in the game
    // Index is item ID    
    public static Item[] ID = {
        new             AirBlock(),     // 0
        new           StoneBlock(),     // 1
        new         BedrockBlock(),     // 2
        new           GlassBlock(),     // 3
        new     BirchLeavesBlock(),     // 4
        new                Water(),     // 5
        new            WaterFlow(),     // 6
        new            SandBlock(),     // 7
        new            DirtBlock(),     // 8
        new           GrassBlock(),     // 9
        new       BirchWoodBlock(),     // 10
        new        PineWoodBlock(),     // 11
        new      PineLeavesBlock(),     // 12
        new      SpruceWoodBlock(),     // 13
        new    SpruceLeavesBlock(),     // 14
        new        ThinBirchWood(),     // 15
        new            PineCones(),     // 16
        new                Grass(),     // 17
        new         GoldOreBlock(),     // 18
        new         CoalOreBlock(),     // 19
        new         LeadOreBlock(),     // 20
        new         IronOreBlock(),     // 21
    };

    // List of biomes in the game
    // Index is biome ID    
    public static Biome[] BIOMES =
    {
        new           OceanBiome(),      // 0
        new     BirchForestBiome(),      // 1
        new           SwampBiome(),      // 2
    };


    // Debug
    public static bool SAVE_CHUNKS_ON_GENERATE = false;
    public static bool UPDATE_PLAYER_POSITION = true;
    public static bool UNLOAD_FAR_CHUNKS = false;

    // Terrain generation
    public static int SEA_LEVEL = 64;
    public static float CONTINENT_SIZE = 800f;
    public static float HILL_SIZE = 105f;
    public static float FLATNESS = 1000f;

    public static bool GENERATE_CAVES = true;
    public static float CAVE_FREQUENCY = 6f;

    public static bool GENERATE_ORES = true;

}
