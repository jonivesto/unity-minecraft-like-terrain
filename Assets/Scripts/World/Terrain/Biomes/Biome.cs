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


    // Returns biome's index in Config.BIOMES[]
    public int GetID()
    {
        for (int i = 1; i < Config.BIOMES.Length; i++)
        {
            if (Config.BIOMES[i].Equals(this)) return i;
        }

        return 0;
    }

    // Runs all decorators
    // Use seed to always get same result when generating
    public void RunDecorators(Seed seed, Chunk chunk, int x, int z, int ground)
    {
        foreach (byte decoratorId in activeDecorators)
        {
            Config.BIOME_DECORATORS[decoratorId].Decorate(seed, chunk, x, z, ground);
        }
    }
}
