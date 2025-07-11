using Godot;
using System;

namespace Snowdrama.Core
{
    public class RandomAndNoise
    {
        /// <summary>
        /// Min C# implementatiion based on the example hash function
        /// in Squirrel Eiserloh's GDC Talk 
        /// https://www.youtube.com/watch?v=LWFzPP8ZbdU
        /// 
        /// Not cryptographically safe!
        /// 
        /// if It sucks or doesn't produce good results it's 100% my implementation
        /// and not their algorithm
        /// </summary>
        /// <param name="position">Some positional value in the noise array</param>
        /// <param name="seed">The noise functions seed</param>
        /// <returns></returns>
        public static uint Squirrel3(int position, uint seed = 69420)
        {
            const uint NOISE_1 = 0xB5297A4D;
            const uint NOISE_2 = 0x68E31DA4;
            const uint NOISE_3 = 0x1B56C4E9;

            uint mangled = (uint)position;
            mangled *= NOISE_1;
            mangled += seed;
            mangled ^= mangled >> 8;
            mangled *= NOISE_2;
            mangled ^= mangled << 8;
            mangled *= NOISE_3;
            mangled ^= mangled >> 8;
            return mangled;
        }

        public static int Squirrel3Range(int min, int max, int position, uint seed = 69420)
        {
            uint value = Squirrel3(position, seed);

            uint difference = (uint)Math.Abs(min - max);

            uint mod = value % difference;

            return (int)mod + min;
        }

        public static float NormalizedSquirrel3(int position, uint seed = 69420)
        {
            uint value = Squirrel3(position, seed);
            //do the calculation in double precision
            double calc = (double)value / uint.MaxValue;
            //cast to float
            return (float)calc;
        }

        public static uint Squirrel3_2D(int x, int y, uint seed = 69420)
        {
            uint xValue = Squirrel3(x, seed);
            uint yValue = Squirrel3(y, seed);

            //??? no clue how good a value this returns
            return xValue ^ yValue;
        }

        public static uint Squirrel3_2D(int x, int y, int z, uint seed = 69420)
        {
            uint xValue = Squirrel3(x, seed);
            uint yValue = Squirrel3(y, seed);
            uint zValue = Squirrel3(z, seed);

            //??? no clue how good a value this returns
            return xValue ^ yValue ^ zValue;
        }



        /// <summary>
        /// Min modified 64 bit version of the Squirrel3 hash function from:
        /// Squirrel Eiserloh's GDC Talk 
        /// https://www.youtube.com/watch?v=LWFzPP8ZbdU
        /// 
        /// Not cryptographically safe!
        /// 
        /// Totally untested on how random it actually is, how performant it is
        /// or how quality the random is.
        /// 
        /// This function is intended to be a joke... unless it works...
        /// </summary>
        /// <param name="position">Some positional value in the noise array</param>
        /// <param name="seed">The noise functions seed</param>
        /// <returns></returns>
        public static long Squirrel64(int position, long seed = 69420717580085)
        {
            const long NOISE_1 = 0xB5297A4D;
            const long NOISE_2 = 0x68E31DA4;
            const long NOISE_3 = 0x1B56C4E9;

            long mangled = (long)position;
            mangled *= NOISE_1;
            mangled += seed;
            mangled ^= mangled >> 16;
            mangled *= NOISE_2;
            mangled ^= mangled << 16;
            mangled *= NOISE_3;
            mangled ^= mangled >> 16;
            return mangled;
        }

        private static Random rand = new Random();
        public static float RandomRange(float min, float max)
        {
            return (float)(min + (rand.NextDouble() * (max - min)));
        }
        public static float RandomPercent()
        {
            return (float)rand.NextDouble() * 100.0f;
        }
        public static float RandomRange(Vector2 range)
        {
            return (float)(range.X + (rand.NextDouble() * (range.Y - range.X)));
        }
        public static double RandomRange(double min, double max)
        {
            return (double)(min + (rand.NextDouble() * (max - min)));
        }
        public static void RandomBytes(ref byte[] byteArrayRef)
        {
            rand.NextBytes(byteArrayRef);
        }
        public static byte[] RandomBytes(int numberOfBytes)
        {
            byte[] bytes = new byte[numberOfBytes];
            rand.NextBytes(bytes);
            return bytes;
        }
        public static int RandomRange(int min, int max)
        {
            return rand.Next(min, max);
        }
        public static float RandomAngleDegrees()
        {
            return (float)rand.NextDouble() * 360.0f;
        }
        public static float RandomAngle()
        {
            return (float)rand.NextDouble() * Mathf.Tau;
        }
        public static float RandomSelection(float[] options)
        {
            return options[RandomRange(0, options.Length)];
        }
        public static int RandomSelection(int[] options)
        {
            return options[RandomRange(0, options.Length)];
        }

        public static Vector2 RandomPosition(Vector2 min, Vector2 max)
        {
            var x = RandomRange(min.X, max.X);
            var y = RandomRange(min.Y, max.Y);
            return new Vector2(x, y);
        }
    }
}