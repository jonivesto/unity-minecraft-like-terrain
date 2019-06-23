public class Config
{
    // List of items in the game
    // Index is item ID    
    // If you dont include item here, it will not work.
    public static Item[] ID = {
        new            AirBlock(),     // 0
        new          StoneBlock(),     // 1
        new        BedrockBlock(),     // 2
        new          GlassBlock(),     // 3
        new    BirchLeavesBlock()      // 4
    };

    // Save options
    public static bool SAVE_CHUNKS_ON_GENERATE = false;

    // Terrain generation
    public static bool GENERATE_CAVES = true;

}
