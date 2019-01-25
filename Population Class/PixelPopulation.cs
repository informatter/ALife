using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpMatter.SharpMath;
using System.IO;

using OrganizationalModel.Managers;
using OrganizationalModel.Export;
using OrganizationalModel.Behaviors;

namespace OrganizationalModel.Population
{
    public class PixelPopulation :MonoBehaviour
    {
        [HideInInspector]
        public Pixel[,] pixel2DArray;
        //public float[,] DensityValues2D;
        public Pixel [,] NextDensityValues2D;

        [HideInInspector]
        public Pixel[,,] pixel3DArray;
        //[HideInInspector]
        //public float[,,] DensityValues3D;
        [HideInInspector]
        public Pixel[,,] NextDensityValues3D;

        private Vector3 cntrMassPixelArray = Vector3.zero;

        public float resolution;
        public int columns, rows, height;
        public bool is3D;
        public bool is2D;

        GameObject agentPopulation; // create variable to store game object that holds script top reference
        private AgentPopulation AP; // store script to reference in variable

        // Use this for initialization
        void Start()
        {
           

                agentPopulation = GameObject.Find("AgentPopulation"); // create variable to store game object that holds script top reference
                AP = agentPopulation.GetComponent<AgentPopulation>(); // store script to reference in variable

                if (is2D)
                {
                    pixel2DArray = new Pixel[columns, rows];
                    // DensityValues2D = new float [columns, rows];
                    NextDensityValues2D = new Pixel[columns, rows];
                }

                if (is3D)
                {
                    pixel3DArray = new Pixel[columns, height, rows];
                    //DensityValues3D = new float [columns, height, rows];
                    NextDensityValues3D = new Pixel[columns, height, rows];
                }

                Init();
                Name();



                if (is3D)
                {
                    bool exportArrayPointCloud = true;
                    bool exportOrganizationPointcloud = false;
                    bool exportMobileAgentData = false;
                    bool exportClusterAgentData = false;

                    string filePath = SimulationManager.Get().PixelArrayFilePath;
                    string fileName = "PixelArray" + ".txt";



                    ExportData.ExportPointCloudToTxtFile(filePath, pixel3DArray, pixel2DArray, AgentPopulation.populationList, fileName, columns, rows, height, is3D, is2D, exportArrayPointCloud, exportOrganizationPointcloud,
                        exportMobileAgentData, exportClusterAgentData);
                }
         

        } // end start


      
        // Update is called once per frame
        void Update()
        {

         

                if (GenerationManager.generationChange)
                {
                    if (is3D)
                    {


                        bool exportArrayPointCloud = false;
                        bool exportOrganizationPointcloud = false;
                        bool exportMobileAgentData = true;
                        bool exportClusterAgentData = false;

                        string filePath = SimulationManager.Get().PixelDataMobileAgentsPath;

                        string fileName = "PixelDataModibleAgents" + GenerationManager.generationCount.ToString() + ".txt";

                        ExportData.ExportPointCloudToTxtFile(filePath, pixel3DArray, pixel2DArray, AgentPopulation.populationList, fileName, columns, rows, height, is3D, is2D, exportArrayPointCloud, exportOrganizationPointcloud,
                            exportMobileAgentData, exportClusterAgentData);



                        string filePath1 = SimulationManager.Get().PixelDataClusterAgentsPath;
                        string fileName1 = "ClusterAgents" + GenerationManager.generationCount.ToString() + ".txt";
                        string fullPath1 = System.IO.Path.Combine(filePath1, fileName1);

                        StreamWriter writer1 = new StreamWriter(fullPath1);


                        //StreamWriter writer2 = new StreamWriter(@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\3D\PixelData\ClusterAgents\" +
                        //    "ClusterAgents" + GenerationManager.generationCount.ToString() + ".txt");

                        for (int i = 0; i < columns; i++)
                        {
                            for (int j = 0; j < rows; j++)
                            {
                                for (int k = 0; k < height; k++)
                                {
                                    string outPut = pixel3DArray[i, k, j].CountClusterAgents.ToString();


                                    writer1.WriteLine(outPut);

                                }
                            }
                        }

                        writer1.Close();



                    }
                }
          

        } // end update


        public Vector3 GetCntrMassPixelArray()
        {

            foreach (Pixel pixel in pixel3DArray)
            {
                cntrMassPixelArray = cntrMassPixelArray + pixel.Position;
            }

            cntrMassPixelArray = cntrMassPixelArray / (columns * rows * height);

            return cntrMassPixelArray;
        }

        /// <summary>
        /// Make Pixelpopulation Center of mass Y value equal to the Init agent population Center of mass Y value.
        /// 
        /// returns new PixelPopulations center of mass
        /// </summary>
        /// <returns></returns>
        public Vector3 MakeCntrMassYValEqual()
        {

          Vector3 CntrMassPixelArray = GetCntrMassPixelArray();
          Vector3 newvec = Vector3.zero;
          float targetYVal = AP.GetInitPopCntrMass().y;

          if (CntrMassPixelArray.y != targetYVal)
           {

            newvec = new Vector3(CntrMassPixelArray.x, targetYVal, CntrMassPixelArray.z);
           }

           return newvec;

           


        }

      

        /// <summary>
        /// Initialize 2D vector array
        /// </summary>
        public void Init()
        {

            if (is2D)
            {

                for (int i = 0; i < columns; i++)
                {


                    for (int j = 0; j < rows; j++)
                    {


                        float x = i * resolution;
                        float z = j * resolution;
                        //pixel2DArray[i, j] = new Pixel(x, 0, z);
                        pixel2DArray[i, j] = new Pixel(x, 0, z,1);
                        NextDensityValues2D[i,j] = new Pixel(x, 0, z, 1);




                    }


                }
            }

            if (is3D)
            {


                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {

                        for (int k = 0; k < height; k++)
                        {
                            float x = i * resolution;
                            float z = j * resolution;
                            float y = k * resolution;

                            //pixel3DArray[i, k, j] = new Pixel(x, y, z);
                            pixel3DArray[i, k, j] = new Pixel(x, y, z,1);
                            NextDensityValues3D[i,j,k] = new Pixel(x, y, z, 1);


                        }

                    }

                }


            }

        }




        /// <summary>
        /// Initialize 2D vector array
        /// </summary>
     





        /// <summary>
        /// returns current pixel
        /// </summary>
        /// <param name="lookup"></param>
        /// <returns></returns>
        public Pixel Lookup2D(Vector3 lookup)
        {

            int column = (int)(SharpMath.Constrain(lookup.x / resolution, 0, columns - 1));
            int row = (int)(SharpMath.Constrain(lookup.y / resolution, 0, rows - 1));

            return pixel2DArray[column, row];
        }


        /// <summary>
        /// returns current pixel 
        /// </summary>
        /// <param name="lookup"></param>
        /// <returns></returns>
        public Pixel Lookup3D(Vector3 agentPosition)
        {
            int column = (int)(SharpMath.Constrain(agentPosition.x / resolution, 0, columns - 1));
            int row = (int)(SharpMath.Constrain(agentPosition.z / resolution, 0, rows - 1));
            int _height = (int)(SharpMath.Constrain(agentPosition.y / resolution, 0, height - 1));


            return pixel3DArray[column, _height, row];
        }








        private void Name()
        {
            if (is2D)
            {
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {

                        pixel2DArray[i, j].PixelName = "Pixel" + " " + i + "," + j;

                    }

                }
            }


            if (is3D)
            {
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        for (int k = 0; k < height; k++)
                        {

                            pixel3DArray[i, k, j].PixelName = "Pixel" + " " + i + "," + j + "," + k;

                        }


                    }

                }
            }


        }







        public void UpdateDensityValues()
        {


            if (is2D)
            {
                for (int i = 1; i < columns - 1; i++)
                {
                    for (int j = 1; j < rows - 1; j++)
                    {
                        float currentDensityVal = pixel2DArray[i, j].DensityValue;
                        NextDensityValues2D[i, j].DensityValue = Kernel3X3(i, j);
                    }
                }

                pixel2DArray = NextDensityValues2D;
            }

            if(is3D)
            {


                for (int i = 1; i < columns-1; i++)
                {
                    for (int j = 1; j < rows-1; j++)
                    {
                        for (int k = 1; k < height-1; k++)
                        {

                            float currentDensityVal = pixel3DArray[i, j,k].DensityValue;
                            NextDensityValues3D[i, j,k].DensityValue = Kernel3X3X3(i, j,k);

                        }


                    }

                }


                pixel3DArray = NextDensityValues3D;


            }

        }


        public float Kernel3X3(int x,int y)
        {
            return 0.0f;

        }


        public float Kernel3X3X3(int x, int y,int z)
        {
            float convolution = 0;

            /// MIDDLE LAYER COUNTER CLOCKWISE
            convolution += pixel3DArray[x, y, z].DensityValue * 1; // CURRENT

            convolution += pixel3DArray[x+1, y, z].DensityValue * 0.2f; // ADJACENT

            convolution += pixel3DArray[x + 1, y, z+1].DensityValue * 0.055f;// DIAGONAL

            convolution += pixel3DArray[x, y, z + 1].DensityValue * 0.2f; // ADJACENT

            convolution += pixel3DArray[x-1, y, z + 1].DensityValue * 0.055f;// DIAGONAL

            convolution += pixel3DArray[x - 1, y, z].DensityValue * 0.2f;// ADJACENT

            convolution += pixel3DArray[x - 1, y, z-1].DensityValue * 0.055f;// DIAGONAL

            convolution += pixel3DArray[x, y, z - 1].DensityValue * 0.2f;// ADJACENT

            convolution += pixel3DArray[x+1, y, z - 1].DensityValue * 0.055f;// DIAGONAL

            /// TOP LAYER COUNTER CLOCKWISE
            convolution += pixel3DArray[x, y+1, z].DensityValue * 1; // CURRENT

            convolution += pixel3DArray[x + 1, y + 1, z].DensityValue * 0.2f; // ADJACENT

            convolution += pixel3DArray[x + 1, y + 1, z + 1].DensityValue * 0.055f;// DIAGONAL

            convolution += pixel3DArray[x, y + 1, z + 1].DensityValue * 0.2f; // ADJACENT

            convolution += pixel3DArray[x - 1, y + 1, z + 1].DensityValue * 0.055f;// DIAGONAL

            convolution += pixel3DArray[x - 1, y + 1, z].DensityValue * 0.2f;// ADJACENT

            convolution += pixel3DArray[x - 1, y + 1, z - 1].DensityValue * 0.055f;// DIAGONAL

            convolution += pixel3DArray[x, y + 1, z - 1].DensityValue * 0.2f;// ADJACENT

            convolution += pixel3DArray[x + 1, y + 1, z - 1].DensityValue * 0.055f;// DIAGONAL


            /// BOTTOM LAYER COUNTER CLOCKWISE
            convolution += pixel3DArray[x, y - 1, z].DensityValue * 1; // CURRENT

            convolution += pixel3DArray[x + 1, y - 1, z].DensityValue * 0.2f; // ADJACENT

            convolution += pixel3DArray[x + 1, y - 1, z + 1].DensityValue * 0.055f;// DIAGONAL

            convolution += pixel3DArray[x, y - 1, z + 1].DensityValue * 0.2f; // ADJACENT

            convolution += pixel3DArray[x - 1, y - 1, z + 1].DensityValue * 0.055f;// DIAGONAL

            convolution += pixel3DArray[x - 1, y - 1, z].DensityValue * 0.2f;// ADJACENT

            convolution += pixel3DArray[x - 1, y - 1, z - 1].DensityValue * 0.055f;// DIAGONAL

            convolution += pixel3DArray[x, y - 1, z - 1].DensityValue * 0.2f;// ADJACENT

            convolution += pixel3DArray[x + 1, y - 1, z - 1].DensityValue * 0.055f;// DIAGONAL


            return convolution/27;

            //return convolution;

        }








    } //END CLASS

}
