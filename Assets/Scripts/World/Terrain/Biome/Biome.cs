
public abstract class Biome
{
    // Biome's display name
    public string biomeName;

    // ID of the block used to cover the stone terrain
    // Default is stone block 
    public int surfaceBlock = 1;
    public int hiddenSurfaceBlock = 8;

    // Block layers covering the stone terrain
    public int surfaceDepth = 4;

    // How many times the tree decorator will try to place trees in one chunk
    // value 0 means this biome does not have trees
    public int treesPerChunk = 7;

    // Tree species that can grow in this biome
    // You can increase the chance of one tree species over others by adding the same value many times
    public int[] treeSpecies = 
    {
        0,1,2,3
    };

    // Grass item id
    public int grass = 0;

    // Amount of grass
    public float grassDensity = 0.5f;

    // Structures that can spawn on this biome
    // Structure index in Config.STRUCTURES[]
    public int[] structures =
    {
        0, 1
    };

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
