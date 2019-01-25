using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using OrganizationalModel.Population;
using OrganizationalModel.BaseClass;
using OrganizationalModel.Managers;

public class PeoplePopulation : MonoBehaviour {

    public GameObject Person;
    public GameObject populationHolder;
    public int number;
    //public float distributionRangeMin;
    //public float distributionRangeMax;

    private bool createPopulation = false;

    public static List<GameObject> peopleList = new List<GameObject>();

    public static float minVal ;
    public static float maxVal ;
    // Use this for initialization
    void Start () {
        
       
    }
	
	// Update is called once per frame
	void Update () {



        if (AgentPopulation.populationList.Count != 0 && createPopulation == false)
        {
            GetMinAndMax2DCoordinates(out minVal, out maxVal);
            InitializePeople2DRandom();
            //print(minVal);
            //print(maxVal);
            Parent();
            Material();

            string filePath = SimulationManager.Get().PeopleFilePath;
            string fileName = "PeopleDistribution" + ".txt";
            string fullPath = System.IO.Path.Combine(filePath, fileName);

            StreamWriter writer = new StreamWriter(fullPath);


           // StreamWriter writer = new StreamWriter(@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\2D\People\PeopleDistribution.txt");
            for (int i = 0; i < peopleList.Count; i++)
            {

                string outPut = peopleList[i].gameObject.transform.position.x.ToString() + "," + peopleList[i].gameObject.transform.position.z.ToString() + "," + peopleList[i].gameObject.transform.position.y.ToString();
                writer.WriteLine(outPut);
            }

            writer.Close();

            createPopulation = true;
        }





     

    }


    private void InitializePeople2DRandom()
    {
        if (number == 1)
         {
            for (int i = 0; i < number; i++)
            {


                float middle = maxVal / 2;
                Vector3 distributionArea = new Vector3(Random.Range(middle, middle), 0, Random.Range(middle, middle));
                peopleList.Add((Instantiate(Person, distributionArea, Quaternion.identity)));

            }
        }

        if(number>1)
        {
            for (int i = 0; i < number; i++)
            {


            
                Vector3 distributionArea = new Vector3(Random.Range(minVal+5, maxVal-5), 0, Random.Range(minVal+5, maxVal-5));
                peopleList.Add((Instantiate(Person, distributionArea, Quaternion.identity)));

            }
        }

    }


    private void Material()
    {
        for (int i = 0; i < peopleList.Count; i++)
        {
            peopleList[i].GetComponent<MeshRenderer>().material.color = Color.clear;
            

        }
    }


    private void Parent()
    {
        for (int i = 0; i < peopleList.Count; i++)
        {
            peopleList[i].transform.parent = populationHolder.transform;
            peopleList[i].tag = "Person";

        }
    }

   /// <summary>
   /// Retrive Min and Max 2D values from a 3D Vector3 Array
   /// </summary>
   /// <param name="data"></param>
   /// <param name="minVal"></param>
   /// <param name="maxVal"></param>
    public void GetMinAndMax2DCoordinates( out float minVal, out float maxVal )
    {
       
            List<float> coordinatesX = new List<float>();
            List<float> coordinatesZ = new List<float>();

            for (int i = 0; i < AgentPopulation.populationList.Count; i++)
            {
                coordinatesX.Add(AgentPopulation.populationList[i].transform.position.x);
                //coordinatesY.Add(data[i].transform.position.z);
            }

            coordinatesX.Sort();
           // coordinatesZ.Sort();

            minVal = coordinatesX[0];
            maxVal = coordinatesX[coordinatesX.Count - 1];

            //minY = coordinatesY[0];
            //maxY = coordinatesY[coordinatesY.Count - 1];
        




    }

}
