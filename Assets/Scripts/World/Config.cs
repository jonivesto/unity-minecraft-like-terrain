﻿public class Config
{
    // List of items in the game
    // Index is item ID    
    // If you dont include item here, it will not work.
    public static Item[] ID = {
        new            AirBlock(),     // 0
        new          StoneBlock(),     // 1
        new        BedrockBlock(),     // 2
        new          GlassBlock(),     // 3
        new    BirchLeavesBlock(),     // 4
        new               Water(),     // 5
        new           WaterFlow(),     // 6
        new           SandBlock(),     // 7
        new           DirtBlock(),     // 8
        new          GrassBlock(),     // 9
    };

    // List of biomes in the game
    // Index is biome ID    
    // If you dont include biome here, it will not work.
    public static Biome[] BIOMES =
    {
        new         OceanBiome(),      // 0
        new   BirchForestBiome(),      // 1
        new         SwampBiome(),      // 2
    };

    // Debug
    public static bool SAVE_CHUNKS_ON_GENERATE = false;
    public static bool UPDATE_PLAYER_POSITION = false;

    // Terrain generation
    public static int SEA_LEVEL = 64;
    public static float CONTINENT_SIZE = 800f;
    public static float HILL_SIZE = 105f;
    public static float FLATNESS = 1000f;
    public static int SURFACE_DEPTH = 4;

    public static float HUMIDITY_MAP_SCALE = 1600f;
    public static float TEMPERATURE_MAP_SCALE = 2000f;

}
