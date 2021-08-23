using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
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
