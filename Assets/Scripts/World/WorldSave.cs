using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class WorldSave
{
    private string worldName;
    private byte terrainId;
    private Seed seed;

    public WorldSave(string worldName, Seed seed, byte terrainId)
    {
        this.terrainId = terrainId;
        this.seed = seed;
        SetWorld(worldName);
    }

    private void SetWorld(string worldName)
    {
        this.worldName = worldName;

        // Create folders for this world if they do not exist
        string path = Path.Combine(Application.persistentDataPath, worldName);
        if (!Directory.Exists(path))
        {
            // Main directory
            Directory.CreateDirectory(path);

            // Chunks directory
            string chunkPath = Path.Combine(path, "Chunks");
            Directory.CreateDirectory(chunkPath);

            // Save world seed
            using (StreamWriter file = new StreamWriter(path + "/Seed.txt"))
            {
                file.WriteLine(seed.ToString());
            }

            Debug.Log("World save created at: " + path);
        }
        else
        {
            Debug.Log("World save exists at: " + path);
        }

        // Terrain sub-folder for this terrainId   
        // Create if not exist
        path = Path.Combine(path, "Chunks/" + terrainId);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
            
    }

    public void SaveChunk(Chunk chunk)
    {
        string path = Application.persistentDataPath 
            + "/" + worldName
            + "/Chunks"
            + "/" + terrainId
            + "/" + chunk.chunkTransform.ToString() 
            + ".chk";


        // Create file or find existing one
        FileStream file;

        if (File.Exists(path))
        {
            file = File.OpenWrite(path);
        }
        else
        {
            file = File.Create(path);
        }

        // Write and close
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(file, chunk.chunkData);
        file.Close();
    }

    public bool ChunkFileExists(ChunkTransform chunkTransform)
    {
        string path = Application.persistentDataPath 
            + "/" + worldName 
            + "/Chunks"
            + "/" + terrainId
            + "/" + chunkTransform.ToString() 
            + ".chk";

        return File.Exists(path);
    }

    public ChunkData LoadChunk(ChunkTransform chunkTransform)
    {
        string path = Application.persistentDataPath 
            + "/" + worldName
            + "/Chunks"
            + "/" + terrainId
            + "/" + chunkTransform.ToString() 
            + ".chk";

        // Make sure file exists
        FileStream file;

        if (File.Exists(path))
        {
            file = File.OpenRead(path);
        }
        else
        {
            Debug.LogError(chunkTransform.ToString() + " - File not found");
            return null;
        }

        // Load and close
        BinaryFormatter formatter = new BinaryFormatter();
        ChunkData chunkData = (ChunkData)formatter.Deserialize(file);
        file.Close();

        return chunkData;
    }
}
