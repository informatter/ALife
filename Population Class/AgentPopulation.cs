using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Text;
using System;

using OrganizationalModel.UtilityFunctions;
using OrganizationalModel.BaseClass;
using OrganizationalModel.Managers;
using OrganizationalModel.Export;
using OrganizationalModel.ScalarFields;
////////////////////////////////CLASS DESCRIPTION////////////////////////////////
/// <summary>
/// 
/// This class handles the population of agents, and their initial states, color, and name
/// each state is deffined as a tag, and their name corresponds to their index order. Agent 0, Agent 1 ... etc
/// Also a random agent of the population is chosen to be a Signal Emmiter and be able to activate another Agent
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
/// /
/// 
/// In ActivateRandomAgents function I changed the index to 14. To always activate agent 14, to edit go to method
/// 
/// 
/// 
/// 
/// 
/// </summary>

namespace OrganizationalModel.Population
{
    public class AgentPopulation : MonoBehaviour
    {


        Dictionary<int, List<Agent>> dictionary = new Dictionary<int, List<Agent>>();

        ////////////////////////////////DECLARE GLOBAL VARIABLES////////////////////////////////

        // REFERENCE PREFABS / EMPTY GAMEOBJECTS
        public GameObject agentPrefab;
        public GameObject AgentPopulationHolder;
        public Material transparent;

        //CLASS INSTANCES



        //PUBLIC LISTS

        // PUBLIC STATIC LISTS
        public static KdTree<Agent> populationList = new KdTree<Agent>();

      



        public static int totalCancelledAgents = 0;
        
        public static bool migrateCluster = false;

        public static KdTree<Agent> deActivatedAgentList;

        public static KdTree<Agent> deActivatedPassiveAgentList;

        public static KdTree<Agent> signalReceiverAgentList;

        public static KdTree<Agent> signalReceiverPassiveAgentList;

        public static KdTree<Agent> freezedAgentList;

        public static KdTree<Agent> emmiterAgentList;

        public static KdTree<Agent> cancelledAgentList;

        public static KdTree<Agent> wondererAgentList;



        public static List<float> range = new List<float>();


        public static List<int> indexActivatedAgentsList = new List<int>();


        //PRIVATE LISTS

        // list that holds  the indexes of the population
        private List<int> populationListIndexes = new List<int>();

        // PUBLIC VARIABLES
    

  

        public float arraySpacing;
        public int populationCount;
        public static int timeToInitFirstAgent = 0;

   

        //PRIVATE VARIABLES
        public static bool runCoroutine = false;
        private bool convertToAgentCoroutine = false;
        private bool getPixelArrayCntrMass = false;
        public static int time;

        // NON STATIC PUBLIC VARIABLES


        // Use this for initialization

        Agent agent = new Agent();

        float startTime;

        public static bool IntiPopulation = false;

        public int staticAgentsInCluster = 0;

       

       GameObject generationManager;
        private GenerationManager GM;

        GameObject pixelPopualtion;
        private PixelPopulation pixelPop;


        private bool exportBodyPlanBeforeWindForce = false;

        Vector3 centrMassPplPop = Vector3.zero; 

        Vector3 centrMassInitPop = Vector3.zero;


        private bool exportDataTemp = false;

      

        void Start()
        {
            generationManager = GameObject.Find("GenerationManager");
            GM = generationManager.GetComponent<GenerationManager>();
            pixelPopualtion = GameObject.Find("PixelPopulation");
            pixelPop = pixelPopualtion.GetComponent<PixelPopulation>();


            cancelledAgentList = new KdTree<Agent>();
                startTime = Time.time;


                if (SimulationManager.Get().is2D)
                {
                    InitializeAgents2DArray(populationCount);

                    IntiPopulation = true;

                }

                if (SimulationManager.Get().is3D)
                {
                    InitializeAgents3DArray(populationCount);

                    range = SharpMatter.SharpMath.SharpMath.Range(populationList.Count);

                    IntiPopulation = true;
                }



                Parent();
                AgentName();
                InitializeState();

                string filePath = SimulationManager.Get().ClusterOrganizationFilePath;
                string fileName = "OrganizationInitialState" + ".txt";

                StreamWriter writer = new StreamWriter(filePath + fileName);


                // StreamWriter writer = new StreamWriter(@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\3D\Cluster\Init state\OrganizationInitialState.txt");
                for (int i = 0; i < populationList.Count; i++)
                {

                    string outPut = populationList[i].gameObject.transform.position.x.ToString() + "," + populationList[i].gameObject.transform.position.z.ToString() + "," + populationList[i].gameObject.transform.position.y.ToString();
                    writer.WriteLine(outPut);
                }

                writer.Close();


                

           

        } // END START

        // Update is called once per frame


        void Update()
        {
           
                if (getPixelArrayCntrMass == false)
                {
                    //print(pixelPop.GetCntrMassPixelArray());

                    pixelPop.GetCntrMassPixelArray();


                    getPixelArrayCntrMass = true;
                }




                //Only run method once

                time += (int)Time.time;

                //time == timeToInitFirstAgent &&
                if (time == timeToInitFirstAgent && runCoroutine == false && PeoplePopulation.peopleList.Count != 0)
                {

                   


                    //MovePopTowardsPixelArrayCntrMass();



                    // ActivateAgentsBasedOnInitClusterPositionFromRhino(1);

                    // ActivateAgentBasedOnCntOfPplMassB();
                    ActivateMiddleAgentOfBottomLayer();
               // PickRandomAgentstoActivateFromBottomLayer(2);

                    runCoroutine = true;
                }





               // cancelledAgentList = FindObjectsWithTag("Cancelled");
                deActivatedAgentList = FindObjectsWithTag("De-Activated");
                signalReceiverAgentList = FindObjectsWithTag("SignalReceiver");
                signalReceiverPassiveAgentList = FindObjectsWithTag("SignalReceiverPassive");
                freezedAgentList = FindObjectsWithTag("Freezed");
           
                //if (GenerationManager.generationCount != 0)
                //{
                //    //deActivatedPassiveAgentList = FindObjectsWithTag("De-ActivatedPassive");
                //    wondererAgentList = FindObjectsWithTag("Wanderer");
                //}




                #region EXPORT DATA




                if (GM.windEffectCounter > GM.WindEffectMaxDuration)
                {

                    /// DISPLACEMENT
                    string filePath = SimulationManager.Get().WindAnalysisFilePath;//@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\3D\Cluster\WindAnalysis\";
                    string fileName = "TotalWindDisplacement" + GenerationManager.generationCount.ToString() + ".txt";

                    StreamWriter writer = new StreamWriter(filePath + fileName);

                    for (int i = 0; i < populationList.Count; i++)
                    {
                        if (populationList[i].gameObject.tag != "Cancelled")
                        {
                            string OutPut = populationList[i].GetComponent<OrganizationalModel.Behaviors.OrganizationBehavior>().totalDisplacement.ToString();
                            writer.WriteLine(OutPut);
                        }
                    }

                    writer.Close();






                }

                if (GenerationManager.playWindEffect && exportBodyPlanBeforeWindForce == false)
                {


                    /// DISPLACEMENT
                    string filePath = SimulationManager.Get().ClusterOrganizationFilePath; //@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\3D\Cluster\ClusterOrganization\";
                    string fileName = "ClusterOrganization" + "BeforeWindForce" + ".txt";

                    StreamWriter writer = new StreamWriter(filePath + fileName);

                    for (int i = 0; i < populationList.Count; i++)
                    {
                        if (populationList[i].gameObject.tag != "Cancelled")
                        {
                            string OutPut = populationList[i].GetComponent<OrganizationalModel.Behaviors.OrganizationBehavior>().totalDisplacement.ToString();
                            writer.WriteLine(OutPut);
                        }
                    }

                    writer.Close();

                }


                if (GenerationManager.playWindEffect)
                {
                    bool exportOrganizationPointcloud = true;
                    string filePath1 = SimulationManager.Get().ClusterOrganizationFilePath;//@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\3D\Cluster\ClusterOrganization\";
                    string fileName1 = "ClusterOrganization" + "AfterWindForce" + ".txt";

                    ExportData.ExportPointCloudToTxtFile(filePath1, fileName1, AgentPopulation.populationList, SimulationManager.Get().is3D, SimulationManager.Get().is2D, exportOrganizationPointcloud);

                }


                if (AgentPopulation.freezedAgentList.Count == AgentPopulation.populationList.Count - totalCancelledAgents && exportDataTemp == false)// ONLY USED WHEN ONLY 1 ORGANIZATION IS WANTED. --> ONLY ONE ITERATION

                // if (GenerationManager.generationChange)
                {

                    /// DISPLACEMENT
                    string filePathZ = SimulationManager.Get().WindAnalysisFilePath;//@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\3D\Cluster\WindAnalysis\";
                    string fileNameZ = "TotalWindDisplacement" + GenerationManager.generationCount.ToString() + ".txt";

                    StreamWriter writerZ = new StreamWriter(filePathZ + fileNameZ);

                    for (int i = 0; i < populationList.Count; i++)
                    {
                        if (populationList[i].gameObject.tag != "Cancelled")
                        {
                            string OutPut = populationList[i].GetComponent<OrganizationalModel.Behaviors.OrganizationBehavior>().totalDisplacement.ToString();
                            writerZ.WriteLine(OutPut);
                        }
                    }

                    writerZ.Close();



                    bool exportOrganizationPointcloud = true;
                    string filePath = SimulationManager.Get().ClusterOrganizationFilePath;
                    string fileName = "ClusterOrganization" + GenerationManager.generationCount.ToString() + ".txt";

                    ExportData.ExportPointCloudToTxtFile(filePath, fileName, AgentPopulation.populationList, SimulationManager.Get().is3D, SimulationManager.Get().is2D, exportOrganizationPointcloud);

                    string filePathMultiple = SimulationManager.Get().individualOrganizationFilePath;
                    ExportData.ExportPointCloudToMultipleTxtFiled(filePathMultiple, populationList, SimulationManager.Get().is3D, SimulationManager.Get().is2D, exportOrganizationPointcloud);

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///DICTIONARY
                    //for (int i = 0; i < populationList.Count; i++)
                    //{
                    //    if (populationList[i].gameObject.tag != "Cancelled")
                    //    {
                    //        dictionary.Add(i, populationList[i].GetComponent<OrganizationalModel.Behaviors.OrganizationBehavior>().neighbours);
                    //    }
                    //}

                    //using (var writer = new StreamWriter(@"C:\Users\nicol\OneDrive\Desktop\New folder\test.txt"))
                    //{
                    //    foreach (var pair in dictionary)
                    //    {
                    //        writer.WriteLine("{0},{1};", pair.Key, String.Join(",", pair.Value.Select(x => x.ToString()).ToArray()));
                    //    }
                    //}
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    ///// ENERGY DATA
                    string filePath2 = SimulationManager.Get().EnergyLevelsFilePath;//@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\3D\Cluster\EnergyLevels\";
                    string fileName2 = "EnergyLevels" + GenerationManager.generationCount.ToString() + ".txt";
                    string fullPath2 = System.IO.Path.Combine(filePath2, fileName2);
                    StreamWriter writer2 = new StreamWriter(fullPath2);

                    for (int i = 0; i < populationList.Count; i++)
                    {
                        if (populationList[i].gameObject.tag != "Cancelled")
                        {
                            string OutPut = populationList[i].GetComponent<OrganizationalModel.Behaviors.OrganizationBehavior>().energyLevel.ToString();
                            writer2.WriteLine(OutPut);
                        }
                    }

                    writer2.Close();

                    ///// ENERGY DATA

                    ///// NEIGHBOUR DATA
                    string filePath3 = SimulationManager.Get().NeighbourCountFilePath;//@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\3D\Cluster\NeighbourCount\";
                    string fileName3 = "NeighbourCount" + GenerationManager.generationCount.ToString() + ".txt";
                    string fullPath3 = System.IO.Path.Combine(filePath3, fileName3);

                    StreamWriter writer3 = new StreamWriter(fullPath3);

                    for (int i = 0; i < populationList.Count; i++)
                    {
                        if (populationList[i].gameObject.tag != "Cancelled")
                        {
                            string OutPut = populationList[i].GetComponent<OrganizationalModel.Behaviors.OrganizationBehavior>().neighbours.Count.ToString();


                            writer3.WriteLine(OutPut);
                        }
                    }

                    writer3.Close();


                    string filePath4 = SimulationManager.Get().CommunicationTypeFilePath;//@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\3D\Cluster\CommunicationType\";
                    string fileName4 = "CommunicationType" + GenerationManager.generationCount.ToString() + ".txt";
                    string fullPath4 = System.IO.Path.Combine(filePath4, fileName4);

                    StreamWriter writer4 = new StreamWriter(fullPath4);

                    for (int i = 0; i < populationList.Count; i++)
                    {
                        if (populationList[i].gameObject.tag != "Cancelled")
                        {
                            string OutPut = populationList[i].GetComponent<OrganizationalModel.Behaviors.OrganizationBehavior>().communicationType.ToString();


                            writer4.WriteLine(OutPut);
                        }
                    }

                    writer4.Close();






                    string filePath5 = SimulationManager.Get().DirectionalityRuleFilePath;//@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\3D\Cluster\DirectionalityRule\";
                    string fileName5 = "DirectionalityRule" + GenerationManager.generationCount.ToString() + ".txt";
                    string fullPath5 = System.IO.Path.Combine(filePath5, fileName5);

                    StreamWriter writer5 = new StreamWriter(fullPath5);

                    for (int i = 0; i < populationList.Count; i++)
                    {
                        if (populationList[i].gameObject.tag != "Cancelled")
                        {
                            string OutPut = populationList[i].GetComponent<OrganizationalModel.Behaviors.OrganizationBehavior>().directionalityRule.ToString();


                            writer5.WriteLine(OutPut);
                        }
                    }

                    writer5.Close();



                    string filePath6 = SimulationManager.Get().DensityThresholdFilePath;
                    string fileName6 = "DensityThreshold" + GenerationManager.generationCount.ToString() + ".txt";
                    string fullPath6 = System.IO.Path.Combine(filePath6, fileName6);

                    StreamWriter writer6 = new StreamWriter(fullPath6);

                    for (int i = 0; i < populationList.Count; i++)
                    {
                        if (populationList[i].gameObject.tag != "Cancelled")
                        {
                            string OutPut = populationList[i].GetComponent<OrganizationalModel.Behaviors.OrganizationBehavior>().currentScalarValueDensityCheck.ToString();


                            writer6.WriteLine(OutPut);
                        }
                    }

                    writer6.Close();







                    ///// NEIGHBOUR DATA




                    ///// STATE HISTORY DATA FREEZED
                    string filePath1 = @"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\3D\Cluster\AgentHistory\AgentStateHistory\Freezed\";
                    string fileName1 = "FreezedStateHistory" + GenerationManager.generationCount.ToString() + ".txt";
                    bool freezed = true;
                    ExportData.ExportStateHistoryData(filePath1, fileName1, populationList, freezed, false, false, false);

                    /////  STATE HISTORY DATA  FREEZED




                    ///// STATE HISTORY DATA DEACTIVATED
                    string filePathA = @"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\3D\Cluster\AgentHistory\AgentStateHistory\DeActivated\";
                    string fileNameA = "DeActivatedStateHistory" + GenerationManager.generationCount.ToString() + ".txt";

                    ExportData.ExportStateHistoryData(filePathA, fileNameA, populationList, false, true, false, false);

                    /////  STATE HISTORY DATA  DEACTIVATED



                    ///// STATE HISTORY DATA EMMITER
                    string filePathB = @"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\3D\Cluster\AgentHistory\AgentStateHistory\Emmiter\";
                    string fileNameB = "EmmiterStateHistory" + GenerationManager.generationCount.ToString() + ".txt";

                    ExportData.ExportStateHistoryData(filePathB, fileNameB, populationList, false, false, true, false);

                    /////  STATE HISTORY DATA  EMMITER


                    ///// STATE HISTORY DATA RECEIVER
                    string filePathC = @"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\3D\Cluster\AgentHistory\AgentStateHistory\Receiver\";
                    string fileNameC = "ReceiverStateHistory" + GenerationManager.generationCount.ToString() + ".txt";

                    ExportData.ExportStateHistoryData(filePathC, fileNameC, populationList, false, false, false, true);

                    /////  STATE HISTORY DATA  RECEIVER








                    //StringBuilder tsvData = new StringBuilder();
                    //// tsvData.AppendLine("Agent , Neighbour History, Inactive history State, De Activated History state, Emitter History state, Receiver History state");

                    //tsvData.AppendLine("Agent\tNeighbour History");

                    //for (int i = 0; i < populationList.Count; i++)
                    //{
                    //    if (populationList[i] != null)
                    //    {
                    //        string agentName = populationList[i].name;


                    //        for (int j = 0; j < populationList[i].neighbourHistory.Count; j++)
                    //        {
                    //            string agentHistory = populationList[i].neighbourHistory[j].name.ToString();
                    //            string inactiveHistoryState = populationList[i].freezedHistoryCount.ToString();
                    //            string deActivatedHistoryState = populationList[i].deActivatedHistoryCount.ToString();
                    //            string emmiterHistoryState = populationList[i].emmiterHistoryCount.ToString();
                    //            string receiverHistoryState = populationList[i].receiverHistoryCount.ToString();

                    //            string data = agentName + "\t" + agentHistory;//+ "," + inactiveHistoryState + "," + deActivatedHistoryState + "," + emmiterHistoryState + "," + receiverHistoryState;
                    //            tsvData.AppendLine(data);

                    //        }
                    //    }




                    //}

                    //string Path = @"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\3D\Cluster\AgentHistory\HistoryData" + GenerationManager.generationCount.ToString() + ".tsv";

                    //File.AppendAllText(Path, tsvData.ToString());




                    exportDataTemp = true;
                    #endregion



                }






           


        } // END UPDATE





        ////////////////////////////////METHODS////////////////////////////////////////////////



     


        private void DisplayTopology()
        {

            for (int i = 0; i < populationList.Count; i++)
            {
                populationList[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
            }


        }



        private KdTree<Agent> FindObjectsWithTag(string tag)
        {
            KdTree<Agent> data = new KdTree<Agent>();


            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (GameObject item in allObjects)
                if (item.GetComponent<Agent>() && item.tag == tag)
                    data.Add(item.GetComponent<Agent>());

            return data;
        }






        public  Vector3 GetInitPopCntrMass()
        {
           
            for (int i = 0; i < populationList.Count; i++)
            {
                centrMassInitPop = centrMassInitPop + populationList[i].transform.position;
            }

            centrMassInitPop = centrMassInitPop / populationList.Count;
            return centrMassInitPop;
        }


        /// <summary>
        /// This method activates n number of agents from initial position in rhino.
        /// </summary>
        public void ActivateAgentsBasedOnInitClusterPositionFromRhino(int num)
        {
            List<Vector3> initClusterCntrs = GetClusterInitPosFromRhino();

         //   for (int i = 0; i < num; i++)
         //   {

               // Agent agentToActivate = Utility.ClosestObject(initClusterCntrs[0], populationList);

                 List<Agent> KNearest = Utility.KNearestNeighbours(initClusterCntrs[0], populationList, num);

            for (int i = 0; i < KNearest.Count; i++)
            {

            
                string[] separators = { " " };
                string value = KNearest[i].name;
                string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                string targetEmmiterAgentIndex = words[1]; /// THIS WILL ALWAYS OUTPUT A NUMBER ---> EXAMPLE: Place Holder 100.1 it will output 100
                int index = Convert.ToInt32(targetEmmiterAgentIndex);
                indexActivatedAgentsList.Add(index);
                SystemActivatedAgentHistory.systemActivatedAgentHistory++;
                SystemActivatedAgentHistory.systemActivatedAgentHistoryList.Add(SystemActivatedAgentHistory.systemActivatedAgentHistory);

                KNearest[i].tag = "SignalEmmiter";



            }

        




        }



        public List<Vector3> GetClusterInitPosFromRhino()
        {
            //string[] lines = System.IO.File.ReadAllLines(@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\2D\ScalarField\ToUnity\ClusterstartingPositions.txt");
            string[] lines = System.IO.File.ReadAllLines(SimulationManager.Get().ImportToUnityClusterStartingPositionsFilePath);
            List<Vector3> data = new List<Vector3>();
            foreach (string item in lines)
            {
                string[] sArray = item.Split(',');

                // store as a Vector3
                Vector3 vector = new Vector3(
                    float.Parse(sArray[0]),
                    float.Parse(sArray[2]),
                    float.Parse(sArray[1]));

                data.Add(vector);
            }
            return data;

        }

        

        /// <summary>
        /// Initialize population in a 2D Array
        /// </summary>
        /// <param name="number"></param> number of agents to initialize
        private void InitializeAgents2DArray(int number)
        {

            for (int i = 0; i < Mathf.Sqrt(number); i++)
            {

                for (int j = 0; j < Mathf.Sqrt(number); j++)
                {

                    Vector3 distributionArea = new Vector3(i * arraySpacing, 0, j * arraySpacing);

                    populationList.Add((Instantiate(agentPrefab.GetComponent<Agent>(), distributionArea, Quaternion.identity)));



                }

            }


            for (int i = 0; i < populationList.Count; i++)
            {
                Vector3 delta = new Vector3(UnityEngine.Random.onUnitSphere.x, 0, UnityEngine.Random.onUnitSphere.z);
                populationList[i].transform.position += delta * 0.02f;
            }

           
            //////////////////////////////////////////////////////////


            /////////////////////////////////////////////////////////////  


        }

        /// <summary>
        /// Initialize population in a 3D Array
        /// </summary>
        /// <param name="number"></param> number of agents to initialize

        private void InitializeAgents3DArray(int number)
        {
            for (int i = 0; i < System.Math.Ceiling(Mathf.Pow(number, 1.0f / 3.0f)); i++)
            {

                for (int j = 0; j < System.Math.Ceiling(Mathf.Pow(number, 1.0f / 3.0f)); j++)
                {

                    for (int k = 0; k < System.Math.Ceiling(Mathf.Pow(number, 1.0f / 3.0f)); k++)
                    {
                        //Vector3 distributionArea = new Vector3(i * arraySpacing + (14.952f + 6.998f), j * arraySpacing+10, k * arraySpacing+(14.952f + 6.998f));
                        Vector3 distributionArea = new Vector3(i * arraySpacing-14 , j * arraySpacing+10, k * arraySpacing-14);

                        populationList.Add((Instantiate(agentPrefab.GetComponent<Agent>(), distributionArea, Quaternion.identity)));

               




                    }
                }

            }

            for (int i = 0; i < populationList.Count; i++)
            {
                populationList[i].transform.position += new Vector3(14.54f, 0, 14.54f);
             //   populationList[i].transform.position += UnityEngine.Random.onUnitSphere * 0.1f;




            }

            //for (int i = 0; i < populationList.Count; i++)
            //{
            //    populationList[i].transform.position += new Vector3(0, 5, 0);
            //}

            //MovePopTowardsPixelArrayCntrMass();

            

        }


        private void InitializeAgents2DRandom(int number, float dustributionRangeMin, float dustributionRangeMax)
        {
            for (int i = 0; i < number; i++)
            {
                

                Vector3 distributionArea = new Vector3(UnityEngine.Random.Range(0, dustributionRangeMax), 0, UnityEngine.Random.Range(0, dustributionRangeMax));
                populationList.Add((Instantiate(agentPrefab.GetComponent<Agent>(), distributionArea, Quaternion.identity)));

            }

        }

       
        /// <summary>
        /// This methods make agentPopulation list in the center of mass of the Pixel Array
        /// </summary>
        /// 
        private Vector3 GetDelta()
        {
            Vector3 target = pixelPop.MakeCntrMassYValEqual();

            Vector3 currentCntrMass = GetInitPopCntrMass();

            Vector3 delta = target - currentCntrMass;

            return delta;
            
        }
        private void MovePopTowardsPixelArrayCntrMass()
        {
            Vector3 target = pixelPop.MakeCntrMassYValEqual();

            Vector3 currentCntrMass = GetInitPopCntrMass();

            Vector3 delta = target - currentCntrMass;

            for (int i = 0; i < populationList.Count; i++)
            {
                populationList[i].transform.position += delta;
            }
        }



        private List<Agent> PickRandomAgentstoActivateFromBottomLayer(int number)
        {
            List<Agent> bottomLayerAgents = new List<Agent>();
            for (int i = 0; i < populationList.Count; i++)
            {
                if(populationList[i].gameObject.transform.position.y<11)
                {
                    bottomLayerAgents.Add(populationList[i]);
                }
            }
            List<Agent> listToReturn = new List<Agent>();
            for (int i = 0; i < number; i++)
            {

                //1670--->population of 3000.about middle agent

                //1098--->population of 2000.about middle agent
                //544 --->population of 1000.about middle agent
                // 363 --> population of 700
                //445 --> population of 600
                //291--->population of 400.about middle agent /// 203 ---> 5 m height for opolation and cloud
                //171 --->population of 300.about middle agent
                // 62 --->population of 100.about middle agent
                int index = UnityEngine.Random.Range(0, bottomLayerAgents.Count);
                //int index = populationList.Count/ 2;
              //  int index = 291; // 
                indexActivatedAgentsList.Add(index); //index
                SystemActivatedAgentHistory.systemActivatedAgentHistory++;
                SystemActivatedAgentHistory.systemActivatedAgentHistoryList.Add(SystemActivatedAgentHistory.systemActivatedAgentHistory);
                listToReturn.Add(bottomLayerAgents[index]); //index



            }


           
          

            for (int i = 0; i < listToReturn.Count; i++)
            {
                listToReturn[i].tag = "SignalEmmiter";

         
            }

            return listToReturn;
        }


        private void ActivateMiddleAgentOfBottomLayer()
        {
            Vector3 centr = Vector3.zero;
            List<Agent> bottomLayer = new List<Agent>();
            for (int i = 0; i < populationList.Count; i++)
            {
                if (populationList[i].gameObject.transform.position.y < 11) bottomLayer.Add(populationList[i]);

            }

            for (int i = 0; i < bottomLayer.Count; i++)
            {
                centr = centr + bottomLayer[i].transform.position;
            }

            centr = centr / bottomLayer.Count;

            Agent agentToActivate = Utility.ClosestObject(centr, bottomLayer);

            string[] separators = { " " };
            string value = agentToActivate.name;
            string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string targetEmmiterAgentIndex = words[1]; /// THIS WILL ALWAYS OUTPUT A NUMBER ---> EXAMPLE: Place Holder 100.1 it will output 100
            int index = Convert.ToInt32(targetEmmiterAgentIndex);
            indexActivatedAgentsList.Add(index);
            SystemActivatedAgentHistory.systemActivatedAgentHistory++;
            SystemActivatedAgentHistory.systemActivatedAgentHistoryList.Add(SystemActivatedAgentHistory.systemActivatedAgentHistory);

            agentToActivate.tag = "SignalEmmiter";

          
        }
     
        /// <summary>
        /// Activates first agent based on its proximity to the center of mass of the people population
        /// </summary>
        private void ActivateAgentBasedOnCntOfPplMass()
        {
            Vector3 centerMass= GetCntrMassPeoplePopulation();

            Agent agentToActivate = Utility.ClosestObject(centerMass, populationList);

            string[] separators = { " " };
            string value = agentToActivate.name;
            string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string targetEmmiterAgentIndex = words[1]; /// THIS WILL ALWAYS OUTPUT A NUMBER ---> EXAMPLE: Place Holder 100.1 it will output 100
            int index = Convert.ToInt32(targetEmmiterAgentIndex);
            indexActivatedAgentsList.Add(index); 
            SystemActivatedAgentHistory.systemActivatedAgentHistory++;
            SystemActivatedAgentHistory.systemActivatedAgentHistoryList.Add(SystemActivatedAgentHistory.systemActivatedAgentHistory);


            agentToActivate.tag = "SignalEmmiter";

         ///   base.ChangeDisplayType(agentToActivate.GetComponent<Agent>(), 0);
            //if (SimulationManager.Get().displayColorbyEnergy)
            //{

            //    agentToActivate.GetComponent<MeshRenderer>().material.color = agent.EnergyColor(SimulationManager.Get().startColorEnergy, SimulationManager.Get().endColorEnergy, agent.energyLevel);
            //}

            //if (SimulationManager.Get().displayColorByState)
            //{
            //    agentToActivate.GetComponent<MeshRenderer>().material.color = Color.blue;

            //}

            //if(SimulationManager.Get().displayColorbyDisplacement)
            //{
            //    agentToActivate.GetComponent<MeshRenderer>().material.color = Color.grey;
            //}

            //if(SimulationManager.Get().GPUInstancing)
            //{
            //    float r = SimulationManager.Get().monochromeColor.r;
            //    float g = SimulationManager.Get().monochromeColor.g;
            //    float b = SimulationManager.Get().monochromeColor.b;
            //    MaterialPropertyBlock properties = new MaterialPropertyBlock();


            //    properties.SetColor("_Color", new Color(r,g,b));


            //    MeshRenderer meshR = agentToActivate.GetComponent<MeshRenderer>();
            //    if (meshR)
            //    {
            //        meshR.SetPropertyBlock(properties);
            //    }
            //}


        }



        private void ActivateAgentBasedOnCntOfPplMassB()
        {
            List<Vector3> clusterPplCenters = ClusterPeopleCenters();

            for (int i = 0; i < clusterPplCenters.Count; i++)
            {
                Agent agentToActivate = Utility.ClosestObject(clusterPplCenters[i], populationList);

                string[] separators = { " " };
                string value = agentToActivate.name;
                string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                string targetEmmiterAgentIndex = words[1]; /// THIS WILL ALWAYS OUTPUT A NUMBER ---> EXAMPLE: Place Holder 100.1 it will output 100
                int index = Convert.ToInt32(targetEmmiterAgentIndex);
                indexActivatedAgentsList.Add(index);
                SystemActivatedAgentHistory.systemActivatedAgentHistory++;
                SystemActivatedAgentHistory.systemActivatedAgentHistoryList.Add(SystemActivatedAgentHistory.systemActivatedAgentHistory);

                agentToActivate.tag = "SignalEmmiter";

            

            }
                for (int i = 0; i < clusterPplCenters.Count; i++)
                {
                    print(clusterPplCenters[i]);
                }


            

        }


        /// <summary>
        /// Get cluster centers from grasshopper
        /// </summary>
        /// <returns></returns>
        private List<Vector3> ClusterPeopleCenters()
        {
            List<Vector3> data = new List<Vector3>();
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\2D\ScalarField\ToUnity\PplClusterCenterMass\Centers.txt");

         

            foreach (string item in lines)
            {
                string[] sArray = item.Split(',');

                // store as a Vector3
                Vector3 vector = new Vector3(
                    float.Parse(sArray[0]),
                    float.Parse(sArray[1]),
                    float.Parse(sArray[2]));

                data.Add(vector);
            }
            return data;
        }

        /// <summary>
        /// Gets the center of mass of the people population
        /// </summary>
        /// <returns></returns>
        private Vector3 GetCntrMassPeoplePopulation()
        {
            

            for (int i = 0; i < PeoplePopulation.peopleList.Count; i++)
            {
                centrMassPplPop = centrMassPplPop + PeoplePopulation.peopleList[i].transform.position;
            }

            centrMassPplPop = centrMassPplPop / PeoplePopulation.peopleList.Count;
            return centrMassPplPop;

        }
        






        /// <summary>
        /// Set Parent GameObject in the inspector to hold all Agent objects
        /// </summary>

        private void Parent()
        {
            for (int i = 0; i < populationList.Count; i++)
            {
               

                populationList[i].transform.SetParent(transform);

            }


        }



        /// <summary>
        /// Set Agent name According to Index order
        /// </summary>
        private void AgentName()
        {
            for (int i = 0; i < populationList.Count; i++)
            {
                populationList[i].name = "Agent" + " " + i;
                populationList[i].GetComponent<Agent>().agentName = "Agent" + " " + i;  //ReOrganizationBehavior
                populationListIndexes.Add(i);
            }
        }


        /// <summary>
        /// Set initial state of agents; i.e tag
        /// </summary>
        private void InitializeState()
        {


            for (int i = 0; i < populationList.Count; i++)
            {
             

               agent.ChangeStateToDeactivated(populationList[i],0);


            }


        }








    }// END CLASS 

}

