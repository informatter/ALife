using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

namespace SharpMatter.SharpDataStructures
{
    /// <summary>
    /// Vector data structure
    /// </summary>
    [Serializable]
    public struct  VectorData : IEquatable<VectorData>
    {

        #region Static Operators

        public static bool operator ==(VectorData a, VectorData b)
        {
            return a.A == b.A && a.B == b.B;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(VectorData a, VectorData b)
        {
            return a.A != b.A || a.B != b.B;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>

        public static bool operator <(VectorData a, VectorData b)
        {
            return a.A < b.A || a.A == b.A && a.B < b.B || a.A == b.A && a.B == b.B;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >(VectorData a, VectorData b)
        {
            return a.A > b.A || a.A == b.A && a.B > b.B || a.A == b.A && a.B == b.B;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <=(VectorData a, VectorData b)
        {
            return a.A < b.A || a.A == b.A || a.A == b.A;


        }

        /// <summary>
        /// return true if vector a is larger than or equal to vector b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >=(VectorData a, VectorData b)
        {
            return a.A > b.A || a.A == b.A && a.B > b.B || a.A == b.A && a.B == b.B;

        }

        #endregion

        public float A;
        public float B;
        
        /// <summary>
        /// Creates data structure with min and max values.
        /// </summary>
        /// <param name="A"></param> Minimum value
        /// <param name="B"></param> Maximum Value
        public VectorData( float A, float B )
        {
            this.A = A;
            this.B = B;
        }


        

        public VectorData Clear()
        {
            return new VectorData(0, 0);
        }


        public static List<VectorData> ConvertStringToVectorData(string filePath)
        {
            string[] lines = System.IO.File.ReadAllLines(filePath);
            List<VectorData> returnData = new List<VectorData>();
            foreach (var item in lines)
            {
                string[] sArray = item.Split(',');

                // store as a Vector3
                VectorData a = new VectorData(
                  float.Parse(sArray[0]),
                  float.Parse(sArray[1]));


                returnData.Add(a);
            }

            return returnData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public override string ToString()
        {
            double a = this.A;
            double b = this.B;
          

            return "{" + a.ToString() + "," + " " + b.ToString() + "}";

        }






        #region Equality and Hashcode Override

        public bool Equals(VectorData obj)
        {
            if (obj == null)
                return false;

            if (this.A == obj.A && this.B == obj.B)
                return true;
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            /////  https://msdn.microsoft.com/en-us/library/system.object.gethashcode(v=vs.110).aspx.
            if (!(obj is VectorData)) return false;

            VectorData vec = (VectorData)obj;
            return this.A == vec.A && this.B == vec.B;



        }


        public override int GetHashCode()
        {
            ///  https://msdn.microsoft.com/en-us/library/system.object.gethashcode(v=vs.110).aspx.
            return ShiftAndWrap(this.A.GetHashCode(), 2) ^ this.B.GetHashCode();
        }


        private int ShiftAndWrap(int value, int positions)
        {
            /// https://msdn.microsoft.com/en-us/library/system.object.gethashcode(v=vs.110).aspx.
            positions = positions & 0x1F;

            // Save the existing bit pattern, but interpret it as an unsigned integer.
            uint number = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
            // Preserve the bits to be discarded.
            uint wrapped = number >> (32 - positions);
            // Shift and wrap the discarded bits.
            return BitConverter.ToInt32(BitConverter.GetBytes((number << positions) | wrapped), 0);
        }


        #endregion





      




    }

}
