
public class BirchForestBiome : Biome
{
    public BirchForestBiome()
    {
        surfaceBlock = 9;

        treesPerChunk = 5;

        treeSpecies = new TreeSpecies[]
        {
            TreeSpecies.Birch,
            TreeSpecies.Birch,
            TreeSpecies.ThinBirch,
            TreeSpecies.Pine
        };
}
}
