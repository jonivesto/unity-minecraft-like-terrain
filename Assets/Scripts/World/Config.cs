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
        new                 Lava(),     // 22
    };

    // List of biomes in the game
    // Index is biome ID    
    public static Biome[] BIOMES =
    {
        new           OceanBiome(),      // 0
        new     BirchForestBiome(),      // 1
        new      PineForestBiome(),      // 2
    };

    // List of structures in the game
    // Index is structure ID 
    public static Structure[] STRUCTURES =
    {
        new   DebugPyramidStructure(),   // 0
        new             TombNetwork(),   // 1
    };

    // List of trees in the game
    // Index is tree ID 
    public static TreeType[] TREES =
    {
        new            BirchTree(),      // 0
        new             PineTree(),      // 1
        new        ThinBirchTree(),      // 2
        new           SpruceTree(),      // 3
    };

    // Debug
    public static bool SAVE_CHUNKS_ON_GENERATE = false;
    public static bool UPDATE_PLAYER_POSITION = true;
    public static bool UNLOAD_FAR_CHUNKS = false;

    // Terrain generation
    public static Seed seed;
    public const int WORLD_HEIGHT = 208;
    public const int CHUNK_SIZE = 16;

    public static int SEA_LEVEL = 80;
    public static float CONTINENT_SIZE = 800f;
    public static float HILL_SIZE = 110f;
    public static float FLATNESS = 1600f;
    public static float BIOME_SIZE = 400f;
    public static float HILLS_MULTIPLIER = 100f;

    public static bool GENERATE_CAVES = true;
    public static float CAVE_FREQUENCY = 20f;
    public static bool GENERATE_ORES = true;

}
