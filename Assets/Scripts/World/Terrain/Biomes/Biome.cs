public abstract class Biome
{
    // Biome's display name
    public string biomeName;

    // ID of the block used to cover the stone terrain
    // Default is stone block 
    public int surfaceBlock = 1; 

    // Block layers covering the stone terrain
    public int surfaceDepth = 4;

    // Decorators for this biome type. (like trees and rocks)
    // Default is none
    public byte[] activeDecorators = new byte[]{};

    // How many times the tree decorator will try to place trees in one chunk
    // value 0 means this biome does not have trees
    public int treesPerChunk = 5;

    public int woodBlock = 10;
    public int leavesBlock = 4;

    public int minTreeHeight = 5;
    public int maxTreeHeight = 8;



    // Returns biome's index in Config.BIOMES[]
    public int GetID()
    {
        for (int i = 1; i < Config.BIOMES.Length; i++)
        {
            if (Config.BIOMES[i].Equals(this)) return i;
        }

        return 0;
    }

    
}
