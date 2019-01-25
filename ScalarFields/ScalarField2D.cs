using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;


using SharpMatter.SharpMath;
using OrganizationalModel.Managers;
using OrganizationalModel.Export;
using OrganizationalModel.Behaviors;
using OrganizationalModel.Population;
using OrganizationalModel.UtilityFunctions;

namespace OrganizationalModel.ScalarFields
{

    public class ScalarField2D : MonoBehaviour
    {
        public Color colorPink;
        public bool showEmotionField;
        public bool showPheromoneField;
        public bool showDataMap;
       
        public GameObject voxel;
        private GameObject[,] voxelPrefab;
        private Cell[,] scalarField;
        private float[,] scalarFieldValuesRules;
        public static float[,] scalarFieldValuesProximity;
        public static float[,] scalarFieldHumanData;
        public float[,] pheromoneField;

        private float[,,] scalarField3DValues;

        public float resolution;
        public int columns, rows, height;
        private bool createFieldValues = false;

        private Texture2D texture;

        [Range(0.03f,.2f)]
        public float humanInfluence = 0.05f;

        public Gradient myGradient;
        // Use this for initialization
        void Start()
        {
            if (SimulationManager.Get().ScalarField2d)
            {
                voxelPrefab = new GameObject[ columns, rows];
                scalarField = new Cell[columns, rows];
                scalarFieldValuesProximity = new float[columns, rows];
                scalarFieldValuesRules = new float[columns, rows];
                scalarFieldHumanData = new float[columns, rows];
                pheromoneField = new float[columns, rows];
                Init();
                SetName();
                if(showDataMap==false)   this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                if (showDataMap) InitTexture();

                string filePath1 = SimulationManager.Get().ScalarFieldValuesFilePath;
                string fileName1 = "FieldPointCloud" + ".txt";
                string fullPath1 = System.IO.Path.Combine(filePath1, fileName1);

                StreamWriter writer = new StreamWriter(fullPath1);

                //StreamWriter writer = new StreamWriter(@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\2D\ScalarField\" +
                //            "FieldPointCloud" + ".txt");

                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {

                        string outPut = scalarField[i, j].X.ToString() + "," + scalarField[i, j].Z.ToString() + "," + scalarField[i, j].Y.ToString();
                        writer.WriteLine(outPut);


                    }
                }

                writer.Close();

                string filePath2 = SimulationManager.Get().ScalarFieldValuesFilePath;
                string fileName2 = "CellName" + ".txt";
                string fullPath2 = System.IO.Path.Combine(filePath2, fileName2);

                StreamWriter writer2 = new StreamWriter(fullPath2);

                //StreamWriter writer1 = new StreamWriter(@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\2D\ScalarField\" +
                //          "CellName" + ".txt");

                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {

                        string outPut = scalarField[i, j].CellName.ToString();
                        writer2.WriteLine(outPut);


                    }
                }

                writer2.Close();
            }






        }

        // Update is called once per frame
        void Update()
        {
            if (SimulationManager.Get().ScalarField2d)
            {

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                if (SimulationManager.Get().updateFieldRealTime)
                {
                    if (PeoplePopulation.peopleList.Count != 0) //&& createFieldValues == false) // People dont update
                    {


                        if (SimulationManager.Get().radialScalarField)
                        {
                            CreateDistanceField(PeoplePopulation.peopleList);
                            CreateEmotionField(PeoplePopulation.peopleList);

                            //CreatePheromoneField();
                            //PheromoneDecayFactor();
                        }

                        if (showEmotionField)
                        {
                            UpdateTextureEmotion();
                        }

                        if (showPheromoneField)
                        {
                            CreatePheromoneField();
                           // PheromoneDecayFactor();
                            UpdateTexturePheromone();
                        }








                        if (SimulationManager.Get().interpolatedScalarField) CreateColorValueField2D();

                        string filePath = SimulationManager.Get().ScalarFieldValuesFilePath;
                        string fileName = "ScalarFieldValuesProximity" + ".txt";
                        string fullPath = System.IO.Path.Combine(filePath, fileName);

                        StreamWriter writer = new StreamWriter(fullPath);

                        for (int i = 0; i < columns; i++)
                        {
                            for (int j = 0; j < rows; j++)
                            {



                                scalarFieldValuesProximity[i, j] = scalarField[i, j].ScalarValueProximity;
                                string outPut = scalarFieldValuesProximity[i, j].ToString();

                                writer.WriteLine(outPut);


                            }
                        }

                        writer.Close();



                        //string filePath0 = SimulationManager.Get().ScalarFieldValuesFilePath;
                        //string fileName0 = "ScalarFieldValuesHumanData" + ".txt";
                        //string fullPath0 = System.IO.Path.Combine(filePath0, fileName0);

                        //StreamWriter writer0 = new StreamWriter(fullPath);


                        //for (int i = 0; i < columns; i++)
                        //{
                        //    for (int j = 0; j < rows; j++)
                        //    {



                        //        scalarFieldValuesProximity[i, j] = scalarField[i, j].ScalarValueHumanData;
                        //        string outPut = scalarFieldHumanData[i, j].ToString();

                        //        writer0.WriteLine(outPut);


                        //    }
                        //}

                        //writer0.Close();






                        string filePath1 = SimulationManager.Get().ScalarFieldValuesFilePath;
                        string fileName1 = "ScalarFieldValuesRules" + ".txt";
                        string fullPath1 = System.IO.Path.Combine(filePath1, fileName1);

                        StreamWriter writer1 = new StreamWriter(fullPath1);



                        for (int i = 0; i < columns; i++)
                        {
                            for (int j = 0; j < rows; j++)
                            {



                                scalarFieldValuesRules[i, j] = scalarField[i, j].ScalarValueRules;
                                string outPut = scalarFieldValuesRules[i, j].ToString();

                                writer1.WriteLine(outPut);


                            }
                        }

                        writer1.Close();





                    }

                }// END CONDITIONS UPDATE FIELD IN REAL TIME

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                if (PeoplePopulation.peopleList.Count != 0 && createFieldValues == false && SimulationManager.Get().updateFieldRealTime==false) // People dont update
                {


                    if (SimulationManager.Get().radialScalarField)
                    {
                        CreateDistanceField(PeoplePopulation.peopleList);
                        CreateEmotionField(PeoplePopulation.peopleList);
                    }

                    if (SimulationManager.Get().interpolatedScalarField) CreateColorValueField2D();

                    string filePath = SimulationManager.Get().ScalarFieldValuesFilePath;
                    string fileName = "ScalarFieldValuesProximity" + ".txt";
                    string fullPath = System.IO.Path.Combine(filePath, fileName);

                    StreamWriter writer = new StreamWriter(fullPath);

              

                    for (int i = 0; i < columns; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {

                         

                            scalarFieldValuesProximity[i, j] = scalarField[i, j].ScalarValueProximity;
                            string outPut = scalarFieldValuesProximity[i, j].ToString();

                            writer.WriteLine(outPut);


                        }
                    }

                    writer.Close();






                    string filePath1 = SimulationManager.Get().ScalarFieldValuesFilePath;
                    string fileName1 = "ScalarFieldValuesRules" + ".txt";
                    string fullPath1 = System.IO.Path.Combine(filePath1, fileName1);

                    StreamWriter writer1 = new StreamWriter(fullPath1);

                    //StreamWriter writer = new StreamWriter(@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\2D\ScalarField\" +
                    //      "ScalarFieldValues" + ".txt");

                    for (int i = 0; i < columns; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {

                  

                            scalarFieldValuesRules[i, j] = scalarField[i, j].ScalarValueRules;
                            string outPut = scalarFieldValuesRules[i, j].ToString();

                            writer1.WriteLine(outPut);


                        }
                    }

                    writer1.Close();



                    //string filePathRed = SimulationManager.Get().ScalarFieldHumanEmotionFilePath;
                    //string fileNameRed = "ScalarFieldValuesHumanData" + ".txt";
                    //string fullPathRed = System.IO.Path.Combine(filePathRed, fileNameRed);

                    //StreamWriter writerRed = new StreamWriter(fullPath);


                    //for (int i = 0; i < columns; i++)
                    //{
                    //    for (int j = 0; j < rows; j++)
                    //    {



                    //        scalarFieldHumanData[i, j] = scalarField[i, j].ScalarValueHumanData;
                    //        string outPut = scalarFieldHumanData[i, j].ToString();

                    //        writerRed.WriteLine(outPut);


                    //    }
                    //}

                    //writerRed.Close();



                    createFieldValues = true;


                } // END CONDITIONS OF FIELD VALUE SNOT UPDATING IN REAL TIME
            }

          




        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="humans"></param>
        public void CreateDistanceField(List<GameObject> humans)
        {
            float maxSeparationDistance = 1.8f;
            for (int i = 0; i < columns; i++)
            {

                for (int j = 0; j < rows; j++)
                {

                    float val = GetDistanceValues(scalarField[i, j], humans) * humanInfluence;
                    if (val < 0.1) val = 0; // 0.30 --> original value used// When remap is used keep this commented out

                    //float valRemap = SharpMath.Remap(val, 0.0f, 1.0f, maxSeparationDistance, 0.0f);

                    //if (valRemap < 0) valRemap = 0;

                    scalarField[i, j].ScalarValueProximity = val;

                    //scalarField[i, j].ScalarValue = valRemap;
                }

            }

        }

        public void CreatePheromoneField()
        {
            for (int i = 0; i < columns; i++)
            {

                for (int j = 0; j < rows; j++)
                {

                    pheromoneField[i, j] = scalarField[i, j].Pheromone;
;
                }

            }
        }

        public void PheromoneDecayFactor()
        {
            for (int i = 0; i < columns; i++)
            {

                for (int j = 0; j < rows; j++)
                {

                    pheromoneField[i, j] -= 0.5f;
                    ;
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="humans"></param>
        public void CreateEmotionField(List<GameObject> humans)
        {
            float maxSeparationDistance = 1.8f;

            float scale=0.05f;

          

            for (int i = 0; i < columns; i++)
            {

                for (int j = 0; j < rows; j++)
                {


             
                    float val = GetDistanceValues(scalarField[i, j], humans) * scale;// smaller values smooth out the field. good values are between 0.02 and 0.05
                    if (val < 0.1) val = 0; // 0.30 --> original value used// When remap is used keep this commented out

                    // float valRemap = SharpMath.Remap(val, 0.0f, 1.0f, maxSeparationDistance, 0.0f); // rempa is used here to inverse the distance relationship. Closet the ppl larger the value for the pixel

                    //  if (valRemap < 0) valRemap = 0;

                    scalarField[i, j].ScalarValueHumanData = val;

                    //  scalarField[i, j].ScalarValueProximity = valRemap;
                }

            }

        }





        private void CreateColorValueField2D()
        {
            float[,] data = GetMeshColorValuesFromRhino();


            for (int i = 0; i < columns; i++)
            {

                for (int j = 0; j < rows; j++)
                {


                    scalarField[i, j].ScalarValueRules = data[i, j];


                }

            }
        }








        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="humans"></param>
        /// <returns></returns>
        private float GetDistanceValues(Cell cell, List<GameObject> humans)
        {
            List<float> distanceList = new List<float>();

            for (int i = 0; i < humans.Count; i++)
            {

                float distance = Vector3.Distance(cell.Position, humans[i].gameObject.transform.position);
                distanceList.Add(distance);
            }


            int smallestIndex = distanceList.IndexOf(distanceList.Min());

            return distanceList[smallestIndex];

        }

        /// <summary>
        /// Read exported values from meshField generated in rhino and store in a 2D array
        /// </summary>
        /// <returns></returns>
        private float[,] GetMeshColorValuesFromRhino()
        {

            float[,] dataArray = new float[columns, rows];
            // string[] lines = System.IO.File.ReadAllLines(@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\2D\ScalarField\ToUnity\ColorMapValues.txt");
            string[] lines = System.IO.File.ReadAllLines(SimulationManager.Get().ImportToUnityInterpolatedFieldFilePath);
            var linesArray = Make2DArray(lines, columns, rows);

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    dataArray[i, j] = float.Parse(linesArray[i, j]);

                }
            }
            return dataArray;
        }


        private string[,] Make2DArray(string[] input, int height, int width)
        {
            string[,] output = new string[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    output[i, j] = input[i * width + j];
                }
            }
            return output;
        }


        public void Init()
        {

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {


                    float x = i * resolution;
                    float z = j * resolution;
                    scalarField[i, j] = new Cell(x, 0, z);

                    //  scalarField[i, j] = new Cell(i, 0, j);

                    //if (showDataMap)
                    //{
                    //    voxelPrefab[i,j] =   Instantiate(voxel, new Vector3(x, 0, z), Quaternion.identity);
                    //}



                }
            }

        }


   
        

        /// <summary>
        /// returns current Cell
        /// </summary>
        /// <param name="lookup"></param>
        /// <returns></returns>
        public Cell Lookup2D(Vector3 lookup)
        {

            int column = (int)(SharpMath.Constrain(lookup.x / resolution, 0, columns - 1));
            int row = (int)(SharpMath.Constrain(lookup.z / resolution, 0, rows - 1));

            return scalarField[column, row];
        }

   

        private void SetName()
        {

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {

                    scalarField[i, j].CellName = "Cell" + " " + i + "," + j;

                }

            }
        }


        public void InitTexture()
        {
            texture = new Texture2D(columns, rows);

            Renderer rendered =  gameObject.GetComponent<Renderer>();

            rendered.material.mainTexture = texture;

          
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                   
                    texture.SetPixel(i, j, Color.white);
                }
            }

            texture.Apply();

        }


        public void UpdateTextureEmotion()
        {
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    texture.SetPixel(i, j, Utility.Lerp3(Color.red, Color.yellow, Color.blue, scalarFieldValuesProximity[i, j]));

                  //  texture.SetPixel(i, j, Utility.Lerp3(Color.white, colorPink, Color.black, scalarFieldValuesProximity[i, j]));


                    // texture.SetPixel(i, j, myGradient.Evaluate( scalarFieldHumanData[i, j]));

                    //myGradient.Evaluate(t);


                }
            }

            texture.Apply();
        }

        public void UpdateTexturePheromone()
        {
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {

                  texture.SetPixel(i, j, Color.Lerp(Color.white, Color.black, pheromoneField[i, j]));

                   // texture.SetPixel(i, j, Utility.Lerp3(Color.red, Color.yellow, Color.blue, pheromoneField[i, j]));


                }
            }

            texture.Apply();
        }




    }

}
