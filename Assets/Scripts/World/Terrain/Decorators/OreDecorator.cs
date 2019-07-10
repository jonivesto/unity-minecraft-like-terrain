using NoiseTest;

public static class OreDecorator
{
    // 0 = blockID
    // 1 = instance size        [0, 9]
    // 2 = max occur height     [0, 200]
    // 3 = min occur height     [0, 200]
    // 4 = noise size           [0, 200]
    // 5 = noise displacement   [0, 200]
    static byte[,] ores = {
        {21, 7, 100, 0, 5, 3}, // Iron
        {20, 7, 57, 30, 5, 5}, // Lead
        {19, 7, 200, 0, 5, 8}, // Coal
        {18, 7, 40, 0, 5, 9}, // Gold
    };


    public static int GetOreAt(int blockSet, TerrainGenerator t, int x, int y, int z)
    {
        blockSet = 1;

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
                if (noise.Evaluate(ores[i, 5] + x / s, ores[i, 5] + y / s, ores[i, 5] + z / s) > (ores[i, 1]) / 10f)
                {
                    blockSet = ores[i, 0];
                }
            }
        }

        return blockSet;
    }
}
