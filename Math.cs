using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    /// <summary>
    /// Vector 3D
    /// </summary>
    public struct Vector3
    {
        /// <summary>
        /// Empty Vector 3D
        /// </summary>
        public static Vector3 Zero { get; set; } = new Vector3(0, 0, 0);

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        /// <summary>
        /// Initialize Vector 3D
        /// </summary>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        /// <param name="Z">Z</param>
        public Vector3(int X, int Y, int Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
    }

    /// <summary>
    /// Vector 4D
    /// </summary>
    public struct Vector4
    {
        /// <summary>
        /// Empty Vector 4D
        /// </summary>
        public static Vector4 Zero { get; set; } = new Vector4(0, 0, 0, 0);

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int W { get; set; }

        /// <summary>
        /// Initialize Vector 4D
        /// </summary>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        /// <param name="Z">Z</param>
        /// <param name="Z">W</param>
        public Vector4(int X, int Y, int Z, int W)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
        }
    }
    
    /// <summary>
    /// Vector 3D (float)
    /// </summary>
    public struct Vector3F
    {
        /// <summary>
        /// Empty Vector 3D
        /// </summary>
        public static Vector3F Zero { get; set; } = new Vector3F(0.0F, 0.0F, 0.0F);

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        /// <summary>
        /// Initialize Vector 3D
        /// </summary>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        /// <param name="Z">Z</param>
        public Vector3F(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
    }

    /// <summary>
    /// Vector 4D
    /// </summary>
    public struct Vector4f
    {
        /// <summary>
        /// Empty Vector 4D
        /// </summary>
        public static Vector4f Zero { get; set; } = new Vector4f(0.0F, 0.0F, 0.0F, 0.0F);

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        /// <summary>
        /// Initialize Vector 4D
        /// </summary>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        /// <param name="Z">Z</param>
        /// <param name="Z">W</param>
        public Vector4f(float X, float Y, float Z, float W)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
        }
    }

    /// <summary>
    /// The math class, every math here!
    /// </summary>
    public class Math
    {
        /// <summary>
        /// Clamp value to min and max
        /// </summary>
        /// <param name="value">Start value</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <returns>Clamped value</returns>
        public static int Clamp(int value, int min = 0, int max = 100)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        /// <summary>
        /// Clamp value to min and max
        /// </summary>
        /// <param name="value">Start value</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <returns>Clamped value</returns>
        public static float Clamp(float value, float min = 0, float max = 100)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        /// <summary>
        /// Clamp value to min and max
        /// </summary>
        /// <param name="value">Start value</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <returns>Clamped value</returns>
        public static double Clamp(double value, double min = 0, double max = 100)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        /// <summary>
        /// Clamp value to min and max
        /// </summary>
        /// <param name="value">Start value</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <returns>Clamped value</returns>
        public static long Clamp(long value, long min = 0, long max = 100)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        /// <summary>
        /// Sinusoid
        /// </summary>
        /// <param name="frequency">Frequency</param>
        /// <param name="rate">Rate</param>
        /// <param name="bufferSize">Size of the buffer</param>
        /// <returns>Sinusoid wave</returns>
        public static short[] Sin(int frequency = 1000, int rate = 8000, int bufferSize = 100)
        {
            short[] buffer = new short[bufferSize];
            double amplitude = 0.25 * short.MaxValue;
            for (int n = 0; n < buffer.Length; n++)
            {
                buffer[n] = (short)(amplitude * System.Math.Sin((2 * System.Math.PI * n * frequency) / rate));
            }
            return buffer;
        }
    }
}
