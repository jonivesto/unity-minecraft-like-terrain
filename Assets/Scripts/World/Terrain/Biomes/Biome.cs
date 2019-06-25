public abstract class Biome
{
    public string biomeName;

    public float heightMultiplier = 50f;
    public float hillMultiplier;
    public float terrainMultiplier = 10f;
    public int surfaceBlock = 1;


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
