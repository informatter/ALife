using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ClassExtensions;
using SharpMatter.SharpDataStructures;
using SharpMatter.SharpMath;
using OrganizationalModel.UtilityFunctions;
using OrganizationalModel.Population;
using OrganizationalModel.Managers;
using OrganizationalModel.ScalarFields;
using OrganizationalModel.Behaviors;
using System.Linq;

namespace OrganizationalModel.BaseClass
{
    public partial class Agent: IPhysicsBehavior<float, Vector3>
    {
        #region PLACE HOLDERS



        /// <summary>
        /// This method contains courotine to loop through all place holders to activate an agent
        ///  
        /// </summary>
        /// <param name="activateAgentsMaxNeighbours"></param>
        public void ActivateAgents()
        {

            StartCoroutine(DelaySignalReceiverSearch());


        }


        public void AddFixedJointPlaceHolders(PlaceHolder data, float breakForce)
        {
            if (data.gameObject.GetComponent<Rigidbody>() != null)
            {
                data.gameObject.AddComponent<FixedJoint>();
                data.gameObject.GetComponent<FixedJoint>().connectedBody = this.gameObject.GetComponent<Rigidbody>();
                data.gameObject.GetComponent<FixedJoint>().breakForce = breakForce;
              //  data.gameObject.GetComponent<FixedJoint>().m


            }



        }





        #region DIRECTIONALITY RULES 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param> Agent to create place holders on
        /// <param name="offset"></param> place holder offset from agent / PACKING DENSITY
        /// <returns> Returns a list of Vector positions </returns>
        public List<Vector3> CreatePlaceHolderVectorPositions(Agent agent, float offset, List<Agent> Neighbours, OrganizationalModel.BaseClass.DirectionalityRules rules)
        {

            // Create list to hold place holder positions
            List<Vector3> placeHolderPositions = new List<Vector3>();
            float similarThreshhold = 0.2f;

            // get radius of agent prefab
            float agentPrefabRadius = SimulationManager.Get().agentPrefab.GetComponent<SphereCollider>().radius;
            // get radius of place holder prefab
            float placeHolderPrefabRadius = SimulationManager.Get().placeHolderPrefab.GetComponent<SphereCollider>().radius;

       


         //  if (SimulationManager.Get().is3D)
        //    {





                if (SimulationManager.Get()._12Neighbours)
                {



                    Vector3 cntrPoint = agent.transform.position;

                    float offsetToAvoidCollision = 0.98f; //(agentPrefabRadius *2) * 1.02f; 0.98
                    offset += offsetToAvoidCollision;

                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule01) // All horizontal placeHolders 
                    {
                        directionalityRule = 1;

                        Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P1 = (P1 * offset) + cntrPoint;

                        Vector3 P2 = new Vector3(2 * agentPrefabRadius, 0, 0);

                        P2 = (P2 * offset) + cntrPoint;


                        Vector3 P3 = new Vector3(agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P3 = (P3 * offset) + cntrPoint;


                        Vector3 P4 = new Vector3(-agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P4 = (P4 * offset) + cntrPoint;


                        Vector3 P5 = new Vector3(-2 * agentPrefabRadius, 0, 0);

                        P5 = (P5 * offset) + cntrPoint;


                        Vector3 P6 = new Vector3(-agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P6 = (P6 * offset) + cntrPoint;



                 

                    
                            placeHolderPositions.Add(P1);
                            placeHolderPositions.Add(P2);
                            placeHolderPositions.Add(P3);
                            placeHolderPositions.Add(P4);
                            placeHolderPositions.Add(P5);
                            placeHolderPositions.Add(P6);

                    }





                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule02) 
                    {
                        directionalityRule = 2;

                        Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P1 = (P1 * offset) + cntrPoint;


                        Vector3 P3 = new Vector3(agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P3 = (P3 * offset) + cntrPoint;



                        placeHolderPositions.Add(P1);
                        placeHolderPositions.Add(P3);
                  

                    }



                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule03) 
                    {
                        directionalityRule = 3; 

                        Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P1 = (P1 * offset) + cntrPoint;


                        Vector3 P3 = new Vector3(agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P3 = (P3 * offset) + cntrPoint;

                        Vector3 P4 = new Vector3(-agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P4 = (P4 * offset) + cntrPoint;

                        Vector3 P6 = new Vector3(-agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P6 = (P6 * offset) + cntrPoint;




                        placeHolderPositions.Add(P1);
                        placeHolderPositions.Add(P3);
                        placeHolderPositions.Add(P4);
                        placeHolderPositions.Add(P6);


                    }




                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule04)
                    {
                        directionalityRule = 4;

                        Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P1 = (P1 * offset) + cntrPoint;


                        Vector3 P3 = new Vector3(agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P3 = (P3 * offset) + cntrPoint;


                        Vector3 P5 = new Vector3(-2 * agentPrefabRadius, 0, 0);

                        P5 = (P5 * offset) + cntrPoint;



                        placeHolderPositions.Add(P1);
                        placeHolderPositions.Add(P3);
                        placeHolderPositions.Add(P5);
                   


                    }








                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule05) /// Tree
                    {

                        directionalityRule = 5;


                        Vector3 P7 = new Vector3(0, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);

                        P7 = (P7 * offset) + cntrPoint;

                        Vector3 P8 = new Vector3(agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P8 = (P8 * offset) + cntrPoint;

                        Vector3 P9 = new Vector3(-agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P9 = (P9 * offset) + cntrPoint;


                  


                            placeHolderPositions.Add(P7);
                            placeHolderPositions.Add(P8);
                            placeHolderPositions.Add(P9);



                    }




                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule06) 
                    {

                        directionalityRule = 6;

                        Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P1 = (P1 * offset) + cntrPoint;

                        Vector3 P2 = new Vector3(2 * agentPrefabRadius, 0, 0);

                        P2 = (P2 * offset) + cntrPoint;

                        Vector3 P3 = new Vector3(agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P3 = (P3 * offset) + cntrPoint;

                        Vector3 P7 = new Vector3(0, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);

                        P7 = (P7 * offset) + cntrPoint;

                        Vector3 P8 = new Vector3(agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P8 = (P8 * offset) + cntrPoint;

                        Vector3 P9 = new Vector3(-agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P9 = (P9 * offset) + cntrPoint;




                        placeHolderPositions.Add(P1);
                        placeHolderPositions.Add(P2);
                        placeHolderPositions.Add(P3);
                        placeHolderPositions.Add(P7);
                        placeHolderPositions.Add(P8);
                        placeHolderPositions.Add(P9);



                    }






                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule07)
                    {

                        directionalityRule = 7;


                        Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P1 = (P1 * offset) + cntrPoint;

                        Vector3 P6 = new Vector3(-agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P6 = (P6 * offset) + cntrPoint;

                        Vector3 P8 = new Vector3(agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P8 = (P8 * offset) + cntrPoint;


                
                            placeHolderPositions.Add(P1);
                            placeHolderPositions.Add(P6);
                            placeHolderPositions.Add(P8);
                           


                    }



                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule08)
                    {

                        directionalityRule = 8;


                        Vector3 P7 = new Vector3(0, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);

                        P7 = (P7 * offset) + cntrPoint;

                        Vector3 P8 = new Vector3(agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P8 = (P8 * offset) + cntrPoint;

                        Vector3 P9 = new Vector3(-agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P9 = (P9 * offset) + cntrPoint;

                        Vector3 P10 = new Vector3(0, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);

                        P10 = (P10 * offset) + cntrPoint;


                        Vector3 P11 = new Vector3(-agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P11 = (P11 * offset) + cntrPoint;


                        Vector3 P12 = new Vector3(agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P12 = (P12 * offset) + cntrPoint;




                        placeHolderPositions.Add(P7);
                        placeHolderPositions.Add(P8);
                        placeHolderPositions.Add(P9);
                        placeHolderPositions.Add(P10);
                        placeHolderPositions.Add(P11);
                        placeHolderPositions.Add(P12);



                    }




                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule09) /// All placeHolders
                    {


                        directionalityRule = 9;

                        Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P1 = (P1 * offset) + cntrPoint;

                        Vector3 P2 = new Vector3(2 * agentPrefabRadius, 0, 0);

                        P2 = (P2 * offset) + cntrPoint;

                        Vector3 P3 = new Vector3(agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P3 = (P3 * offset) + cntrPoint;


                        Vector3 P4 = new Vector3(-agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P4 = (P4 * offset) + cntrPoint;


                        Vector3 P5 = new Vector3(-2 * agentPrefabRadius, 0, 0);

                        P5 = (P5 * offset) + cntrPoint;


                        Vector3 P6 = new Vector3(-agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P6 = (P6 * offset) + cntrPoint;



                        // 3 neighbours on the top layer =============================================================


                        Vector3 P7 = new Vector3(0, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);

                        P7 = (P7 * offset) + cntrPoint;


                        Vector3 P8 = new Vector3(agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P8 = (P8 * offset) + cntrPoint;

                        Vector3 P9 = new Vector3(-agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P9 = (P9 * offset) + cntrPoint;

                        //3 neighbours on the bottom layer =============================================================


                        Vector3 P10 = new Vector3(0, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);

                        P10 = (P10 * offset) + cntrPoint;


                        Vector3 P11 = new Vector3(-agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P11 = (P11 * offset) + cntrPoint;


                        Vector3 P12 = new Vector3(agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P12 = (P12 * offset) + cntrPoint;




                            placeHolderPositions.Add(P1);
                            placeHolderPositions.Add(P2);
                            placeHolderPositions.Add(P3);
                            placeHolderPositions.Add(P4);
                            placeHolderPositions.Add(P5);
                            placeHolderPositions.Add(P6);
                            placeHolderPositions.Add(P7);
                            placeHolderPositions.Add(P8);
                            placeHolderPositions.Add(P9);
                            placeHolderPositions.Add(P10);
                            placeHolderPositions.Add(P11);
                            placeHolderPositions.Add(P12);
                     



                    }



                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule10)
                    {
                        directionalityRule = 10;

                        Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P1 = (P1 * offset) + cntrPoint;


                        Vector3 P2 = new Vector3(2 * agentPrefabRadius, 0, 0);

                        P2 = (P2 * offset) + cntrPoint;

                        Vector3 P5 = new Vector3(-2 * agentPrefabRadius, 0, 0);

                        P5 = (P5 * offset) + cntrPoint;


                        Vector3 P6 = new Vector3(-agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P6 = (P6 * offset) + cntrPoint;

                        Vector3 P7 = new Vector3(0, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);

                        P7 = (P7 * offset) + cntrPoint;

                        Vector3 P11 = new Vector3(-agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P11 = (P11 * offset) + cntrPoint;




                        placeHolderPositions.Add(P1);
                        placeHolderPositions.Add(P2);
                        placeHolderPositions.Add(P5);
                        placeHolderPositions.Add(P6);
                        placeHolderPositions.Add(P7);
                        placeHolderPositions.Add(P11);


                    }




                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule11)
                    {
                        directionalityRule = 11;

                        Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P1 = (P1 * offset) + cntrPoint;



                        Vector3 P3 = new Vector3(agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P3 = (P3 * offset) + cntrPoint;


                        Vector3 P6 = new Vector3(-agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P6 = (P6 * offset) + cntrPoint;

                        Vector3 P8 = new Vector3(agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P8 = (P8 * offset) + cntrPoint;

                        Vector3 P12 = new Vector3(agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P12 = (P12 * offset) + cntrPoint;


                        placeHolderPositions.Add(P1);
                        placeHolderPositions.Add(P3);
                     
                        placeHolderPositions.Add(P6);
                        placeHolderPositions.Add(P8);
                        placeHolderPositions.Add(P12);


                    }






                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule12)
                    {
                        directionalityRule = 12;



                        Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P1 = (P1 * offset) + cntrPoint;

                        Vector3 P3 = new Vector3(agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P3 = (P3 * offset) + cntrPoint;

                        Vector3 P4 = new Vector3(-agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P4 = (P4 * offset) + cntrPoint;
               

                        Vector3 P6 = new Vector3(-agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P6 = (P6 * offset) + cntrPoint;

                        Vector3 P9 = new Vector3(-agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P9 = (P9 * offset) + cntrPoint;


                        Vector3 P12 = new Vector3(agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P12 = (P12 * offset) + cntrPoint;


                        placeHolderPositions.Add(P1);
                        placeHolderPositions.Add(P3);
                        placeHolderPositions.Add(P4);
               
                        placeHolderPositions.Add(P6);
                    
                        placeHolderPositions.Add(P9);
                        placeHolderPositions.Add(P12);


                    }




                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule13)
                    {
                        directionalityRule = 13;

                        Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P1 = (P1 * offset) + cntrPoint;

                        Vector3 P3 = new Vector3(agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P3 = (P3 * offset) + cntrPoint;

                        Vector3 P4 = new Vector3(-agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P4 = (P4 * offset) + cntrPoint;


                        Vector3 P5 = new Vector3(-2 * agentPrefabRadius, 0, 0);

                        P5 = (P5 * offset) + cntrPoint;

                        Vector3 P6 = new Vector3(-agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P6 = (P6 * offset) + cntrPoint;

                        Vector3 P7 = new Vector3(0, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);

                        P7 = (P7 * offset) + cntrPoint;

                        Vector3 P9 = new Vector3(-agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P9 = (P9 * offset) + cntrPoint;

                        Vector3 P12 = new Vector3(agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P12 = (P12 * offset) + cntrPoint;


                        placeHolderPositions.Add(P1);
                        placeHolderPositions.Add(P3);
                        placeHolderPositions.Add(P4);
                        placeHolderPositions.Add(P5);
                        placeHolderPositions.Add(P6);
                        placeHolderPositions.Add(P7);
                        placeHolderPositions.Add(P9);
                        placeHolderPositions.Add(P12);


                    }



                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule14)
                    {
                        directionalityRule = 14;
                        Vector3 P2 = new Vector3(2 * agentPrefabRadius, 0, 0);

                        P2 = (P2 * offset) + cntrPoint;

                        Vector3 P8 = new Vector3(agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P8 = (P8 * offset) + cntrPoint;

                        Vector3 P11 = new Vector3(-agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P11 = (P11 * offset) + cntrPoint;

                        Vector3 P12 = new Vector3(agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P12 = (P12 * offset) + cntrPoint;


                     

                            placeHolderPositions.Add(P2);
                            placeHolderPositions.Add(P8);
                            placeHolderPositions.Add(P11);
                            placeHolderPositions.Add(P12);


                        
                    }



                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule15)
                    {
                        directionalityRule = 15;

                        Vector3 P2 = new Vector3(2 * agentPrefabRadius, 0, 0);

                        P2 = (P2 * offset) + cntrPoint;

                        Vector3 P6 = new Vector3(-agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P6 = (P6 * offset) + cntrPoint;

                        Vector3 P7 = new Vector3(0, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);

                        P7 = (P7 * offset) + cntrPoint;

                        Vector3 P8 = new Vector3(agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P8 = (P8 * offset) + cntrPoint;


                        Vector3 P11 = new Vector3(-agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P11 = (P11 * offset) + cntrPoint;

                        Vector3 P12 = new Vector3(agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P12 = (P12 * offset) + cntrPoint;


                        placeHolderPositions.Add(P2);
                        placeHolderPositions.Add(P6);
                        placeHolderPositions.Add(P7);
                        placeHolderPositions.Add(P8);
                        placeHolderPositions.Add(P11);
                        placeHolderPositions.Add(P12);


                    }





                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule16)
                    {
                        directionalityRule = 16;

                        Vector3 P2 = new Vector3(2 * agentPrefabRadius, 0, 0);

                        P2 = (P2 * offset) + cntrPoint;

                        Vector3 P4 = new Vector3(-agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P4 = (P4 * offset) + cntrPoint;

                        Vector3 P6 = new Vector3(-agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P6 = (P6 * offset) + cntrPoint;

                        Vector3 P7 = new Vector3(0, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);

                        P7 = (P7 * offset) + cntrPoint;


                        Vector3 P8 = new Vector3(agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P8 = (P8 * offset) + cntrPoint;


                        Vector3 P11 = new Vector3(-agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P11 = (P11 * offset) + cntrPoint;

                       


           

                            placeHolderPositions.Add(P2);
                            placeHolderPositions.Add(P4);
                            placeHolderPositions.Add(P6);
                            placeHolderPositions.Add(P7);
                            placeHolderPositions.Add(P8);
                            placeHolderPositions.Add(P11);
                          


                        
                    }




                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule17)  
                    {
                        directionalityRule = 17;

                        Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P1 = (P1 * offset) + cntrPoint;

                        Vector3 P6 = new Vector3(-agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P6 = (P6 * offset) + cntrPoint;

                        Vector3 P8 = new Vector3(agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P8 = (P8 * offset) + cntrPoint;

                        Vector3 P12 = new Vector3(agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P12 = (P12 * offset) + cntrPoint;

                        placeHolderPositions.Add(P1);
                        placeHolderPositions.Add(P6);
                        placeHolderPositions.Add(P8);
                        placeHolderPositions.Add(P12);


                    }




                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule18)
                    {
                        directionalityRule = 18;

                        Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P1 = (P1 * offset) + cntrPoint;

                        Vector3 P2 = new Vector3(2 * agentPrefabRadius, 0, 0);

                        P2 = (P2 * offset) + cntrPoint;



                        placeHolderPositions.Add(P1);
                        placeHolderPositions.Add(P2);
               
                    }





                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule19)
                    {
                        directionalityRule = 19;

                        Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P1 = (P1 * offset) + cntrPoint;

                        Vector3 P2 = new Vector3(2 * agentPrefabRadius, 0, 0);

                        P2 = (P2 * offset) + cntrPoint;

                        Vector3 P7 = new Vector3(0, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);

                        P7 = (P7 * offset) + cntrPoint;



                        placeHolderPositions.Add(P1);
                        placeHolderPositions.Add(P2);
                        placeHolderPositions.Add(P7);

                    }



                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule20)
                    {
                        directionalityRule = 20;
                        Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P1 = (P1 * offset) + cntrPoint;

                        Vector3 P2 = new Vector3(2 * agentPrefabRadius, 0, 0);

                        P2 = (P2 * offset) + cntrPoint;

                        Vector3 P4 = new Vector3(-agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P4 = (P4 * offset) + cntrPoint;


                        Vector3 P5 = new Vector3(-2 * agentPrefabRadius, 0, 0);

                        P5 = (P5 * offset) + cntrPoint;

                       

                        Vector3 P6 = new Vector3(-agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P6 = (P6 * offset) + cntrPoint;

                        Vector3 P8 = new Vector3(agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P8 = (P8 * offset) + cntrPoint;

                        Vector3 P9 = new Vector3(-agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P9 = (P9 * offset) + cntrPoint;

                        Vector3 P11 = new Vector3(-agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P11 = (P11 * offset) + cntrPoint;


                        placeHolderPositions.Add(P1);
                        placeHolderPositions.Add(P2);
                        placeHolderPositions.Add(P4);
                        placeHolderPositions.Add(P5);
                        placeHolderPositions.Add(P6);
                        placeHolderPositions.Add(P8);
                        placeHolderPositions.Add(P9);
                        placeHolderPositions.Add(P11);

                    }










                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule21)
                    {
                        directionalityRule = 21;

                        Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P1 = (P1 * offset) + cntrPoint;


                        Vector3 P2 = new Vector3(2 * agentPrefabRadius, 0, 0);

                        P2 = (P2 * offset) + cntrPoint;


                        Vector3 P4 = new Vector3(-agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P4 = (P4 * offset) + cntrPoint;


                        Vector3 P5 = new Vector3(-2 * agentPrefabRadius, 0, 0);

                        P5 = (P5 * offset) + cntrPoint;


                        Vector3 P7 = new Vector3(0, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);

                        P7 = (P7 * offset) + cntrPoint;


                        Vector3 P10 = new Vector3(0, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);

                        P10 = (P10 * offset) + cntrPoint;




                        placeHolderPositions.Add(P1);
                        placeHolderPositions.Add(P2);
                        placeHolderPositions.Add(P4);
                        placeHolderPositions.Add(P5);
                        placeHolderPositions.Add(P7);
                        placeHolderPositions.Add(P10);


                    }






                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule22)
                    {
                        directionalityRule = 22;

                        Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P1 = (P1 * offset) + cntrPoint;


                        Vector3 P2 = new Vector3(2 * agentPrefabRadius, 0, 0);

                        P2 = (P2 * offset) + cntrPoint;


                     

                        Vector3 P5 = new Vector3(-2 * agentPrefabRadius, 0, 0);

                        P5 = (P5 * offset) + cntrPoint;


                        Vector3 P6 = new Vector3(-agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P6 = (P6 * offset) + cntrPoint;

                        Vector3 P11 = new Vector3(-agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P11 = (P11 * offset) + cntrPoint;


                        Vector3 P12 = new Vector3(agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P12 = (P12 * offset) + cntrPoint;




                        placeHolderPositions.Add(P1);
                        placeHolderPositions.Add(P2);          
                        placeHolderPositions.Add(P5);
                        placeHolderPositions.Add(P6);
                        placeHolderPositions.Add(P11);
                        placeHolderPositions.Add(P12);

                    }




                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule23)
                    {
                        directionalityRule = 23;

                        Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P1 = (P1 * offset) + cntrPoint;


                        Vector3 P2 = new Vector3(2 * agentPrefabRadius, 0, 0);

                        P2 = (P2 * offset) + cntrPoint;



                        Vector3 P4 = new Vector3(-agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                        P4 = (P4 * offset) + cntrPoint;




                        Vector3 P5 = new Vector3(-2 * agentPrefabRadius, 0, 0);

                        P5 = (P5 * offset) + cntrPoint;



                        Vector3 P7 = new Vector3(0, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);

                        P7 = (P7 * offset) + cntrPoint;


                        Vector3 P8 = new Vector3(agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P8 = (P8 * offset) + cntrPoint;


                        Vector3 P10 = new Vector3(0, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);

                        P10 = (P10 * offset) + cntrPoint;



                        Vector3 P11 = new Vector3(-agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P11 = (P11 * offset) + cntrPoint;


                     




                        placeHolderPositions.Add(P1);
                        placeHolderPositions.Add(P2);
                        placeHolderPositions.Add(P4);
                        placeHolderPositions.Add(P5);            
                        placeHolderPositions.Add(P7);
                        placeHolderPositions.Add(P8);
                        placeHolderPositions.Add(P10);
                        placeHolderPositions.Add(P11);
                     

                    }





                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule24)
                    {
                        directionalityRule = 24;

                      

                        Vector3 P8 = new Vector3(agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P8 = (P8 * offset) + cntrPoint;




                        Vector3 P9 = new Vector3(-agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P9 = (P9 * offset) + cntrPoint;

                  


                        Vector3 P10 = new Vector3(0, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);

                        P10 = (P10 * offset) + cntrPoint;


                        Vector3 P11 = new Vector3(-agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P11 = (P11 * offset) + cntrPoint;



                        placeHolderPositions.Add(P8);
                        placeHolderPositions.Add(P9);
                        placeHolderPositions.Add(P10);
                        placeHolderPositions.Add(P11);


                    }



                    if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule25) /// bottom three
                    {
                        directionalityRule = 25;



                        Vector3 P10 = new Vector3(0, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);

                        P10 = (P10 * offset) + cntrPoint;


                        Vector3 P11 = new Vector3(-agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P11 = (P11 * offset) + cntrPoint;

                        Vector3 P12 = new Vector3(agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                        P12 = (P12 * offset) + cntrPoint;



                    
                        placeHolderPositions.Add(P10);
                        placeHolderPositions.Add(P11);
                        placeHolderPositions.Add(P12);


                    }




                if (rules == OrganizationalModel.BaseClass.DirectionalityRules.Rule26) 
                {
                    directionalityRule = 26;


                    Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                    P1 = (P1 * offset) + cntrPoint;


                    Vector3 P2 = new Vector3(2 * agentPrefabRadius, 0, 0);

                    P2 = (P2 * offset) + cntrPoint;



                    Vector3 P3 = new Vector3(agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                    P3 = (P3 * offset) + cntrPoint;


                    Vector3 P4 = new Vector3(-agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                    P4 = (P4 * offset) + cntrPoint;



                    placeHolderPositions.Add(P1);
                    placeHolderPositions.Add(P2);
                    placeHolderPositions.Add(P3);
                    placeHolderPositions.Add(P4);

                }






                //Vector3 P3 = new Vector3(agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                //P3 = (P3 * offset) + cntrPoint;


                //Vector3 P4 = new Vector3(-agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                //P4 = (P4 * offset) + cntrPoint;


                //Vector3 P5 = new Vector3(-2 * agentPrefabRadius, 0, 0);

                //P5 = (P5 * offset) + cntrPoint;


                //Vector3 P6 = new Vector3(-agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);

                //P6 = (P6 * offset) + cntrPoint;



                //// 3 neighbours on the top layer =============================================================


                //Vector3 P7 = new Vector3(0, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);

                //P7 = (P7 * offset) + cntrPoint;


                //Vector3 P8 = new Vector3(agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                //P8 = (P8 * offset) + cntrPoint;

                //Vector3 P9 = new Vector3(-agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                //P9 = (P9 * offset) + cntrPoint;

                ////3 neighbours on the bottom layer =============================================================


                //Vector3 P10 = new Vector3(0, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);

                //P10 = (P10 * offset) + cntrPoint;


                //Vector3 P11 = new Vector3(-agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                //P11 = (P11 * offset) + cntrPoint;


                //Vector3 P12 = new Vector3(agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);

                //P12 = (P12 * offset) + cntrPoint;




                //if (neighbours.Count > 0)
                //{
                //    for (int i = 0; i < neighbours.Count; i++)
                //    {

                //        if (!SharpMath.Similar(neighbours[i].transform.position, P1, similarThreshhold)) placeHolderPositions.Add(P1);
                //        if (!SharpMath.Similar(neighbours[i].transform.position, P2, similarThreshhold)) placeHolderPositions.Add(P2);

                //        if (!SharpMath.Similar(neighbours[i].transform.position, P3, similarThreshhold)) placeHolderPositions.Add(P3);
                //        if (!SharpMath.Similar(neighbours[i].transform.position, P4, similarThreshhold)) placeHolderPositions.Add(P4);

                //        if (!SharpMath.Similar(neighbours[i].transform.position, P5, similarThreshhold)) placeHolderPositions.Add(P5);
                //        if (!SharpMath.Similar(neighbours[i].transform.position, P6, similarThreshhold)) placeHolderPositions.Add(P6);

                //        if (!SharpMath.Similar(neighbours[i].transform.position, P7, similarThreshhold)) placeHolderPositions.Add(P7);
                //        if (!SharpMath.Similar(neighbours[i].transform.position, P8, similarThreshhold)) placeHolderPositions.Add(P8);
                //        if (!SharpMath.Similar(neighbours[i].transform.position, P9, similarThreshhold)) placeHolderPositions.Add(P9);


                //        if (!SharpMath.Similar(neighbours[i].transform.position, P10, similarThreshhold)) placeHolderPositions.Add(P10);

                //        if (!SharpMath.Similar(neighbours[i].transform.position, P11, similarThreshhold)) placeHolderPositions.Add(P11);
                //        if (!SharpMath.Similar(neighbours[i].transform.position, P12, similarThreshhold)) placeHolderPositions.Add(P12);
                //    }
                //}

                //if (neighbours.Count == 0)
                //{

                //    placeHolderPositions.Add(P1);
                //    placeHolderPositions.Add(P2);
                //    placeHolderPositions.Add(P3);
                //    placeHolderPositions.Add(P4);
                //    placeHolderPositions.Add(P5);
                //    placeHolderPositions.Add(P6);
                //    placeHolderPositions.Add(P7);
                //    placeHolderPositions.Add(P8);
                //    placeHolderPositions.Add(P9);
                //    placeHolderPositions.Add(P10);
                //    placeHolderPositions.Add(P11);
                //    placeHolderPositions.Add(P12);
                //}







            }






          //  }

            return placeHolderPositions;
        }



        #endregion


        /// <summary>
        /// Instantiates palace holders and adds them to the local place holder list, sets their parent, their name and their initial state
        /// </summary>
        /// <param name="agent"></param> Agent to create place holders on
        /// <param name="offset"></param> offset distance from agent / packing density
        public void CreatePlaceHolders(Agent agent, float offset, DirectionalityRules rules)
        {
            InstantiatePlaceHolders(agent, offset, rules);
            SetPlaceHolderParent();
            SetPlaceHolderName();
            InitializeState(placeHolderLocalList);
            SetMaterial();
        }












        /// <summary>
        /// Lo0p through all the Agents PlaceHolders with a slight delay between each iteration. This is to ensure that they dont pick the same Agent to Activate
        /// activateAgentsMaxNeighbours parameter is used for reorganization behavior, only deactivated agents neighbour count with the specified parameter value will be activated
        /// </summary>
        /// <param name="activateAgentsMaxNeighbours"></param>
        /// <returns></returns>
        public IEnumerator DelaySignalReceiverSearch()
        {
            if (GenerationManager.generationCount == 0)
            {
                for (int i = 0; i < selectedActivatedPlaceHolders.Count; i++)
                {
                    if (selectedActivatedPlaceHolders[i] != null && selectedActivatedPlaceHolders[i].tag != "DeActivatedPlaceHolder" 
                        && selectedActivatedPlaceHolders[i].tag != "CollidedPlaceHolder" && AgentPopulation.deActivatedAgentList.Count!=0)
                    {

                        Agent signalReceiver = Utility.ClosestObject(selectedActivatedPlaceHolders[i], AgentPopulation.deActivatedAgentList);

                        ChangeStateToSignalReceiver(signalReceiver, 0);
                        //SelectDeActivatedAgent(signalReceiver);

                        mySignalReceiversTemp.Add(signalReceiver);
                        /// adds designated signal receiver to its corresponding place holder
                        selectedActivatedPlaceHolders[i].MysignalReceiver = signalReceiver;


                        /// adds designated place holders  to the signal receivers target list
                        signalReceiver.GetComponent<OrganizationBehavior>().placeHolderTargetForSignalReceiver = selectedActivatedPlaceHolders[i].GetComponent<PlaceHolder>();

                        yield return new WaitForSeconds(0.05f);

                    }


                }
            }

           



            if (GenerationManager.generationCount > 0 ) 
            {


                for (int i = 0; i < selectedActivatedPlaceHolders.Count; i++)
                {
                    if (selectedActivatedPlaceHolders[i] != null && selectedActivatedPlaceHolders[i].tag != "DeActivatedPlaceHolder"
                        && selectedActivatedPlaceHolders[i].tag != "CollidedPlaceHolder" && AgentPopulation.deActivatedAgentList.Count != 0)
                    {
                    

                        Agent signalReceiver = Utility.ClosestObject(selectedActivatedPlaceHolders[i], GetDeacTivatedAgentsWithingNeighbourRange());

                     ChangeStateToSignalReceiver(signalReceiver, 0);

                       


                        mySignalReceiversTemp.Add(signalReceiver);
                        /// adds designated signal receiver to its corresponding place holder
                        selectedActivatedPlaceHolders[i].MysignalReceiver = signalReceiver;


                        /// adds designated place holders  to the signal receivers target list
                        signalReceiver.GetComponent<ReOrganizationBehavior>().placeHolderTargetForSignalReceiver = selectedActivatedPlaceHolders[i].GetComponent<PlaceHolder>();

                        yield return new WaitForSeconds(0.05f);

                    }


                }

                
            }



        }





        /// TESTED IN C# GH AND IS WORKING PERFECTLY
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Agent> GetDeacTivatedAgentsWithingNeighbourRange()
        {
            List<Agent> data = new List<Agent>();

            bool checkCall=false;
           
                for (int i = 0; i < AgentPopulation.deActivatedAgentList.Count; i++)
                {
                    if (AgentPopulation.deActivatedAgentList[i] != null)
                    {
                        if (AgentPopulation.deActivatedAgentList[i].neighbours.Count == 0 || AgentPopulation.deActivatedAgentList[i].neighbours.Count == 1 || AgentPopulation.deActivatedAgentList[i].neighbours.Count == 2)
                        {
                            data.Add(AgentPopulation.deActivatedAgentList[i]);
                        }
                    }
                }
            if (data.Count == 0)
            {

                for (int i = 0; i < AgentPopulation.deActivatedAgentList.Count; i++)
                {
                    if (AgentPopulation.deActivatedAgentList[i] != null)
                    {
                        if (AgentPopulation.deActivatedAgentList[i].neighbours.Count == 0 || AgentPopulation.deActivatedAgentList[i].neighbours.Count == 1 || AgentPopulation.deActivatedAgentList[i].neighbours.Count == 2
                        || AgentPopulation.deActivatedAgentList[i].neighbours.Count == 3)
                        {
                            data.Add(AgentPopulation.deActivatedAgentList[i]);
                        }
                    }
                }

                checkCall = CheckMethodCall();

            }

            if (data.Count == 0 && checkCall == true)
            {
                for (int i = 0; i < AgentPopulation.deActivatedAgentList.Count; i++)
                {
                    if (AgentPopulation.deActivatedAgentList[i] != null)
                    {
                        if (AgentPopulation.deActivatedAgentList[i].neighbours.Count == 4 || AgentPopulation.deActivatedAgentList[i].neighbours.Count == 5)

                        {
                            data.Add(AgentPopulation.deActivatedAgentList[i]);
                        }
                    }
                }



            }

            if (data.Count == 0) Debug.LogError("GetDeacTivatedAgentsWithingNeighbourRange() returned an empty list!!!"); //throw new System.Exception("GetDeacTivatedAgentsWithingNeighbourRange() returned an empty list!!!");
            return data;
        }

        private bool CheckMethodCall()
        {
            return true;
        }


/// <summary>
/// Scale Down PlaceHolders which have been set to "DeActicated"
/// </summary>
public void DisplayDeActivatedPlaceHolders()
        {
            for (int i = 0; i < selectedActivatedPlaceHolders.Count; i++)
            {
                if (selectedActivatedPlaceHolders[i].tag == "DeActivatedPlaceHolder")
                {
                    selectedActivatedPlaceHolders[i].gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                }
            }


        }




        /// <summary>
        /// Evaluates local density based on radial neighbour search
        /// </summary>
        /// <param name="placeHolder"></param>
        /// <param name="visionRadius"></param>
        /// <param name="dataToSearch"></param>
        /// <returns></returns>
        public KdTree<Agent> GetNeighboursSpherical(PlaceHolder placeHolder, float visionRadius, KdTree<Agent> dataToSearch)
        {

            KdTree<Agent> neighbours = new KdTree<Agent>();

            foreach (Agent neighbour in dataToSearch)
            {
                if (neighbour.tag != "Deactivated" && neighbour.tag != "SignalReceiver" && neighbour.tag != "Cancelled" && neighbour.name != this.gameObject.name)
                {
                    if (neighbour != placeHolder && Vector3.Distance(neighbour.transform.position, placeHolder.transform.position) < visionRadius)
                    {
                        

                        neighbours.Add(neighbour);
                    }
                }
            }



            return neighbours;

        }



        /// <summary>
        /// return list of placeHolders
        /// </summary>
        /// <param name="placeHolderPositions"></param> Vector positions on instantiate place holders
        ///  <returns></returns> Returns a list of place holder game objects 
        ///  
        public List<PlaceHolder> GetPlaceHolderList(List<Vector3> placeHolderPositions)
        {
            List<PlaceHolder> placeHolderList = new List<PlaceHolder>();

            if (SimulationManager.Get().is2D)
            {
                if (SimulationManager.Get().gridDistribution_6Neighbours)
                {


                    for (int i = 0; i < placeHolderPositions.Count; i++)
                    {

                        placeHolderList.Add((Instantiate(SimulationManager.Get().placeHolderPrefab.GetComponent<PlaceHolder>(), placeHolderPositions[i], Quaternion.identity)));

                    }
                }

                if (SimulationManager.Get()._12Neighbours)
                {

                    for (int i = 0; i < placeHolderPositions.Count; i++)
                    {

                        placeHolderList.Add((Instantiate(SimulationManager.Get().placeHolderPrefab.GetComponent<PlaceHolder>(), placeHolderPositions[i], Quaternion.identity)));

                    }

                }

            }



            if (SimulationManager.Get().is3D)
            {
                if (SimulationManager.Get().gridDistribution_6Neighbours)
                {


                    for (int i = 0; i < placeHolderPositions.Count; i++)
                    {

                        placeHolderList.Add((Instantiate(SimulationManager.Get().placeHolderPrefab.GetComponent<PlaceHolder>(), placeHolderPositions[i], Quaternion.identity)));

                    }
                }

                if (SimulationManager.Get()._12Neighbours)
                {

                    for (int i = 0; i < placeHolderPositions.Count; i++)
                    {

                        placeHolderList.Add((Instantiate(SimulationManager.Get().placeHolderPrefab.GetComponent<PlaceHolder>(), placeHolderPositions[i], Quaternion.identity)));

                   

                    }



                }

            }




            return placeHolderList;
        }


        public List<Vector3> Draw2dCircle2(Vector3 origin)
        {


            int nPts = 50;
            float angCount = Mathf.PI * 2 / nPts; // radians
            float theta = 0;
            float cosTheta = 0;
            float sinTheta = 0;
            float radius = 0.4f;

          //  if (obj.tag == "DeActivatedPlaceHolder") radius = 0.1f;

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


        /// <summary>
        /// Set initial state of Place holders ; i.e tag
        /// </summary>
        public void InitializeState(List<PlaceHolder> placeHolders)
        {

            for (int i = 0; i < placeHolders.Count; i++)
            {
                placeHolders[i].tag = "ActivatedPlaceHolder";

            }


        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="offset"></param>
        private void InstantiatePlaceHolders(Agent agent, float offset, DirectionalityRules rules)
        {


            List<Vector3> placeHolderPositions = CreatePlaceHolderVectorPositions(agent, offset, neighbours, rules);
            placeHolderLocalList = GetPlaceHolderList(placeHolderPositions);


        }


      




        
        /// <summary>
        /// This method enables each placeHolder to decide which one is going to be active to select a signal receiver 
        /// using a specified density rule and a search radius.
        /// </summary>
        /// <param name="densityRule"></param>
        /// <param name="searchRadius"></param>
        public void PlaceHolderBehavior(DensityRules densityRule, float searchRadius)
        {

            foreach (Transform child in transform)
            {
                placeHolders.Add(child.GetComponent<PlaceHolder>());
            }

            if (this.gameObject.name == "Agent" + " " + AgentPopulation.indexActivatedAgentsList[0].ToString())
            {
                if (SimulationManager.Get().randomInitialConditions) communicationType = UnityEngine.Random.Range(1, 12);
                else
                    communicationType = UnityEngine.Random.Range(12, 12);

                selectedActivatedPlaceHolders = placeHolders.ListSlice(0, communicationType - 1);

              

            }

            else
            {


                for (int i = 0; i < placeHolders.Count; i++)
                {
                    if (densityRule == DensityRules.RuleA)
                    {
                        RuleA(placeHolders[i], searchRadius,0);
                    }

                    if (densityRule == DensityRules.RuleB)
                    {
                        RuleB(placeHolders[i], searchRadius, 1);
                    }

                }

   

                selectedActivatedPlaceHolders = placeHolders;

                DisplayDeActivatedPlaceHolders();

             


            }







        }



        public List<Vector3> Draw2dCircle2(Vector3 origin, PlaceHolder obj)
        {


            int nPts = 50;
            float angCount = Mathf.PI * 2 / nPts; // radians
            float theta = 0;
            float cosTheta = 0;
            float sinTheta = 0;
            float radius = 0.4f;

            if (obj.tag == "DeActivatedPlaceHolder") radius = 0.1f;

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



        /// <summary>
        /// The corresponding agent will become the parent of its place holders
        /// </summary>
        /// 
        public void SetPlaceHolderParent()
        {
           
            for (int i = 0; i < AgentPopulation.populationList.Count; i++)
            {
                if (AgentPopulation.populationList[i].name == "Agent" + " " + i)
                {

                    for (int j = 0; j < placeHolderLocalList.Count; j++)
                    {
                        placeHolderLocalList[j].transform.parent = this.gameObject.transform;

                    }

                }
            }

        }

        /// <summary>
        /// Set the place holders name according to each agent
        /// </summary>

        public void SetPlaceHolderName()
        {
            for (int i = 0; i < AgentPopulation.populationList.Count; i++)
            {
                for (int j = 0; j < placeHolderLocalList.Count; j++)
                {

                    if (this.gameObject.name == "Agent" + " " + i)
                    {
                        placeHolderLocalList[j].PlaceHoldername = "Place Holder" + " " + i + "." + j;
                        placeHolderLocalList[j].name = "Place Holder" + " " + i + "." + j;
                    }

                }

            }
        }




       


   



    

        public void SetMaterial()
        {
            if (placeHolderLocalList.Count != 0)
            {
                for (int i = 0; i < placeHolderLocalList.Count; i++)
                {
                   placeHolderLocalList[i].GetComponent<MeshRenderer>().enabled = true;

                    placeHolderLocalList[i].GetComponent<MeshRenderer>().material.color = Color.black;

                }

                
            }

        }

      






    
    







        /// <summary>
        /// Methods will set PlaceHolder to "DeActivatedPlaceHolder" if its neighbour density is > densityThreshold
        /// </summary>
        /// <param name="placeHolder"></param> 
        /// <param name="densityThreshold"></param>
        public void RuleA(PlaceHolder placeHolder, float searchRadius,int densityThreshold = 0)

        {

           
            KdTree<Agent> neighbourList = GetNeighboursSpherical(placeHolder, searchRadius, AgentPopulation.populationList);
            List<string> neighbourListNames = new List<string>();

            for (int i = 0; i < neighbourList.Count; i++)
            {
                neighbourListNames.Add(neighbourList[i].name);
            }

          
            placeHolder.neighbourNames = neighbourListNames;
          

            if (neighbourList.Count > densityThreshold)
            {
                placeHolder.tag = "DeActivatedPlaceHolder";


            }



        }


        public void RuleB(PlaceHolder placeHolder, float searchRadius, int densityThreshold = 1)

        {


            KdTree<Agent> neighbourList = GetNeighboursSpherical(placeHolder, searchRadius, AgentPopulation.populationList);
            List<string> neighbourListNames = new List<string>();

            for (int i = 0; i < neighbourList.Count; i++)
            {
                neighbourListNames.Add(neighbourList[i].name);
            }


            placeHolder.neighbourNames = neighbourListNames;


            if (neighbourList.Count > densityThreshold)
            {
                placeHolder.tag = "DeActivatedPlaceHolder";


            }

            else  placeHolder.tag = "ActivatedPlaceHolder";
                



        }


       
        
        #endregion

    }
}
