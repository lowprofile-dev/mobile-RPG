using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using Random = System.Random;

namespace DunGen
{
    /**
     * A series of classes for getting a random value between a given range
     */

    [Serializable]
	public class IntRange
	{
        public int Min;
        public int Max;


        public IntRange() { }
        public IntRange(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public int GetRandom(Random random)
        {
            if (Min > Max)
                Max = Min;

            return random.Next(Min, Max + 1);
        }

		public override string ToString()
		{
			return Min + " - " + Max;
		}
	}

	[Serializable]
    public class FloatRange
    {
        public float Min;
        public float Max;


        public FloatRange() { }
        public FloatRange(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float GetRandom(Random random)
        {
            if (Min > Max)
            {
                float temp = Min;
                Min = Max;
                Max = temp;
            }

            float range = Max - Min;
            return Min + ((float)random.NextDouble() * range);
        }
    }
}
