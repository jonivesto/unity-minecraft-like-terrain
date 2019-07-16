
static class TreeDecorator
{
    public static void GenerateTrees(TerrainGenerator t)
    {       
        System.Random r = t.seed.ChunkBuild(t.chunk.chunkTransform);

        // Pick a random starting point
        int x = r.Next(16);      
        int z = r.Next(16);
        int y = t.GetGroundAt(x + t.worldX, z + t.worldZ);

        // Biome the tree is on
        Biome biome = t.GetBiomeAt(x + t.worldX, z + t.worldZ, y);

        // Tree for this biome
        TreeType tree = Config.TREES[r.Next(Config.TREES.Length)];

        // Dont make trees on flat
        if (t.GetHillsAt(x + t.worldX, z + t.worldZ) == 0) return;

        // If picked point biome can grow trees
        for (int id = 0; id < biome.treesPerChunk; id++)
        {
            // New position for each tree
            x = r.Next(16);          
            z = r.Next(16);
            y = t.GetGroundAt(x + t.worldX, z + t.worldZ);

            biome = t.GetBiomeAt(x + t.worldX, z + t.worldZ, y);
            tree = Config.TREES[biome.treeSpecies[r.Next(biome.treeSpecies.Length)]];

            // Block ID the tree will be created on
            int groundBlock = t.chunk.GetBlock(x, y, z);

            // Only grass and dirt can grow trees (IDs: 9, 8)
            if (groundBlock != 9 && groundBlock != 8) continue;

            // Randomize by shortening the height by a random value 
            int treeHeight = tree.height - r.Next(3);

            // Skip this tree if the terrain does not allow tree in this position
            if (TreeBlocked(x + t.worldX, y, z + t.worldZ, treeHeight, t)) continue;
            
            // Create tree
            // Set wood blocks
            for (int i = 1; i < treeHeight; i++)
            {
                t.chunk.SetBlock(x, y + i, z, tree.woodBlock);
            }

            // Set leaves
            int[] model = tree.leafModel;

            for (int i = 0; i < model.Length; i+=3)
            {
                t.terrainEngine.WorldSetBlockNoReplace(x + t.worldX + model[i], y + treeHeight + model[i+1], z + t.worldZ + model[i+2], tree.leafBlock);
            }

            // Set fruits
            if (tree.fruitBlock != 0)
            {
                int[] fruitModel = tree.fruitModel;

                for (int i = 0; i < fruitModel.Length; i += 3)
                {
                    // fruit spawn 50/50 change
                    if (r.Next(2) < 1) continue;

                    t.terrainEngine.WorldSetBlockNoReplace(x + t.worldX + fruitModel[i], y + treeHeight + fruitModel[i + 1], z + t.worldZ + fruitModel[i + 2], tree.fruitBlock);
                }
            }
        }

    }

    private static bool TreeBlocked(int x, int y, int z, int h, TerrainGenerator t)
    {
        // Surrounding blocks
        int[] patternX = new int[] { -1, -1, -1, 0, 1, 1, 1, 0 };
        int[] patternZ = new int[] { -1, 0, 1, 1, 1, 0, -1, -1 };

        for (int i = 1; i < h + 2; i++)
        {          
            // If wood is blocked
            if (!t.AllowOverwrite(t.terrainEngine.WorldGetBlock(x, y + i, z))) return true;          

            // Don't check surroundings for the first wood block
            if (i == 1) continue;

            // If surrounding blocks are blocked
            for (int j = 0; j < 8; j++)
            {
                if (!t.AllowOverwrite(t.terrainEngine.WorldGetBlock(x + patternX[j], y + i, z + patternZ[j])))
                {                 
                    return true;
                }
            }
        }

        // All clear
        return false;
    }
    /*
    // Returns wood and leaf block IDs for given species
    private static int GetBlocks(TreeSpecies species, bool getLeaves)
    {
        switch (species)
        {
            case TreeSpecies.Birch: return (getLeaves) ? 4 : 10;

            case TreeSpecies.Pine: return (getLeaves) ? 12 : 11;

            case TreeSpecies.Spruce: return (getLeaves) ? 14 : 13;

            case TreeSpecies.ThinBirch: return (getLeaves) ? 14 : 15;

            // Birch is default
            default: return (getLeaves) ? 4 : 10;
        }
    }

    // Returns shape of the tree's leaves
    private static int[] GetLeafModels(TreeSpecies species)
    {
        switch (species)
        {
            case TreeSpecies.Birch: return new int[] {
                0, 0, 0,

                1, 0, 0,
                0, 0, 1,
                0, 0, -1,
                -1, 0, 0,

                1, -1, 0,
                0, -1, 1,
                0, -1, -1,
                -1, -1, 0,
                -1, -1, -1,
                -1, -1, 1,
                1, -1, -1,
                1, -1, 1,

                2, -1, 0,
                0, -1, 2,
                0, -1, -2,
                -2, -1, 0,


                2, -2, 0,
                0, -2, 2,
                0, -2, -2,
                -2, -2, 0,

                -2, -2, -2,
                -2, -2, 2,
                2, -2, -2,
                2, -2, 2,

                -2, -2, -1,
                -2, -2, 1,
                2, -2, -1,
                2, -2, 1,

                -1, -2, -2,
                -1, -2, 2,
                1, -2, -2,
                1, -2, 2,


                1, -3, 0,
                0, -3, 1,
                0, -3, -1,
                -1, -3, 0,
                -1, -3, -1,
                -1, -3, 1,
                1, -3, -1,
                1, -3, 1,

                2, -3, 0,
                0, -3, 2,
                0, -3, -2,
                -2, -3, 0,
            };

            case TreeSpecies.ThinBirch:
                return new int[] {
                0, 0, 0,

                1, 0, 0,
                0, 0, 1,
                0, 0, -1,
                -1, 0, 0,

                1, -1, 0,
                0, -1, 1,
                0, -1, -1,
                -1, -1, 0,
                -1, -1, -1,
                -1, -1, 1,
                1, -1, -1,
                1, -1, 1,

            };

            case TreeSpecies.Spruce:
                return new int[] {
                0, 1, 0,
                0, 0, 0,

                1, 0, 0,
                0, 0, 1,
                0, 0, -1,
                -1, 0, 0,               

                1, -2, 0,
                0, -2, 1,
                0, -2, -1,
                -1, -2, 0,
                -1, -2, -1,
                -1, -2, 1,
                1, -2, -1,
                1, -2, 1,

                2, -2, 0,
                0, -2, 2,
                0, -2, -2,
                -2, -2, 0,

                1, -4, 0,
                0, -4, 1,
                0, -4, -1,
                -1, -4, 0,
                -1, -4, -1,
                -1, -4, 1,
                1, -4, -1,
                1, -4, 1,

                1, -5, 0,
                0, -5, 1,
                0, -5, -1,
                -1, -5, 0,
                -1, -5, -1,
                -1, -5, 1,
                1, -5, -1,
                1, -5, 1,

                2, -5, 0,
                0, -5, 2,
                0, -5, -2,
                -2, -5, 0,
                
            };

            case TreeSpecies.Pine:
                return new int[] {
                0, 0, 0,

                1, 0, 0,
                0, 0, 1,
                0, 0, -1,
                -1, 0, 0,

                1, -1, 0,
                0, -1, 1,
                0, -1, -1,
                -1, -1, 0,
                -1, -1, -1,
                -1, -1, 1,
                1, -1, -1,
                1, -1, 1,

                2, -1, 0,
                0, -1, 2,
                0, -1, -2,
                -2, -1, 0,


                2, -2, 0,
                0, -2, 2,
                0, -2, -2,
                -2, -2, 0,

                -2, -2, -2,
                -2, -2, 2,
                2, -2, -2,
                2, -2, 2,

                -2, -2, -1,
                -2, -2, 1,
                2, -2, -1,
                2, -2, 1,

                -1, -2, -2,
                -1, -2, 2,
                1, -2, -2,
                1, -2, 2,


                1, -3, 0,
                0, -3, 1,
                0, -3, -1,
                -1, -3, 0,
                -1, -3, -1,
                -1, -3, 1,
                1, -3, -1,
                1, -3, 1,

                2, -3, 0,
                0, -3, 2,
                0, -3, -2,
                -2, -3, 0,
            };

            default: return null;
        }
    }

    // Returns fruit item id
    // default = 0 = this species has no fruits
    private static int GetFruit(TreeSpecies species)
    {
        switch (species)
        {
            case TreeSpecies.Pine: return 16;

            default: return 0;
        }
    }

    // Returns fruit spawn points for species
    private static int[] GetFruitModel(TreeSpecies species)
    {
        switch (species)
        {
            case TreeSpecies.Pine: return new int[] 
            {
                1, -4, 0,
                2, -4, 0,
                0, -4, 1,
                0, -4, 2,
            };

            // Default is null = no fruits for this species
            default: return null;
        }
    }*/
}
