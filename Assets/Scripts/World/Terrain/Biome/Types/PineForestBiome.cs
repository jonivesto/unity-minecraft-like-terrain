
public class PineForestBiome : Biome
{
    public PineForestBiome()
    {
        surfaceBlock = 9;

        treesPerChunk = 4;

        treeSpecies = new TreeSpecies[]
        {
            TreeSpecies.Pine,
            TreeSpecies.Pine,
        };

        grass = 17;
    }
}
