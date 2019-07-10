using NoiseTest;

public static class CaveNoise
{
    public static byte Evaluate(TerrainGenerator t, int x, int y, int z)
    {
        OpenSimplexNoise start = t.simplex4;

        float f = Config.CAVE_FREQUENCY;
        if (start.Evaluate(x/f,y/f,z/f)>0.5) return 0;

        //TODO

        return 1;
    }
}
