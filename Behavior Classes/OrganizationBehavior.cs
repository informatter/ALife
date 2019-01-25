using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



using SharpMatter.SharpMath;
using SharpMatter.SharpDataStructures;
using ClassExtensions;
using System.Linq;

using OrganizationalModel.UtilityFunctions;
using OrganizationalModel.BaseClass;
using OrganizationalModel.Population;
using OrganizationalModel.Managers;
using OrganizationalModel.ScalarFields;

namespace OrganizationalModel.Behaviors
{
  
    public partial class OrganizationBehavior : Agent, IOrganization<Agent>
    {
       
      


        ////////////////////////////////CONTROL LOG / NOTES ////////////////////////////////
        /// <summary>

        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// </summary>




        ////////////////////////////////DECLARE GLOBAL VARIABLES////////////////////////////////



        //CLASS INSTANCES

        //PUBLIC LISTS


        //PUBLIC STATIC LISTS

        //PRIVATE LISTS




        // PUBLIC VARIABLES

        private float averegeDist = 0;

        public float distanceToTarget;

        //PRIVATE VARIABLES
        private int signalReceiverPassiveThreshhold = 100;
        public int signalEmitterCounter = 0;
        private bool searchClosestNeighbour = false;
        public bool searchClosestNeighbourAgain = false;
        private bool createPlaceHolders = false;

     
        
  

        private  int time = 0;

        


        // NON STATIC PUBLIC VARIABLES

        GameObject agentPopulation; // create variable to store game object that holds script top reference
        private AgentPopulation AP; // store script to reference in variable

       
        // NON STATIC PUBLIC VARIABLES
        GameObject pixelPopulation; /// create variable to store game object that holds script top reference
        private PixelPopulation pixelPop; /// store script to reference in variable

        GameObject scalarField;
        private ScalarField2D SF;

        GameObject generationManager;
        private GenerationManager GM;

      
        float startTime;




        private bool addPlaceHolderTargetHistoryCoroutine = false;
   
        Rigidbody rb;
      

        public float totalDisplacement;
        public float averegeNeighbourDisplacement;
        private List<float> neighboursDisplacement = new List<float>();

      
        private List<Vector3> displacementPositions = new List<Vector3>();

        private Vector3 cloudCenter = Vector3.zero;

        [HideInInspector]
        public float distFromCloudCenter;

        [HideInInspector]
        public float placeHolderOffset;

        public BehaviorController behavior = new BehaviorController();
        public static bool runSimulation;

         float scale =0;

       
       
        Material emissiveMaterial;
        float scalarFieldValuesProximityTotal = 0;
         float averege = 0; // averege values of scalarFieldValuesProximityTotal

        [Range(0.03f, 2f)]
        public float emmisiveScale = 0.05f;

        bool addMyNeighbourData =false;

        float RigidBodyWindForce = 8f;

        float neighBourDisplacementThreshold = 500;
        void Start()
        {

           
            emissiveMaterial = gameObject.GetComponent<MeshRenderer>().material;
            startTime = Time.time;


        


            ///////////////////////////////VARIABLE INITIALIZATION////////////////////////////////

            // NON STATIC PUBLIC VARIABLE INITIALIZATION


            agentPopulation = GameObject.Find("AgentPopulation"); // create variable to store game object that holds script top reference
            AP = agentPopulation.GetComponent<AgentPopulation>(); // store script to reference in variable


            pixelPopulation = GameObject.Find("PixelPopulation");
            pixelPop = pixelPopulation.GetComponent<PixelPopulation>();
            //  NON STATIC PUBLIC VARIABLE INITIALIZATION
      

          
             scalarField = GameObject.Find("ScalarField2D"); 
             SF = scalarField.GetComponent<ScalarField2D>();

        generationManager = GameObject.Find("GenerationManager");
            GM = generationManager.GetComponent<GenerationManager>();

            ///////////////////////////////METHOD INITIALIZATION////////////////////////////////




            base.IMass = 1f;//REAL MASS OF UNIT
  
            if(SimulationManager.Get().addRigidBodyCollider==false)
            {
                base.IMaxForce = 0.06f;
                base.IMaxSpeed = 0.06f;
            }

            if (SimulationManager.Get().addRigidBodyCollider)
            {
                base.IMaxForce = 2.6f;
                base.IMaxSpeed = 2.6f;
            }

            base.VisionRadius = (this.gameObject.GetComponent<SphereCollider>().radius * 2) * 0.9f;

            rb = GetComponent<Rigidbody>();
    

            base.energyDecreasingFactor = 0.1f;
            base.energyLevel = 650f; // 650 maH --> litjuim batter capacity that we are using
            

            base.energyCapacity = 650f; // 650 maH --> litjuim batter capacity that we are using








            if (SimulationManager.Get().addRigidBodyCollider)
            {
                this.gameObject.GetComponent<SphereCollider>().isTrigger = false; 
                rb.isKinematic = false;

            }


        } // END START


        void Update()
        {
           
           // if (runSimulation)
           if(SimulationManager.Get().runSimulation)
            {
                if (GenerationManager.generationCount == 0)
                {
                    //  if(SimulationManager.Get().oneGeneration==false) CheckNeighours(); // this does not decrese performance

                   CheckNeighours();

                    if (base.energyLevel < 0) base.energyLevel = 0;

                    base.neighbours = base.neighboursTemp.Distinct().ToList();

                    //if(displacementPositions.Count>500)
                    //{
                    //    displacementPositions.RemoveRange(0, displacementPositions.Count-500);
                    //}

                    RulesContinous();
                    IAgentBehaviorConditions();

                    base.CalculateEmmiterHistory();
                    base.CalculateReceiverHistory();
                    base.CalculateDeActivatedHistory();
                    base.CalculateFreezedHistory();

      



                    for (int i = 0; i < AgentPopulation.signalReceiverPassiveAgentList.Count; i++)
                    {
                        if (AgentPopulation.signalReceiverPassiveAgentList[i].GetComponent<OrganizationBehavior>().signalReceiverPassiveCounter > signalReceiverPassiveThreshhold)
                        {
                            base.ChangeStateToSignalEmmiter(AgentPopulation.signalReceiverPassiveAgentList[i], 0);
                        }
                    }


                    if (SimulationManager.Get().addFixedJoints == false && SimulationManager.Get().addRigidBodyCollider == false && gameObject.tag == "SignalReceiver")
                    {
                      
                        base.IRunPhysics();
                    }

                    //if (SimulationManager.Get().addFixedJoints == false && SimulationManager.Get().addRigidBodyCollider == false)
                    //{

                    //    RulesContinous();
                    //    IAgentBehaviorConditions();
                    //}

               

                    //if (SimulationManager.Get().addFixedJoints == true && SimulationManager.Get().addRigidBodyCollider == false)
                    //{

                    //    RulesContinous();
                    //    IAgentBehaviorConditions();
                    //    base.IRunPhysics();
                    //}




                }
            }




        } //END UPDATE


        void FixedUpdate()
        {
            if (SimulationManager.Get().runSimulation)
            {
                if (SimulationManager.Get().addFixedJoints && SimulationManager.Get().addRigidBodyCollider)
                {
                    //  if (gameObject.tag == "SignalReceiver" || gameObject.tag == "Freezed" || gameObject.tag == "SignalEmmiter")// base.IRunPhysics();


                    //if (gameObject.tag == "SignalReceiver" || gameObject.tag == "Freezed") base.IRunPhysics();
                   
                    IAgentBehaviorConditions();
                }
            }
        }


        ////////////////////////////////METHODS////////////////////////////////////////////////////////////////////////  ////////////////////////////////METHODS////////////////////////////////////////////////////////////////////////



    public void DisplayEmotion()
    {
            SimulationManager.Get().displayColorByState = false;
            SimulationManager.Get().displayColorbyEnergy = false;
            SimulationManager.Get().displayColorbyDisplacement = false;
            SimulationManager.Get().displayColorByComunication = false;
            SimulationManager.Get().displayColorByNeighbours = false;
            SimulationManager.Get().displayTopology = false;
            SimulationManager.Get().GPUInstancing = false;
            


            SimulationManager.Get().updateFieldRealTime = true;

            foreach (var item in ScalarFields.ScalarField2D.scalarFieldValuesProximity) // using  ScalarField2D.scalarFieldValuesProximity makes change faster, using  ScalarField2D.scalarFieldHumanData makes change much slower
            {
                scalarFieldValuesProximityTotal += item;
            }

            // float averege2 = scalarFieldValuesProximityTotal / ScalarFields.ScalarField2D.scalarFieldHumanData.Length;

            averege = scalarFieldValuesProximityTotal / ScalarFields.ScalarField2D.scalarFieldHumanData.Length;
            UserInterface.UI.averegeFiledValues = averege;


            scale += Mathf.Clamp(Mathf.Sin(averege), 0.001f, .2f);
            UserInterface.UI.rateOfChange = scale;
            //Color c = Utility.Lerp3(Color.white, Color.red, Color.blue, Mathf.Cos(base.currentScalarValueHumanData * Mathf.PI + scale)); //Mathf.Cos(base.currentScalarValueHumanData  * Mathf.PI + scale));

             Color c = Utility.Lerp3(Color.grey, Utility.RGBToFloat(255, 0, 13), Utility.RGBToFloat(250, 179, 0), Mathf.Cos(base.currentScalarValueHumanData * Mathf.PI + scale));

         //   Color c = Color.Lerp(Color.grey, Color.black, Mathf.Cos(base.currentScalarValueHumanData * Mathf.PI + scale));

            // Color c = Utility.Lerp3(Color.black, Color.red, Color.blue, Mathf.Cos(base.currentScalarValueHumanData * Mathf.PI + scale)); //Mathf.Cos(base.currentScalarValueHumanData  * Mathf.PI + scale));

            gameObject.GetComponent<MeshRenderer>().material.color = c;

            emissiveMaterial.SetColor("_Emission", c);
           


            DynamicGI.SetEmissive(gameObject.GetComponent<MeshRenderer>(), c);

        }
        




        public List<Vector3> Draw2dCircle(Vector3 origin, PlaceHolder obj)
        {
           

            int nPts = 50;
            float angCount = Mathf.PI * 2 / nPts; // radians
            float theta = 0;
            float cosTheta = 0;
            float sinTheta = 0;
            float radius = 0.4f;

           // if(obj.tag =="DeActivatedPlaceHolder") radius =0.1f;

           List<Vector3> listPt = new List<Vector3>();

            // equation of a circle : x = r*cos(theta)
            //                        y = r*sin(theta)

          

                for (int i = 0; i < nPts; i++)

                {
                    theta = i * angCount;
                    cosTheta = origin.x + radius * Mathf.Cos(theta);
                    sinTheta = origin.z + radius * Mathf.Sin(theta);
                    

                    Vector3 pts = new Vector3(cosTheta, origin.y, sinTheta);
                    listPt.Add(pts);


                }

            

            return listPt;
        }



        /////////////////////////////STATES //////////////////////////////////////////

        #region STATES RULES

        /// <summary>
        /// 
        /// </summary>

        public void RulesContinous()
        {
            base.ChangeStateToSignalEmmiter_Continuous();
            base.ChangeStateToFreezed_Continuous();
        }

        public void IAgentBehaviorConditions()
        {

            ISignalEmmiterConditions();
            ISignalReceiverConditions();
            ISignalReceiverPassiveConditions();
            FreezedAgentConditions();
            DeActivatedAgentConditions();


           


        }

      


        #endregion



   

        private void OnDrawGizmos()
        {
            if (GenerationManager.generationCount == 0)
            {
               
                if (this.gameObject.tag == "Freezed" || this.gameObject.tag == "SignalEmmiter" || this.gameObject.tag == "SignalReceiverPassive")
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

        //private void SeekSimpleForRB(Agent agent, PlaceHolder target, float maxForce)
        //{
        //    Vector3 desiredVelToClosestNeighbour = target.transform.position - agent.transform.position;

        //    ApplyForcesRigidBody(desiredVelToClosestNeighbour, maxForce);
        //}

        private void SeekSimpleForRB(Agent agent, PlaceHolder target)
        {
            Vector3 desiredVelToClosestNeighbour = target.transform.position - agent.transform.position;
            desiredVelToClosestNeighbour.Normalize();
            desiredVelToClosestNeighbour *= base.IMaxSpeed;
            Vector3 steer = desiredVelToClosestNeighbour - rb.velocity;


            Vector3 steerLimit = steer.Limit(base.IMaxForce);

            ApplyForcesRigidBody(desiredVelToClosestNeighbour, base.IMaxForce);
        }


        #region COLLIDERS
        private void OnTriggerEnter(Collider other)
        {
            if (GenerationManager.generationCount == 0)
            {
             
                if (other.gameObject.tag != "De-Activated" && other.gameObject.tag != "ActivatedPlaceHolder"
                    && other.gameObject.tag != "CollidedPlaceHolder")
                {

                    base.neighboursTemp.Add(other.gameObject.GetComponent<Agent>());
                    base.neighboursTempVector.Add(other.gameObject.transform.position);
                }

                if (other.gameObject.tag == "SignalEmmiter")
                {


                    base.ChangeStateToSignalReceiverPassive(this.gameObject.GetComponent<Agent>(),0);
                 

                }





            }



        }

        //private void OnTriggerExit(Collider other)
        //{


        //    for (int i = base.neighbours.Count - 1; i >= 0; i--)
        //    {
        //        if (base.neighbours != null)
        //        {
        //            base.neighbours.Remove(other.GetComponent<Agent>());
        //        }
        //    }
        //}


        private void OnCollisionEnter(Collision collision)
        {


            if (GenerationManager.generationCount == 0)
            {
                

                if (SimulationManager.Get().addFixedJoints  && this.gameObject.tag!="Freezed" )
                {
                    
                    AddFixedJoint(collision);
                    //AddSpringJoint(collision);
                }
                if (collision.gameObject.tag != "De-Activated" && collision.gameObject.tag != "ActivatedPlaceHolder"
                    && collision.gameObject.tag != "CollidedPlaceHolder")
                {

                    base.neighboursTemp.Add(collision.gameObject.GetComponent<Agent>());
                    base.neighboursTempVector.Add(collision.gameObject.transform.position);

                }

                //if (this.gameObject.tag == "Freezed")
                //{
                //    for (int i = 0; i < base.neighbours.Count; i++)
                //    {

                //        if (collision.gameObject.tag == "SignalEmmiter" && collision.gameObject.name != base.neighbours[i].name)
                //        {


                //            base.ChangeStateToSignalReceiverPassive(this.gameObject.GetComponent<Agent>(), 0);


                //        }

                //    }
                //}

                if (collision.gameObject.tag == "SignalEmmiter" && this.gameObject.tag!="Freezed" &&  this.gameObject.tag != "SignalEmmiter")
                {


                    base.ChangeStateToSignalReceiverPassive(this.gameObject.GetComponent<Agent>(), 0);


                }


            }

        }


        private void OnCollisionExit(Collision collision)
        {

            //for (int i = base.neighbours.Count - 1; i >= 0; i--)
            //{
            //    if (base.neighbours != null)
            //    {
            //        base.neighbours.Remove(collision.gameObject.GetComponent<Agent>());
            //    }
            //}

            //FixedJoint[] fixedJoints;

            //fixedJoints = GetComponents<FixedJoint>();

            //foreach (FixedJoint joint in fixedJoints)
            //{
            //    Destroy(joint);
            //}
                
        }






        bool hasJoint;
     

        private void AddFixedJoint(Collision data)
        {


            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = data.rigidbody;
            joint.breakForce = 300;
            joint.breakTorque = 300;
       

            base.neighbourHistory.Add(data.gameObject.GetComponent<Agent>());
        }


        private void AddSpringJoint(Collision data)
        {
            
                SpringJoint joint = gameObject.AddComponent<SpringJoint>();
                joint.connectedBody = data.rigidbody;
                gameObject.GetComponent<SpringJoint>().spring = 30000;
                gameObject.GetComponent<SpringJoint>().damper = 30000;

          

            base.neighbourHistory.Add(data.gameObject.GetComponent<Agent>());
        }


       




        #endregion



    }//////END CLASS


}
