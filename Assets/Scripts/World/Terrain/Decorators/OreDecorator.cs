using NoiseTest;

public static class OreDecorator
{
    // 0 = blockID
    // 1 = instance size        [0, 1]
    // 2 = max occur height     [0, 200]
    // 3 = min occur height     [0, 200]
    // 4 = noise size           [0, 200]
    static byte[,] ores = {
        {21, 9, 100, 0, 10}, // Iron
        {20, 8, 57, 30, 11}, // Lead
        {19, 8, 200, 0, 9}, // Coal
        {18, 9, 40, 0, 13}, // Gold
    };


    public static int GetOreAt(TerrainGenerator t, int x, int y, int z)
    {
        // Ore to be returned
        int ore = 0;

        // Each array
        for (int i = 0; i < ores.GetLength(0); i++)
        {            
            // Ore can spawn in this height
            if(y < ores[i, 2] && y > ores[i, 3])
            {
                // Select noise to be used for this ore type
                OpenSimplexNoise noise;

                if (i % 2 != 0)
                {
                    noise = t.simplex1;
                }
                else
                {
                    noise = t.simplex2;
                }

                // Get from noise
                float s = ores[i, 4];
                if (noise.Evaluate(x / s, y / s, z / s) > (ores[i, 1]) / 10f)
                {
                    ore = ores[i, 0];
                }
            }
        }

        return ore;
    }
}
