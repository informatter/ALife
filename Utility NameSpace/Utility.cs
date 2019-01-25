using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System;

using SharpMatter.Geometry;
using System.IO;
using ClassExtensions;
using OrganizationalModel.BaseClass;


////////////////////////////////CLASS DESCRIPTION////////////////////////////////
/// <summary>
/// 
/// Library with a collection of useful methods, work in progress
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
//
/// 
/// 
/// 
/// 
/// 
/// 
/// </summary>

namespace OrganizationalModel.UtilityFunctions
{
    public class Utility : MonoBehaviour
    {


        public static  Agent ClosestObject ( Agent gameObjectToSearchFrom, List<Agent> collectionOfGameObjects)
        {


            List<float> distanceList = new List<float>();
            for (int i = 0; i < collectionOfGameObjects.Count; i++)
            {

              
                float distance = Vector3.Distance(gameObjectToSearchFrom.transform.position, collectionOfGameObjects[i].transform.position);
                distanceList.Add(distance);
            }

            int smallestIndex = distanceList.IndexOf(distanceList.Min());

            return collectionOfGameObjects[smallestIndex];

          
          
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObjectToSearchFrom"></param>
        /// <param name="collectionOfGameObjects"></param>
        /// <returns></returns>
        public static Agent ClosestObject(Agent gameObjectToSearchFrom, KdTree<Agent> collectionOfGameObjects)
        {
            Agent closestObject=null; 

            foreach(Agent agent in collectionOfGameObjects)
            {
                closestObject = collectionOfGameObjects.FindClosest(gameObjectToSearchFrom.transform.position);
            }


            return closestObject;



        }



        /// <summary>
        /// Get K Nearest negihbours. Testes in GH C# and working perfectly.
        /// </summary>
        /// <param name="objecttoSearchFrom"></param>
        /// <param name="objectsToSearch"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static  List<Agent> KNearestNeighbours(Vector3 objecttoSearchFrom, KdTree<Agent> objectsToSearch, int num)
        {
           
            List<Agent> closestNeighbours = new List<Agent>();
            /// returns closest point from a point collection.
            /// here it makes sure that the point does not return itself

            List<float> distanceList = new List<float>();
            List<float> distanceListSort = new List<float>();

            List<Agent> newList = new List<Agent>(); /// add all objects except itself
            for (int i = 0; i < objectsToSearch.Count(); i++)
            {


                float distance = Vector3.Distance(objecttoSearchFrom, objectsToSearch[i].transform.position);

                distanceList.Add(distance);
                distanceListSort.Add(distance);
                newList.Add(objectsToSearch[i]);
               
            }

            distanceListSort.Sort(); // sort distane list to get access to n small values
            List<float> getKSmallestDistances = distanceListSort.ListSlice(0, num - 1);
              

            for (int i = 0; i < getKSmallestDistances.Count; i++)
            {

                closestNeighbours.Add(newList[distanceList.IndexOf(getKSmallestDistances[i])]);
            }



        
            return closestNeighbours;

        }


        public static PlaceHolder ClosestObject(Agent gameObjectToSearchFrom, List<PlaceHolder> collectionOfGameObjects)
        {


            List<float> distanceList = new List<float>();
            for (int i = 0; i < collectionOfGameObjects.Count; i++)
            {


                float distance = Vector3.Distance(gameObjectToSearchFrom.transform.position, collectionOfGameObjects[i].transform.position);
                distanceList.Add(distance);
            }

            int smallestIndex = distanceList.IndexOf(distanceList.Min());

            return collectionOfGameObjects[smallestIndex];



        }



        public static PlaceHolder ClosestObject(Agent gameObjectToSearchFrom, KdTree<PlaceHolder> collectionOfGameObjects)
        {
            PlaceHolder closestObject = null;

            foreach (PlaceHolder agent in collectionOfGameObjects)
            {
                closestObject = collectionOfGameObjects.FindClosest(gameObjectToSearchFrom.transform.position);
            }


            return closestObject;



        }


        public static Pixel ClosestObject(Agent gameObjectToSearchFrom, List<Pixel> collectionOfGameObjects)
        {


            List<float> distanceList = new List<float>();
            for (int i = 0; i < collectionOfGameObjects.Count; i++)
            {


                float distance = Vector3.Distance(gameObjectToSearchFrom.transform.position, collectionOfGameObjects[i].transform.position);
                distanceList.Add(distance);
            }

            int smallestIndex = distanceList.IndexOf(distanceList.Min());

            return collectionOfGameObjects[smallestIndex];



        }





        public static  Agent ClosestObject ( PlaceHolder gameObjectToSearchFrom, List<Agent> collectionOfGameObjects)
        {


            List<float> distanceList = new List<float>();
            for (int i = 0; i < collectionOfGameObjects.Count; i++)
            {

              
                float distance = Vector3.Distance(gameObjectToSearchFrom.transform.position, collectionOfGameObjects[i].transform.position);
                distanceList.Add(distance);
            }

            int smallestIndex = distanceList.IndexOf(distanceList.Min());

            return collectionOfGameObjects[smallestIndex];

          
          
        }



        public static Agent ClosestObject(PlaceHolder gameObjectToSearchFrom, KdTree<Agent> collectionOfGameObjects)
        {
            Agent closestObject = null;

            foreach (Agent agent in collectionOfGameObjects)
            {
                closestObject = collectionOfGameObjects.FindClosest(gameObjectToSearchFrom.transform.position);
            }


            return closestObject;



        }







        public static Agent ClosestObject(Vector3 vectorToSearchFrom, List<Agent> collectionOfGameObjects)
        {


            List<float> distanceList = new List<float>();
            for (int i = 0; i < collectionOfGameObjects.Count; i++)
            {


                float distance = Vector3.Distance(vectorToSearchFrom, collectionOfGameObjects[i].transform.position);
                distanceList.Add(distance);
            }

            int smallestIndex = distanceList.IndexOf(distanceList.Min());

            return collectionOfGameObjects[smallestIndex];



        }




        public static Agent ClosestObject(Vector3 gameObjectToSearchFrom, KdTree<Agent> collectionOfGameObjects)
        {
            Agent closestObject = null;

            foreach (Agent agent in collectionOfGameObjects)
            {
                closestObject = collectionOfGameObjects.FindClosest(gameObjectToSearchFrom);
            }


            return closestObject;



        }








        public static Vector3 ClosestPoint(Vector3 gameObjectToSearchFrom, List<Vector3> collectionOfGameObjects)
        {


            List<float> distanceList = new List<float>();
            
            if (gameObjectToSearchFrom != null)
            {
                for (int i = 0; i < collectionOfGameObjects.Count; i++)
                {

                    if (collectionOfGameObjects[i] != null)
                    {

                        float distance = Vector3.Distance(gameObjectToSearchFrom, collectionOfGameObjects[i]);
                        distanceList.Add(distance);
                    }
                }
            }

             int smallestIndex = distanceList.IndexOf(distanceList.Min());
            


            return collectionOfGameObjects[smallestIndex];



        }







        public static Vec3 ClosestPoint(Vector3 gameObjectToSearchFrom, KdTreeVec3<Vec3> collectionOfGameObjects)
        {


            Vec3 closestObject = null;

            foreach (Vec3 agent in collectionOfGameObjects)
            {
                closestObject = collectionOfGameObjects.FindClosest(new Vec3 (gameObjectToSearchFrom.x, gameObjectToSearchFrom.y, gameObjectToSearchFrom.z));
            }


            return closestObject;



        }









        /// <summary>
        /// Get closestObject from Same collection. Methods makes sure that the source object doesnt
        /// return itself
        /// </summary>
        /// <param name="gameObjectToSearchFrom"></param>
        /// <param name="listToSearch"></param>
        /// <returns></returns>

        public static  Agent ClosestObjectFromSameCollection(Agent gameObjectToSearchFrom, List<Agent> listToSearch)
        {
            // returns closest point from a point collection.
            // here it makes sure that the point does not return itself

            List<float> distanceList = new List<float>();
            List<Agent> newList = new List<Agent>(); // add all objects except itself
            for (int i = 0; i < listToSearch.Count(); i++)
            {

                if (gameObjectToSearchFrom != (listToSearch[i]))
                {

                    float distance = Vector3.Distance(gameObjectToSearchFrom.transform.position, listToSearch[i].transform.position);

                    distanceList.Add(distance);
                    newList.Add(listToSearch[i]);
                }
            }
            int smallestIndex = distanceList.IndexOf(distanceList.Min());

            return newList[smallestIndex];

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        public static void CreateTextFileIndexes(string filePath, string[] data)

        {
            System.IO.File.WriteAllLines(filePath, data);

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObjectToSearchFrom"></param>
        /// <param name="collectionOfGameObjects"></param>
        /// <returns></returns>
        public static Agent FarthestObject(Agent gameObjectToSearchFrom, List<Agent> collectionOfGameObjects)
        {
            // THIS METHOD HAS BEEN TESTED IN C# GRASSHOPPER AND IT WORKS FINE

            List<float> distanceList = new List<float>();
            for (int i = 0; i < collectionOfGameObjects.Count; i++)
            {

                float distance = Vector3.Distance(gameObjectToSearchFrom.transform.position, collectionOfGameObjects[i].transform.position);
                distanceList.Add(distance);
            }

            int largestIndex = distanceList.IndexOf(distanceList.Max());

            return collectionOfGameObjects[largestIndex];
        }




        public static Agent FarthestObject(PlaceHolder gameObjectToSearchFrom, List<Agent> collectionOfGameObjects)
        {
            // THIS METHOD HAS BEEN TESTED IN C# GRASSHOPPER AND IT WORKS FINE

            List<float> distanceList = new List<float>();
            for (int i = 0; i < collectionOfGameObjects.Count; i++)
            {

                float distance = Vector3.Distance(gameObjectToSearchFrom.transform.position, collectionOfGameObjects[i].transform.position);
                distanceList.Add(distance);
            }

            int largestIndex = distanceList.IndexOf(distanceList.Max());

            return collectionOfGameObjects[largestIndex];
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointToSearchFrom"></param>
        /// <param name="pointCloudToSearch"></param>
        /// <returns></returns>
        public static Vector3 FarthestPoint(Vector3 pointToSearchFrom, List<Vector3> pointCloudToSearch)
        {
            List<float> distanceList = new List<float>();
            for (int i = 0; i < pointCloudToSearch.Count; i++)
            {

                float distance = Vector3.Distance(pointToSearchFrom , pointCloudToSearch[i]);
                distanceList.Add(distance);
            }

            int largestIndex = distanceList.IndexOf(distanceList.Max());

            return pointCloudToSearch [largestIndex];
        }




        public static Color Lerp3(Color a, Color b, Color c, float t)
        {
            if (t < 0.5f) // 0.0 to 0.5 goes to a -> b
                return Color.Lerp(a, b, t / 0.5f);
            else // 0.5 to 1.0 goes to b -> c
                return Color.Lerp(b, c, (t - 0.5f) / 0.5f);
        }





        public static Vector3 RandomSpherePoint(float x0, float y0, float z0, float radius)
        {
            float u = UnityEngine.Random.Range(0.0f, 1.0f);
            float v = UnityEngine.Random.Range(0.0f, 1.0f);
            float theta = 2 * Mathf.PI * u;
            float phi = Mathf.Acos(2 * v - 1);
            float x = x0 + (radius * Mathf.Sin(phi) * Mathf.Cos(theta));
            float y = y0 + (radius * Mathf.Sin(phi) * Mathf.Sin(theta));
            float z = z0 + (radius * Mathf.Cos(phi));
            return new Vector3(x, y, z);
        }


        public static Color RGBToFloat(float r, float g, float b)
        {

            if (r > 255)
                r = 255f;

            if (g > 255)
                g = 255f;

            if (b > 255)
                b = 255f;

            r /= 255f;
            g /= 255f;
            b /= 255f;

            Color c = new Color(r, g, b);

            return c;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vec1"></param>
        /// <param name="vec2"></param>
        /// <param name="threshHold"></param>
        /// <returns></returns>
        public static bool Similar(Vector3 vec1, Vector3 vec2, float threshHold)
        {
            
            return Mathf.Abs((vec1 - vec2).magnitude) <= threshHold;
        }





        /// <summary>
        /// Uniform distribution of points one a sphere
        /// </summary>
        /// <param name="numDirections"></param>
        /// <returns></returns> returns an array of Vector3
        /// 

        public static Vector3[] UnifromSphericalPointDistribution(int numDirections)
        {
            // var pts = new Point3d[numDirections];
            Vector3[] pts = new Vector3[numDirections];
            float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
            float off = 2f / numDirections;

            foreach (var k in Enumerable.Range(0, numDirections))
            {
                float y = k * off - 1 + (off / 2);
                float r = Mathf.Sqrt(1 - y * y);
                float phi = k * inc;
                float x = Mathf.Cos(phi) * r;
                float z = Mathf.Sin(phi) * r;
                pts[k] = new Vector3(x, y, z);

            }

            return pts;
        }



        public static Vec3[] UnifromSphericalPointDistribution_Vec3(int numDirections)
        {
            // var pts = new Point3d[numDirections];
            Vec3[] pts = new Vec3[numDirections];
            float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
            float off = 2f / numDirections;

            foreach (var k in Enumerable.Range(0, numDirections))
            {
                float y = k * off - 1 + (off / 2);
                float r = Mathf.Sqrt(1 - y * y);
                float phi = k * inc;
                float x = Mathf.Cos(phi) * r;
                float z = Mathf.Sin(phi) * r;
                pts[k] = new Vec3(x, y, z);

            }

            return pts;
        }



        /// <summary>
        /// Retrive the index of PlaceHolder
        /// Tested in C# GH and working fine
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static  int PlaceHolderIndexSeparator(string data)
        {

            string[] separators = { ".", " " };
            string value = data;
            string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string targetEmmiterAgentIndex = words[2]; /// THIS WILL ALWAYS OUTPUT A NUMBER ---> EXAMPLE: Place Holder 100.1 it will output 100
            int index = Convert.ToInt32(targetEmmiterAgentIndex);

            return index;


        }

        /// <summary>
        /// Retrieve index od f Agent
        /// Tested in C# GH and working fine
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AgentIndexSeparator(string data)
        {

            string[] separators = { " " };
            string value = data;
            string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string targetEmmiterAgentIndex = words[1]; /// THIS WILL ALWAYS OUTPUT A NUMBER ---> EXAMPLE: Agent 100 it will output 100
            int index = Convert.ToInt32(targetEmmiterAgentIndex);

            return index;


        }



    }
        

}
