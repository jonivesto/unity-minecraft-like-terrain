using System;

public class Seed
{
    public string value;

    public Seed()
    {
        Random random = new Random();

        for (int i = 0; i < 16; i++)
        {
            value += random.Next(1, 10);
        }
    }

    public Seed(string value)
    {
        this.value = value;
    }

    public int GetInt(int index)
    {
        return value[index] - '0';
    }

    public float GetFloat(int index)
    {
        return (float)value[index] - '0';
    }

}
