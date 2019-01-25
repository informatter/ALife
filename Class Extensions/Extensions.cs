using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace ClassExtensions
{

    public static class ArrayExtensions
    {

        public static T[] RemoveFromArray<T>(this T[] original, T itemToRemove)
        {
            int numIdx = System.Array.IndexOf(original, itemToRemove);
            if (numIdx == -1) return original;
            List<T> tmp = new List<T>(original);
            tmp.RemoveAt(numIdx);
            return tmp.ToArray();
        }
    }
   

    public static class ListExtensions
    {

        ////////////////////////////////CLASS DESCRIPTION////////////////////////////////
        /// <summary>
        /// 
        /// Collection of methods for extending the .NET Framework List<T> Class
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// </summary>


        ////////////////////////////////CONTROL LOG / NOTES ////////////////////////////////
        /// <summary>
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// </summary>




      

        /// <summary>
        /// Count number of duplicate values in a List this method is intended to be used with the 
        /// FindDuplicateItems of the ListExtensions class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>Original list
        /// <param name="listOfDupItems"></param> List of duplicate values
        /// <returns></returns> the total number of times an item is repeated
        /// 
        public static int CountDuplicateValues<T>(this List<T> list, List<T> listOfDupItems)
        {
            int count = 0;
            if (listOfDupItems.Count == 0)
            {
                return count;
            }

            if (listOfDupItems.Count > 0)
            {
                count = list.Where(x => x.Equals(listOfDupItems[0])).Count();
            }

            return count;
        }


        /// <summary>
        /// Find duplicate items in a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns> returns a collection of duplicate items
        public static List<T> FindDuplicateItems<T>(this List<T> list)
        {
            var dups = list.GroupBy(x => x)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();


            return dups;
        }



        /// <summary>
        /// Get last Nth elements from a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="li"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static List<T> GetNLastElements<T>(this List<T> li, int number)
        {
            var LstItems = li.Skip(Math.Max(0, li.Count() - number)).Take(number).ToList();
            return li.Skip(Math.Max(0, li.Count() - number)).Take(number).ToList();
        }


        /// <summary>
        /// returns number of times  an item appears in a list 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param> item to search for
        /// <returns> returns number of times  an item appears in a list </returns>
        public static int NumberOfOccurences<T>(this List<T> list, T item)
        {
            int count = list.Where(a => a.Equals(item)).Count();
            return count;
        }



        /// <summary>
        /// This method is exactly the same as the cull by indexes component in grasshopper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param> Input list to process
        /// <param name="indices"></param> Input List containing Indices that are the placeholders
        /// for the data to remove in the input list to process
        public static void RemoveAllIndices<T>(this List<T> list, IEnumerable<int> indices)
        {
            //do not remove Distinct() call here, it's important
            var indicesOrdered = indices.Distinct().ToArray();
            if (indicesOrdered.Length == 0)
                return;

            Array.Sort(indicesOrdered);


            if (indicesOrdered[0] < 0 || indicesOrdered[indicesOrdered.Length - 1] >= list.Count)
                throw new ArgumentOutOfRangeException();

            int indexToRemove = 0;
            int newIdx = 0;

            for (int originalIdx = 0; originalIdx < list.Count; originalIdx++)
            {
                if (indexToRemove < indicesOrdered.Length && indicesOrdered[indexToRemove] == originalIdx)
                {
                    indexToRemove++;
                }
                else
                {
                    list[newIdx++] = list[originalIdx];
                }
            }

            list.RemoveRange(newIdx, list.Count - newIdx);
        }

        /// <summary>
        /// Function will reorder elements in list randomly
        /// </summary>
        /// <typeparam name="T"></typeparam> generic type
        /// <param name="list"></param> input list

        public static void JitterList<T>(this List<T> list)
        {

            list.Sort((x, y) => UnityEngine.Random.Range(-1, 1));

        }

        /// <summary>
        /// Return a collection of elements from the specified
        /// indexes
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="li"></param> input list
        /// <param name="start"></param> starting  index
        /// <param name="end"></param> End of  index 
        /// <returns></returns> list with specefied index range
        /// starting index and ending index are inclusive

        public static List<T> ListSlice<T>(this List<T> li, int start, int end)
        {
            end += 1;
            if (start < 0)    // support negative indexing
            {
                start = li.Count + start;
            }
            if (end < 0)    // support negative indexing
            {
                end = li.Count + end;
            }
            if (start > li.Count)    // if the start value is too high
            {
                start = li.Count;
            }
            if (end > li.Count)    // if the end value is too high
            {
                end = li.Count;
            }
            var count = end - start;             // calculate count (number of elements)
            return li.GetRange(start, count);    // return a shallow copy of li of count elements
        }


        /// <summary>
        /// Pick n random elements from imput list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param> Input List
        /// <param name="number"></param> Number of items to select
        /// <returns></returns> Random elements from input list

        public static List<T> PickRandomElements<T>(this List<T> list, int number)
        {
            List<T> listToReturn = new List<T>();
            for (int i = 0; i < number; i++)
            {
                int index = UnityEngine.Random.Range(0, list.Count);
                listToReturn.Add(list[index]);
            }

            return listToReturn;
        }

        /// <summary>
        /// Pick a random element from an imput list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param> Input List
        /// <returns></returns> Returns a random element from input list

        public static T PickRandomElement<T>(this List<T> list)
        {
            int index = UnityEngine.Random.Range(0, list.Count);
            var data = list[index];
            return data;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns> returns the most common element in the list
        public static T MostCommon<T>(this IEnumerable<T> list)
        {
            var most = list.GroupBy(i => i).OrderByDescending(grp => grp.Count())
          .Select(grp => grp.Key).First();
            return most;
        }



    }// END CLASS


  


    public static class Vector3Extensions
    {


   


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Vector3 Limit(this Vector3 vec, float max)
        {
            if (vec.sqrMagnitude > max * max)
            {
                vec.Normalize();
                //Vector3 a = vec.Multiply(max);
                return vec *= max;

            }
            else { return vec; }



        }


        /// <summary>
        /// Multiplies every component of the vector by a scalar value
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="val"></param>
        /// <returns></returns> returns a Vector3

        public static Vector3 Multiply(this Vector3 vec, float val)
        {
            vec.x *= val;
            vec.y *= val;
            vec.z *= val;

            return new Vector3(vec.x, vec.y, vec.z);

        }






    }/// END CLASS


   public static class ColorExtensions
    {
        public static Color Lerp3 (this Color a, Color b, Color c, float t)
        {
            if (t < 0.5f) // 0.0 to 0.5 goes to a -> b
                return Color.Lerp(a, b, t / 0.5f);
            else // 0.5 to 1.0 goes to b -> c
                return Color.Lerp(b, c, (t - 0.5f) / 0.5f);
        }
    }
    public static class MaterialExtenions
    {

        public static void ChangeAlpha(this Material mat, float alphaValue)
        {
            Color oldColor = mat.color;
            Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaValue);
            mat.SetColor("_Color", newColor);
        }
    }


    public static class GameObjectExtensions
    {

        public static T GetComponent2<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponent(typeof(T)) as T;
        }
    }




}
