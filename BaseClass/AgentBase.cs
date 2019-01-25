using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ClassExtensions;
using SharpMatter.SharpDataStructures;
using SharpMatter.SharpMath;
using OrganizationalModel.UtilityFunctions;
using OrganizationalModel.Population;
using OrganizationalModel.Managers;
using OrganizationalModel.ScalarFields;
using OrganizationalModel.Behaviors;

using SharpMatter.Geometry;

////////////////////////////////CLASS DESCRIPTION////////////////////////////////
/// <summary>
/// 
/// THIS CLASS CONTAINS ALL THE BASE FUNCTIONS AND PROPERTIES OF THE AGENT  BASE FUNCTIONALITY. IT WAS DEVELOPED SO BEHAVIOR CLASSES CAN INHERIT FROM 
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
/// 




namespace OrganizationalModel.BaseClass
{
    public enum DirectionalityRules
    { Rule01, Rule02, Rule03, Rule04, Rule05, Rule06, Rule07, Rule08, Rule09, Rule10, Rule11, Rule12, Rule13, Rule14, Rule15, Rule16, Rule17, Rule18, Rule19, Rule20, Rule21, Rule22, Rule23,Rule24, Rule25, Rule26 } //,Rule24,Rule25,Rule26 }

    public enum DensityRules
    { RuleA, RuleB, RuleC, RuleD,RuleE}
    public partial class Agent : MonoBehaviour, IPhysicsBehavior<float, Vector3>
    {
        public Gradient energyGradientColor;
        public Gradient ruleSetColor;

        [HideInInspector]
        public float IMaxForce { get; set; }
        [HideInInspector]
        public float IMass { get; set; }
        [HideInInspector]
        public float IMaxSpeed { get; set; }
        [HideInInspector]
        public float VisionRadius { get; set; }

        //[HideInInspector]
        public int communicationType; // FOR DEBUGGING PURPOSES!!!

        [HideInInspector]
        public Vector3 velocity;
        [HideInInspector]
        public Vector3 acceleration;

        [HideInInspector]
        public string agentName;


        // public List<float> distance;

        [HideInInspector]
        public float distanceToNeighbour;



        [HideInInspector]
        public int signalReceiverPassiveCounter;

        [HideInInspector]
        public List<Agent> neighboursTemp;

        [HideInInspector]
        public List<Vector3> neighboursTempVector;

        public List<Agent> neighbours;

        [HideInInspector]
        public List<Vector3> neighboursVector;


        [HideInInspector]
        public List<Agent> targetAgent;

        public List<Agent> targetForSignalReceiver;



        [HideInInspector]
        public List<Agent> mySignalReceiversTemp;

        public List<Agent> mySignalReceivers;


        [HideInInspector]
        public List<Agent> agentsToRepell;
        [HideInInspector]
        public List<Agent> agentsToAvoid;
        [HideInInspector]
        public List<Agent> closestN;




        #region CALCULATE STATE HISTORY COROUTINES

        [HideInInspector]
        public bool calculateEmmiterHistoryCoroutine;
        [HideInInspector]
        public bool calculateReceiverHistoryCoroutine;
        [HideInInspector]
        public bool calculateDeActivatedHistoryCoroutine;
        [HideInInspector]
        public bool calculateFreezedHistoryCoroutine;
        [HideInInspector]
        public bool calculateDeActivatedHistoryIfDoesNotChangeInGenerationCoroutine;

        #endregion


        #region ENERGY DATA



        [HideInInspector]
        public float energyDecreasingFactor;


        public float energyLevel;


        public float energyCapacity;

        #endregion


        #region PIXEL DATA




        // [HideInInspector]
        public string currentPixel;

        public float currentPixelDensityVal;

        [HideInInspector]
        public VectorData currentAgentXValRange;

        [HideInInspector]
        public int totalGenerationsBeingStatic;

        public int timesInCurrentPixel; /// BY AGENT NAME
        public int currentPixelTotalVisits;
        [HideInInspector]
        public List<string> myNameInCurrentPixel;

        #endregion

        #region RULES DIRECTIONALITY

       public  int directionalityRule;
        #endregion

        #region SCALAR FIELD DATA

        public string currentScalarCell;
        public float currentScalarValueRules;
        public float currentScalarValuePackingProximity;
        public float currentScalarValueDensityCheck;
        public float currentScalarValueHumanData;
        #endregion


        #region PLACE HOLDERS VARIABLES

        //[HideInInspector]
        //public List<PlaceHolder> activatedPlaceHolders;
        public List<PlaceHolder> selectedActivatedPlaceHolders;

        //public int selectedActivatedPlaceHolders;
        // public int totalActivatedPlaceHolders; // FOR DEBUGGING PURPOSES!!!

        [HideInInspector]
        public List<PlaceHolder> placeHolderLocalList;

        public List<PlaceHolder> placeHolders; // Place holders are directly the children objects of each agent

        public PlaceHolder placeHolderTargetForSignalReceiver;



        #endregion




        #region SIMULATION HISTORY DATA

        [HideInInspector]
        public int localCollisionHistory;
        [HideInInspector]
        public int emmiterHistoryCount;
        [HideInInspector]
        public int receiverHistoryCount;
        [HideInInspector]
        public int freezedHistoryCount;
        [HideInInspector]
        public int deActivatedHistoryCount;
        [HideInInspector]
        public Agent mostCommonAgentInConnectionHistory;
        [HideInInspector]
        public List<Agent> neighbourHistory;

        [HideInInspector]
        public List<Vector3> positionHistory;
        [HideInInspector]
        public List<Vector3> trailData;

        [HideInInspector]
        public List<Vector3> DupPositionHistory;
        [HideInInspector]
        public List<Vector3> positionHistoryTraceback;

        public Agent mySignalEmmiter;

        [HideInInspector]
        public List<Agent> familyTree;
        [HideInInspector]
        public List<Agent> familyTreeTemp;

        #endregion





        public List<PlaceHolder> palceHolderTargetHistoryList;

        [HideInInspector]
        public bool calculateRulesDiscreteCoroutine = false;
        [HideInInspector]
        public bool searchClosestNeighbourOrganization = false;
        [HideInInspector]
        public bool searchClosestNeighbourRe_Org = false;

        private bool couroutineTest;

        List<float> rangeList;

        int a = 0;
        

        float maxVal = 0.63f;// min value scalarField ruleset
        float minVal = 2.99f; // max value scalarField ruleset

        //CONSTRUCTOR
        public Agent()

        {
           

            this.couroutineTest = false;
            this.rangeList = new List<float>();

            // NON STATIC PUBLIC VARIABLE INITIALIZATION
            //this.distance = new List<float>();

            this.palceHolderTargetHistoryList = new List<PlaceHolder>();

            this.velocity = new Vector3(0, 0, 0);

            this.acceleration = new Vector3(0, 0, 0);
            this.neighboursTemp = new List<Agent>();
            this.neighboursTempVector = new List<Vector3>();
            this.neighbours = new List<Agent>();
            this.neighboursVector = new List<Vector3>();

            this.targetAgent = new List<Agent>();

            this.agentsToRepell = new List<Agent>();
            this.agentsToAvoid = new List<Agent>();
            this.neighbourHistory = new List<Agent>();
            this.targetForSignalReceiver = new List<Agent>();
            this.closestN = new List<Agent>();
            this.mySignalReceiversTemp = new List<Agent>();
            this.mySignalReceivers = new List<Agent>();
            this.positionHistory = new List<Vector3>();
            this.trailData = new List<Vector3>();
            this.placeHolderLocalList = new List<PlaceHolder>();
            this.placeHolders = new List<PlaceHolder>();
            this.selectedActivatedPlaceHolders = new List<PlaceHolder>();
            this.placeHolderTargetForSignalReceiver = null;

            this.myNameInCurrentPixel = new List<string>();



            this.DupPositionHistory = new List<Vector3>();
            this.positionHistoryTraceback = new List<Vector3>();





            this.mySignalEmmiter = null;
            this.localCollisionHistory = 0;

            this.emmiterHistoryCount = 0;
            this.receiverHistoryCount = 0;
            this.freezedHistoryCount = 0;
            this.deActivatedHistoryCount = 0;

            this.familyTree = new List<Agent>();

            this.totalGenerationsBeingStatic = 0;

            this.timesInCurrentPixel = 0;
            this.currentPixelTotalVisits = 0;

            this.currentPixelDensityVal = 0;
            
            this.calculateEmmiterHistoryCoroutine = false;
            this.calculateReceiverHistoryCoroutine = false;
            this.calculateDeActivatedHistoryCoroutine = false;
            this.calculateFreezedHistoryCoroutine = false;
            this.calculateDeActivatedHistoryIfDoesNotChangeInGenerationCoroutine = false;

            familyTreeTemp = new List<Agent>();

            this.mostCommonAgentInConnectionHistory = null;


   


        }
        /// <summary>
        /// Get the total distance an agent has moved
        /// </summary>
        /// <param name="positions"></param>
        /// <returns></returns>
        public float GetTotalDisplacement(List<Vector3> positions)
        {
            List<Vector3> vecs = new List<Vector3>();
            List<float> vecsMag = new List<float>();
            float totalDisplacement = 0;
            for (int i = 0; i < positions.Count - 1; i++)
            {

                vecs.Add(positions[i + 1] - positions[i]);

            }

            for (int i = 0; i < vecs.Count; i++)
            {
                vecsMag.Add(vecs[i].magnitude);
            }

            for (int i = 0; i < vecsMag.Count; i++)
            {
                totalDisplacement += vecsMag[i];
            }


            return totalDisplacement;

        }

        public IEnumerator RemoveNeighboursByTag(Agent agent, string tag)
        {
            for (int i = agent.neighbours.Count - 1; i >= 0; i--)
            {

                if (agent.neighbours[i]!= null && agent.neighbours[i].gameObject.tag == tag)
                {
                  //  if (agent.neighbours[i].gameObject.tag == tag)
                  //  {
                        agent.neighbours.RemoveAt(i);
                   // }
                }

            }

            yield return new WaitForSeconds(1f);
        }


        public IEnumerator RemoveNeighboursByTag(Agent agent, List<string> tags)
        {
            for (int j = 0;  j < tags.Count;  j++)
            {
                for (int i = agent.neighbours.Count - 1; i >= 0; i--)
                {


                    if (agent.neighbours[i].gameObject.tag == tags[j])
                    {
                        agent.neighbours.RemoveAt(i);
                    }

                }
            }
           

            yield return new WaitForSeconds(1f);
        }




        public Color DisplacementColor(Color startColor, Color endColor, float displacementLevel)
        {

            return Color.Lerp(startColor, endColor, displacementLevel * 0.001f);




        }

        public Color ConnectivityColor(Color startColor, Color endColor, int neighbourCount)
        {
            
            return Color.Lerp(startColor, endColor, neighbourCount * 0.1f);

          



        }


        #region AGENT BEHAVIORS

        public void Wander()
        {
            ///The displacement force  has its origin at the circle's center, and is constrained by the circle radius. This is an imaginary circle placed at the tip of the agents Vel vector
            ///The greater the radius and the distance from character to the circle, the stronger the "push" the character will receive every game frame.
            /// This displacement force will be used to interfere with the character's route. It is used to calculate the wander force.
            /// 


            float WanderAngle = 10f;
            Vector3 circleCenter;

            circleCenter = velocity;
            circleCenter.Normalize();
            circleCenter *= (this.gameObject.GetComponent<SphereCollider>().radius * 2f);

            WanderAngle += UnityEngine.Random.Range(0f, 50f);

            Vector3 displacement = Vector3.right;
            displacement *= this.gameObject.GetComponent<SphereCollider>().radius * 1.5f;

            float len = displacement.magnitude;
            float sin = Mathf.Sin(WanderAngle) * len;
            float cos = Mathf.Cos(WanderAngle) * len;

            float tx = displacement.x;
            float tz = displacement.z;
            displacement.x = (cos * tx) + (sin * tz);
            displacement.z = (cos * tz) - (sin * tx);


            Vector3 wanderforce = circleCenter + displacement;

            IApplyForces(wanderforce);











        }
        #endregion

        #region CHANGE AGENT STATES



        /// <summary>
        /// Changes state of agent to Signal Receiver Passive. Total displacement value is used when color display is set to visualize
        /// displacement values, else input just 0 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalDisplacement"></param>
        public void ChangeStateToDeactivatedPassive(Agent data, float totalDisplacement)
        {

            data.gameObject.tag = "De-ActivatedPassive";



        }




        /// <summary>
        /// Changes state of agent to Signal Receiver Passive. Total displacement value is used when color display is set to visualize
        /// displacement values, else input just 0 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalDisplacement"></param>
        public void ChangeStateToDeactivated(Agent data, float totalDisplacement)
        {
            if(SimulationManager.Get().runSimulation==false) data.GetComponent<MeshRenderer>().enabled = false;
            data.gameObject.tag = "De-Activated";


            //if (SimulationManager.Get().displayColorbyEnergy)
            //{


            //    if (GenerationManager.generationCount != 0)
            //    {

            //        data.GetComponent<MeshRenderer>().material.color = EnergyColor(SimulationManager.Get().startColorEnergy, SimulationManager.Get().endColorEnergy, energyLevel);
            //    }

            //    else
            //        data.GetComponent<MeshRenderer>().material.color = Color.black;
            //}

            //if (SimulationManager.Get().displayColorbyDisplacement)
            //{

            //    data.GetComponent<MeshRenderer>().material.color = DisplacementColor(SimulationManager.Get().startColorDisplacement, SimulationManager.Get().endColorDisplacement, totalDisplacement);
            //}


            //if (SimulationManager.Get().displayColorByState)
            //{

            //    data.GetComponent<MeshRenderer>().material.color = Color.black;
            //}

            //if (SimulationManager.Get().GPUInstancing)
            //{
            //    float r = SimulationManager.Get().monochromeColor.r;
            //    float g = SimulationManager.Get().monochromeColor.g;
            //    float b = SimulationManager.Get().monochromeColor.b;
            //    MaterialPropertyBlock properties = new MaterialPropertyBlock();


            //    properties.SetColor("_Color", new Color(r, g, b));


            //    MeshRenderer meshR = data.GetComponent<MeshRenderer>();
            //    if (meshR)
            //    {
            //        meshR.SetPropertyBlock(properties);
            //    }
            //}

            //if (SimulationManager.Get().displayColorByNeighbours)
            //{

            //    if (GenerationManager.generationCount != 0)
            //    {

            //        data.GetComponent<MeshRenderer>().material.color = ConnectivityColor(SimulationManager.Get().startColorNeighbours, SimulationManager.Get().endColorNeighbours, neighbours.Count);
            //    }

            //    else
            //        data.GetComponent<MeshRenderer>().material.color = Color.black;

            //}


        }





        /// <summary>
        /// Changes state of agent to Signal Receiver Passive. Total displacement value is used when color display is set to visualize
        /// displacement values, else input just 0 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalDisplacement"></param>
        /// 

        public void ChangeStateToFreezed(Agent data, float totalDisplacement)
        {
            data.gameObject.tag = "Freezed";


            //if (SimulationManager.Get().displayColorbyEnergy)
            //{
            //    data.GetComponent<MeshRenderer>().material.color = EnergyColor(SimulationManager.Get().startColorEnergy, SimulationManager.Get().endColorEnergy, energyLevel);
            //}

            //if (SimulationManager.Get().displayColorbyDisplacement)
            //{
            //    data.GetComponent<MeshRenderer>().material.color = DisplacementColor(SimulationManager.Get().startColorDisplacement, SimulationManager.Get().endColorDisplacement, totalDisplacement);
            //}


            //if (SimulationManager.Get().displayColorByState)
            //{

            //    data.GetComponent<MeshRenderer>().material.color = Color.grey;
            //}

            //if (SimulationManager.Get().GPUInstancing)
            //{
            //    float r = SimulationManager.Get().monochromeColor.r;
            //    float g = SimulationManager.Get().monochromeColor.g;
            //    float b = SimulationManager.Get().monochromeColor.b;
            //    MaterialPropertyBlock properties = new MaterialPropertyBlock();


            //    properties.SetColor("_Color", new Color(r, g, b));


            //    MeshRenderer meshR = data.GetComponent<MeshRenderer>();
            //    if (meshR)
            //    {
            //        meshR.SetPropertyBlock(properties);
            //    }
            //}


            //if (SimulationManager.Get().displayColorByNeighbours)
            //{

            //    data.GetComponent<MeshRenderer>().material.color = ConnectivityColor(SimulationManager.Get().startColorNeighbours, SimulationManager.Get().endColorNeighbours, neighbours.Count);
            //}
            signalReceiverPassiveCounter = 0;



        }




        /// <summary>
        /// This method changes the state of the agent emmiter to freezed based on neighbour count and states
        /// </summary>
        public virtual void ChangeStateToFreezed_Continuous()
        {
            if (this.gameObject.tag == "SignalEmmiter")
            {

                /// if all  its neighbours are signal emmiters, change own state to freezed
                int count = 0;

                int deActivatedPlaceHolderCount = 0;

                if (mySignalReceivers.Count != 0)
                {

                    for (int i = 0; i < neighbours.Count; i++)
                    {
                        if (neighbours[i] != null)
                        {
                            for (int j = 0; j < mySignalReceivers.Count; j++)
                            {
                                if (neighbours[i].gameObject.tag == "SignalEmmiter" && mySignalReceivers[j].name == neighbours[i].name)
                                {
                                    count++;
                                }
                            }
                        }
                    }


                    if (count == mySignalReceivers.Count)
                    {

                        ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);
                    }




                }

                /// If all my placeHolders are deactivated, change my state to freezed;
                if(placeHolders.Count!=0)
                {
                    int totalPlaceHolders = placeHolders.Count;
                    for (int i = 0; i < placeHolders.Count; i++)
                    {
                        if (placeHolders[i] != null)
                        {
                            if (placeHolders[i].tag == "DeActivatedPlaceHolder") deActivatedPlaceHolderCount++;
                        }
                    }

                    if(totalPlaceHolders == deActivatedPlaceHolderCount) ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);
                }



                /// THIS PART WAS COMMENTED OUR BECAUSE ITS WAS PRODUCING THE LAST signal receiver to be cancelled due to the fact that its signal emmiter 
                /// was changing its state to freezed. The signal emmiter was changing its state to freezed because 


                //if (AgentPopulation.deActivatedAgentList.Count == 0 && mySignalReceivers.Count == 0)
                //{
                //    ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);
                //}



                /// THIS VERSION IS SUPPOSED TO NOT HAVE ANY CANCELLED AGENTS IN THE FIRST GENERATION
                if (AgentPopulation.deActivatedAgentList.Count == 0 && AgentPopulation.signalReceiverAgentList.Count==0 &&  mySignalReceiversTemp.Count == 0)
                {
                    ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);
                }





                /// change state to freezed When there are no more signal receivers or deactivated agents in simulation
                //if (AgentPopulation.deActivatedAgentList.Count == 0 && AgentPopulation.signalReceiverAgentList.Count == 0 && mySignalReceivers.Count != 0)
                //{
                //    ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);
                //}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
///DOUBLE CHECK IF THIS CODE WILL AFFECT RE-ORGANIZATION. OR WHEN WIND IS ON.... PERHAPS IT NEEDS TO BE ON EACH CLASS SEPARELTY.... I ADDED THIS ON DECEMBER 28,2018

                int freezedReceivers = 0;
                for (int i = 0; i < mySignalReceivers.Count; i++)
                {
                    if(mySignalReceivers[i]!=null)
                    {
                        if (mySignalReceivers[i].gameObject.tag == "Freezed") freezedReceivers++;
                    }
                }

                if(AgentPopulation.deActivatedAgentList.Count == 0 && AgentPopulation.signalReceiverAgentList.Count == 0 && freezedReceivers>=1)
                {
                    ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);
                }


                ////////////////////////////////////////////////////////////////////////////////////////////////////////////





            }



        } // END METHOD






        /// <summary>
        /// Changes state of agent to Signal Receiver Passive. Total displacement value is used when color display is set to visualize
        /// displacement values, else input just 0 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalDisplacement"></param>
        /// 
        public virtual void ChangeStateToSignalEmmiter(Agent data, float totalDisplacement)
        {
            data.gameObject.tag = "SignalEmmiter";

            if(GenerationManager.generationCount>0 && GenerationManager.activatedAgentPerGeneration.Length==0)
            {
                GenerationManager.activatedAgentPerGeneration[0] = data;
            }

            //if (SimulationManager.Get().displayColorbyEnergy)
            //{
            //    data.GetComponent<MeshRenderer>().material.color = EnergyColor(SimulationManager.Get().startColorEnergy, SimulationManager.Get().endColorEnergy, energyLevel);
            //}

            //if (SimulationManager.Get().displayColorbyDisplacement)
            //{
            //    data.GetComponent<MeshRenderer>().material.color = DisplacementColor(SimulationManager.Get().startColorDisplacement, SimulationManager.Get().endColorDisplacement, totalDisplacement);
            //}


            //if (SimulationManager.Get().displayColorByState)
            //{

            //    data.GetComponent<MeshRenderer>().material.color = Color.blue;
            //}

            //if (SimulationManager.Get().GPUInstancing)
            //{
            //    float r = SimulationManager.Get().monochromeColor.r;
            //    float g = SimulationManager.Get().monochromeColor.g;
            //    float b = SimulationManager.Get().monochromeColor.b;
            //    MaterialPropertyBlock properties = new MaterialPropertyBlock();


            //    properties.SetColor("_Color", new Color(r, g, b));


            //    MeshRenderer meshR = data.GetComponent<MeshRenderer>();
            //    if (meshR)
            //    {
            //        meshR.SetPropertyBlock(properties);
            //    }
            //}

            //if (SimulationManager.Get().displayColorByNeighbours)
            //{

            //    data.GetComponent<MeshRenderer>().material.color = ConnectivityColor(SimulationManager.Get().startColorNeighbours, SimulationManager.Get().endColorNeighbours, neighbours.Count);
            //}



            //if (communicationType < 2 && SimulationManager.Get().displayColorByComunication)
            //{
            //    gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorLine;
            //}

            //if (communicationType >= 2 && communicationType < 12 && SimulationManager.Get().displayColorByComunication)
            //{
            //    gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorBranch;
            //}

            //if (communicationType == 12 && SimulationManager.Get().displayColorByComunication)
            //{
            //    gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorStar;
            //}


        }




        /// <summary>
        /// This method changes the states of the singal emmiters neigbours from passive to emmiters.
        /// </summary>
        public void ChangeStateToSignalEmmiter_Continuous()
        {


            if (this.gameObject.tag == "SignalEmmiter")
            {
                // get all neighbours which are signal receiver passive
                int SignalReceiverCount = 0;
                if (mySignalReceivers.Count != 0)
                {
                    for (int i = 0; i < neighbours.Count; i++)
                    {
                        if (neighbours[i] != null)
                        {
                            if (neighbours[i].gameObject.tag == "SignalReceiverPassive" )
                            {
                                SignalReceiverCount++;
                            }
                        }
                    }
                }

                /// Only change states of the neighbours that are Passive Signal Receivers to Signal Emmiters
                if (SignalReceiverCount == mySignalReceivers.Count)
                {
                    for (int i = 0; i < neighbours.Count; i++)
                    {
                        if (neighbours[i] != null)
                        {

                            if (neighbours[i].tag == "SignalReceiverPassive" )
                            {
                                ChangeStateToSignalEmmiter(neighbours[i].GetComponent<Agent>(), 0);
                            }
                        }
                        
                    }

                }

            }


        }







        /// <summary>
        /// Changes state of agent to Signal Receiver Passive. Total displacement value is used when color display is set to visualize
        /// displacement values, else input just 0 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalDisplacement"></param>

        public void ChangeStateToSignalReceiver(Agent data, float totalDisplacement)
        {


            data.gameObject.tag = "SignalReceiver";


            //if (SimulationManager.Get().displayColorbyEnergy)
            //{
            //    data.GetComponent<MeshRenderer>().material.color = EnergyColor(SimulationManager.Get().startColorEnergy, SimulationManager.Get().endColorEnergy, energyLevel);
            //}

            //if (SimulationManager.Get().displayColorbyDisplacement)
            //{
            //    data.GetComponent<MeshRenderer>().material.color = DisplacementColor(SimulationManager.Get().startColorDisplacement, SimulationManager.Get().endColorDisplacement, totalDisplacement);
            //}


            //if (SimulationManager.Get().displayColorByState)
            //{
            //    Color color = (Utility.RGBToFloat(54, 141, 255));
            //    data.GetComponent<MeshRenderer>().material.color = color;
            //}

            //if (SimulationManager.Get().GPUInstancing)
            //{
            //    float r = SimulationManager.Get().monochromeColor.r;
            //    float g = SimulationManager.Get().monochromeColor.g;
            //    float b = SimulationManager.Get().monochromeColor.b;
            //    MaterialPropertyBlock properties = new MaterialPropertyBlock();


            //    properties.SetColor("_Color", new Color(r, g, b));


            //    MeshRenderer meshR = data.GetComponent<MeshRenderer>();
            //    if (meshR)
            //    {
            //        meshR.SetPropertyBlock(properties);
            //    }
            //}


            //if (SimulationManager.Get().displayColorByNeighbours)
            //{

            //    data.GetComponent<MeshRenderer>().material.color = ConnectivityColor(SimulationManager.Get().startColorNeighbours, SimulationManager.Get().endColorNeighbours, neighbours.Count);
            //}




        }



        /// <summary>
        /// Changes state of agent to Signal Receiver Passive. Total displacement value is used when color display is set to visualize
        /// displacement values, else input just 0 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalDisplacement"></param>
        public void ChangeStateToSignalReceiverPassive(Agent data, float totalDisplacement)
        {

            data.gameObject.tag = "SignalReceiverPassive";



            //if (SimulationManager.Get().displayColorbyEnergy)
            //{
            //    data.GetComponent<MeshRenderer>().material.color = EnergyColor(SimulationManager.Get().startColorEnergy, SimulationManager.Get().endColorEnergy, energyLevel);
            //}

            //if (SimulationManager.Get().displayColorbyDisplacement)
            //{

            //    data.GetComponent<MeshRenderer>().material.color = DisplacementColor(SimulationManager.Get().startColorDisplacement, SimulationManager.Get().endColorDisplacement, totalDisplacement);
            //}


            //if (SimulationManager.Get().displayColorByState)
            //{
            //    //Color color = (Utility.RGBToFloat(54, 141, 255));
            //    //data.GetComponent<MeshRenderer>().material.color = Color.red;

            //    Color color = (Utility.RGBToFloat(54, 141, 255));
            //    data.GetComponent<MeshRenderer>().material.color = color;
            //}

            //if (SimulationManager.Get().GPUInstancing)
            //{
            //    float r = SimulationManager.Get().monochromeColor.r;
            //    float g = SimulationManager.Get().monochromeColor.g;
            //    float b = SimulationManager.Get().monochromeColor.b;
            //    MaterialPropertyBlock properties = new MaterialPropertyBlock();


            //    properties.SetColor("_Color", new Color(r, g, b));


            //    MeshRenderer meshR = data.GetComponent<MeshRenderer>();
            //    if (meshR)
            //    {
            //        meshR.SetPropertyBlock(properties);
            //    }
            //}


            //if (SimulationManager.Get().displayColorByNeighbours)
            //{

            //    data.GetComponent<MeshRenderer>().material.color = ConnectivityColor(SimulationManager.Get().startColorNeighbours, SimulationManager.Get().endColorNeighbours, neighbours.Count);
            //}



        }








        public void ChangeStateToWanderer(Agent data, float totalDisplacement)
        {

            data.gameObject.tag = "Wanderer";



        }



        #endregion

        #region CHANGE AGENT DISPLAY BASED ON INSPECTOR INPUT

        /// <summary>
        /// This method changes the display of the agents thorugh the inspector
        /// </summary>
        /// <param name="data"></param> current agent to modify
        /// <param name="displacement"></param> toggle true or false if displacement graphics will be shown
        /// <param name="deltaValue"></param> displacement value
        public void ChangeDisplayInspectorInput(Agent data, bool displacement, float deltaValue = 0)
        {
            float _deltaValue = 0;
           
            if (displacement == true) _deltaValue = deltaValue;
            if (displacement == false) _deltaValue = 0;





            if (data.tag == "De-Activated" && SimulationManager.Get().runSimulation == false) data.GetComponent<MeshRenderer>().enabled = false;

            if (data.tag == "De-Activated")
            {

                if (SimulationManager.Get().displayTopology == false) data.GetComponent<MeshRenderer>().enabled = true;

                if (SimulationManager.Get().GPUInstancing == false)
                {
                    MaterialPropertyBlock properties = new MaterialPropertyBlock();
                    properties.Clear();
                    data.GetComponent<MeshRenderer>().SetPropertyBlock(properties);
                }




                if (SimulationManager.Get().displayColorbyEnergy)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    //data.GetComponent<MeshRenderer>().material.color = EnergyColor(SimulationManager.Get().startColorEnergy, SimulationManager.Get().endColorEnergy, energyLevel * 2);
                    // data.GetComponent<MeshRenderer>().material.color = EnergyColor(SimulationManager.Get().startColorEnergy, SimulationManager.Get().endColorEnergy, SharpMath.Normalize(energyLevel, 0, 1) * 50);
                    //data.GetComponent<MeshRenderer>().material.color = gradient.Evaluate(SharpMath.Remap(energyLevel, 0, 650, 0, 1));

                  
                    if (GenerationManager.generationCount != 0)
                    {

                        data.GetComponent<MeshRenderer>().material.color = energyGradientColor.Evaluate(SharpMath.Remap(energyLevel, 0, 650, 0, 1));
                    }

                    else
                        data.GetComponent<MeshRenderer>().material.color = Color.black;

                }



                if (SimulationManager.Get().displayColorbyDisplacement)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    data.GetComponent<MeshRenderer>().material.color = DisplacementColor(SimulationManager.Get().startColorDisplacement, SimulationManager.Get().endColorDisplacement, _deltaValue * 10);
                }

                if (SimulationManager.Get().displayColorByDistanceToNeighbours)

                {
                    data.GetComponent<MeshRenderer>().enabled = true;

                    data.GetComponent<MeshRenderer>().material.color = Color.Lerp(SimulationManager.Get().packProxStart, SimulationManager.Get().packProxEnd, currentScalarValuePackingProximity * 3);


                }

                if (SimulationManager.Get().GPUInstancing)
                {
                    MaterialPropertyBlock properties = new MaterialPropertyBlock();
                    MeshRenderer meshR = data.GetComponent<MeshRenderer>();
                    meshR.enabled = true;
                    float r = SimulationManager.Get().monochromeColor.r;
                    float g = SimulationManager.Get().monochromeColor.g;
                    float b = SimulationManager.Get().monochromeColor.b;

                    properties.SetColor("_Color", new Color(r, g, b));
                    if (meshR) meshR.SetPropertyBlock(properties);


                }

                if (SimulationManager.Get().displayColorByNeighbours)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    if (GenerationManager.generationCount != 0)
                    {

                        data.GetComponent<MeshRenderer>().material.color = ConnectivityColor(SimulationManager.Get().startColorNeighbours, SimulationManager.Get().endColorNeighbours, neighbours.Count);
                    }

                    else
                        data.GetComponent<MeshRenderer>().material.color = Color.black;

                }


                if (SimulationManager.Get().displayColorByComunication)
                    data.GetComponent<MeshRenderer>().enabled = true;
                {
                    if (GenerationManager.generationCount != 0)
                    {
                        if (communicationType < 2 && SimulationManager.Get().displayColorByComunication)
                        {
                            gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorLine;
                        }

                        if (communicationType >= 2 && communicationType < 6 && SimulationManager.Get().displayColorByComunication)
                        {
                            gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorBranch;
                        }

                        if (communicationType >= 6 && communicationType < 8 && SimulationManager.Get().displayColorByComunication)
                        {
                            gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorStar;
                        }

                        if (communicationType >= 8 && communicationType <= 12 && SimulationManager.Get().displayColorByComunication)
                        {
                            gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorCore;
                        }
                    }

                    else
                        data.GetComponent<MeshRenderer>().material.color = Color.black;

                }



                if (SimulationManager.Get().displayColorByRule)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;

          
                    data.GetComponent<MeshRenderer>().material.color = ruleSetColor.Evaluate(SharpMath.Remap(currentScalarValueRules, minVal, maxVal, 0, 1));

                }


                if (SimulationManager.Get().displayTopology)
                {
                    //  if (GenerationManager.generationCount != 0)
                    //  {
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                    //  }
                    //  else
                    //     data.GetComponent<MeshRenderer>().material.color = Color.black;
                }

                if (SimulationManager.Get().displayColorByState)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    data.GetComponent<MeshRenderer>().material.color = Color.black;
                }

            } // END DEACTIVATED AGENT CONDITIONS


            if (data.tag == "Freezed")
            {
               
                if (SimulationManager.Get().displayTopology == false) data.GetComponent<MeshRenderer>().enabled = true;

                if (SimulationManager.Get().GPUInstancing == false)
                {
                    MaterialPropertyBlock properties = new MaterialPropertyBlock();
                    properties.Clear();
                    data.GetComponent<MeshRenderer>().SetPropertyBlock(properties);
                }



                if (SimulationManager.Get().displayColorbyEnergy)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    //data.GetComponent<MeshRenderer>().material.color = EnergyColor(SimulationManager.Get().startColorEnergy, SimulationManager.Get().endColorEnergy, energyLevel * 2);
                    // data.GetComponent<MeshRenderer>().material.color = EnergyColor(SimulationManager.Get().startColorEnergy, SimulationManager.Get().endColorEnergy, SharpMath.Normalize(energyLevel, 0, 1) * 50);
                    data.GetComponent<MeshRenderer>().material.color = energyGradientColor.Evaluate(SharpMath.Remap(energyLevel, 0, 650, 0, 1));

                }



                if (SimulationManager.Get().displayColorbyDisplacement)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    data.GetComponent<MeshRenderer>().material.color = DisplacementColor(SimulationManager.Get().startColorDisplacement, SimulationManager.Get().endColorDisplacement, _deltaValue * 10);
                }

                if (SimulationManager.Get().displayColorByDistanceToNeighbours)

                {
                    data.GetComponent<MeshRenderer>().enabled = true;
  
                    data.GetComponent<MeshRenderer>().material.color = Color.Lerp(SimulationManager.Get().packProxStart, SimulationManager.Get().packProxEnd, currentScalarValuePackingProximity*3);

             
                }


                if (SimulationManager.Get().GPUInstancing)
                {
                    MaterialPropertyBlock properties = new MaterialPropertyBlock();
                    MeshRenderer meshR = data.GetComponent<MeshRenderer>();
                    meshR.enabled = true;
                    float r = SimulationManager.Get().monochromeColor.r;
                    float g = SimulationManager.Get().monochromeColor.g;
                    float b = SimulationManager.Get().monochromeColor.b;

                    properties.SetColor("_Color", new Color(r, g, b));
                    if (meshR) meshR.SetPropertyBlock(properties);

                }

                if (SimulationManager.Get().displayColorByNeighbours)
                {
                   Material  emissiveMaterial = gameObject.GetComponent<MeshRenderer>().material;


                    Color c = ConnectivityColor(SimulationManager.Get().startColorNeighbours, SimulationManager.Get().endColorNeighbours, neighbours.Count);


                   // Color c = Color.Lerp(Color.black, Color.white, neighbours.Count*.1f);

                    data.GetComponent<MeshRenderer>().enabled = true;
                    data.GetComponent<MeshRenderer>().material.color = c;


                    
                    //emissiveMaterial.SetColor("_Emission", c);

                    //DynamicGI.SetEmissive(data.GetComponent<MeshRenderer>(), c );

                    //DynamicGI.UpdateEnvironment();
                    //DynamicGI.UpdateMaterials(data.GetComponent<MeshRenderer>());

                    // RendererExtensions.UpdateGIMaterials(data.GetComponent<MeshRenderer>());

                    //GameObject light = GameObject.Find("Directional Light") ;

                    //light.SetActive(false);

                }



                if (SimulationManager.Get().displayColorByComunication)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;

                    if (communicationType < 2 && SimulationManager.Get().displayColorByComunication)
                    {
                        gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorLine;
                    }

                    if (communicationType >= 2 && communicationType < 6 && SimulationManager.Get().displayColorByComunication)
                    {
                        gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorBranch;
                    }

                    if (communicationType >= 6 && communicationType < 8 && SimulationManager.Get().displayColorByComunication)
                    {
                        gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorStar;
                    }

                    if (communicationType >= 8 && communicationType <= 12 && SimulationManager.Get().displayColorByComunication)
                    {
                        gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorCore;
                    }

                }

                if (SimulationManager.Get().displayColorByEmotion)
                {
                  

                    data.GetComponent<MeshRenderer>().enabled = true;


   
                }

                if (SimulationManager.Get().displayTopology)
                {

                    gameObject.GetComponent<MeshRenderer>().enabled = false;


                }

                if (SimulationManager.Get().displayColorByRule)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;


                    data.GetComponent<MeshRenderer>().material.color = ruleSetColor.Evaluate(SharpMath.Remap(currentScalarValueRules, minVal, maxVal, 0, 1));

                }




                if (SimulationManager.Get().displayColorByState)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    data.GetComponent<MeshRenderer>().material.color = Color.grey;
                }

              

            } // END FREEZED AGENT CONDITIONS

            if (data.tag == "SignalEmmiter")
            {

                if (SimulationManager.Get().displayTopology == false) data.GetComponent<MeshRenderer>().enabled = true;

                if (SimulationManager.Get().GPUInstancing == false)
                {
                    MaterialPropertyBlock properties = new MaterialPropertyBlock();
                    properties.Clear();
                    data.GetComponent<MeshRenderer>().SetPropertyBlock(properties);
                }


                if (SimulationManager.Get().displayColorbyEnergy)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    //data.GetComponent<MeshRenderer>().material.color = EnergyColor(SimulationManager.Get().startColorEnergy, SimulationManager.Get().endColorEnergy, energyLevel * 2);
                    // data.GetComponent<MeshRenderer>().material.color = EnergyColor(SimulationManager.Get().startColorEnergy, SimulationManager.Get().endColorEnergy, SharpMath.Normalize(energyLevel, 0, 1) * 50);
                    data.GetComponent<MeshRenderer>().material.color = energyGradientColor.Evaluate(SharpMath.Remap(energyLevel, 0, 650, 0, 1));

                }




                if (SimulationManager.Get().displayColorbyDisplacement)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    data.GetComponent<MeshRenderer>().material.color = DisplacementColor(SimulationManager.Get().startColorDisplacement, SimulationManager.Get().endColorDisplacement, _deltaValue * 10);
                }

                if (SimulationManager.Get().displayColorByDistanceToNeighbours)

                {
                    data.GetComponent<MeshRenderer>().enabled = true;

                    data.GetComponent<MeshRenderer>().material.color = Color.Lerp(SimulationManager.Get().packProxStart, SimulationManager.Get().packProxEnd, currentScalarValuePackingProximity * 3);


                }



                if (SimulationManager.Get().GPUInstancing)
                {
                    MaterialPropertyBlock properties = new MaterialPropertyBlock();
                    MeshRenderer meshR = data.GetComponent<MeshRenderer>();
                    meshR.enabled = true;
                    float r = SimulationManager.Get().monochromeColor.r;
                    float g = SimulationManager.Get().monochromeColor.g;
                    float b = SimulationManager.Get().monochromeColor.b;

                    properties.SetColor("_Color", new Color(r, g, b));
                    if (meshR) meshR.SetPropertyBlock(properties);

                }

                if (SimulationManager.Get().displayColorByNeighbours)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    data.GetComponent<MeshRenderer>().material.color = ConnectivityColor(SimulationManager.Get().startColorNeighbours, SimulationManager.Get().endColorNeighbours, neighbours.Count);
                }

                if (SimulationManager.Get().displayColorByComunication)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;

                    if (communicationType < 2 && SimulationManager.Get().displayColorByComunication)
                    {
                        gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorLine;
                    }

                    if (communicationType >= 2 && communicationType < 6 && SimulationManager.Get().displayColorByComunication)
                    {
                        gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorBranch;
                    }

                    if (communicationType >= 6 && communicationType < 8 && SimulationManager.Get().displayColorByComunication)
                    {
                        gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorStar;
                    }

                    if (communicationType >= 8 && communicationType <= 12 && SimulationManager.Get().displayColorByComunication)
                    {
                        gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorCore;
                    }

                }

                if (SimulationManager.Get().displayColorByRule)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;



                    data.GetComponent<MeshRenderer>().material.color = ruleSetColor.Evaluate(SharpMath.Remap(currentScalarValueRules, minVal, maxVal, 0, 1));

                }


                if (SimulationManager.Get().displayTopology)
                {

                    gameObject.GetComponent<MeshRenderer>().enabled = false;


                }


                if (SimulationManager.Get().displayColorByState)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    data.GetComponent<MeshRenderer>().material.color = Color.blue;
                }


            } // END SIGNAL EMMITER AGENT CONDITIONS


            if (data.tag == "SignalReceiver")
            {

                Color color = (Utility.RGBToFloat(54, 141, 255));
                if (SimulationManager.Get().displayTopology == false) data.GetComponent<MeshRenderer>().enabled = true;

                if (SimulationManager.Get().GPUInstancing == false)
                {
                    MaterialPropertyBlock properties = new MaterialPropertyBlock();
                    properties.Clear();
                    data.GetComponent<MeshRenderer>().SetPropertyBlock(properties);
                }



                if (SimulationManager.Get().displayColorbyEnergy)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    //data.GetComponent<MeshRenderer>().material.color = EnergyColor(SimulationManager.Get().startColorEnergy, SimulationManager.Get().endColorEnergy, energyLevel * 2);
                    // data.GetComponent<MeshRenderer>().material.color = EnergyColor(SimulationManager.Get().startColorEnergy, SimulationManager.Get().endColorEnergy, SharpMath.Normalize(energyLevel, 0, 1) * 50);
                    data.GetComponent<MeshRenderer>().material.color = energyGradientColor.Evaluate(SharpMath.Remap(energyLevel, 0, 650, 0, 1));

                }




                if (SimulationManager.Get().displayColorbyDisplacement)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    data.GetComponent<MeshRenderer>().material.color = DisplacementColor(SimulationManager.Get().startColorDisplacement, SimulationManager.Get().endColorDisplacement, _deltaValue * 10);
                }

                if (SimulationManager.Get().displayColorByDistanceToNeighbours)

                {
                    data.GetComponent<MeshRenderer>().enabled = true;

                    data.GetComponent<MeshRenderer>().material.color = Color.Lerp(SimulationManager.Get().packProxStart, SimulationManager.Get().packProxEnd, currentScalarValuePackingProximity * 3);


                }



                if (SimulationManager.Get().GPUInstancing)
                {
                    MaterialPropertyBlock properties = new MaterialPropertyBlock();
                    MeshRenderer meshR = data.GetComponent<MeshRenderer>();
                    meshR.enabled = true;
                    float r = SimulationManager.Get().monochromeColor.r;
                    float g = SimulationManager.Get().monochromeColor.g;
                    float b = SimulationManager.Get().monochromeColor.b;

                    properties.SetColor("_Color", new Color(r, g, b));
                    if (meshR) meshR.SetPropertyBlock(properties);

                }

                if (SimulationManager.Get().displayColorByNeighbours)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    data.GetComponent<MeshRenderer>().material.color = ConnectivityColor(SimulationManager.Get().startColorNeighbours, SimulationManager.Get().endColorNeighbours, neighbours.Count);
                }

                if (SimulationManager.Get().displayColorByComunication)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    if (communicationType < 2 && SimulationManager.Get().displayColorByComunication)
                    {
                        gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorLine;
                    }

                    if (communicationType >= 2 && communicationType < 6 && SimulationManager.Get().displayColorByComunication)
                    {
                        gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorBranch;
                    }

                    if (communicationType >= 6 && communicationType < 8 && SimulationManager.Get().displayColorByComunication)
                    {
                        gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorStar;
                    }

                    if (communicationType >= 8 && communicationType <= 12 && SimulationManager.Get().displayColorByComunication)
                    {
                        gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorCore;
                    }
                }


                if (SimulationManager.Get().displayTopology)
                {

                    gameObject.GetComponent<MeshRenderer>().enabled = false;


                }

                if (SimulationManager.Get().displayColorByRule)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;


                    data.GetComponent<MeshRenderer>().material.color = ruleSetColor.Evaluate(SharpMath.Remap(currentScalarValueRules, minVal, maxVal, 0, 1));

                }



                if (SimulationManager.Get().displayColorByState)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    data.GetComponent<MeshRenderer>().material.color = color;
                }



            } // END SIGNAL RECEIVER AGENT CONDITIONS

            if (data.tag == "SignalReceiverPassive")
            {

                Color color = (Utility.RGBToFloat(54, 141, 255));
                if (SimulationManager.Get().displayTopology == false) data.GetComponent<MeshRenderer>().enabled = true;

                if (SimulationManager.Get().GPUInstancing == false)
                {
                    MaterialPropertyBlock properties = new MaterialPropertyBlock();
                    properties.Clear();
                    data.GetComponent<MeshRenderer>().SetPropertyBlock(properties);
                }



                if (SimulationManager.Get().displayColorbyEnergy)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    //data.GetComponent<MeshRenderer>().material.color = EnergyColor(SimulationManager.Get().startColorEnergy, SimulationManager.Get().endColorEnergy, energyLevel * 2);
                    // data.GetComponent<MeshRenderer>().material.color = EnergyColor(SimulationManager.Get().startColorEnergy, SimulationManager.Get().endColorEnergy, SharpMath.Normalize(energyLevel, 0, 1) * 50);
                    data.GetComponent<MeshRenderer>().material.color = energyGradientColor.Evaluate(SharpMath.Remap(energyLevel, 0, 650, 0, 1));

                }




                if (SimulationManager.Get().displayColorbyDisplacement)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    data.GetComponent<MeshRenderer>().material.color = DisplacementColor(SimulationManager.Get().startColorDisplacement, SimulationManager.Get().endColorDisplacement, _deltaValue * 10);
                }
                if (SimulationManager.Get().displayColorByDistanceToNeighbours)

                {
                    data.GetComponent<MeshRenderer>().enabled = true;

                    data.GetComponent<MeshRenderer>().material.color = Color.Lerp(SimulationManager.Get().packProxStart, SimulationManager.Get().packProxEnd, currentScalarValuePackingProximity * 3);


                }


                if (SimulationManager.Get().GPUInstancing)
                {
                    MaterialPropertyBlock properties = new MaterialPropertyBlock();
                    MeshRenderer meshR = data.GetComponent<MeshRenderer>();
                    meshR.enabled = true;
                    float r = SimulationManager.Get().monochromeColor.r;
                    float g = SimulationManager.Get().monochromeColor.g;
                    float b = SimulationManager.Get().monochromeColor.b;

                    properties.SetColor("_Color", new Color(r, g, b));
                    if (meshR) meshR.SetPropertyBlock(properties);

                }

                if (SimulationManager.Get().displayColorByNeighbours)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    data.GetComponent<MeshRenderer>().material.color = ConnectivityColor(SimulationManager.Get().startColorNeighbours, SimulationManager.Get().endColorNeighbours, neighbours.Count);
                }

                if (SimulationManager.Get().displayColorByComunication)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;

                    if (communicationType < 2 && SimulationManager.Get().displayColorByComunication)
                    {
                        gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorLine;
                    }

                    if (communicationType >= 2 && communicationType < 6 && SimulationManager.Get().displayColorByComunication)
                    {
                        gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorBranch;
                    }

                    if (communicationType >= 6 && communicationType<8 && SimulationManager.Get().displayColorByComunication)
                    {
                        gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorStar;
                    }

                    if (communicationType >= 8 && communicationType <= 12 && SimulationManager.Get().displayColorByComunication)
                    {
                        gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorCore;
                    }

                }


                if (SimulationManager.Get().displayTopology)
                {

                    gameObject.GetComponent<MeshRenderer>().enabled = false;


                }


                if (SimulationManager.Get().displayColorByRule)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;



                    data.GetComponent<MeshRenderer>().material.color = ruleSetColor.Evaluate(SharpMath.Remap(currentScalarValueRules, minVal, maxVal, 0, 1));

                }



                if (SimulationManager.Get().displayColorByState)
                {
                    data.GetComponent<MeshRenderer>().enabled = true;
                    data.GetComponent<MeshRenderer>().material.color = color;
                }



            } // END SIGNAL RECEIVER PASSIVE AGENT CONDITIONS

            //if (data.tag == "Wanderer")
            //{
            //    Color color = (Utility.RGBToFloat(54, 141, 255));
            //    if (SimulationManager.Get().displayTopology == false) data.GetComponent<MeshRenderer>().enabled = true;

            //    if (SimulationManager.Get().GPUInstancing == false)
            //    {
            //        MaterialPropertyBlock properties = new MaterialPropertyBlock();
            //        properties.Clear();
            //        data.GetComponent<MeshRenderer>().SetPropertyBlock(properties);
            //    }



            //    if (SimulationManager.Get().displayColorbyEnergy)
            //    {
            //        data.GetComponent<MeshRenderer>().enabled = true;
            //        data.GetComponent<MeshRenderer>().material.color = EnergyColor(SimulationManager.Get().startColorEnergy, SimulationManager.Get().endColorEnergy, energyLevel * 2);
            //        // data.GetComponent<MeshRenderer>().material.color = EnergyColor(SimulationManager.Get().startColorEnergy, SimulationManager.Get().endColorEnergy, SharpMath.Normalize(energyLevel, 0, 1) * 50);
            //    }



            //    if (SimulationManager.Get().displayColorbyDisplacement)
            //    {
            //        data.GetComponent<MeshRenderer>().enabled = true;
            //        data.GetComponent<MeshRenderer>().material.color = DisplacementColor(SimulationManager.Get().startColorDisplacement, SimulationManager.Get().endColorDisplacement, _deltaValue * 10);
            //    }
            //    if (SimulationManager.Get().displayColorByDistanceToNeighbours)

            //    {
            //        data.GetComponent<MeshRenderer>().enabled = true;

            //        data.GetComponent<MeshRenderer>().material.color = Color.Lerp(SimulationManager.Get().packProxStart, SimulationManager.Get().packProxEnd, currentScalarValuePackingProximity * 3);


            //    }



            //    if (SimulationManager.Get().displayColorByState)
            //    {
            //        data.GetComponent<MeshRenderer>().enabled = true;
            //        data.GetComponent<MeshRenderer>().material.color = color;
            //    }


            //    if (SimulationManager.Get().GPUInstancing)
            //    {
            //        MaterialPropertyBlock properties = new MaterialPropertyBlock();
            //        MeshRenderer meshR = data.GetComponent<MeshRenderer>();
            //        meshR.enabled = true;
            //        float r = SimulationManager.Get().monochromeColor.r;
            //        float g = SimulationManager.Get().monochromeColor.g;
            //        float b = SimulationManager.Get().monochromeColor.b;

            //        properties.SetColor("_Color", new Color(r, g, b));
            //        if (meshR) meshR.SetPropertyBlock(properties);

            //    }

            //    if (SimulationManager.Get().displayColorByNeighbours)
            //    {
            //        data.GetComponent<MeshRenderer>().enabled = true;
            //        data.GetComponent<MeshRenderer>().material.color = ConnectivityColor(SimulationManager.Get().startColorNeighbours, SimulationManager.Get().endColorNeighbours, neighbours.Count);
            //    }

            //    if (SimulationManager.Get().displayColorByComunication)
            //    {
            //        data.GetComponent<MeshRenderer>().enabled = true;

            //        if (communicationType < 2 && SimulationManager.Get().displayColorByComunication)
            //        {
            //            gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorLine;
            //        }

            //        if (communicationType >= 2 && communicationType < 12 && SimulationManager.Get().displayColorByComunication)
            //        {
            //            gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorBranch;
            //        }

            //        if (communicationType == 12 && SimulationManager.Get().displayColorByComunication)
            //        {
            //            gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorStar;
            //        }

            //    }


            //    if (SimulationManager.Get().displayTopology)
            //    {

            //        gameObject.GetComponent<MeshRenderer>().enabled = false;




            //    }

            //}



        } // END METHOD

        #endregion


        #region ENERGY LEVEL RULES BASED ON NEIGHBOUR CODNITIONS

        /// <summary>
        /// Share energy with neighbours only if the energy level of the current agent is > the neighbour energy averege
        /// </summary>
        /// <param name="agentToSearchFrom"></param>

        public void EnergyLevelSharing(Agent agentToSearchFrom)
        {
            float currentEnergyLevel = agentToSearchFrom.energyLevel;
            List<float> neighbourEnergyList = new List<float>();

            float totalNeighbourEnergyLevels = neighbourEnergyList.Sum();

            float neighbourEnergyAverege = totalNeighbourEnergyLevels / neighbours.Count;

            bool shareEnergy = false;
            bool lifeSaver = false;

           
            for (int i = 0; i < neighbours.Count; i++)
            {
                if (neighbours[i] != null)
                {
                    neighbourEnergyList.Add(neighbours[i].energyLevel);
                }
            }

            if (currentEnergyLevel > neighbourEnergyAverege) shareEnergy = true;

            List<Agent> neighboursWithLowEnergy = new List<Agent>();
          
          
        
            if (shareEnergy)
            {
                /// share energy with neighbours
                for (int i = 0; i < neighbours.Count; i++)
                {
                    if (neighbours[i] != null)
                    {
                        neighbours[i].energyLevel += energyDecreasingFactor;

                        if (neighbours[i].energyLevel < 200 && agentToSearchFrom.energyLevel > energyCapacity * 0.6f )
                        {
                            lifeSaver = true;
                            neighbours[i].energyLevel += energyDecreasingFactor * 10;
                        }
                    }
                }

                /// Decrease energy of current agent by the energy decreasingFactor * num of neighbours
                agentToSearchFrom.energyLevel -= (energyDecreasingFactor * neighbours.Count);
                if(lifeSaver == true)
                {
                    agentToSearchFrom.energyLevel -= energyDecreasingFactor * 10;
                }
            }


        }


        public Color EnergyColor(Color startColor, Color endColor, float energyLevel)
        {

            return Color.Lerp(startColor, endColor, energyLevel*0.001f);

            //return Color.Lerp(startColor, endColor, energyLevel * 0.0009f);
          
            //return Color.Lerp(startColor, endColor, t);


        }

        public void CalculateEnergyLevels( bool gridDistribution_6Neighbours, bool _12Neighbours, bool is3D, bool is2D )
        {
            if (this.energyLevel < 0) this.energyLevel = 0;
            //if (this.energyLevel > this.energyCapacity ) this.energyLevel = energyCapacity;

            if (gridDistribution_6Neighbours)
            {
                int maxNeighbours;
                if (is2D)
                {
                    maxNeighbours = 4;

                    if (neighbours.Count == maxNeighbours)
                    {
                        energyLevel -= 0;
                    }

                    else if (neighbours.Count == 3)
                    {
                        energyLevel -= energyDecreasingFactor / 3;
                    }

                    else if (neighbours.Count == 2)
                    {
                        energyLevel -= energyDecreasingFactor / 2;
                    }

                    else if (neighbours.Count == 1)
                    {
                        energyLevel -= energyDecreasingFactor / 2;
                    }

                    else if (neighbours.Count == 0)
                    {
                        energyLevel -= energyDecreasingFactor / 1;
                    }




                }

                if (is3D)
                {
                    maxNeighbours = 6;

                    if (neighbours.Count == maxNeighbours)
                    {
                        energyLevel -= 0;
                    }

                    else if (neighbours.Count == 5)
                    {
                        energyLevel -= energyDecreasingFactor / 5;
                    }

                    else if (neighbours.Count == 4)
                    {
                        energyLevel -= energyDecreasingFactor / 4;
                    }

                    else if (neighbours.Count == 3)
                    {
                        energyLevel -= energyDecreasingFactor / 3;
                    }

                    else if (neighbours.Count == 2)
                    {
                        energyLevel -= energyDecreasingFactor / 2;
                    }

                    else if (neighbours.Count == 1)
                    {
                        energyLevel -= energyDecreasingFactor / 2;
                    }

                    else if (neighbours.Count == 0)
                    {
                        energyLevel -= energyDecreasingFactor / 1;
                    }

                }



            }



            else if (_12Neighbours)
            {



                int maxNeighbours;

                if (is2D)
                {

                    maxNeighbours = 6;

                    if (neighbours.Count == maxNeighbours)
                    {
                        energyLevel -= 0;
                    }

                    else if (neighbours.Count == 5)
                    {
                        energyLevel -= energyDecreasingFactor / 5;
                    }

                    else if (neighbours.Count == 4)
                    {
                        energyLevel -= energyDecreasingFactor / 4;
                    }

                    else if (neighbours.Count == 3)
                    {
                        energyLevel -= energyDecreasingFactor / 3;
                    }

                    else if (neighbours.Count == 2)
                    {
                        energyLevel -= energyDecreasingFactor / 2;
                    }

                    else if (neighbours.Count == 1)
                    {
                        energyLevel -= energyDecreasingFactor / 2;
                    }

                    else if (neighbours.Count == 0)
                    {
                        energyLevel -= energyDecreasingFactor / 1;
                    }





                }

                if (is3D)
                {
                    maxNeighbours = 12;

                    if (neighbours.Count == maxNeighbours)
                    {
                        energyLevel -= 0;
                    }

                    else if (neighbours.Count == 11)
                    {
                        energyLevel -= energyDecreasingFactor / 11;
                    }

                    else if (neighbours.Count == 10)
                    {
                        energyLevel -= energyDecreasingFactor / 10;
                    }

                    else if (neighbours.Count == 9)
                    {
                        energyLevel -= energyDecreasingFactor / 9;
                    }

                    else if (neighbours.Count == 8)
                    {
                        energyLevel -= energyDecreasingFactor / 8;
                    }

                    else if (neighbours.Count == 7)
                    {
                        energyLevel -= energyDecreasingFactor / 7;
                    }

                    else if (neighbours.Count == 6)
                    {
                        energyLevel -= energyDecreasingFactor / 6;
                    }

                    else if (neighbours.Count == 5)
                    {
                        energyLevel -= energyDecreasingFactor / 5;
                    }

                    else if (neighbours.Count == 4)
                    {
                        energyLevel -= energyDecreasingFactor / 4;
                    }

                    else if (neighbours.Count == 3)
                    {
                        energyLevel -= energyDecreasingFactor / 3;
                    }

                    else if (neighbours.Count == 2)
                    {
                        energyLevel -= energyDecreasingFactor / 2;
                    }

                    else if (neighbours.Count == 1)
                    {
                        energyLevel -= energyDecreasingFactor / 2;
                    }

                    else if (neighbours.Count == 0)
                    {
                        energyLevel -= energyDecreasingFactor / 1;
                    }



                }







            }





        }




        #endregion







        /////////////////////////////SEARCH BEHAVIORS  //////////////////////////////////////////

        #region SEARCH BEHAVIORS
        public virtual KdTree<Agent> FindNeighboursKd(Agent agent, float visionRadius, KdTree<Agent> dataToSearch)
        {

            KdTree<Agent> neighbours = new KdTree<Agent>();

            foreach (Agent neighbour in dataToSearch)
            {
               
                if (neighbour != agent && Vector3.Distance(neighbour.transform.position, agent.transform.position) < visionRadius)
                {

                    neighbours.Add(neighbour);
                }

            }

            return neighbours;

        }


        public virtual List<Agent> FindNeighbours(Agent agent, float visionRadius, KdTree<Agent> dataToSearch)
        {

            List<Agent> neighbours = new List<Agent>();

            foreach (Agent neighbour in dataToSearch)
            {

                if (neighbour != agent && Vector3.Distance(neighbour.transform.position, agent.transform.position) < visionRadius)
                {

                    neighbours.Add(neighbour);
                }

            }

            return neighbours;

        }


        public virtual KdTree<Agent> FindNeighboursKDIgnoreName(Agent agent, float visionRadius, KdTree<Agent> dataToSearch, string nameToIgnore)
        {

            KdTree<Agent> neighbours = new KdTree<Agent>();

            foreach (Agent neighbour in dataToSearch)
            {

                if (neighbour != agent && neighbour.name != nameToIgnore && Vector3.Distance(neighbour.transform.position, agent.transform.position) < visionRadius)
                {

                    neighbours.Add(neighbour);
                }

            }

            return neighbours;

        }







        /// <summary>
        /// Seek a target
        /// </summary>
        /// <param name="source"></param> source object
        /// <param name="target"></param> target object
        public void Seek(Agent source, Agent target)
        {

            Vector3 desiredVelToClosestNeighbour = target.transform.position - source.transform.position;
            desiredVelToClosestNeighbour.Normalize();
            desiredVelToClosestNeighbour *= this.IMaxSpeed; ; //0.015f; this.IMaxSpeed

            Vector3 steer = desiredVelToClosestNeighbour - this.velocity;


            Vector3 steerLimit = steer.Limit(this.IMaxForce);



            IApplyForces(steerLimit);

        }



        public void Seek(Agent source, PlaceHolder target)
        {

            Vector3 desiredVelToClosestNeighbour = target.transform.position - source.transform.position;
            desiredVelToClosestNeighbour.Normalize();
            desiredVelToClosestNeighbour *= this.IMaxSpeed; 

            Vector3 steer = desiredVelToClosestNeighbour - this.velocity;


            Vector3 steerLimit = steer.Limit(this.IMaxForce);



            IApplyForces(steerLimit);

        }







        /// <summary>
        /// Seek the closest neighboir in a collection from an object to search from
        /// </summary>
        /// <param name="objectToSearchFrom"></param>
        /// <param name="targets"></param>
        public void SeekClosestNeighbour(Agent objectToSearchFrom, List<Agent> targets, out List<Agent> closestNeighbour)
        {
            List<Agent> data = new List<Agent>();
            Agent closestTarget = Utility.ClosestObject(objectToSearchFrom, targets);
            Vector3 desiredVelToClosestNeighbour = closestTarget.transform.position - objectToSearchFrom.transform.position;
            float desiredMagnitude = desiredVelToClosestNeighbour.magnitude;
            desiredVelToClosestNeighbour.Normalize();

            //if (desiredMagnitude < 100)  /// if we are closer than n pixels, set the magnitude of the desired velocity as a function of proximity
            //{


            //    float m = SharpMath.Remap(desiredMagnitude, 0, 100, 0, IMaxSpeed);

            //    desiredVelToClosestNeighbour *= m;
            //}
            //else
            //{
            //    desiredVelToClosestNeighbour *= IMaxSpeed;
            //}




            // desiredVelToClosestNeighbour *= this.IMaxSpeed;

            Vector3 steer = desiredVelToClosestNeighbour - this.velocity;

            Vector3 steerLimit = steer.Limit(this.IMaxForce);

            IApplyForces(steerLimit);

            data.Add(closestTarget);
            //closestNeighbour = closestTarget;
            closestNeighbour = data;



            //Vector3 desired = target.transform.position - source.gameObject.transform.position;
            //float desiredMagnitude = desired.magnitude;
            //desired.Normalize();


            //if (desiredMagnitude < arrivalRadius)  /// if we are closer than n pixels, set the magnitude of the desired velocity as a function of proximity
            //{


            //    float m = SharpMath.Remap(desiredMagnitude, 0, arrivalRadius, 0, maxSpeed);

            //    desired *= m;
            //}
            //else
            //{
            //    desired *= maxSpeed;
            //}


            //Vector3 steer = desired - velocity;

            //steer.Limit(maxForce);

            //IApplyForces(steer);


        }



        public void SeekClosestNeighbour(PlaceHolder objectToSearchFrom, List<Agent> targets, out List<Agent> closestNeighbour)
        {
            List<Agent> data = new List<Agent>();
            Agent closestTarget = Utility.ClosestObject(objectToSearchFrom, targets);
            Vector3 desiredVelToClosestNeighbour = closestTarget.transform.position - objectToSearchFrom.transform.position;
            desiredVelToClosestNeighbour.Normalize();

            desiredVelToClosestNeighbour *= this.IMaxSpeed;

            Vector3 steer = desiredVelToClosestNeighbour - this.velocity;

            Vector3 steerLimit = steer.Limit(this.IMaxForce);

            IApplyForces(steerLimit);

            data.Add(closestTarget);
            //closestNeighbour = closestTarget;
            closestNeighbour = data;


        }





        public void SeekClosestPixel(Agent objectToSearchFrom, Pixel [,,] pixels )
        {
            List<Pixel> pixelPopulationList = ConvertArrayToList(pixels);
         
            Pixel closestTarget = Utility.ClosestObject(objectToSearchFrom, pixelPopulationList);
            Vector3 desiredVelToClosestNeighbour = closestTarget.transform.position - objectToSearchFrom.transform.position;
            desiredVelToClosestNeighbour.Normalize();

            desiredVelToClosestNeighbour *= this.IMaxSpeed;

            Vector3 steer = desiredVelToClosestNeighbour - this.velocity;

            Vector3 steerLimit = steer.Limit(this.IMaxForce);

            IApplyForces(steerLimit);

           

        }


        private List<Pixel>ConvertArrayToList(Pixel [,,] data)
        {
            List<Pixel> returnList = new List<Pixel>();
            for (int i = 0; i < data.GetLength(1); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    for (int k = 0; k < data.GetLength(1); k++)
                    {
                        returnList.Add(data[i, j, k]);
                    }
                }
            }
            return returnList;
        }

        public void SeekClosestNeighbour(Agent objectToSearchFrom, List<Agent> targets)
        {

            Agent closestTarget = Utility.ClosestObject(objectToSearchFrom, targets);
            Vector3 desiredVelToClosestNeighbour = closestTarget.transform.position - objectToSearchFrom.transform.position;
            desiredVelToClosestNeighbour.Normalize();

            desiredVelToClosestNeighbour *= this.IMaxSpeed;

            Vector3 steer = desiredVelToClosestNeighbour - this.velocity;

            Vector3 steerLimit = steer.Limit(this.IMaxForce);

            IApplyForces(steerLimit);




        }



        public void SeekClosestNeighbour(PlaceHolder objectToSearchFrom, List<Agent> targets)
        {

            Agent closestTarget = Utility.ClosestObject(objectToSearchFrom, targets);
            Vector3 desiredVelToClosestNeighbour = closestTarget.transform.position - objectToSearchFrom.transform.position;
            desiredVelToClosestNeighbour.Normalize();

            desiredVelToClosestNeighbour *= this.IMaxSpeed;

            Vector3 steer = desiredVelToClosestNeighbour - this.velocity;

            Vector3 steerLimit = steer.Limit(this.IMaxForce);

            IApplyForces(steerLimit);




        }


        

        #endregion

        /////////////////////////////SEARCH BEHAVIORS END //////////////////////////////////////////

        /////////////////////////////STEERING BEHAVIORS //////////////////////////////////////////


        #region STEERING BEHAVIORS

       




        public void Arrive(Agent source, PlaceHolder target, float arrivalRadius, float maxSpeed, float maxForce)
        {
            Vector3 desired = target.transform.position - source.gameObject.transform.position;
            float desiredMagnitude = desired.magnitude;
            desired.Normalize();


            if (desiredMagnitude < arrivalRadius)  /// if we are closer than n pixels, set the magnitude of the desired velocity as a function of proximity
            {


                float m = SharpMath.Remap(desiredMagnitude, 0, arrivalRadius, 0, maxSpeed);

                desired *= m;
            }
            else
            {
                desired *= maxSpeed;
            }


            Vector3 steer = desired - velocity;

            steer.Limit(maxForce);

            IApplyForces(steer);

        }



        public void Arrive(Agent source, Agent target, float arrivalRadius, float maxSpeed, float maxForce)
        {
            Vector3 desired = target.transform.position - source.gameObject.transform.position;
            float desiredMagnitude = desired.magnitude;
            desired.Normalize();


            if (desiredMagnitude < arrivalRadius)  /// if we are closer than n pixels, set the magnitude of the desired velocity as a function of proximity
            {


                float m = SharpMath.Remap(desiredMagnitude, 0, arrivalRadius, 0, maxSpeed);

                desired *= m;
            }
            else
            {
                desired *= maxSpeed;
            }


            Vector3 steer = desired - velocity;

            steer.Limit(maxForce);

            IApplyForces(steer);

        }







        public List<Agent> AvoidNonTargetEmmiters(Agent agent, float visionRadius, List<Agent> dataToSearch)
        {
            string targetAgentName = null;
            if (agent.GetComponent<OrganizationBehavior>().placeHolderLocalList.Count != 0)
            {
                string[] separators = { ".", " " };
                string value = agent.GetComponent<OrganizationBehavior>().placeHolderLocalList[0].PlaceHoldername;
                string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                targetAgentName = "Agent" + " " + words[2];
            }

            List<Agent> neighbours = new List<Agent>();
            if (targetAgentName != null)
            {


                foreach (Agent neighbour in dataToSearch)
                {
                    if (neighbour.name != targetAgentName && Vector3.Distance(neighbour.transform.position, agent.transform.position) < visionRadius)
                    {

                        neighbours.Add(neighbour);
                    }

                }
            }

            return neighbours;

        }



        public KdTree<Agent> AvoidNonTargetEmmiters(Agent agent, float visionRadius, KdTree<Agent> dataToSearch)
        {
            string targetAgentName = null;
            if (agent.GetComponent<OrganizationBehavior>().placeHolderLocalList.Count != 0)
            {
                string[] separators = { ".", " " };
                string value = agent.GetComponent<OrganizationBehavior>().placeHolderLocalList[0].PlaceHoldername;
                string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                targetAgentName = "Agent" + " " + words[2];
            }

            KdTree<Agent> neighbours = new KdTree<Agent>();
            if (targetAgentName != null)
            {
               
                foreach (Agent neighbour in dataToSearch)
                {
                    if (neighbour.name != targetAgentName && Vector3.Distance(neighbour.transform.position, agent.transform.position) < visionRadius)
                    {


                        neighbours.Add(neighbour);
                    }

                }
            }

            return neighbours;

        }




        public void Separation(KdTree<Agent> neighbours)
        {



            float separationStrength = 0;
            float distanceToNeighbourThreshold = 0;
            Vector3 separation = new Vector3(0, 0, 0);



            if (GenerationManager.generationCount != 0)
            {
                separationStrength = .9f; //0.02
                distanceToNeighbourThreshold = 1.3f; //1.1

            }

            if (GenerationManager.generationCount == 0)
            {
                separationStrength = 0.2f; //0.017f;
                distanceToNeighbourThreshold = 1.4f;  //1.82f;


            }



            for (int i = 0; i < neighbours.Count; i++)
            {
                float distanceToNeighbour = Vector3.Distance(this.gameObject.transform.position, neighbours[i].transform.position);

                if (distanceToNeighbour <= distanceToNeighbourThreshold) /// sensitive parameter related to separation strength //1.82f
                {
                    Vector3 getAway = this.gameObject.transform.position - neighbours[i].gameObject.transform.position;

                    /// scale get away vector by inverse of distance to its neighbour
                    /// this makes the get away vector larger as the agent gets closer to its neighbour
                    separation += getAway / (getAway.magnitude * distanceToNeighbour);
                    separation.Normalize();

                    Vector3 separationForce = separation.Multiply(separationStrength);

                    IApplyForces(separationForce);

                }

            }

        }






        public virtual void Separation(KdTree<Agent> neighbours, float strength, float distanceToNeighbourThreshold)
        {




            Vector3 separation = new Vector3(0, 0, 0);
            for (int i = 0; i < neighbours.Count; i++)
            {
                float distanceToNeighbour = Vector3.Distance(this.gameObject.transform.position, neighbours[i].transform.position);

                if (distanceToNeighbour <= distanceToNeighbourThreshold) /// sensitive parameter related to separation strength //1.82f
                {
                    Vector3 getAway = this.gameObject.transform.position - neighbours[i].gameObject.transform.position;

                    /// scale get away vector by inverse of distance to its neighbour
                    /// this makes the get away vector larger as the agent gets closer to its neighbour
                    separation += getAway / (getAway.magnitude * distanceToNeighbour);
                    separation.Normalize();

                    Vector3 separationForce = separation.Multiply(strength);

                    IApplyForces(separationForce);

                }

            }

        }





        public void Separation(KdTree<Agent> neighbours, float strength, float distanceToNeighbourThreshold, string tagToIgnore)
        {




            Vector3 separation = new Vector3(0, 0, 0);
            for (int i = 0; i < neighbours.Count; i++)
            {
                if (neighbours[i].tag != tagToIgnore || neighbours[i].tag != "SignalReceiverPassive" )
                {
                    float distanceToNeighbour = Vector3.Distance(this.gameObject.transform.position, neighbours[i].transform.position);

                    if (distanceToNeighbour <= distanceToNeighbourThreshold) /// sensitive parameter related to separation strength //1.82f
                    {
                        Vector3 getAway = this.gameObject.transform.position - neighbours[i].gameObject.transform.position;

                        /// scale get away vector by inverse of distance to its neighbour
                        /// this makes the get away vector larger as the agent gets closer to its neighbour
                        separation += getAway / (getAway.magnitude * distanceToNeighbour);
                        separation.Normalize();

                        Vector3 separationForce = separation.Multiply(strength);

                        IApplyForces(separationForce);

                    }
                }

            }

        }


       public IEnumerator  SeparationCoroutine(KdTree<Agent> neighbours, float strength, float distanceToNeighbourThreshold, string tagToIgnore, float interval)
        {

            Vector3 separation = new Vector3(0, 0, 0);
            for (int i = 0; i < neighbours.Count; i++)
            {
                if (neighbours[i].tag != tagToIgnore || neighbours[i].tag != "SignalReceiverPassive")
                {
                    float distanceToNeighbour = Vector3.Distance(this.gameObject.transform.position, neighbours[i].transform.position);

                    if (distanceToNeighbour <= distanceToNeighbourThreshold) /// sensitive parameter related to separation strength //1.82f
                    {
                        Vector3 getAway = this.gameObject.transform.position - neighbours[i].gameObject.transform.position;

                        /// scale get away vector by inverse of distance to its neighbour
                        /// this makes the get away vector larger as the agent gets closer to its neighbour
                        separation += getAway / (getAway.magnitude * distanceToNeighbour);
                        separation.Normalize();

                        Vector3 separationForce = separation.Multiply(strength);

                        IApplyForces(separationForce);

                        yield return new WaitForSeconds(interval);
                    }
                }
                
            }
            

        }








        public void Repell(KdTree<Agent> avoid)
        {

            float repellMaxForce = 0;

            if (GenerationManager.generationCount != 0)
            {
                repellMaxForce = 0.02f;

            }

            if (GenerationManager.generationCount == 0)
            {

                repellMaxForce = 0.025f;


            }


            foreach (Agent agent in avoid)
            {
                float distanceToRepeller = Vector3.Distance(this.gameObject.transform.position, agent.transform.position);

                Vector3 repell = this.gameObject.transform.position - agent.transform.position;

                // Repulstion gets stronger as the agent gets closer to the repeller
                repell /= (repell.magnitude * distanceToRepeller);

                // Repulsion strength is also proportional to the radius of the repeller circle/sphere
                // This allows the user to tweak the repulsion strength by tweaking the radius
                repell *= repellMaxForce;// 0.0115f ///PARAMETER IS SENSIBLE TO MAXsPEED¬!!!! good value for maxSpeed 0.04 and 2d
                                         //repulsion *= 30.0 * repeller.Radius;
                IApplyForces(repell);


            }
        }



        public void Repell(KdTree<Agent> avoid, float repulsionStrength)
        {




            foreach (Agent agent in avoid)
            {
                float distanceToRepeller = Vector3.Distance(this.gameObject.transform.position, agent.transform.position);

                Vector3 repell = this.gameObject.transform.position - agent.transform.position;

                // Repulstion gets stronger as the agent gets closer to the repeller
                repell /= (repell.magnitude * distanceToRepeller);

                // Repulsion strength is also proportional to the radius of the repeller circle/sphere
                // This allows the user to tweak the repulsion strength by tweaking the radius
                repell *= repulsionStrength;// 0.0115f ///PARAMETER IS SENSIBLE TO MAXsPEED¬!!!! good value for maxSpeed 0.04 and 2d
                                            //repulsion *= 30.0 * repeller.Radius;
                IApplyForces(repell);


            }
        }




        #endregion

        /////////////////////////////STEERING BEHAVIORS END //////////////////////////////////////////




        ////////////////////////////////STATE HISTORY////////////////////////////////////////////////

        #region STATE HISTORY
        /// <summary>
        /// Calculate the times an agent changes states to Emmiter during the simulation
        /// </summary>
        public void CalculateEmmiterHistory()
        {
            if (this.gameObject.tag == "SignalEmmiter" && this.calculateEmmiterHistoryCoroutine == false)
            {
                this.emmiterHistoryCount++;
                this.calculateEmmiterHistoryCoroutine = true;

            }

            this.ResetCalculateEmmiterHistory();
        }

        /// <summary>
        /// Reset calculateEmmiterHistory variable to false
        /// </summary>
        private void ResetCalculateEmmiterHistory()
        {
            if (this.gameObject.tag != "SignalEmmiter")
            {
                this.calculateEmmiterHistoryCoroutine = false;
            }
        }

        /// <summary>
        /// Calculate the times an agent changes states to Receiver during the simulation
        /// </summary>
        public void CalculateReceiverHistory()
        {
            if (this.gameObject.tag == "SignalReceiver" && this.calculateReceiverHistoryCoroutine == false)
            {
                this.receiverHistoryCount++;
                this.calculateReceiverHistoryCoroutine = true;

            }

            this.ResetCalculateReceiverHistory();
        }

        /// <summary>
        /// Reset calculateReceiverHistory variable to false
        /// </summary>

        private void ResetCalculateReceiverHistory()
        {
            if (this.gameObject.tag != "SignalReceiver")
            {
                this.calculateReceiverHistoryCoroutine = false;
            }
        }


        /// <summary>
        /// Calculate the times an agent changes states to De-Activated during the simulation
        /// </summary>
        public void CalculateDeActivatedHistory()
        {
            if (this.gameObject.tag == "De-Activated" && this.calculateDeActivatedHistoryCoroutine == false)
            {
                this.deActivatedHistoryCount++;

                this.calculateDeActivatedHistoryCoroutine = true;

            }
            if (this.gameObject.tag == "De-Activated" && this.deActivatedHistoryCount > 0 && this.freezedHistoryCount == 0 && this.emmiterHistoryCount == 0 && this.receiverHistoryCount == 0
                && GenerationManager.generationChange == true && this.calculateDeActivatedHistoryIfDoesNotChangeInGenerationCoroutine == false)
            {
                this.deActivatedHistoryCount++;
                this.calculateDeActivatedHistoryIfDoesNotChangeInGenerationCoroutine = true;
            }
            ResetCalculateDeActivatedHistory();
            ///calculates agents that have only been De-Activated
            if (this.gameObject.tag == "De-Activated" && this.deActivatedHistoryCount > 0 && this.freezedHistoryCount == 0 && this.emmiterHistoryCount == 0 && this.receiverHistoryCount == 0
              && GenerationManager.generationChange == false && this.calculateDeActivatedHistoryIfDoesNotChangeInGenerationCoroutine == true)
            {

                this.calculateDeActivatedHistoryIfDoesNotChangeInGenerationCoroutine = false;
            }
        }

        /// <summary>
        /// Reset calculateDeActivatedHistory variable to false
        /// </summary>
        private void ResetCalculateDeActivatedHistory()
        {
            if (this.gameObject.tag != "De-Activated")
            {
                this.calculateDeActivatedHistoryCoroutine = false;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateFreezedHistory()
        {
            if (this.gameObject.tag == "Freezed" && this.calculateFreezedHistoryCoroutine == false)
            {
                this.freezedHistoryCount++;
                this.calculateFreezedHistoryCoroutine = true;
            }

            ResetCalculateFreezedHistory();
        }
        /// <summary>
        /// 
        /// </summary>

        private void ResetCalculateFreezedHistory()
        {
            if (this.gameObject.tag != "Freezed") this.calculateFreezedHistoryCoroutine = false;
        }


        #endregion

        ////////////////////////////////STATE HISTORY END////////////////////////////////////////////////


        ////////////////////////////////ENVIRONMENT EVALUATION////////////////////////////////////////////////  

        #region ENVIRONMENT EVALUATION  



        /// <summary>
        /// Draws rays from a shpere outward
        /// </summary>
        /// <param name="numDirections"></param>
        /// 
        public void DrawRaysFromSphere(int numDirections, float rayLength, Color color)
        {


            foreach (var direction in Utility.UnifromSphericalPointDistribution(numDirections))
            {
                Debug.DrawRay(this.gameObject.transform.position, direction.normalized * rayLength, color);



            }


        }



        public void Sensors(int numDirections,Color color, int layerMask, out KdTreeVec3<Vec3> missedRays, out List<Vector3> hitRays, bool displayRays, float raysRange)
        {


            // List<Vector3> _missedRays = new List<Vector3>();
            KdTreeVec3<Vec3> _missedRays = new KdTreeVec3<Vec3>();
         //   _missedRays.UpdatePositions();
            List<Vector3> _hitRays = new List<Vector3>();

            foreach (Vec3 direction in Utility.UnifromSphericalPointDistribution_Vec3(numDirections))
            {


                RaycastHit hit;

                Vector3 tempDir = new Vector3((float)direction.x, (float)direction.y, (float) direction.z);
                
                if (Physics.Raycast(this.gameObject.transform.position, tempDir, out hit, raysRange, layerMask))
                {
                    _hitRays.Add(tempDir);
                    if (displayRays) Debug.DrawRay(this.gameObject.transform.position, tempDir * hit.distance, color);


                }


                else
                {
                    tempDir.Normalize();
                    Vec3 tempdir = new Vec3(tempDir.x, tempDir.y, tempDir.z);

                    _missedRays.Add(tempdir); 

                }


            }




            missedRays = _missedRays;
            hitRays = _hitRays;

        }




        public void Sensors(int numDirections, Color color, int layerMask, out List<Vector3> missedRays, out List<Vector3> hitRays, bool displayRays, float raysRange)
        {


            // List<Vector3> _missedRays = new List<Vector3>();
            List<Vector3> _missedRays = new List<Vector3>();
            List<Vector3> _hitRays = new List<Vector3>();

            foreach (Vector3 direction in Utility.UnifromSphericalPointDistribution(numDirections))
            {


                RaycastHit hit;

               

                if (Physics.Raycast(this.gameObject.transform.position, direction, out hit, raysRange, layerMask))
                {
                    _hitRays.Add(direction);
                    if (displayRays) Debug.DrawRay(this.gameObject.transform.position, direction * hit.distance, color);


                }


                else
                {
                   

                    _missedRays.Add(direction); ;

                }


            }




            missedRays = _missedRays;
            hitRays = _hitRays;

        }

        /// <summary>
        /// Gets closest missedRay to target and uses that vector for its new current velocity. With this vector
        /// the steering force is calculated
        /// </summary>
        /// <param name="missedRays"></param>
        /// <param name="targetPlaceHolder"></param>
        public void AvoidObstaclesWithSensors(List<Vector3> missedRays, PlaceHolder targetPlaceHolder)
        {
            Vector3 closestMissedRayToTarget = Utility.ClosestPoint(targetPlaceHolder.transform.position, missedRays);
            closestMissedRayToTarget.Normalize();
            Vector3 desiredVel = targetPlaceHolder.transform.position - this.gameObject.transform.position;
            Vector3 newVel = closestMissedRayToTarget - this.gameObject.transform.position;
            newVel.Normalize();
            newVel *= IMaxSpeed;
            Vector3 steer = desiredVel - newVel;
            steer.Limit(5f);
            IApplyForces(steer);

        }



        public void AvoidObstaclesWithSensors(KdTreeVec3<Vec3> missedRays, PlaceHolder targetPlaceHolder)
        {
            Vec3 closestMissedRayToTarget = Utility.ClosestPoint(targetPlaceHolder.transform.position, missedRays);
            closestMissedRayToTarget.Normalize();
            Vector3 desiredVel = targetPlaceHolder.transform.position - this.gameObject.transform.position;
            Vec3 newVel = closestMissedRayToTarget - this.gameObject.transform.position;
            newVel.Normalize();
            newVel *= IMaxSpeed;
            Vector3 steer = desiredVel - newVel;
            steer.Limit(5f);
            IApplyForces(steer);

        }





        /// <summary>
        /// Gets closest missedRay to target and uses that vector for its new current velocity. With this vector
        /// the steering force is calculated
        /// </summary>
        /// <param name="missedRays"></param>
        /// <param name="targetPlaceHolder"></param>
        public void AvoidObstaclesWithSensors(List<Vector3> missedRays, Agent targetAgent)
        {
            Vector3 closestMissedRayToTarget = Utility.ClosestPoint(targetAgent.transform.position, missedRays);
            Vector3 desiredVel = targetAgent.transform.position - this.gameObject.transform.position;
            Vector3 newVel = closestMissedRayToTarget - this.gameObject.transform.position;
            newVel.Normalize();
            newVel *= IMaxSpeed;
            Vector3 steer = desiredVel - newVel;
            steer.Limit(10f);
            IApplyForces(steer);

        }




        public void DisplayClosestHitObstacle(Vector3 data, Color color)
        {
            Debug.DrawLine(this.gameObject.transform.position, data, color);
        }

        public void DisplayMissedRays(List<Vector3> missedRays, Color color)
        {
            foreach (var direction in missedRays)
            {

                Debug.DrawRay(this.gameObject.transform.position, direction, color);
            }
        }



        #endregion







        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="target"></param>
        public void SeekWithRayCast(Agent agent, PlaceHolder target)

        {
            float force = 20f;
            //Vector3 desiredVelToClosestNeighbour = (target.transform.position - agent.transform.position).normalized;
            Vector3 desiredVelToClosestNeighbour = UnityEngine.Random.onUnitSphere * IMaxForce;


            RaycastHit hit;
            float maxDistance = 10f;
            if (Physics.Raycast(agent.transform.position, agent.transform.forward, out hit, maxDistance))
            {
                if (hit.transform != agent.transform)
                {
                    Debug.DrawLine(agent.transform.position, hit.point, Color.blue);
                    desiredVelToClosestNeighbour = (hit.normal -agent.transform.position)  * force;
                }
            }

            Vector3 leftRayCast = agent.transform.position;
            Vector3 rightRayCast = agent.transform.position;

            leftRayCast.x -= 2;
            rightRayCast.x += 2;

            if (Physics.Raycast(leftRayCast, agent.transform.forward, out hit, maxDistance))
            {
                if (hit.transform != agent.transform)
                {
                    Debug.DrawLine(leftRayCast, hit.point, Color.blue);
                    desiredVelToClosestNeighbour = (hit.normal - agent.transform.position) * force;
                }
            }


            if (Physics.Raycast(rightRayCast, agent.transform.forward, out hit, maxDistance))
            {
                if (hit.transform != agent.transform)
                {
                    Debug.DrawLine(rightRayCast, hit.point, Color.blue);
                    desiredVelToClosestNeighbour = (hit.normal - agent.transform.position) * force;
                }
            }


            Quaternion rotation = Quaternion.LookRotation(desiredVelToClosestNeighbour);
            agent.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);

           

            IApplyForces(desiredVelToClosestNeighbour);


        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="target"></param>
        public void SeekWithRayCast(Agent agent, Agent target)

        {
            

            float force = 20f;
            //Vector3 desiredVelToClosestNeighbour = (target.transform.position - agent.transform.position).normalized;
            Vector3 desiredVelToClosestNeighbour = UnityEngine.Random.onUnitSphere * IMaxForce;

            RaycastHit hit;
            float maxDistance = 1f;
            if (Physics.Raycast(agent.transform.position, agent.transform.forward, out hit, maxDistance))
            {
                if (hit.transform != agent.transform)
                {
                    Debug.DrawLine(agent.transform.position, hit.point, Color.blue);
                    desiredVelToClosestNeighbour = hit.normal * force;
                }
            }

            Vector3 leftRayCast = agent.transform.position;
            Vector3 rightRayCast = agent.transform.position;

            leftRayCast.x -= 2;
            rightRayCast.x += 2;

            if (Physics.Raycast(leftRayCast, agent.transform.forward, out hit, maxDistance))
            {
                if (hit.transform != agent.transform)
                {
                    Debug.DrawLine(leftRayCast, hit.point, Color.blue);
                    desiredVelToClosestNeighbour = hit.normal * force;
                }
            }


            if (Physics.Raycast(rightRayCast, agent.transform.forward, out hit, maxDistance))
            {
                if (hit.transform != agent.transform)
                {
                    Debug.DrawLine(rightRayCast, hit.point, Color.blue);
                    desiredVelToClosestNeighbour = hit.normal * force;
                }
            }


            Quaternion rotation = Quaternion.LookRotation(desiredVelToClosestNeighbour);
            agent.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);


            IApplyForces(desiredVelToClosestNeighbour);


        }







        ////////////////////////////////ENVIRONMENT EVALUATION END////////////////////////////////////////////

  

        #region SCALAR FIELD DATA


        /// <summary>
        /// Looks up current Pixel that corresponds to the agents position. This method already has internalized
        /// conditions for all agent states
        /// </summary>
        /// <param name="pixel"></param> 
        /// <param name="currentPixel"></param> Pixel object output with query data
        public void ScalarFieldDataLookUp(ScalarField2D cell, out Cell currentPixel)
        {


            Cell _currentPixel = cell.Lookup2D(this.gameObject.transform.position);
            currentPixel = _currentPixel;

        }


        /// <summary>
        /// Looks up current Pixel that corresponds to the agents position. This method already has internalized
        /// conditions for all agent states
        /// </summary>
        /// <param name="pixel"></param> 
        /// <param name="currentPixel"></param> Pixel object output with query data
        //public void ScalarField3DDataLookUp(ScalarField2D cell, out Cell currentPixel)
        //{


        //    Cell _currentPixel = cell.Lookup3D(this.gameObject.transform.position);
        //    currentPixel = _currentPixel;

        //}


        #endregion



        #region PIXEL DATA



        /// <summary>
        /// Looks up current Pixel that corresponds to the agents position. This method already has internalized
        /// conditions for all agent states
        /// </summary>
        /// <param name="pixel"></param> 
        /// <param name="currentPixel"></param> Pixel object output with query data
        public void PixelDataLookUp3D(PixelPopulation pixel, out Pixel currentPixel)
        {


            Pixel _currentPixel = pixel.Lookup3D(this.gameObject.transform.position);
            currentPixel = _currentPixel;

        }



        #endregion
        /////////////////////////////PHYSICS //////////////////////////////////////////

        public void IRunPhysics()
        {

            this.velocity += this.acceleration;
            this.velocity.Normalize();
            this.velocity *= this.IMaxSpeed;

            this.gameObject.transform.position += this.velocity;

           //this.gameObject.GetComponent<Rigidbody>().AddForce(this.velocity);


            this.IResetForces();
        }

        public void IResetForces()
        {
            this.acceleration *= 0.0f;
        }

        public void IApplyForces(Vector3 force)
        {

            this.acceleration += force / this.IMass;

        }

       

        /////////////////////////////PHYSICS END //////////////////////////////////////////
    }

}