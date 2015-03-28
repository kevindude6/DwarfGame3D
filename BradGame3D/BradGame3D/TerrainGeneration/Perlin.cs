using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BradGame3D.TerrainGeneration
{
    class Perlin
    {
        const float persistence = 0.5f;
        static public float[][] GenerateWhiteNoise(int width, int height)
        {
            float[][] noise = GetEmptyArray(width, height);

            Random r = new Random();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    noise[i][j] = (float)r.NextDouble();
                }
            }

            return noise;
        }
        static public float[][] GenerateSmoothNoise(float[][] noise, int octave)
        {
            int width = noise.Length;
            int height = noise[0].Length;

            float[][] smoothNoise = GetEmptyArray(width, height);

            int samplePeriod = (int) Math.Pow(2, octave);
            float sampleFrequency = 1.0f / samplePeriod;

            for (int i = 0; i < width; i++)
            {
                int sample_i0 = (i / samplePeriod) * samplePeriod;
                int sample_i1 = (sample_i0 + samplePeriod) % width;

                float horizontal_blend = (i - sample_i0) * sampleFrequency;


                for (int j = 0; j < height; j++)
                {
                    int sample_j0 = (j / samplePeriod) * samplePeriod;
                    int sample_j1 = (sample_j0 + samplePeriod) % width;

                    float vertical_blend = (j - sample_j0) * sampleFrequency;

                    float top = Interpolate(noise[sample_i0][sample_j0], noise[sample_i1][sample_j0], horizontal_blend);
                    float bot = Interpolate(noise[sample_i0][sample_j1], noise[sample_i1][sample_j1], horizontal_blend);

                    smoothNoise[i][j] = Interpolate(top, bot, vertical_blend);
                }
            }
            return smoothNoise;

        }
        static float Interpolate(float a, float b, float x)
        {
            return a * (1 - x) + x * b;
        }

        public static float[][] GeneratePerlinNoise(float[][] baseNoise, int octaves)
        {
            int size = baseNoise.Length;

            //float[][][] smoothNoise = new float[octaves][][];

            //float persistence = 0.25f;

            for (int i = 0; i < octaves; i++)
            {
                //smoothNoise[i] = GenerateSmoothNoise(baseNoise, i);
            }

            float[][] perlinNoise = GetEmptyArray(size,size);
            
            float amplitude = 1.0f;
            float totalAmplitude = 0.0f;

            for(int octave = octaves-1; octave >= 0; octave--)
            {
                amplitude*=persistence;
                totalAmplitude+=amplitude;
                float[][] smoothNoise = GenerateSmoothNoise(baseNoise, octave);
                for(int i = 0; i < size; i++)
                {
                    for(int j = 0; j < size; j++)
                    {
                        //perlinNoise[i][j] += smoothNoise[octave][i][j]*amplitude;
                        perlinNoise[i][j] += smoothNoise[i][j] * amplitude;
                    }
                }
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    perlinNoise[i][j] /= totalAmplitude;
                }
            }
            return perlinNoise;


        }
        public static float[][] GenerateIslandPerlinNoise(float[][] baseNoise, int octaves)
        {

            float[][] perlinNoise = GeneratePerlinNoise(baseNoise, octaves);
            int centerX = 256;
            int centerY = 256;
            int centerRadius = 450;//660
            for (int y = 0; y < perlinNoise.Length; y++)
            {
                for (int x = 0; x < perlinNoise.Length; x++)
                {
                    //perlinNoise[y][x] -= (float) (Math.Sqrt((y - centerY) * (y - centerY) + (x - centerX) * (x - centerX)) * 0.0013);
                    float distOut  = (float) Math.Sqrt((y - centerY) * (y - centerY) + (x - centerX) * (x - centerX));
                    float dist = centerRadius - distOut;
                    if( dist<0)
                        dist = 0;
                    perlinNoise[y][x] -= Math.Abs(1/(dist*0.0098f));
                }

            }
            float max = 0;
            for (int y = 0; y < perlinNoise.Length; y++)
            {
                for (int x = 0; x < perlinNoise.Length; x++)
                {
                    if (perlinNoise[y][x] < 0)
                        perlinNoise[y][x] = 0;
                    if (perlinNoise[y][x] > max)
                        max = perlinNoise[y][x];
                }
            }

            for (int y = 0; y < perlinNoise.Length; y++)
            {
                for (int x = 0; x < perlinNoise.Length; x++)
                {
                    perlinNoise[y][x] /= max;
                }
            }
            return perlinNoise;
            


        }
        static public float[][] GetEmptyArray(int width, int height)
        {
            float[][] bears = new float[height][];
            for (int i = 0; i < width; i++)
            {
                bears[i] = new float[width];
            }
            return bears;
        }
    }
}
