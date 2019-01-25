using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;
using System;

using ClassExtensions;

using SharpMatter.SharpMath;
using SharpMatter.SharpDataStructures;

using OrganizationalModel.UtilityFunctions;
using OrganizationalModel.BaseClass;
using OrganizationalModel.Population;
using OrganizationalModel.Managers;
using OrganizationalModel.ScalarFields;
using OrganizationalModel.Managers;

////////////////////////////////CLASS DESCRIPTION////////////////////////////////
/// <summary>
/// 
/// This class contains all agent behaviors
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
/// Check Seek function some scaling numbers where changed instead of using the scale of MaxForce
/// 
/// 
/// 
/// 
/// 
/// Bug: Agent receivers choose the same signal emmiters, causing less selected signal emmiters and thus, there will be signal receivers flying around when no
/// emmiters are left.
/// 
/// 
/// 
/// 
/// </summary>
/// 
////////////////////////////////CONTROL LOG / NOTES END ////////////////////////////////

namespace OrganizationalModel.Behaviors
{
    public partial class ReOrganizationBehavior : Agent, IReOrganization<Agent, float>
    {

        ////////////////////////////////DECLARE GLOBAL VARIABLES////////////////////////////////
      

        int wandererTreshhold = 400;

        public int counter;
      
        int positionHistoryCounter = 0;

        //CLASS INSTANCES

        //PUBLIC LISTS


        //PUBLIC STATIC LISTS

        //PRIVATE LISTS




        public List<Agent> neighbourDensity = new List<Agent>();

        public int neighbourDensityCount = 0;

       public float distanceToTarget;

        // PUBLIC VARIABLES

        [HideInInspector]
        public float placeHolderOffset;

        public int countToSearchForEmptyPixel = 0;


        //PRIVATE VARIABLES

     

      
     

        private bool addPlaceHolderTargetHistoryCoroutine = false;

        private bool calculateNeighboursCoroutine_ReOrg = false;
        private int time_ReOrg = 0;

        [HideInInspector]
        public bool searchClosestNeighbour_ReOrg = false;

        public bool searchClosestNeighbourAgain_ReOrg = false;

        public bool createPlaceHolders_ReOrg = false;

      

        private bool destroyPlaceHolders = false;
        private bool getActivePlaceHolders_ReOrg = false;


        private int signalReceiverCounter = 0;

       // [HideInInspector]
        public static bool calculatePixelPermanenceClusterAgent = false;
       

        // NON STATIC PUBLIC VARIABLES

        GameObject agentPopulation; // create variable to store game object that holds script top reference
        private AgentPopulation AP; // store script to reference in variable

        GameObject generationManager;
        private GenerationManager GM;


        GameObject pixelPopulation; /// create variable to store game object that holds script top reference
        private PixelPopulation pixelPop; /// store script to reference in variable

    

        float agentRadius;

        float wanderTheta = 0;

        Rigidbody rb;

        GameObject scalarField;
        private ScalarField2D SF;

        public float totalDisplacement;
        public float averegeNeighbourDisplacement;
        private List<float> neighboursDisplacement = new List<float>();


        private List<Vector3> displacementPositions = new List<Vector3>();
        public static bool runSimulation;

        float scalarFieldValuesProximityTotal = 0;
        float averege = 0; // averege values of scalarFieldValuesProximityTotal
        float scale = 0;

        private bool ActivateEmmiterAgain = false;

       

        void Start()
        {
            base.IMass = 1f;//REAL MASS OF UNIT

            if (SimulationManager.Get().addRigidBodyCollider == false)
            {
                base.IMaxForce = 0.06f;
                base.IMaxSpeed = 0.06f;
            }

            if (SimulationManager.Get().addRigidBodyCollider)
            {
                base.IMaxForce = 3.5f;
                base.IMaxSpeed = 3.5f;
            }



            if (SimulationManager.Get().runSimulation) runSimulation = true;
         
        
            base.VisionRadius = (this.gameObject.GetComponent<SphereCollider>().radius * 2) * 1.9f;

         rb = GetComponent<Rigidbody>();
            //rb.velocity = base.velocity;
            //rb.mass = base.IMass = 1;

            agentRadius = this.gameObject.GetComponent<SphereCollider>().radius;
            base.energyDecreasingFactor = 0.05f;
            //base.energyLevel = 650f; // 650 maH --> litjuim batter capacity that we are using

           


            base.energyCapacity = 650f; // 650 maH --> litjuim batter capacity that we are using

            // if (base.heliumLevel < 0) base.heliumLevel = 0;


          



            ///////////////////////////////VARIABLE INITIALIZATION////////////////////////////////

            // NON STATIC PUBLIC VARIABLE INITIALIZATION

            agentPopulation = GameObject.Find("AgentPopulation"); // create variable to store game object that holds script top reference
            AP = agentPopulation.GetComponent<AgentPopulation>(); // store script to reference in variable

            generationManager = GameObject.Find("GenerationManager");
            GM = generationManager.GetComponent<GenerationManager>();

            pixelPopulation = GameObject.Find("PixelPopulation");
            pixelPop = pixelPopulation.GetComponent<PixelPopulation>();

            scalarField = GameObject.Find("ScalarField2D");
            SF = scalarField.GetComponent<ScalarField2D>();

            ///////////////////////////////METHOD INITIALIZATION////////////////////////////////

        

        } // END START



        void Update()
        {
            if (runSimulation)
            {
                if (GenerationManager.generationCount == 0)
                {



                    ShareEnergyLevelsOnGeneration_0();
                    CheckNeighours();

                    base.energyLevel = this.gameObject.GetComponent<OrganizationBehavior>().energyLevel;
                    base.neighbours = base.neighboursTemp.Distinct().ToList();
                    base.neighboursVector = base.neighboursTempVector.Distinct().ToList();
                    base.neighbourHistory = base.neighbours;

                    //base.distance = this.gameObject.GetComponent<SphereCollider>().radius * 2.0f;


                    base.distanceToNeighbour = this.gameObject.GetComponent<OrganizationBehavior>().distanceToNeighbour;

                }

                if (GenerationManager.generationCount != 0)
                {

                    CheckNeighours();
                    if (base.energyLevel < 0) base.energyLevel = 0;
                    if (base.energyLevel > 650) base.energyLevel = 650;
                    base.neighbours = base.neighboursTemp.Distinct().ToList();
                    if (base.positionHistory.Count > 200)
                    {
                        base.positionHistory.RemoveRange(0, base.positionHistory.Count - 200);
                    }







                    IRulesDiscrete();/// conditions of execution are held within the method
                    IRulesContinuous();/// conditions of execution are held within the method


                    IAgentStatesBehaviorConditions();


                    base.CalculateEmmiterHistory();
                    base.CalculateReceiverHistory();
                    base.CalculateDeActivatedHistory();
                    base.CalculateFreezedHistory();






                    if (GenerationManager.generationChange)
                    {
                        IResetProperties();
                    }





                    if (SimulationManager.Get().addFixedJoints == false && SimulationManager.Get().addRigidBodyCollider == false)
                    {
                        if (this.gameObject.tag == "SignalReceiver") base.IRunPhysics();

                    }







                }
            }// END RUN SIMULATION


        } //END UPDATE





        void FixedUpdate()
        {
            

            if (SimulationManager.Get().runSimulation && GenerationManager.generationCount!=0)
            {
                if (SimulationManager.Get().addFixedJoints && SimulationManager.Get().addRigidBodyCollider)
                {
                  //  if (gameObject.tag == "SignalReceiver" || gameObject.tag == "Freezed" || gameObject.tag == "SignalEmmiter") base.IRunPhysics();

                    // //if (gameObject.tag == "SignalReceiver" || gameObject.tag == "Freezed" || gameObject.tag == "De-ActivatedPassive") base.IRunPhysics();

                    //// if (gameObject.tag == "SignalReceiver" || gameObject.tag == "Freezed") base.IRunPhysics();

                     IAgentStatesBehaviorConditions();

                }
            }
        }



        ////////////////////////////////METHODS////////////////////////////////////////////////////////////////////////  ////////////////////////////////METHODS////////////////////////////////////////////////////////////////////////




      



        /////////////////////////////AGENT STATES //////////////////////////////////////////

        #region STATE RULES

        /// <summary>
        /// 
        /// Rules are only executed once!
        /// </summary>
        /// 

        public void IRulesDiscrete()
        {


            if (!GenerationManager.generationChange  && base.calculateRulesDiscreteCoroutine == false)
            {
                bool selectByRange = false;
                bool selectByNum = true;
                int min = 0;
                int max = 0;
                int numNiehbours = 1;
                int agentsToActivate = 1;

                 //ChangeStateToSignalEmmiterOnNeighbourCount_Discrete(selectByRange, selectByNum, min, max, numNiehbours);

                ChangeStateToSignalEmmiterOnNeighbourCount_Discrete(numNiehbours, agentsToActivate);

            

               base.calculateRulesDiscreteCoroutine = true;

            }


        }

        /// <summary>
        /// Rules are calculated every frame per second
        /// </summary>
        public void IRulesContinuous()
        {

            /// Runs rules on all Generations except  Generation_0
            if (!GenerationManager.generationChange &&  base.calculateRulesDiscreteCoroutine == true)
            {
               

                base.ChangeStateToSignalEmmiter_Continuous();
                base.ChangeStateToFreezed_Continuous();

                //if(ActivateEmmiterAgain==true)
                //{
                //    int numNiehbours = 1;
                //    int agentsToActivate = 1;
                //    ChangeStateToSignalEmmiterOnNeighbourCount_Discrete(numNiehbours, agentsToActivate);
                //}
            

            }

        }



       



   

        public void ChangeStateToDeactivatedPassive_Discrete(bool selectByRange, bool selectByNum, int min, int max, int num)
        {
            if (selectByRange)
            {

                if (base.neighbours.Count == min || base.neighbours.Count <= max)
                {
                    base.ChangeStateToSignalEmmiter(this.gameObject.GetComponent<Agent>(), 0);

                }


            }

            if (selectByNum)
            {
                if (base.neighbours.Count == num )
                {
                    base.ChangeStateToSignalEmmiter(this.gameObject.GetComponent<Agent>(), 0);
                }
            }
        }







        /// <summary>
        /// Changes state of Agent on neighbour count on a discrete interval - Per generatiopn change
        /// 
        ///  min neighbours inclusive.
        ///  max neighbours  inclusive.
        /// 
        /// </summary>
        /// <param name="min"> min neighbours, inclusive.</param> 
        /// <param name="max">max neighbours, inclusive.</param>

        public void ChangeStateToSignalEmmiterOnNeighbourCount_Discrete(bool selectByRange,bool selectByNum,int min, int max, int num)
        {
            if (selectByRange)
            {
                if (base.neighbours.Count == min && base.neighbours.Count <= max) base.ChangeStateToSignalEmmiter(this.gameObject.GetComponent<Agent>(), 0);
            }

            if (selectByNum)
            {
                if (base.neighbours.Count == num) base.ChangeStateToSignalEmmiter(this.gameObject.GetComponent<Agent>(), 0);
               
            }

   
        }

        /// <summary>
        /// Only select one one deactivated agent to activate within the numnber of max neighbours specified
        /// </summary>
        /// <param name="num"></param>
        public void ChangeStateToSignalEmmiterOnNeighbourCount_Discrete(int neighbourNum, int numOfAgentsToActivate) 
        {

            int count = 0;

            bool checkCall1 = false;
            
         
            for (int i = 0; i < AgentPopulation.deActivatedAgentList.Count; i++) 

            {
                


                if (AgentPopulation.deActivatedAgentList[i].neighbours.Count == neighbourNum)
                {
                    base.ChangeStateToSignalEmmiter(AgentPopulation.deActivatedAgentList[i].gameObject.GetComponent<Agent>(), 0);
                    count++;
                }

                if (count == numOfAgentsToActivate) break; /// will exit the whole forloop if condition is met
               

              


            }

            /// This is added just in case there are no deactivated agents with the specified neighbour count
            if (count == 0)
            {
                for (int i = 0; i < AgentPopulation.deActivatedAgentList.Count; i++)

                {



                    if (AgentPopulation.deActivatedAgentList[i].neighbours.Count == neighbourNum + 1)
                    {
                        base.ChangeStateToSignalEmmiter(AgentPopulation.deActivatedAgentList[i].gameObject.GetComponent<Agent>(), 0);
                        count++;
                    }

                    if (count == numOfAgentsToActivate) break; /// will exit the whole forloop if condition is met


                    checkCall1 = CheckCall();


                }
            }


            if (count == 0 && checkCall1==true)
            {
                for (int i = 0; i < AgentPopulation.deActivatedAgentList.Count; i++)

                {



                    if (AgentPopulation.deActivatedAgentList[i].neighbours.Count == neighbourNum + 2)
                    {
                        base.ChangeStateToSignalEmmiter(AgentPopulation.deActivatedAgentList[i].gameObject.GetComponent<Agent>(), 0);
                        count++;
                    }

                    if (count == numOfAgentsToActivate) break; /// will exit the whole forloop if condition is met


                  //  checkCall1 = CheckCall();


                }
            }



            for (int i = 0; i < AgentPopulation.deActivatedAgentList.Count; i++)

            {

                if (AgentPopulation.deActivatedAgentList[i].neighbours.Count == 0)
                {
                    base.ChangeStateToSignalEmmiter(AgentPopulation.deActivatedAgentList[i].gameObject.GetComponent<Agent>(), 0);
                    break;
                }

            }

            if (count == 0) Debug.LogError("!!!!!");





        }

        private bool CheckCall()
        {
            return true;
        }




        public override void ChangeStateToFreezed_Continuous()
        {


            if (this.gameObject.tag == "SignalEmmiter")
            {

                /// if all  its neighbours are signal emmiters, change own state to freezed
                int count = 0;

                if (mySignalReceivers.Count != 0)
                {

                    for (int i = 0; i < neighbours.Count; i++)
                    {
                        for (int j = 0; j < mySignalReceivers.Count; j++)
                        {
                            if (neighbours[i].gameObject.tag == "SignalEmmiter" && mySignalReceivers[j].name == neighbours[i].name)
                            {
                                count++;
                            }
                        }
                    }


                    if (count == mySignalReceivers.Count)
                    {

                        ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);
                    }




                }


                if (AgentPopulation.deActivatedAgentList.Count == 0 && mySignalReceivers.Count == 0)
                {
                    ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);
                }

                if (AgentPopulation.deActivatedAgentList.Count == 0 && AgentPopulation.signalReceiverAgentList.Count == 0 && mySignalReceivers.Count != 0)
                {
                    ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);
                }

                // if (signalEmitterCounter > 2000) base.ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);


            } 


        }










            #endregion





            /////////////////////////////AGENT STATES BEHAVIOR CONDITIONS END //////////////////////////////////////////






            #region RESET PROPERTIES

            /// <summary>
            /// 
            /// </summary>
            public void IResetProperties()
        
           {
               
                this.signalReceiverCounter = 0;

            Array.Clear(GenerationManager.activatedAgentPerGeneration, 0, GenerationManager.activatedAgentPerGeneration.Length);

                this.mySignalEmmiter = null; /// ADDED ON 2018/11/18

                

                 this.wandererTreshhold = 0;
          
                base.positionHistory.Clear();

                base.positionHistoryTraceback.Clear();

                base.localCollisionHistory = 0;

                this.distanceToTarget = 0;


                ChangeStateToDeactivated(this.gameObject.GetComponent<Agent>(), 0);

                base.calculateRulesDiscreteCoroutine = false;




          

            this.calculateNeighboursCoroutine_ReOrg = false;
                this.createPlaceHolders_ReOrg = false;
                this.searchClosestNeighbour_ReOrg = false;
                this.time_ReOrg = 0;
            this.searchClosestNeighbourAgain_ReOrg = false;


                this.signalReceiverPassiveCounter = 0;
                base.signalReceiverPassiveCounter = 0;

                base.mySignalReceivers.Clear();
                this.mySignalReceiversTemp.Clear();

            base.placeHolderLocalList.Clear();

                base.placeHolders.Clear();
            this.neighbourDensityCount = 0;
         

          
        }

        #endregion

        ////////////////////////////////PLACE HOLDER FUNCTIONALITY////////////////////////////////////////////////

        ////////////////////////////////PLACE HOLDER FUNCTIONALITY END////////////////////////////////////////////////


        private void OnDrawGizmos()
        {
            /// draw vectors to target
            /// 
            if (GenerationManager.generationCount != 0)
            {

              


                if (this.gameObject.tag == "Freezed" || this.gameObject.tag == "SignalEmmiter" || this.gameObject.tag == "De-Activated")
                {
                    for (int i = 0; i < base.neighbours.Count; i++)
                    {
                        if (base.neighbours[i] != null)
                        {
                            Gizmos.color = Color.white;
                            Vector3 dir = base.neighbours[i].transform.position - this.gameObject.transform.position;
                            Gizmos.DrawRay(this.gameObject.transform.position, dir);
                        }

                    }
                }

                if (SimulationManager.Get().displayTopology)
                {

                    Gizmos.DrawSphere(this.gameObject.transform.position, 0.1f);
                }

            }


        }




        private void CheckNeighours()
        {
            if (base.neighboursTemp.Count != 0)
            {
                for (int i = base.neighboursTemp.Count - 1; i >= 0; i--)
                {
                    if (base.neighboursTemp[i] != null)
                    {
                        if (Vector3.Distance(this.gameObject.transform.position, base.neighboursTemp[i].transform.position) > 1.1f)
                        {
                            base.neighboursTemp.RemoveAt(i);
                        }
                    }
                }
            }


            if (base.neighbours.Count != 0)
            {
                for (int i = base.neighbours.Count - 1; i >= 0; i--)
                {
                    if (base.neighbours[i] != null)
                    {
                        if (Vector3.Distance(this.gameObject.transform.position, base.neighbours[i].transform.position) > 1.1f)
                        {
                            base.neighbours.RemoveAt(i);
                        }
                    }
                }
            }
        }











        #region RIGID BODY UPDATE TEST
        private void ApplyForcesRigidBody(Vector3 force, float maxForce)
        {
            force.Normalize();
            force *= maxForce;
            rb.AddForce(force * maxForce);
        }
        #endregion

        private void SeekSimpleForRB(Agent agent, PlaceHolder target, float maxForce, float maxSpeed)
        {
            Vector3 desiredVelToClosestNeighbour = target.transform.position - agent.transform.position;
            desiredVelToClosestNeighbour.Normalize();
            desiredVelToClosestNeighbour *= maxSpeed;
            Vector3 steer = desiredVelToClosestNeighbour - rb.velocity;


            Vector3 steerLimit = steer.Limit(maxForce);

            ApplyForcesRigidBody(desiredVelToClosestNeighbour, maxForce);
        }

        #region COLLIDERS
        private void OnTriggerEnter(Collider other)
        {
            /// keeps track of neighbours in all other generations
            if (GenerationManager.generationCount != 0 && SimulationManager.Get().addRigidBodyCollider==false)
            {
                if (other.gameObject.tag == "SignalEmmiter")
                {
                  //  base.neighboursTemp.Add(other.gameObject.GetComponent<Agent>());
                    //base.neighbours.Add(other.gameObject.GetComponent<Agent>());
                  //  base.neighboursTempVector.Add(other.gameObject.transform.position);
                    //base.neighbourHistory.Add(other.gameObject.GetComponent<Agent>());
                  //  base.localCollisionHistory++;
                    ChangeStateToSignalReceiverPassive(this.gameObject.GetComponent<Agent>(), 0);
                }

                //if (other.gameObject.tag == "SignalReceiver" && this.gameObject.tag!="De-Activated" && this.gameObject.tag!="Freezed")
                //{
                //    base.neighboursTemp.Add(other.gameObject.GetComponent<Agent>());
                //    base.neighboursTempVector.Add(other.gameObject.transform.position);
                //    base.neighbourHistory.Add(other.gameObject.GetComponent<Agent>());

                //  //  base.localCollisionHistory++;

                //}


                if (other.gameObject.tag != "De-Activated" && other.gameObject.tag != "ActivatedPlaceHolder"
                    && other.gameObject.tag != "CollidedPlaceHolder")
                {

                    base.neighboursTemp.Add(other.gameObject.GetComponent<Agent>());
                    base.neighboursTempVector.Add(other.gameObject.transform.position);
                }


            }


            ///Keeps track of neighbours of generation 0
            if (GenerationManager.generationCount == 0)
            {
                if (other.gameObject.tag != "De-Activated" && other.gameObject.tag != "ActivatedPlaceHolder" && other.gameObject.tag != "CollidedPlaceHolder")
                {
                    /// Increment the collision history of the Agent. during first collision, SystemCollisionHistory = 1
                    base.neighboursTemp.Add(other.gameObject.GetComponent<Agent>());
                    base.neighboursTempVector.Add(other.gameObject.transform.position);
                    base.neighbourHistory.Add(other.gameObject.GetComponent<Agent>());
                    base.localCollisionHistory++;

                }



            }


        }


    


        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

       

        private void OnTriggerExit(Collider other)
        {
            if (SimulationManager.Get().addRigidBodyCollider == false)
            {
                for (int i = base.neighbours.Count - 1; i >= 0; i--)
                {
                    if (base.neighbours != null)
                    {
                        base.neighbours.Remove(other.GetComponent<Agent>());
                    }
                }
            }
        }



        private void OnCollisionEnter(Collision collision)
        {


            if (GenerationManager.generationCount != 0 && SimulationManager.Get().addRigidBodyCollider == true)
            {


                if (SimulationManager.Get().addFixedJoints)
                {

                    AddFixedJoint(collision);
                }

                if (collision.gameObject.tag == "SignalEmmiter" && this.gameObject.tag != "Freezed")
                {


                    base.ChangeStateToSignalReceiverPassive(this.gameObject.GetComponent<Agent>(), 0);


                }


                if (collision.gameObject.tag != "De-Activated" && collision.gameObject.tag != "ActivatedPlaceHolder"
                       && collision.gameObject.tag != "CollidedPlaceHolder")
                {

                    base.neighboursTemp.Add(collision.gameObject.GetComponent<Agent>());
                    base.neighboursTempVector.Add(collision.gameObject.transform.position);
                }


            }


            ///Keeps track of neighbours of generation 0
            if (GenerationManager.generationCount == 0 && SimulationManager.Get().addRigidBodyCollider == true)
            {
                if (collision.gameObject.tag != "De-Activated" && collision.gameObject.tag != "ActivatedPlaceHolder" && collision.gameObject.tag != "CollidedPlaceHolder")
                {
                    /// Increment the collision history of the Agent. during first collision, SystemCollisionHistory = 1
                    base.neighboursTemp.Add(collision.gameObject.GetComponent<Agent>());
                    base.neighboursTempVector.Add(collision.gameObject.transform.position);
                    base.neighbourHistory.Add(collision.gameObject.GetComponent<Agent>());
                    base.localCollisionHistory++;

                }



            }

        }

        //private void OnCollisionExit(Collision collision)
        //{
        //    for (int i = base.neighbours.Count - 1; i >= 0; i--)
        //    {
        //        if (base.neighbours != null)
        //        {
        //            base.neighbours.Remove(collision.gameObject.GetComponent<Agent>());
        //        }
        //    }

        //}


        bool hasJoint;
   

        private void AddFixedJoint(Collision data)
        {


            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = data.rigidbody;
            joint.breakForce = 100;
            joint.breakTorque = 100;


            base.neighbourHistory.Add(data.gameObject.GetComponent<Agent>());
        }


        private void AddSpringJoint(Collision data)
        {
            if (data.gameObject.GetComponent<Rigidbody>() != null && !hasJoint)
            {
                gameObject.AddComponent<SpringJoint>();
                gameObject.GetComponent<SpringJoint>().connectedBody = data.rigidbody;
                gameObject.GetComponent<SpringJoint>().spring = 1;
                gameObject.GetComponent<SpringJoint>().damper = 30000;

                gameObject.GetComponent<SpringJoint>().enableCollision = true;
                hasJoint = true;

            }

            
        }


     

        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////









        #endregion


    }/////////////// END CLASS

}

