using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class scndRadEquation
    {
        public static float CalculDelta(float a, float b, float c)
        {
            float delta = (Mathf.Pow(b, 2)) - (4 * a * c);
            return delta;
        }

        public static List<float> CalculateEquation(float a, float b, float c)
        {
            List<float> results = new List<float>();

            float delta = CalculDelta(a, b, c);

            if (delta > 0 && a != 0)
            {
                results.Add((-b - (Mathf.Sqrt(delta))) / (2 * a));
                results.Add((-b + (Mathf.Sqrt(delta))) / (2 * a));
            }
            else if (delta == 0 && a != 0)
            {
                results.Add(-b / (2 * a));
            }
            return results;
        }
    }
}

