public class Config
{
    // List of items in the game
    // Index is item ID
    public static Item[] ID = {
        new            AirBlock(),     // 0
        new          StoneBlock(),     // 1
        new        BedrockBlock(),     // 2
        new          GlassBlock(),     // 3
        new    BirchLeavesBlock()      // 4
    };

    public static bool SAVE_CHUNKS_ON_GENERATE = false;
}
