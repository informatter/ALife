using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



namespace SharpMatter.SharpMath
{
    public static class SharpMath
    {
        /// <summary>
        /// lerp two values by a factor of t
        /// </summary>
        /// <param name="t0"></param>
        /// <param name="t1"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static double Lerp(double t0, double t1, double t)
        {
            return t0 + (t1 - t0) * t;
        }


        public static float Lerp(float t0, float t1, float t)
        {
            return t0 + (t1 - t0) * t;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="t0"></param>
        /// <param name="t1"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static double Normalize(double t, double t0, double t1)
        {
            return (t - t0) / (t1 - t0);
        }

        public static float Normalize(float t, float t0, float t1)
        {
            return (t - t0) / (t1 - t0);
        }


        /// <summary>
        /// Remap values to a target domain
        /// </summary>
        /// <param name="val"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="targetMin"></param>
        /// <param name="targetMax"></param>
        /// <returns></returns>
        //public static double ReMapValues(double val, double min, double max, double targetMin, double targetMax)
        //{
        //    return targetMin + (targetMax - targetMin) * ((val - min) / (max - min));
        //}


        /// <summary>
        /// Maps a number from one interval to another
        /// </summary>
        /// <param name="t"></param>
        /// <param name="a0"></param>
        /// <param name="a1"></param>
        /// <param name="b0"></param>
        /// <param name="b1"></param>
        /// <returns></returns>
        //public static double Remap(double t, double a0, double a1, double b0, double b1)
        //{
        //    return Lerp(b0, b1, Normalize(t, a0, a1));
        //}


        
        public static List<float> Range(float numberOfSteps)
        {
            float steps = 1.0f / numberOfSteps;
            float count = 0;
            List<float> values = new List<float>();
            for (int i = 0; i < numberOfSteps; i++)
            {
                values.Add(count += steps);
            }

            
            return values;

        }

    






        /// <summary>
        /// /
        /// </summary>
        /// <param name="t"></param>
        /// <param name="a0"></param>
        /// <param name="a1"></param>
        /// <param name="b0"></param>
        /// <param name="b1"></param>
        /// <returns></returns>
        public static float Remap(float t, float a0, float a1, float b0, float b1)
        {
            return Lerp(b0, b1, Normalize(t, a0, a1));
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double Clamp(double t, double min, double max)
        {

            if (max == 0 && min != 0)
            {
                if (t <= min)
                    t = min;
            }

            if (max != 0 && min == 0)
            {
                if (t >= max) t = max;
            }

            if (max != 0 && min != 0)
            {

                if (t >= max) t = max;
                if (t <= min) t = min;

            }

            return t;

        }

    

        public static float Clamp(float t, float min, float max)
        {
            if (max == 0 && min!=0)
            {
                if (t < min)
                    t = min;
            }

            if (max !=0 && min == 0)
            {
                if (t > max) t = max;
            }

            if (max != 0 && min!=0)
            {

                if (t > max) t = max;
                if (t < min) t = min;

            }

            return t;
        }


        public static float Clamp(ref float t, float min, float max)
        {
            if (max == 0 && min != 0)
            {
                if (t < min)
                    t = min;
            }

            if (max != 0 && min == 0)
            {
                if (t > max) t = max;
            }

            if (max != 0 && min != 0)
            {

                if (t > max) t = max;
                if (t < min) t = min;

            }

            return t;
        }



        public static float Constrain(float x, float min, float max)
        {
            return Math.Max(min, (Math.Min(x, max)));
        }


        /// <summary>
        /// Compare to numbers for similarity within a threshHold value to compare with
        /// </summary>
        /// <param name="value1"></param> value1 to compare
        /// <param name="value2"></param> value2 to compare
        /// <param name="threshHold"></param> threshHold to test agains
        /// <returns></returns> returns a boolean value for true or false

        public static bool Similar(float value1, float value2, float threshHold)
        {
            return Math.Abs(value1 - value2) <= threshHold;
        }

        public static bool Similar(float value1, float value2, float threshHold, out float deltaV)
        {
            deltaV = Math.Abs(value1 - value2);
            return Math.Abs(value1 - value2) <= threshHold;
        }


        /// <summary>
        /// Comprare 2 vectors.  returns true if both vectors are similar within the given threshHold
        /// </summary>
        /// <param name="vecA"></param>  first vector
        /// <param name="value2"></param> second vector
        /// <param name="threshHold"></param> threshHold to compare by
        /// <returns> returns true if both vectors are similar within the given threshHold</returns>
        public static bool Similar(Vector3 vecA, Vector3 value2, float threshHold)
        {
            float value1Mag =vecA.magnitude;
            float value2Mag = value2.magnitude;
            return Math.Abs((value1Mag - value2Mag)) <= threshHold;
           
        }






    }
}
