using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


using SharpMatter.SharpMath;
using SharpMatter.SharpDataStructures;
using SharpMatter.Geometry;

using ClassExtensions;
using OrganizationalModel.Population;
using OrganizationalModel.BaseClass;
using OrganizationalModel.Managers;
using OrganizationalModel.UtilityFunctions;

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

namespace OrganizationalModel.Behaviors
{
    public partial class OrganizationBehavior : Agent, IOrganization<Agent>
    {




        #region AGENT BEHAVIOR CONDITIONS


        //public void DeActivatedPassiveAgentConditions()
        //{
        //    if (this.gameObject.tag == "De-ActivatedPassive")
        //    {

        //        StartCoroutine(ApplyWindForce(SimulationManager.Get().windForce, SimulationManager.Get().windForceIntervalSeconds));
        //    }


        //}


        IEnumerator ApplyWindForceSignalReceiver(float force, int interval)
        {
            //Vector3 wind = new Vector3(1, 2, 3);


            if (SimulationManager.Get().is2D)
            {
                Vector3 wind = new Vector3(UnityEngine.Random.onUnitSphere.x, 0, UnityEngine.Random.onUnitSphere.z);
                wind.Normalize();

                wind *= force;

                base.IApplyForces(wind);


                yield return new WaitForSeconds(interval);
            }

            if (SimulationManager.Get().is3D)
            {
               
                 Vector3 wind = new Vector3(5f, 5f,5f);
           
                wind.Normalize();

                wind *= force;

                base.IApplyForces(wind);


                yield return new WaitForSeconds(interval);
            }
        }


            IEnumerator ApplyWindForce(float force, int interval)
        {
            //Vector3 wind = new Vector3(1, 2, 3);


            if (SimulationManager.Get().is2D)
            {
                Vector3 wind = new Vector3(UnityEngine.Random.onUnitSphere.x, 0, UnityEngine.Random.onUnitSphere.z); 
                wind.Normalize();

                wind *= force;

                base.IApplyForces(wind);


                yield return new WaitForSeconds(interval);
            }

            if(SimulationManager.Get().is3D)
            {
                // Vector3 wind = new Vector3(UnityEngine.Random.onUnitSphere.x, UnityEngine.Random.onUnitSphere.y, UnityEngine.Random.onUnitSphere.z);
                // Vector3 wind = new Vector3(5f, 5f,5f);
                Vector3 randomVal = UnityEngine.Random.onUnitSphere;
                Vector3 wind = new Vector3(randomVal.x, randomVal.y, randomVal.z);
                //wind.Normalize();

              //  wind *= force;

                //base.IApplyForces(wind);
                ApplyForcesRigidBody(wind, force);


                yield return new WaitForSeconds(interval);
            }


        }


        //public void DeActivatedPassiveAgentConditions()
        //{
        //    if (this.gameObject.tag == "De-ActivatedPassive")
        //    {

        //        StartCoroutine(ApplyWindForce(SimulationManager.Get().windForce, SimulationManager.Get().windForceIntervalSeconds));
        //    }


        //}


       public void DeActivatedAgentConditions()
        {
            if (this.gameObject.tag == "De-Activated")
            {
                //////////////////// set display////////////////////////////////

                base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false);

                //////////////////// set display////////////////////////////////
            }
        }


        public void FreezedAgentConditions()
        {
            if (this.gameObject.tag == "Freezed")
            {
                if (SimulationManager.Get().oneGeneration && AgentPopulation.freezedAgentList.Count / ((float)AgentPopulation.populationList.Count - AgentPopulation.cancelledAgentList.Count) * 100 < 97)
                {
                    base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
                }


                if (SimulationManager.Get().addWindForce == false) displacementPositions.Clear();

                Cell _currentScalarValue;
                if (SimulationManager.Get().ScalarField2d)
                {
                    ScalarFieldDataLookUp(SF, out _currentScalarValue);

                    base.currentScalarCell = _currentScalarValue.CellName;
                    base.currentScalarValueRules = _currentScalarValue.ScalarValueRules;
                    base.currentScalarValuePackingProximity = _currentScalarValue.ScalarValueProximity;
                    base.currentScalarValueHumanData = _currentScalarValue.ScalarValueHumanData;
                    
                }


                /////////////////////////// Display COLOR BY EMOTION////////////////////////////



                //if (AgentPopulation.freezedAgentList.Count == AgentPopulation.populationList.Count)
                //{

                //    SimulationManager.Get().displayColorByEmotion = true;
                //}

                //if (SimulationManager.Get().displayColorByEmotion)
                //{

                //    DisplayEmotion();
                //}


                /////////////////////////// Display COLOR BY EMOTION////////////////////////////

           
                   
              
                base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());

                if (SimulationManager.Get().addWindForce == false && SimulationManager.Get().addaptToEnvironmentalForces == false )
                {
                    //////////////////// set display////////////////////////////////

                 // base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false, false);

            //    if(SimulationManager.Get().displayColorByDistanceToNeighbours)
              //      {
                        base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false);
               //     }

                    //////////////////// set display////////////////////////////////
                }

                if (SimulationManager.Get().addWindForce && SimulationManager.Get().addaptToEnvironmentalForces==false)
                {


                    StartCoroutine(ApplyWindForce(SimulationManager.Get().windForce, SimulationManager.Get().windForceIntervalSeconds));

                    //////////////////// set display////////////////////////////////

                    base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false);

               

                    //////////////////// set display////////////////////////////////

                    if (SimulationManager.Get().windDisplacementAnalysis) // to not save to memmory all positions per update frame
                    {
                        displacementPositions.Add(this.gameObject.transform.position);




                        this.totalDisplacement = base.GetTotalDisplacement(displacementPositions);

                        /// Get displacement of all neighbours
                        for (int i = 0; i < base.neighbours.Count; i++)
                        {
                            if (base.neighbours[i] != null)
                            {
                                neighboursDisplacement.Add(neighbours[i].GetComponent<OrganizationBehavior>().totalDisplacement);

                            }
                        }

                        float totalNeighboursDisplacement = neighboursDisplacement.Sum();
                        this.averegeNeighbourDisplacement = totalNeighboursDisplacement / base.neighbours.Count;

                        //////////////////// set display////////////////////////////////


                       // base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), true, false, this.totalDisplacement);

                        base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), true, this.totalDisplacement);

                        //////////////////// set display////////////////////////////////
                    }



                }

               
                if (SimulationManager.Get().addWindForce && SimulationManager.Get().addaptToEnvironmentalForces)
                {

                    this.createPlaceHolders = false;
                    this.searchClosestNeighbour = false;
                    this.time = 0;

                    StartCoroutine(ApplyWindForce(SimulationManager.Get().windForce, SimulationManager.Get().windForceIntervalSeconds));

                    displacementPositions.Add(this.gameObject.transform.position);

                  

                    /// Get displacement of all neighbours
                    for (int i = 0; i < base.neighbours.Count; i++)
                    {
                        if (base.neighbours[i] != null)
                        {
                            neighboursDisplacement.Add(neighbours[i].GetComponent<OrganizationBehavior>().totalDisplacement);

                        }
                    }

                    this.totalDisplacement = base.GetTotalDisplacement(displacementPositions);

                    float totalNeighboursDisplacement = neighboursDisplacement.Sum();
                    this.averegeNeighbourDisplacement = totalNeighboursDisplacement / base.neighbours.Count;


                    FixedJoint[] hingeJoints = GetComponents<FixedJoint>();
                    if (this.averegeNeighbourDisplacement > neighBourDisplacementThreshold && AgentPopulation.deActivatedAgentList.Count!=0 && hingeJoints.Length<5)
                    {
                        base.ChangeStateToSignalEmmiter(this.gameObject.GetComponent<Agent>(), 0);

                    }
                    //////////////////// set display////////////////////////////////

                    base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false, this.totalDisplacement);

                    //////////////////// set display////////////////////////////////

                }




                for (int i = base.placeHolderLocalList.Count - 1; i >= 0; i--)
                {

                    if (base.placeHolderLocalList[i] != null)
                    {
                        Destroy(placeHolderLocalList[i].gameObject);
                        base.placeHolderLocalList.RemoveAt(i);

                    }


                }


                for (int i = base.placeHolders.Count - 1; i >= 0; i--)
                {

                    if (base.placeHolders[i] != null)
                    {
                        Destroy(placeHolders[i].gameObject);
                        base.placeHolders.RemoveAt(i);

                    }


                }

                base.placeHolderLocalList.Clear();
                base.placeHolders.Clear();
                base.mySignalReceiversTemp.Clear();
            }


        }

        public void ISignalEmmiterConditions()
        {

            

            if (this.gameObject.tag == "SignalEmmiter")
            {

                if (SimulationManager.Get().oneGeneration && AgentPopulation.freezedAgentList.Count / ((float)AgentPopulation.populationList.Count - AgentPopulation.cancelledAgentList.Count) * 100 < 97)
                {
                    base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
                }

                //if (SimulationManager.Get().displayColorByEmotion)
                //{

                //    DisplayEmotion();
                //}

                if (SimulationManager.Get().addWindForce == false) displacementPositions.Clear();


                if (SimulationManager.Get().addWindForce)
                {
                    string[] separators = { ".", " " };
                    string value = this.gameObject.name;
                    string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    string targetEmmiterAgentIndex = words[1]; /// THIS WILL ALWAYS OUTPUT A NUMBER ---> EXAMPLE: Place Holder 100.1 it will output 100
                    int index = Convert.ToInt32(targetEmmiterAgentIndex);

                   
                    if (AgentPopulation.indexActivatedAgentsList.Count != 0)
                    {
                        if (AgentPopulation.indexActivatedAgentsList[0] != index)
                        {
                            //StartCoroutine(ApplyWindForce(SimulationManager.Get().windForce, SimulationManager.Get().windForceIntervalSeconds)); // Used to update object with my physics engine

                            StartCoroutine(ApplyWindForce(SimulationManager.Get().windForce, SimulationManager.Get().windForceIntervalSeconds)); // used to update agent with Rigid body physics AddForce()

                            // StartCoroutine(ApplyWindForce(7, SimulationManager.Get().windForceIntervalSeconds)); // used to update agent with Rigid body physics AddForce()
                        }
                    }



                }

                // if (SimulationManager.Get().addWindForce == false && SimulationManager.Get().addaptToEnvironmentalForces == false)
                //  {
                //////////////////// set display////////////////////////////////
                if(SimulationManager.Get().windDisplacementAnalysis==false) base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false);
               

                //////////////////// set display////////////////////////////////
                //  }



                time++;

         
                base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());


                base.mySignalReceivers = base.mySignalReceiversTemp.Distinct().ToList();

           


                signalEmitterCounter++;
               


                if (createPlaceHolders == false && time > 10)
                {



                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    Cell _currentScalarValue;


               

                    if (SimulationManager.Get().ScalarField2d)
                    {
                        ScalarFieldDataLookUp(SF, out _currentScalarValue);

                        base.currentScalarCell = _currentScalarValue.CellName;
                        base.currentScalarValueRules = _currentScalarValue.ScalarValueRules;
                        base.currentScalarValuePackingProximity = _currentScalarValue.ScalarValueProximity;
                        base.currentScalarValueHumanData = _currentScalarValue.ScalarValueHumanData;
                        //  placeHolderOffset = base.currentScalarValueRules * 0.4f;
                        //  if (placeHolderOffset < 1) placeHolderOffset = 0.0f; // 0.30 is the index[0] of domain.B when scaled to * 0.4


                       // placeHolderOffset = currentScalarValueRules * .4f;
                         placeHolderOffset = 0.0f;
                    }
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



                   



                    this.gameObject.GetComponent<SphereCollider>().radius = this.gameObject.GetComponent<SphereCollider>().radius + placeHolderOffset;






                    if (SimulationManager.Get().singleRule)
                    {
                        CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule18);

                        if (SimulationManager.Get().addWindForce && SimulationManager.Get().addaptToEnvironmentalForces)
                        {

                            if (this.averegeNeighbourDisplacement > neighBourDisplacementThreshold)
                            {
                                CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule01);
                            }

                        }





                    }


                    if (SimulationManager.Get().multipleRules)
                    {

                        if (SimulationManager.Get().radialScalarField)
                        {

                            //List<float> values = new List<float>();

                            //for (int i = 0; i < ScalarFields.ScalarField2D.scalarFieldValuesProximity.GetLength(0); i++)
                            //{
                            //    for (int j = 0; j < ScalarFields.ScalarField2D.scalarFieldValuesProximity.GetLength(1); j++)
                            //    {
                            //        values.Add(ScalarFields.ScalarField2D.scalarFieldValuesProximity[i, j]);

                            //    }
                            //}

                            //values.Sort();

                            //float minVal = values[0];
                            //float maxVal = values[values.Count-1];

                            //float remapProximityValues = SharpMath.Remap(base.currentScalarValuePackingProximity, minVal, maxVal, 0, 1);



                            ////   if (base.currentScalarValueRules >= domains[1].A && base.currentScalarValueRules < domains[1].B && this.gameObject.transform.position.y<10.2 && this.gameObject.transform.position.y >= 9.8) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule19);

                            //if (remapProximityValues<=0.25f) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule01);
                            //if (remapProximityValues > 0.25f && remapProximityValues < 0.50f) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule18);
                            //if (remapProximityValues >= 0.5f && remapProximityValues < 0.75f) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule10);
                            //if (remapProximityValues >= 0.75) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule03);






                        }






                        if (SimulationManager.Get().interpolatedScalarField)
                        {
                            if (SimulationManager.Get().addWindForce && SimulationManager.Get().addaptToEnvironmentalForces)
                            {
                                List<VectorData> domains = VectorData.ConvertStringToVectorData(SimulationManager.Get().ImportToUnityDomainsFilePath);


                                if (this.averegeNeighbourDisplacement < neighBourDisplacementThreshold)
                                {
                                    if (base.currentScalarValueRules >= domains[0].A && base.currentScalarValueRules < domains[0].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule04);

                                    if (base.currentScalarValueRules >= domains[1].A && base.currentScalarValueRules < domains[1].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule18);

                                    if (base.currentScalarValueRules >= domains[2].A && base.currentScalarValueRules < domains[2].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule01);

                                    if (base.currentScalarValueRules >= domains[3].A && base.currentScalarValueRules <= domains[3].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule03);
                                }

                                if (this.averegeNeighbourDisplacement > neighBourDisplacementThreshold)
                                {
                                    CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule03);
                                }

                                
                            }

                            else
                            {
                                List<VectorData> domains = VectorData.ConvertStringToVectorData(SimulationManager.Get().ImportToUnityDomainsFilePath);

                                if (base.currentScalarValueRules >= domains[0].A && base.currentScalarValueRules < domains[0].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule12);

                                if (base.currentScalarValueRules >= domains[1].A && base.currentScalarValueRules < domains[1].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule26);
                              

                              

                                if (base.currentScalarValueRules >= domains[2].A && base.currentScalarValueRules < domains[2].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule15);

                                if (base.currentScalarValueRules >= domains[3].A && base.currentScalarValueRules <= domains[3].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule16);


                            }




                        }




                    }



                    /// ADDING THIS PINS SIGNAL EMMITTERS!!!!!!!!!!!!!!!
                    if (SimulationManager.Get().addFixedJoints)
                    {
                        for (int i = 0; i < placeHolderLocalList.Count; i++)
                        {
                            base.AddFixedJointPlaceHolders(placeHolderLocalList[i], 20);
                        }
                    }


                    createPlaceHolders = true;

                }

                

                /// Only search closest neighbour once and search the first closest neighbour after n seconds
                /// time > AgentPopulation.timeToInitFirstAgent * 20 &&
                if (this.searchClosestNeighbour == false && time > 50 && AgentPopulation.deActivatedAgentList.Count > 0)
                {
                   

                    if (SimulationManager.Get().addWindForce == false)
                    {
                        float maxThreshold = 1.65f;

                        base.currentScalarValueDensityCheck = base.currentScalarValueRules;
                        if (base.currentScalarValueDensityCheck > maxThreshold) base.currentScalarValueDensityCheck = maxThreshold;
                        if (base.currentScalarValueDensityCheck < 1.1f) base.currentScalarValueDensityCheck = 1.1f;

                       // base.PlaceHolderBehavior(DensityRules.RuleA, base.currentScalarValueDensityCheck); //0.9f   //1.2f    1.65f    1.5f 

                       base.PlaceHolderBehavior(DensityRules.RuleA, 1.0f);




                    }



                    if (SimulationManager.Get().addWindForce == true && SimulationManager.Get().addaptToEnvironmentalForces ==false)
                    {
                        // get my total displacement
                        displacementPositions.Add(this.gameObject.transform.position);


                        this.totalDisplacement = base.GetTotalDisplacement(displacementPositions);

          

                        float maxThreshold = 1.68f;

                        base.currentScalarValueDensityCheck = base.currentScalarValueRules;
                        if (base.currentScalarValueDensityCheck > maxThreshold) base.currentScalarValueDensityCheck = maxThreshold;
                        if (base.currentScalarValueDensityCheck < 1.15f) base.currentScalarValueDensityCheck = 1.1f;

                        base.PlaceHolderBehavior(DensityRules.RuleA, base.currentScalarValueDensityCheck); //0.9f   //1.2f    1.65f    1.5f 

                        //base.PlaceHolderBehavior(DensityRules.RuleA, 1.2f); //0.9f   //1.2f    1.65f    1.5f 

                        if (SimulationManager.Get().windDisplacementAnalysis == true) base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), true, this.totalDisplacement);
                    }




                    if (SimulationManager.Get().addWindForce && SimulationManager.Get().addaptToEnvironmentalForces)
                    {
                       // get my total displacement
                        displacementPositions.Add(this.gameObject.transform.position);

                        // get neighbours averege displacement
                        for (int i = 0; i < base.neighbours.Count; i++)
                        {
                            if (base.neighbours[i] != null)
                            {
                                neighboursDisplacement.Add(neighbours[i].GetComponent<OrganizationBehavior>().totalDisplacement);

                            }
                        }

                        this.totalDisplacement = base.GetTotalDisplacement(displacementPositions);

                        float totalNeighboursDisplacement = neighboursDisplacement.Sum();
                        this.averegeNeighbourDisplacement = totalNeighboursDisplacement / base.neighbours.Count;



                        // if my current displacement is > than the averege sum of my neighbours displacement

                        if(this.averegeNeighbourDisplacement > neighBourDisplacementThreshold) // choose my minimum density check value to be tighly packed
                        {
                            base.currentScalarValueDensityCheck = 0.90f;
                            base.PlaceHolderBehavior(DensityRules.RuleA, base.currentScalarValueDensityCheck);
                        }

                        else // use heat map to read density values
                        {
                            float maxThreshold = 1.68f;

                            base.currentScalarValueDensityCheck = base.currentScalarValueRules;
                            if (base.currentScalarValueDensityCheck > maxThreshold) base.currentScalarValueDensityCheck = maxThreshold;
                            base.PlaceHolderBehavior(DensityRules.RuleA, base.currentScalarValueDensityCheck);
                        }
                    }



                    //if (base.currentScalarValuePackingProximity > maxThreshold) base.currentScalarValuePackingProximity = maxThreshold;
                    //if (base.currentScalarValuePackingProximity < 1.26f) base.currentScalarValuePackingProximity = 0.90f;




                    //  base.PlaceHolderBehavior(DensityRules.RuleA, base.currentScalarValuePackingProximity); //0.9f   //1.2f    1.65f    1.5f 


                    // base.PlaceHolderBehavior(DensityRules.RuleA,1.2f);

                    if (SimulationManager.Get().windDisplacementAnalysis == true) base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), true, this.totalDisplacement);
                    base.ActivateAgents();



                
                    this.searchClosestNeighbour= true;

                }


                if (placeHolders.Count != 0)
                {
                    if (SimulationManager.Get().drawPlaceHolders2D)
                    {
                        //  Color color;

                        Color color = Color.white;



                        for (int i = 0; i < placeHolders.Count; i++)
                        {
                            List<Vector3> ptsCirc = Draw2dCircle(placeHolders[i].transform.position, placeHolders[i]); // get list of vector points on circle for each place holder

                            //  if (placeHolders[i].tag == "DeActivatedPlaceHolder") color = Color.red;
                            //    else color = Color.white;

                            for (int j = 0; j < ptsCirc.Count - 1; j++)
                            {
                                //if (j == ptsCirc.Count)
                                //{
                                //    Debug.DrawLine(ptsCirc[j], ptsCirc[0], color);
                                //}
                                Debug.DrawLine(ptsCirc[j], ptsCirc[j + 1], color);

                                //if (j == ptsCirc.Count-1)
                                //{
                                //    Debug.DrawLine(ptsCirc[j], ptsCirc[j+1], color);
                                //}
                            }
                        }
                    }




                }


                if (mySignalReceivers.Count == 1 && mySignalReceivers[0].tag == "Freezed" && searchClosestNeighbourAgain == false)
                {
                    int activatedPlaceHolderCount = 0;
                    for (int i = 0; i < placeHolders.Count; i++)
                    {
                        if (placeHolders[i] != null)
                        {
                            if (placeHolders[i].tag == "ActivatedPlaceHolder")
                            {


                                activatedPlaceHolderCount++;

                            }
                        }
                    }

                    if (activatedPlaceHolderCount == 1)
                    {
                        mySignalReceivers.Clear();
                        mySignalReceiversTemp.Clear();


                        base.ActivateAgents();
                        searchClosestNeighbourAgain = true;
                    }
                }



                if (mySignalReceivers.Count == 2 && mySignalReceivers[0].tag == "Freezed" && mySignalReceivers[1].tag == "Freezed" && searchClosestNeighbourAgain == false)
                {
                    int activatedPlaceHolderCount = 0;
                    for (int i = 0; i < placeHolders.Count; i++)
                    {
                        if (placeHolders[i] != null)
                        {
                            if (placeHolders[i].tag == "ActivatedPlaceHolder")
                            {


                                activatedPlaceHolderCount++;

                            }
                        }
                    }

                    if (activatedPlaceHolderCount == 2)
                    {
                        mySignalReceivers.Clear();
                        mySignalReceiversTemp.Clear();


                        base.ActivateAgents();
                        searchClosestNeighbourAgain = true;
                    }
                }

                /////////////////// IF ALL MY PLACEHOLDERS ARE DEACTIVATED CHANGE MY STATE TO AGENT  /////////////////// 

                if (searchClosestNeighbour == true && createPlaceHolders == true)
                {
                    int DeactivatedPlaceHolderCount = 0;
                    int totalPlaceHolders = placeHolders.Count;

                    for (int i = 0; i < placeHolders.Count; i++)
                    {
                        if (placeHolders[i] != null)
                        {
                            if (placeHolders[i].tag == "DeActivatedPlaceHolder")
                            {
                                DeactivatedPlaceHolderCount++;
                            }

                        }
                    }

                    if (DeactivatedPlaceHolderCount == placeHolders.Count)
                    {
                        base.ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);

                        //if (SimulationManager.Get().addaptToEnvironmentalForces == false) base.ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);
                        //if (SimulationManager.Get().addaptToEnvironmentalForces == true) base.ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);//base.ChangeStateToDeactivatedPassive(this.gameObject.GetComponent<Agent>(), 0);

                    }
                }


                ////// If one of my neighbours neighbour is one of my signal receivers///// change my state to freezed. This is intended to be used when wind is activated. Due to causes on impresisions.
                //// BUT THIS IS THROWING OUT A LOT OF CANCELLED AGENTS!!!

                if (SimulationManager.Get().addRigidBodyCollider && SimulationManager.Get().addWindForce)
                {
                    if (searchClosestNeighbour == true && createPlaceHolders == true && AgentPopulation.deActivatedAgentList.Count == 0)
                    {
                        // int count = 0;
                        //for (int i = 0; i < base.neighbours.Count; i++)
                        //{
                        //    if (base.neighbours[i] != null)
                        //    {
                        //        List<Agent> myNeighboursNeighbours = base.neighbours[i].neighbours;

                        //        foreach (var item in myNeighboursNeighbours)
                        //        {
                        //            if (item != null)
                        //            {
                        //                foreach (var receiver in mySignalReceivers)
                        //                {
                        //                    if (receiver != null)
                        //                    {
                        //                        if (item.name == receiver.name)
                        //                        {
                        //                            print("I am " + " " + gameObject.name + " " + " and Found my lost receiver!");
                        //                            base.ChangeStateToFreezed(receiver, 0);
                        //                           // count++;

                        //                        }
                        //                    }
                        //                }
                        //            }
                        //        }

                        //    }
                        //}

                        ////if (count >= 1) base.ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);

                        //List<Agent> neighbours = base.FindNeighbours(this.gameObject.GetComponent<Agent>(), 1.2f, AgentPopulation.populationList);
                        //foreach (var receiver in mySignalReceivers)
                        //{
                        //    if (receiver != null)
                        //    {
                        //        foreach (var neighbour in neighbours)
                        //        {
                        //            if (neighbour != null)
                        //            {
                        //                if (neighbour.name == receiver.name)
                        //                {
                        //                    base.ChangeStateToFreezed(receiver, 0);

                        //                }
                        //            }
                        //        }
                        //    }
                        //}

                        int countFreezedReceivers = 0;
                        /// check how many of my receivers are freezed
                        foreach (var item in mySignalReceivers)
                        {
                            if (item.tag == "Freezed") countFreezedReceivers++;


                        }

                        // if all my receivers are freezed i will change my state to freezed
                        if (countFreezedReceivers == mySignalReceivers.Count) base.ChangeStateToFreezed(gameObject.GetComponent<Agent>(), 0);

                    }
                }

                ////// If one of my neighbours neighbour is one of my signal receivers///// change my state to freezed. This is intended to be used when wind is activated. Due to causes on impresisions.




                if (searchClosestNeighbour == true && createPlaceHolders == true)
                {
                    base.communicationType = base.mySignalReceivers.Count;

                }

              
                if (searchClosestNeighbour == true && createPlaceHolders == true && placeHolders.Count == 0 && AgentPopulation.deActivatedAgentList.Count == 0)
                {
                 
                        base.ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);
                
                }


               
            }

    

        }// END METHOD






        public void ISignalReceiverConditions()
        {

            if (this.transform.tag == "SignalReceiver")
            {

                if (SimulationManager.Get().oneGeneration && AgentPopulation.freezedAgentList.Count / ((float)AgentPopulation.populationList.Count - AgentPopulation.cancelledAgentList.Count) * 100 < 97)
                {
                    base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
                }

                //if (SimulationManager.Get().displayColorByEmotion)
                //{

                //    MaterialPropertyBlock properties = new MaterialPropertyBlock();
                //    MeshRenderer meshR = gameObject.GetComponent<MeshRenderer>();
                //    meshR.enabled = true;
                //    float r = SimulationManager.Get().monochromeColor.r;
                //    float g = SimulationManager.Get().monochromeColor.g;
                //    float b = SimulationManager.Get().monochromeColor.b;

                //    properties.SetColor("_Color", new Color(r, g, b));
                //    if (meshR) meshR.SetPropertyBlock(properties);
                //}

                //////////////////// set display////////////////////////////////

                base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false);

                //////////////////// set display////////////////////////////////

                if (SimulationManager.Get().exportTrailPaths)
                {
                    base.trailData.Add(this.gameObject.transform.position);
                }


               this.gameObject.GetComponent<SphereCollider>().enabled = false;
            

                base.neighboursTemp.Clear();
                base.neighbours.Clear();

                //Pixel currentPixel;
                //base.PixelDataLookUp3D(pixelPop, out currentPixel);

                //currentPixel.MobileAgentCounter();
                //currentPixel.AddSignalreceiverName(this.gameObject.name);
                //base.currentPixel = currentPixel.PixelName;










                PlaceHolder target = base.placeHolderTargetForSignalReceiver;

                if (addPlaceHolderTargetHistoryCoroutine == false)
                {
                    base.palceHolderTargetHistoryList.Add(target);
                    addPlaceHolderTargetHistoryCoroutine = true;
                }




                //if (target == null)
                //{


                //    this.gameObject.tag = "Cancelled";
                //    AgentPopulation.totalCancelledAgents++;
                //    this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                //    this.gameObject.SetActive(false);


                //}










                if (target != null)
                {
                    /// RETRIEVE SIGNAL EMMITER
                    string[] separators = { ".", " " };
                    string value = this.gameObject.GetComponent<OrganizationBehavior>().placeHolderTargetForSignalReceiver.name;
                    string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    string targetEmmiterAgentIndex = words[2]; /// THIS WILL ALWAYS OUTPUT A NUMBER ---> EXAMPLE: Place Holder 100.1 it will output 100
                    int index = Convert.ToInt32(targetEmmiterAgentIndex);
                    base.mySignalEmmiter = AgentPopulation.populationList[index];


                    this.distanceToTarget = Vector3.Distance(this.gameObject.transform.position, target.transform.position);

                    if (this.distanceToTarget <= 0.05)
                    {
                        this.gameObject.GetComponent<SphereCollider>().enabled = true;
                    }

                    if (SimulationManager.Get().addRigidBodyCollider == false)
                    {
                        base.Seek(this.gameObject.GetComponent<Agent>(), target);

                        //   KdTree<Agent> n = base.FindNeighboursKDIgnoreName(this.gameObject.GetComponent<Agent>(), 2, AgentPopulation.populationList, base.mySignalEmmiter.name);
                        //base.Repell(n, 0.022f);

                     //   base.Separation(n, 0.07f, 1.1f);

                    }

                    if (SimulationManager.Get().addRigidBodyCollider == true) this.SeekSimpleForRB(this.gameObject.GetComponent<Agent>(), target);


                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




                //  Debug.DrawLine(this.gameObject.transform.position, target.transform.position, Color.red);





                }///end if target is not null conditions

            }// END SIGNAL RECEIVER CONDITIONS




        }


    
        /// <summary>
        /// S
        /// </summary>
        public void ISignalReceiverPassiveConditions()
        {
            if (this.gameObject.tag == "SignalReceiverPassive")
            {

                if (SimulationManager.Get().oneGeneration && AgentPopulation.freezedAgentList.Count / ((float)AgentPopulation.populationList.Count - AgentPopulation.cancelledAgentList.Count) * 100 < 97)
                {
                    base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
                }

                //if (SimulationManager.Get().displayColorByEmotion)
                //{

                //    DisplayEmotion();
                //}

                if (SimulationManager.Get().addWindForce)
                {
                    //StartCoroutine(ApplyWindForce(SimulationManager.Get().windForce, SimulationManager.Get().windForceIntervalSeconds)); // Used to update object with my physics engine

                    StartCoroutine(ApplyWindForce(SimulationManager.Get().windForce, SimulationManager.Get().windForceIntervalSeconds)); // used to update agent with Rigid body physics AddForce()
                }

                //////////////////// set display////////////////////////////////

                base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false);

                //////////////////// set display////////////////////////////////
                base.signalReceiverPassiveCounter++;
         

               


            }

        }


        #endregion


    



        public List<GameObject> FindPeople()
   {
     GameObject[] people=GameObject.FindGameObjectsWithTag("Person");

     return people.ToList();
   }
       
    }//END CLASS

}

