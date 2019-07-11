using Perlin3D;

public static class CaveNoise
{
    public static byte Evaluate(TerrainGenerator t, int x, int y, int z)
    {
        //OpenSimplexNoise start = t.simplex4;

        float f = Config.CAVE_FREQUENCY;
        // if (start.Evaluate(x/f,y/f,z/f)>0.5) return 0;

        int px = x - 32;
        int pz = z - 32;

        if (PerlinNoise3D.Evaluate(px / f, y / f, pz / f) > 0.6) return 0;


        return 1;
    }
}
