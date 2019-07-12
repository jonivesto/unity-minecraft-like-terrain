using UnityEngine;

namespace NoiseTest
{
    public static class Perlin
    {
        public static float Noise(float x, float y)
        {
            return Mathf.PerlinNoise(x, y);
        }

        public static float Noise3D(float x, float y, float z)
        {
            float AB = Mathf.PerlinNoise(x, y);
            float BC = Mathf.PerlinNoise(y, z);
            float AC = Mathf.PerlinNoise(x, z);

            float BA = Mathf.PerlinNoise(y, x);
            float CB = Mathf.PerlinNoise(z, y);
            float CA = Mathf.PerlinNoise(z, x);

            float ABC = AB + BC + AC + BA + CB + CA;
            return ABC / 6f;
        }

        public static float NoiseDistorted(float x, float y, float strength)
        {
            float xDistortion = strength * Distort(x + 2.3f, y + 2.9f);
            float yDistortion = strength * Distort(x - 3.1f, y - 4.3f);

            return Mathf.PerlinNoise(x + xDistortion, y + yDistortion);
        }

        private static float Distort(float x, float y)
        {
            float wiggleDensity = 4.7f;
            return Mathf.PerlinNoise(x * wiggleDensity, y * wiggleDensity);
        }
    }
}