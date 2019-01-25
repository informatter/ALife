using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using OrganizationalModel.Managers;
using OrganizationalModel.Population;
using OrganizationalModel.Behaviors;
using OrganizationalModel.BaseClass;
namespace OrganizationalModel.UserInterface
{
    public class UI : MonoBehaviour
    {
        Agent agent = new Agent();
        List<float> energyLevels = new List<float>();
        List<float> TotalDisplacement = new List<float>();
        float totalClusterDisplacement = 0;
        float avergeDisplacement=0;
        float initEnergy = 650 * AgentPopulation.populationList.Count;
        float totalEnergyConsumed = 0;
      
        public float avaregeEnergyConsumed = 0;

        float gameTime = 0f;
        
        string timeFormat;
        float timeSinceAssemblyFinished = 0;
        string timeSinceAssemblyFinishedFormat;

        // NON STATIC PUBLIC VARIABLES TO REFERENCE FROM OTHER CLASSES  
        GUIStyle guiStyleTxt = new GUIStyle();
        GUIStyle guiStyleTitles = new GUIStyle();

        GUIStyle guiStyleButton = new GUIStyle();

        bool runSimulation;

        public static float averegeFiledValues;
        public static float  rateOfChange;



       List<int> clusterNeighbourCountList = new List<int>();

         int totalNeighbours = 0;
       
         int totalPossibleNeighbours;
        float clusterDensity = 0;
         float averegeClusterDensity = 0;
   
        private void Start()
        {
            ///////////////////////////////VARIABLE INITIALIZATION////////////////////////////////
            // // NON STATIC PUBLIC VARIABLE INITIALIZATION

           

            if (SimulationManager.Get().runSimulation) runSimulation = true;


            guiStyleTxt.normal.background = null;
            guiStyleTxt.normal.textColor = new Color(1, 1, 1);
            guiStyleTxt.fontSize = 22;
            

            guiStyleTitles.normal.background = null;
            guiStyleTitles.normal.textColor = new Color(1, 1, 1);
            guiStyleTitles.fontSize = 22;
            guiStyleTitles.fontStyle = FontStyle.Bold;

            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, new Color(1, 1, 1, 0.1f));
         
            texture.Apply();
            guiStyleButton.normal.background = texture;

            guiStyleButton.fontSize = 22;
            guiStyleButton.normal.textColor = new Color(1, 1, 1);
            ///////////////////////////////METHOD INITIALIZATION////////////////////////////////
        }


        private void Update()
        {


            if (OrganizationBehavior.runSimulation)
            {
                int totalCancelledAgents = AgentPopulation.totalCancelledAgents;

                if (AgentPopulation.freezedAgentList != null)
                {
                    if (AgentPopulation.freezedAgentList.Count / (float)AgentPopulation.populationList.Count * 100 < 97) gameTime += Time.deltaTime;

                    if (AgentPopulation.freezedAgentList.Count / (float)AgentPopulation.populationList.Count * 100 > 97) timeSinceAssemblyFinished += Time.deltaTime;

                }



                int seconds = (int)(gameTime % 60);
                int minutes = (int)(gameTime / 60) % 60;
                int hours = (int)(gameTime / 3600) % 24;

                timeFormat = string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);

                if (SimulationManager.Get().addWindForce)
                {
                    int seconds1 = (int)(timeSinceAssemblyFinished % 60);
                    int minutes1 = (int)(timeSinceAssemblyFinished / 60) % 60;
                    int hours1 = (int)(timeSinceAssemblyFinished / 3600) % 24;

                    timeSinceAssemblyFinishedFormat = string.Format("{0:0}:{1:00}:{2:00}", hours1, minutes1, seconds1);

                    if (timeSinceAssemblyFinishedFormat == string.Format("{0:0}:{1:00}:{2:00}", hours1, 1, 45))
                    {
                       // Debug.Break(); // pauses game


                    }

                    
                }
            }

           
        }

      


        private void OnGUI()
        {
           

                if (SimulationManager.Get().addWindForce)
                {

                    for (int i = 0; i < AgentPopulation.populationList.Count; i++)
                    {
                        TotalDisplacement.Add(AgentPopulation.populationList[i].GetComponent<OrganizationBehavior>().totalDisplacement);
                    }

                    totalClusterDisplacement = TotalDisplacement.Sum() * 0.001f; // convert mm to m
                    avergeDisplacement = (totalClusterDisplacement / AgentPopulation.populationList.Count);
                }

            if (SimulationManager.Get().oneGeneration && AgentPopulation.freezedAgentList.Count /( (float)AgentPopulation.populationList.Count - AgentPopulation.cancelledAgentList.Count )* 100 < 97)
            {
                for (int i = 0; i < AgentPopulation.populationList.Count; i++)
                {


                    energyLevels.Add(650 - AgentPopulation.populationList[i].energyLevel);



                }

                if (energyLevels.Count > AgentPopulation.populationList.Count)
                {

                    energyLevels.RemoveRange(0, energyLevels.Count - AgentPopulation.populationList.Count);
                }


                if (energyLevels.Count != 0)
                {
                    totalEnergyConsumed = energyLevels.Sum();

                    avaregeEnergyConsumed = totalEnergyConsumed / AgentPopulation.populationList.Count;

                }

            }



            if (SimulationManager.Get().oneGeneration ==false)
            {
                for (int i = 0; i < AgentPopulation.populationList.Count; i++)
                {


                    energyLevels.Add(650 - AgentPopulation.populationList[i].energyLevel);



                }

                if (energyLevels.Count > AgentPopulation.populationList.Count)
                {

                    energyLevels.RemoveRange(0, energyLevels.Count - AgentPopulation.populationList.Count);
                }


                if (energyLevels.Count != 0)
                {
                    totalEnergyConsumed = energyLevels.Sum();

                    avaregeEnergyConsumed = totalEnergyConsumed / AgentPopulation.populationList.Count;

                }

            }




            //   if (AgentPopulation.freezedAgentList.Count == AgentPopulation.populationList.Count - AgentPopulation.cancelledAgentList.Count)
            //    {

            totalPossibleNeighbours = AgentPopulation.populationList.Count * 12;

                for (int i = 0; i < AgentPopulation.populationList.Count; i++)
                {
                      clusterNeighbourCountList.Add( AgentPopulation.populationList[i].neighbours.Count);
                
                }

                if (clusterNeighbourCountList.Count > AgentPopulation.populationList.Count)
                {

                    clusterNeighbourCountList.RemoveRange(0, clusterNeighbourCountList.Count - AgentPopulation.populationList.Count);
                }

            if (clusterNeighbourCountList.Count != 0)
            {
                totalNeighbours = clusterNeighbourCountList.Sum();
                clusterDensity = (totalNeighbours / (float) totalPossibleNeighbours) *100 ;
               averegeClusterDensity= clusterDensity / (float) AgentPopulation.populationList.Count;

            }
            

       //  }






            GUI.Label(new Rect(10, 0, 500, 200), "Goal:" + " ", guiStyleTitles);
                GUI.Label(new Rect(70, 0, 500, 200), AgentPopulation.freezedAgentList.Count / (float)(AgentPopulation.populationList.Count-AgentPopulation.cancelledAgentList.Count) * 100 + "%", guiStyleTxt);

                GUI.Label(new Rect(10, 60, 500, 200), "Assembly time:" + "  ", guiStyleTitles);
                GUI.Label(new Rect(185, 60, 500, 200), timeFormat, guiStyleTxt);
                if (SimulationManager.Get().addWindForce)
                {
                    GUI.Label(new Rect(10, 95, 500, 200), "Time since Assembly:" + "  ", guiStyleTitles);
                    GUI.Label(new Rect(250, 95, 500, 200), timeSinceAssemblyFinishedFormat, guiStyleTxt);
                }

                GUI.Label(new Rect(10, 165, 500, 200), "Total Generations:" + "  ", guiStyleTitles);
                GUI.Label(new Rect(205, 165, 500, 200), GenerationManager.generationCount.ToString(), guiStyleTxt);
                GUI.Label(new Rect(10, 200, 500, 200), "Total Population:" + "  ", guiStyleTitles);
                GUI.Label(new Rect(195, 200, 500, 200), AgentPopulation.populationList.Count.ToString(), guiStyleTxt);

                GUI.Label(new Rect(10, 270, 500, 200), "Total energy consumed:" + "  ", guiStyleTitles);
                GUI.Label(new Rect(270, 270, 500, 200), totalEnergyConsumed.ToString("0.00#") + " " + "kJ", guiStyleTxt);

                GUI.Label(new Rect(10, 305, 500, 200), "Averege energy consumed:" + "  ", guiStyleTitles);
                GUI.Label(new Rect(305, 305, 500, 200), avaregeEnergyConsumed.ToString("0.00#") + " " + "kJ", guiStyleTxt);

            if (SimulationManager.Get().addWindForce == false)
            {
                GUI.Label(new Rect(10, 375, 500, 200), "Cluster density:" + " ", guiStyleTitles);

                GUI.Label(new Rect(180, 375, 500, 200), clusterDensity.ToString("0.00#") + "%", guiStyleTxt);

                GUI.Label(new Rect(10, 410, 500, 200), "Averege density:" + " ", guiStyleTitles);

                GUI.Label(new Rect(190, 410, 500, 200), averegeClusterDensity.ToString("0.00#") + "%", guiStyleTxt);
            }

            if (SimulationManager.Get().addWindForce)
                {
                    if (SimulationManager.Get().windDisplacementAnalysis)
                    {
                        GUI.Label(new Rect(10, 375, 500, 200), "Total displacement:" + "  ", guiStyleTitles);
                        GUI.Label(new Rect(220, 375, 500, 200), totalClusterDisplacement.ToString("0.00#") + " " + "m", guiStyleTxt);

                        GUI.Label(new Rect(10, 410, 500, 200), "Averege displacement:" + "  ", guiStyleTitles);
                        GUI.Label(new Rect(260, 410, 500, 200), avergeDisplacement.ToString("0.00#") + " " + "m", guiStyleTxt);
                    }
                }


            //GUI.Label(new Rect(10, 480, 500, 200), "Cluster density:" + " ", guiStyleTitles);

            //GUI.Label(new Rect(180, 480, 500, 200), clusterDensity.ToString("0.00#") + "%", guiStyleTxt);

            //GUI.Label(new Rect(10, 515, 500, 200), "Averege density:" + " ", guiStyleTitles);

            //GUI.Label(new Rect(190, 515, 500, 200), averegeClusterDensity.ToString("0.00#") + "%", guiStyleTxt);


            DrawQuad(new Rect(0, 0, 400, 450), new Color(1, 1, 1, 0.1f));
         


            //  float alphaData = Mathf.Sin(averegeFiledValues);
            //  float thetaData = Mathf.Cos(averegeFiledValues);
            //  float alphaRemap = SharpMatter.SharpMath.SharpMath.Remap(alphaData, -1, 1, 0, 1000);
            //  float thetaRemap = SharpMatter.SharpMath.SharpMath.Remap(thetaData, -1, 1, 0, 1000);

            //  float betaData = Mathf.Sin(rateOfChange);

            //  float betaRemap = SharpMatter.SharpMath.SharpMath.Remap(betaData, -1, 1, 0, 1000); 


            //  GUI.Label(new Rect(Screen.width-200, 130, 500, 200), "Alpha:" + " ", guiStyleTitles);
            //  GUI.Label(new Rect(Screen.width - 130, 130, 500, 200), alphaRemap.ToString("0.00#"), guiStyleTxt);


            //GUI.Label(new Rect(Screen.width - 200, 160, 500, 200), "Theta:" + " ", guiStyleTitles);

            //GUI.Label(new Rect(Screen.width - 130, 160, 500, 200), thetaRemap.ToString("0.00#"), guiStyleTxt);

            //  GUI.Label(new Rect(Screen.width - 200, 190, 500, 200), "Beta:" + " ", guiStyleTitles);

            //  GUI.Label(new Rect(Screen.width - 130, 190, 500, 200), betaRemap.ToString("0.00#"), guiStyleTxt);



            if(SimulationManager.Get().displayColorByDistanceToNeighbours)
            {
                SimulationManager.Get().displayColorByState = false;
                SimulationManager.Get().displayColorbyEnergy = false;
                SimulationManager.Get().displayColorbyDisplacement = false;
                SimulationManager.Get().displayColorByComunication = false;
                SimulationManager.Get().displayColorByNeighbours = false;
                SimulationManager.Get().displayTopology = false;
                SimulationManager.Get().GPUInstancing = false;
                SimulationManager.Get().displayColorByEmotion = false;
                SimulationManager.Get().displayColorByDistanceToNeighbours = true;
                SimulationManager.Get().displayColorByRule = false;
            }

            if (SimulationManager.Get().displayColorByRule)
            {
                SimulationManager.Get().displayColorByState = false;
                SimulationManager.Get().displayColorbyEnergy = false;
                SimulationManager.Get().displayColorbyDisplacement = false;
                SimulationManager.Get().displayColorByComunication = false;
                SimulationManager.Get().displayColorByNeighbours = false;
                SimulationManager.Get().displayTopology = false;
                SimulationManager.Get().GPUInstancing = false;
                SimulationManager.Get().displayColorByEmotion = false;
                SimulationManager.Get().displayColorByDistanceToNeighbours = false;
                SimulationManager.Get().displayColorByRule = true;
            }


            GUI.Label(new Rect(10, 460, 500, 200), "Display settings" + "  ", guiStyleTitles);

            if (GUI.Button(new Rect(10, 500, 90, 30), "Display state"))
            {
                SimulationManager.Get().displayColorByState = true;
                SimulationManager.Get().displayColorbyEnergy = false;
                SimulationManager.Get().displayColorbyDisplacement = false;
                SimulationManager.Get().displayColorByComunication = false;
                SimulationManager.Get().displayColorByNeighbours = false;
                SimulationManager.Get().displayTopology = false;
                SimulationManager.Get().GPUInstancing = false;
                SimulationManager.Get().displayColorByEmotion = false;
                SimulationManager.Get().displayColorByDistanceToNeighbours = false;
                SimulationManager.Get().displayColorByRule = false;
            }

            if (GUI.Button(new Rect(10, 540, 150, 30), "Display energy levels"))
            {
                SimulationManager.Get().displayColorByState = false;
                SimulationManager.Get().displayColorbyEnergy = true;
                SimulationManager.Get().displayColorbyDisplacement = false;
                SimulationManager.Get().displayColorByComunication = false;
                SimulationManager.Get().displayColorByNeighbours = false;
                SimulationManager.Get().displayTopology = false;
                SimulationManager.Get().GPUInstancing = false;
                SimulationManager.Get().displayColorByEmotion = false;
                SimulationManager.Get().displayColorByDistanceToNeighbours = false;
                SimulationManager.Get().displayColorByRule = false;
            }

            if (GUI.Button(new Rect(10, 580, 160, 30), "Display displacement"))
            {
                SimulationManager.Get().displayColorByState = false;
                SimulationManager.Get().displayColorbyEnergy = false;
                SimulationManager.Get().displayColorbyDisplacement = true;
                SimulationManager.Get().displayColorByComunication = false;
                SimulationManager.Get().displayColorByNeighbours = false;
                SimulationManager.Get().displayTopology = false;
                SimulationManager.Get().GPUInstancing = false;
                SimulationManager.Get().displayColorByEmotion = false;
                SimulationManager.Get().displayColorByDistanceToNeighbours = false;
                SimulationManager.Get().displayColorByRule = false;
            }

            if (GUI.Button(new Rect(10, 620, 180, 30), "Display communication type"))
            {
                SimulationManager.Get().displayColorByState = false;
                SimulationManager.Get().displayColorbyEnergy = false;
                SimulationManager.Get().displayColorbyDisplacement = false;
                SimulationManager.Get().displayColorByComunication = true;
                SimulationManager.Get().displayColorByNeighbours = false;
                SimulationManager.Get().displayTopology = false;
                SimulationManager.Get().GPUInstancing = false;
                SimulationManager.Get().displayColorByEmotion = false;
                SimulationManager.Get().displayColorByDistanceToNeighbours = false;
                SimulationManager.Get().displayColorByRule = false;
            }

            if (GUI.Button(new Rect(10, 660, 170, 30), "Display neighbour density"))
            {
                SimulationManager.Get().displayColorByState = false;
                SimulationManager.Get().displayColorbyEnergy = false;
                SimulationManager.Get().displayColorbyDisplacement = false;
                SimulationManager.Get().displayColorByComunication = false;
                SimulationManager.Get().displayColorByNeighbours = true;
                SimulationManager.Get().displayTopology = false;
                SimulationManager.Get().GPUInstancing = false;
                SimulationManager.Get().displayColorByEmotion = false;
                SimulationManager.Get().displayColorByDistanceToNeighbours = false;
                SimulationManager.Get().displayColorByRule = false;
            }

            if (GUI.Button(new Rect(10, 700, 130, 30), "Display topology"))
            {
                SimulationManager.Get().displayColorByState = false;
                SimulationManager.Get().displayColorbyEnergy = false;
                SimulationManager.Get().displayColorbyDisplacement = false;
                SimulationManager.Get().displayColorByComunication = false;
                SimulationManager.Get().displayColorByNeighbours = false;
                SimulationManager.Get().displayTopology = true;
                SimulationManager.Get().GPUInstancing = false;
                SimulationManager.Get().displayColorByEmotion = false;

                SimulationManager.Get().displayColorByDistanceToNeighbours = false;
                SimulationManager.Get().displayColorByRule = false;

            }

            if (GUI.Button(new Rect(10, 740, 130, 30), "Display emotion"))
            {
                SimulationManager.Get().displayColorByState = false;
                SimulationManager.Get().displayColorbyEnergy = false;
                SimulationManager.Get().displayColorbyDisplacement = false;
                SimulationManager.Get().displayColorByComunication = false;
                SimulationManager.Get().displayColorByNeighbours = false;
                SimulationManager.Get().displayTopology = false;
                SimulationManager.Get().displayColorByEmotion = true;
                SimulationManager.Get().GPUInstancing = false;
                SimulationManager.Get().displayColorByDistanceToNeighbours = false;
                SimulationManager.Get().displayColorByRule = false;

            }


            if (GUI.Button(new Rect(10, 780, 130, 30), "GPU rendering"))
            {
                SimulationManager.Get().displayColorByState = false;
                SimulationManager.Get().displayColorbyEnergy = false;
                SimulationManager.Get().displayColorbyDisplacement = false;
                SimulationManager.Get().displayColorByComunication = false;
                SimulationManager.Get().displayColorByNeighbours = false;
                SimulationManager.Get().displayTopology = false;
                SimulationManager.Get().GPUInstancing = true;
                SimulationManager.Get().displayColorByEmotion = false;
                SimulationManager.Get().displayColorByDistanceToNeighbours = false;
                SimulationManager.Get().displayColorByRule = false;
            }

            GUI.Label(new Rect(10, 860, 130, 200), "Setup" + "  ", guiStyleTitles);

                if (GUI.Button(new Rect(10, 820, 130, 30), "Run simulation"))
                {
                    SimulationManager.Get().runSimulation = true;
                    OrganizationBehavior.runSimulation = true;
                    ReOrganizationBehavior.runSimulation = true;
                
               

                }

                if (GUI.Button(new Rect(10, 900, 130, 30), "Random initial conditions"))
                {
                    SimulationManager.Get().randomInitialConditions = true;

                }

            if (GUI.Button(new Rect(10, 940, 130, 30), "One generation"))
            {

                SimulationManager.Get().oneGeneration = true;

            }

            if (GUI.Button(new Rect(10, 980, 130, 30), "Multiple generations"))
            {
                SimulationManager.Get().multipleGenerations = true;

            }





        }



        void DrawQuad(Rect position, Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            GUI.skin.box.normal.background = texture;
            GUI.Box(position, GUIContent.none);
        }


    }

}
