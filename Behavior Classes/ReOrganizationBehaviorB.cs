using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ClassExtensions;

using SharpMatter.SharpMath;
using SharpMatter.SharpDataStructures;

using OrganizationalModel.UtilityFunctions;
using OrganizationalModel.BaseClass;
using OrganizationalModel.Population;
using OrganizationalModel.Managers;
using System.Linq;
using System.Text;
using System;




namespace OrganizationalModel.Behaviors { 
public partial class ReOrganizationBehavior : Agent, IReOrganization<Agent, float>

{

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

                Vector3 wind = new Vector3(5f, 5f, 5f);

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

            if (SimulationManager.Get().is3D)
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


        #region AGENT BEHAVIOR CONDITIONS

        /// <summary>
        /// 
        /// </summary>
        /// 


        public void IAgentStatesBehaviorConditions()
        {
           ISignalReceiverPassiveBehaviorConditions();
           ISignalReceiverBehaviorConditions();
            ISignalEmmiterBehaviorConditions();
             FreezedAgentBehaviorConditions();

            DeactivatedAgentBehaviorConditions();

            WandererBehaviorConditions();
        }

        #region WANDERING STUFf...
        public void WandererBehaviorConditions()
        {

            if (this.gameObject.tag == "Wanderer")
            {

             
                countToSearchForEmptyPixel++;

                base.neighboursTemp.Clear();
                Pixel currentPixel;
                PixelDataLookUp3D(pixelPop, out currentPixel);
                base.currentPixel = currentPixel.PixelName;

                base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
                base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());

                if (SimulationManager.Get().addFixedJoints)
                {
                    var Container = gameObject.GetComponent<FixedJoint>();

                    Destroy(Container);
                }
                   
                //    if (currentPixel.CountMobileAgents > 1) 

                base.Wander();

                if (currentPixel.CountMobileAgents < 2 || countToSearchForEmptyPixel > wandererTreshhold) base.ChangeStateToSignalReceiver(this.gameObject.GetComponent<Agent>(), 0);

               // if (countToSearchForEmptyPixel > internalTreshhold) base.ChangeStateToSignalReceiver(this.gameObject.GetComponent<Agent>(), 0);

                Bounds();

                KdTree<Agent> neighbours = base.FindNeighboursKd(this.gameObject.GetComponent<Agent>(), 3.0f, AgentPopulation.populationList);

                base.Separation(neighbours, 0.2f, 1.5f);



            }

        }


        private void Bounds()
        {
            float maxX = 80;
            float maxY = 80;
            float maxZ = 80;



            if (this.gameObject.transform.position.x < 0)
                base.IApplyForces(new Vector3(-this.gameObject.transform.position.x, 0, 0));
            else if (this.gameObject.transform.position.x > maxX)
                base.IApplyForces(new Vector3(maxX - this.gameObject.transform.position.x, 0, 0));

            if (this.gameObject.transform.position.y < 0)
                base.IApplyForces(new Vector3(0, -this.gameObject.transform.position.y, 0));
            else if (this.gameObject.transform.position.y > maxY)
                base.IApplyForces(new Vector3(0, maxY - this.gameObject.transform.position.y, 0));

            if (this.gameObject.transform.position.z < 0)
                base.IApplyForces(new Vector3(0, -this.gameObject.transform.position.z, 0));
            else if (this.gameObject.transform.position.z > maxZ)
                base.IApplyForces(new Vector3(0, 0, maxZ - this.gameObject.transform.position.z));


        }


        #endregion
        public void CalculateEnergyLevelsOnGeneration_0()
        {


            if (this.gameObject.tag == "Freezed") base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
            if (this.gameObject.tag == "SignalEmmiter") base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
            if (this.gameObject.tag == "SignalReceiver") base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
            if (this.gameObject.tag == "SignalReceiverPassive") base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
        }



        public void ShareEnergyLevelsOnGeneration_0()
        {


            if (this.gameObject.tag == "Freezed") base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());
            if (this.gameObject.tag == "SignalEmmiter") base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());
            if (this.gameObject.tag == "SignalReceiverPassive") base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());
        }



        public void DeactivatedAgentBehaviorConditions()
        {
            if (this.gameObject.tag == "De-Activated")
            {
                if(SimulationManager.Get().addWindForce) StartCoroutine(ApplyWindForce(SimulationManager.Get().windForce, SimulationManager.Get().windForceIntervalSeconds));

                //////////////////// set display////////////////////////////////

                base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false);

                //////////////////// set display////////////////////////////////

                //Pixel currentPixel;
                //PixelDataLookUp3D(pixelPop, out currentPixel);
                //base.currentPixel = currentPixel.PixelName;

                base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
                base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());






            }


        }





        public void FreezedAgentBehaviorConditions()
        {

            if (this.gameObject.tag == "Freezed")
            {
                //if(this.gameObject.name== GenerationManager.activatedAgentPerGeneration[0].name)
                //{
                //    ActivateEmmiterAgain = true;
                //}

                Pixel currentPixel;
                base.PixelDataLookUp3D(pixelPop, out currentPixel);
                currentPixel.AddClusterAgentNames(this.gameObject.name);

                base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
                base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());
                base.currentPixel = currentPixel.PixelName;


                Cell _currentScalarValue;
                if (SimulationManager.Get().ScalarField2d)
                {
                    ScalarFieldDataLookUp(SF, out _currentScalarValue);

                    base.currentScalarCell = _currentScalarValue.CellName;
                    base.currentScalarValueRules = _currentScalarValue.ScalarValueRules;
                    base.currentScalarValuePackingProximity = _currentScalarValue.ScalarValueProximity;
                    base.currentScalarValueHumanData = _currentScalarValue.ScalarValueHumanData;

                }

                //if (AgentPopulation.freezedAgentList.Count == AgentPopulation.populationList.Count)
                //{

                //    SimulationManager.Get().displayColorByState = false;
                //    SimulationManager.Get().displayColorbyEnergy = false;
                //    SimulationManager.Get().displayColorbyDisplacement = false;
                //    SimulationManager.Get().displayColorByComunication = false;
                //    SimulationManager.Get().displayColorByNeighbours = false;
                //    SimulationManager.Get().displayTopology = false;
                //    SimulationManager.Get().displayColorByEmotion = true;
                //    SimulationManager.Get().GPUInstancing = false;
                //}

                if (SimulationManager.Get().displayColorByEmotion)
                {
                    SimulationManager.Get().updateFieldRealTime = true;

                    foreach (var item in ScalarFields.ScalarField2D.scalarFieldValuesProximity)
                    {
                        scalarFieldValuesProximityTotal += item;
                    }

                    float averege2 = scalarFieldValuesProximityTotal / ScalarFields.ScalarField2D.scalarFieldHumanData.Length;

                    // averege = scalarFieldValuesProximityTotal / ScalarFields.ScalarField2D.scalarFieldValuesProximity.Length;
                    UserInterface.UI.averegeFiledValues = averege2;


                    scale += Mathf.Clamp(Mathf.Sin(averege2), 0.008f, .2f);
                    UserInterface.UI.rateOfChange = scale;
                    //Color c = Utility.Lerp3(Color.white, Color.red, Color.blue, Mathf.Cos(base.currentScalarValueHumanData * Mathf.PI + scale)); //Mathf.Cos(base.currentScalarValueHumanData  * Mathf.PI + scale));

                    Color c = Utility.Lerp3(Color.white, Utility.RGBToFloat(255, 0, 13), Utility.RGBToFloat(250, 179, 0), Mathf.Cos(base.currentScalarValueHumanData * Mathf.PI + scale)); //base.currentScalarValueHumanData * Mathf.PI + scale

                    gameObject.GetComponent<MeshRenderer>().material.color = c;

                    //  emissiveMaterial.SetColor("_Emission", c);


                    //  DynamicGI.SetEmissive(gameObject.GetComponent<MeshRenderer>(), c);


                }



                if (SimulationManager.Get().addWindForce == false && SimulationManager.Get().addaptToEnvironmentalForces == false)
                {

                    //////////////////// set display////////////////////////////////

                    base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false);

                    //////////////////// set display////////////////////////////////
                }

                if (SimulationManager.Get().addWindForce)
                {


                    //StartCoroutine(ApplyWindForce(SimulationManager.Get().windForce, SimulationManager.Get().windForceIntervalSeconds));
                    StartCoroutine(ApplyWindForce(SimulationManager.Get().windForce, SimulationManager.Get().windForceIntervalSeconds));


                    displacementPositions.Add(this.gameObject.transform.position);


                    /// Get displacement of all neighbours
                    for (int i = 0; i < base.neighbours.Count; i++)
                    {
                        if (base.neighbours[i] != null)
                        {
                            this.neighboursDisplacement.Add(neighbours[i].GetComponent<ReOrganizationBehavior>().totalDisplacement);

                        }
                    }

                    this.totalDisplacement = base.GetTotalDisplacement(displacementPositions);

                    float totalNeighboursDisplacement = neighboursDisplacement.Sum();
                    this.averegeNeighbourDisplacement = totalNeighboursDisplacement / base.neighbours.Count;
                    //////////////////// set display////////////////////////////////

                

                    base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false, this.totalDisplacement);

                    //////////////////// set display////////////////////////////////


                }


                if (SimulationManager.Get().addWindForce && SimulationManager.Get().addaptToEnvironmentalForces)
                {

                    this.createPlaceHolders_ReOrg = false;
                    this.searchClosestNeighbour_ReOrg= false;
                    this.time_ReOrg = 0;

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
                    if (this.averegeNeighbourDisplacement > 70f && AgentPopulation.deActivatedAgentList.Count != 0 && hingeJoints.Length < 5)
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


        /// <summary>
        /// 
        /// </summary>
        public void ISignalEmmiterBehaviorConditions()
        {
            if (this.gameObject.tag == "SignalEmmiter")
            {
                if (SimulationManager.Get().addWindForce)
                {
                    //StartCoroutine(ApplyWindForce(SimulationManager.Get().windForce, SimulationManager.Get().windForceIntervalSeconds)); // Used to update object with my physics engine

                    StartCoroutine(ApplyWindForce(6f, SimulationManager.Get().windForceIntervalSeconds)); // used to update agent with Rigid body physics AddForce()
                }

                // if (SimulationManager.Get().addWindForce == false && SimulationManager.Get().addaptToEnvironmentalForces == false)
                //  {
                //////////////////// set display////////////////////////////////

               base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false);

                //////////////////// set display////////////////////////////////
                //  }


                time_ReOrg++;

                base.neighboursTemp.Distinct().ToList();
                Pixel currentPixel;
                PixelDataLookUp3D(pixelPop, out currentPixel);
                base.currentPixel = currentPixel.PixelName;
                base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());



                base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);

                base.mySignalReceivers = base.mySignalReceiversTemp.Distinct().ToList();


                if (GenerationManager.generationChange)
                {



                    currentPixel.ClusterAgentCounter();
                    currentPixel.AddClusterAgentNames(this.gameObject.name);
                    base.myNameInCurrentPixel = currentPixel.ClusterHistoryrAgentNames;
                    int occurences = currentPixel.ClusterHistoryrAgentNames.NumberOfOccurences(this.gameObject.name);
                    base.timesInCurrentPixel = occurences;



                }








                if (createPlaceHolders_ReOrg == false && time_ReOrg > 10)
                {




                    Cell _currentScalarValue;


                    ////if (SimulationManager.Get().ScalarField3d)
                    ////{
                    ////    ScalarField3DDataLookUp(SF, out _currentScalarValue);

                    ////    base.currentScalarCell = _currentScalarValue.CellName;
                    ////    base.currentScalarValue = _currentScalarValue.ScalarValue;

                    ////   // placeHolderOffset = base.currentScalarValue;
                    ////}

                    if (SimulationManager.Get().ScalarField2d)
                    {
                        ScalarFieldDataLookUp(SF, out _currentScalarValue);

                        base.currentScalarCell = _currentScalarValue.CellName;
                        base.currentScalarValueRules = _currentScalarValue.ScalarValueRules;
                        base.currentScalarValuePackingProximity = _currentScalarValue.ScalarValueProximity;
                        //  placeHolderOffset = base.currentScalarValueRules * 0.4f;
                        //  if (placeHolderOffset < 1) placeHolderOffset = 0.0f; // 0.30 is the index[0] of domain.B when scaled to * 0.4


                        //placeHolderOffset = base.currentScalarValueProximity;

                        placeHolderOffset = 0.0f;
                    }
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////







                    this.gameObject.GetComponent<SphereCollider>().radius = this.gameObject.GetComponent<SphereCollider>().radius + placeHolderOffset;






                    if (SimulationManager.Get().singleRule)
                    {
                        // CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule18);

                        if (GenerationManager.generationCount == 1) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule05);
                        if (GenerationManager.generationCount == 2) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule14);
                        if (GenerationManager.generationCount == 3) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule25);
                        if (GenerationManager.generationCount == 4) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule12);
                        if (GenerationManager.generationCount == 4) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule20);
                        if (GenerationManager.generationCount == 4) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule17);

                    }


                    if (SimulationManager.Get().multipleRules)
                    {

                        //if (SimulationManager.Get().radialScalarField)
                        //{

                        //    List<float> values = new List<float>();

                        //    for (int i = 0; i < ScalarFields.ScalarField2D.scalarFieldValuesProximity.GetLength(0); i++)
                        //    {
                        //        for (int j = 0; j < ScalarFields.ScalarField2D.scalarFieldValuesProximity.GetLength(1); j++)
                        //        {
                        //            values.Add(ScalarFields.ScalarField2D.scalarFieldValuesProximity[i, j]);

                        //        }
                        //    }

                        //    values.Sort();

                        //    float minVal = values[0];
                        //    float maxVal = values[values.Count - 1];

                        //    float remapProximityValues = SharpMath.Remap(base.currentScalarValuePackingProximity, minVal, maxVal, 0, 1);

                        //    if (remapProximityValues <= 0.25f) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule01);
                        //    if (remapProximityValues > 0.25f && remapProximityValues < 0.50f) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule18);
                        //    if (remapProximityValues >= 0.5f && remapProximityValues < 0.75f) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule10);
                        //    if (remapProximityValues >= 0.75) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule03);






                        //}



                        if (SimulationManager.Get().interpolatedScalarField)
                        {
                            if (SimulationManager.Get().addWindForce && SimulationManager.Get().addaptToEnvironmentalForces)
                            {
                                List<VectorData> domains = VectorData.ConvertStringToVectorData(SimulationManager.Get().ImportToUnityDomainsFilePath);


                                if (this.averegeNeighbourDisplacement < 70)
                                {
                                    if (base.currentScalarValueRules >= domains[0].A && base.currentScalarValueRules < domains[0].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule04);

                                    if (base.currentScalarValueRules >= domains[1].A && base.currentScalarValueRules < domains[1].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule18);

                                    if (base.currentScalarValueRules >= domains[2].A && base.currentScalarValueRules < domains[2].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule01);

                                    if (base.currentScalarValueRules >= domains[3].A && base.currentScalarValueRules <= domains[3].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule03);
                                }

                                if (this.averegeNeighbourDisplacement > 70)
                                {
                                    CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule09);
                                }
                            }

                            else
                            {
                                List<VectorData> domains = VectorData.ConvertStringToVectorData(SimulationManager.Get().ImportToUnityDomainsFilePath);

                                if (base.currentScalarValueRules >= domains[0].A && base.currentScalarValueRules < domains[0].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule21);

                                if (base.currentScalarValueRules >= domains[1].A && base.currentScalarValueRules < domains[1].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule10);

                                if (base.currentScalarValueRules >= domains[2].A && base.currentScalarValueRules < domains[2].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule01);

                                if (base.currentScalarValueRules >= domains[3].A && base.currentScalarValueRules <= domains[3].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule11);


                            }




                        }




                    }


                    /// PINS SIGNAL RECEIVER, NO WIND FORCE EFECT

                    if (SimulationManager.Get().addFixedJoints)
                    {
                        for (int i = 0; i < placeHolderLocalList.Count; i++)
                        {
                            base.AddFixedJointPlaceHolders(placeHolderLocalList[i], 50);
                        }
                    }




                    this.createPlaceHolders_ReOrg = true;

                }



                /// Only search closest neighbour once and search the first closest neighbour after n seconds
                /// time > AgentPopulation.timeToInitFirstAgent * 20 &&
                if (this.searchClosestNeighbour_ReOrg== false && time_ReOrg > 50 && AgentPopulation.deActivatedAgentList.Count > 0)
                {


                    if (SimulationManager.Get().addWindForce == false)
                    {
                        float maxThreshold = 1.68f;

                        base.currentScalarValueDensityCheck = base.currentScalarValueRules;
                        if (base.currentScalarValueDensityCheck > maxThreshold) base.currentScalarValueDensityCheck = maxThreshold;
                        if (base.currentScalarValueDensityCheck < 1.15f) base.currentScalarValueDensityCheck = 0.90f;

                        //base.PlaceHolderBehavior(DensityRules.RuleA, base.currentScalarValueDensityCheck); //0.9f   //1.2f    1.65f    1.5f 

                        base.PlaceHolderBehavior(DensityRules.RuleA, 1.2f);
                    }



                    if (SimulationManager.Get().addWindForce == true && SimulationManager.Get().addaptToEnvironmentalForces == false)
                    {
                        // get my total displacement
                        displacementPositions.Add(this.gameObject.transform.position);


                        this.totalDisplacement = base.GetTotalDisplacement(displacementPositions);

                        //////////////////// set display////////////////////////////////

                        base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false, this.totalDisplacement);

                        //////////////////// set display////////////////////////////////
                        float maxThreshold = 1.68f;

                        base.currentScalarValueDensityCheck = base.currentScalarValueRules;
                        if (base.currentScalarValueDensityCheck > maxThreshold) base.currentScalarValueDensityCheck = maxThreshold;
                        if (base.currentScalarValueDensityCheck < 1.15f) base.currentScalarValueDensityCheck = 1.1f;

                      //  base.PlaceHolderBehavior(DensityRules.RuleA, base.currentScalarValueDensityCheck); //0.9f   //1.2f    1.65f    1.5f 

                        base.PlaceHolderBehavior(DensityRules.RuleA, 1.2f); //0.9f   //1.2f    1.65f    1.5f 
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

                        //////////////////// set display////////////////////////////////

                        base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false, this.totalDisplacement);

                        //////////////////// set display////////////////////////////////

                        // if my current displacement is > than the averege sum of my neighbours displacement

                        if (this.averegeNeighbourDisplacement > 0.7f) // choose my minimum density check value to be tighly packed
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


                    base.ActivateAgents();






                    this.searchClosestNeighbour_ReOrg = true;

                }




                if (mySignalReceivers.Count == 1 && mySignalReceivers[0].tag == "Freezed" && this.searchClosestNeighbourAgain_ReOrg == false)
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
                        this.searchClosestNeighbourAgain_ReOrg = true;
                    }
                }



                if (mySignalReceivers.Count == 2 && mySignalReceivers[0].tag == "Freezed" && mySignalReceivers[1].tag == "Freezed" && this.searchClosestNeighbourAgain_ReOrg == false)
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
                        this.searchClosestNeighbourAgain_ReOrg = true;
                    }
                }

                /////////////////// IF ALL MY PLACEHOLDERS ARE DEACTIVATED CHANGE MY STATE TO AGENT  /////////////////// 

                if (this.searchClosestNeighbourRe_Org == true && this.createPlaceHolders_ReOrg == true)
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


                //if (this.searchClosestNeighbourRe_Org == true && this.createPlaceHolders_ReOrg == true)
                //{
                //    base.communicationType = base.mySignalReceivers.Count;

                //}

                // if (SimulationManager.Get().addaptToEnvironmentalForces == true)
                if (this.searchClosestNeighbourRe_Org == true && this.createPlaceHolders_ReOrg == true && placeHolders.Count == 0 && AgentPopulation.deActivatedAgentList.Count == 0)
                {
                    //  if(placeHolders.Count==0 && AgentPopulation.deActivatedAgentList.Count==0)
                    //   {
                    base.ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);
                    //  }
                }




                base.communicationType = base.mySignalReceivers.Count;

            }

          

        }


        public void ISignalReceiverBehaviorConditions()
        {


            if (this.gameObject.tag == "SignalReceiver")
            {
                //Pixel currentPixel;
                //base.PixelDataLookUp3D(pixelPop, out currentPixel);

                //currentPixel.MobileAgentCounter();
                //currentPixel.AddSignalreceiverName(this.gameObject.name);
                //base.currentPixel = currentPixel.PixelName;


                base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);

                PlaceHolder target = base.placeHolderTargetForSignalReceiver;
                //////////////////// set display////////////////////////////////

                base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false);

                //////////////////// set display////////////////////////////////

                if (SimulationManager.Get().addRigidBodyCollider == false)
                {
                    this.gameObject.GetComponent<SphereCollider>().enabled = false;
                    base.neighboursTemp.Clear();
                    base.neighbours.Clear();

                }






                if (SimulationManager.Get().addRigidBodyCollider)
                {

                    this.gameObject.GetComponent<SphereCollider>().enabled = false;


                    // get all my joints and destroy them
                    FixedJoint[] hingeJoints = GetComponents<FixedJoint>();

                    foreach (FixedJoint item in hingeJoints)
                    {
                        Destroy(item);


                    }

                    /// Loop through all neighbours
                    for (int i = 0; i < base.neighbours.Count; i++)
                    {
                        if (base.neighbours[i] != null)
                        {
                            // for each neighbour get a list of all his fixed joints
                            FixedJoint[] neighboursJoints = base.neighbours[i].GetComponents<FixedJoint>();
                            // iterate through the list of the neighbours joints
                            foreach (var item in neighboursJoints)
                            {

                                Rigidbody connectedBody = item.connectedBody;

                                // if the connected body of my neighbour = my name, destroy my neighbours joint
                                if (connectedBody.name == this.gameObject.name)
                                {
                                    // print("my neighbour" + " " + "agent" + base.neighbours[i].name + " " + "has a connected body " + " " + connectedBody.name + " " + "Which is me!!");
                                    Destroy(item);
                                    //break;
                                }
                            }
                        }

                    }

                    base.neighboursTemp.Clear();
                    base.neighbours.Clear();

                }




                if (target == null)
                {



                    if (SimulationManager.Get().addRigidBodyCollider == false)
                    {
                        base.ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);
                        this.gameObject.GetComponent<SphereCollider>().enabled = true;
                    }

                    if (SimulationManager.Get().addRigidBodyCollider)
                    {

                        base.ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);
                        this.gameObject.GetComponent<SphereCollider>().enabled = true;


                    }

                }








                if (target != null)
                {

                    /// RETRIEVE SIGNAL EMMITER
                    string[] separators = { ".", " " };
                    string value = this.gameObject.GetComponent<ReOrganizationBehavior>().placeHolderTargetForSignalReceiver.name;
                    string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    string targetEmmiterAgentIndex = words[2]; /// THIS WILL ALWAYS OUTPUT A NUMBER ---> EXAMPLE: Place Holder 100.1 it will output 100
                    int index = Convert.ToInt32(targetEmmiterAgentIndex);
                    base.mySignalEmmiter = AgentPopulation.populationList[index];


                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    this.distanceToTarget = Vector3.Distance(this.gameObject.transform.position, target.transform.position);

                    if (this.distanceToTarget <= 0.1)
                    {
                        if (SimulationManager.Get().addRigidBodyCollider == false)
                        {
                            this.gameObject.GetComponent<SphereCollider>().enabled = true;
                        }


                        if (SimulationManager.Get().addRigidBodyCollider)
                        {

                            this.gameObject.GetComponent<SphereCollider>().enabled = true;


                        }


                    }

                    // if (mySignalEmmiter.tag == "Freezed" && distanceToTarget <= 0.1) base.ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);

                    if (this.distanceToTarget >= 3)
                    {
                        // StartCoroutine(ApplyWindForce(2.5f,10));
                        //// StartCoroutine(ApplyWindForce2(0.01f, 10));

                        // this.velocity.Normalize();
                        // Vector3 force = Vector3.zero;
                        // force += this.velocity * 0.5f;//this.drag;
                        // base.IApplyForces(force);





                    }

                    if (SimulationManager.Get().addRigidBodyCollider == false)
                    {
                        //KdTree<Agent> n = base.FindNeighboursKDIgnoreName(this.gameObject.GetComponent<Agent>(), 2, AgentPopulation.populationList, base.mySignalEmmiter.name);
                        //base.Repell(n, 0.036f);
                        base.Seek(this.gameObject.GetComponent<Agent>(), target);
                    }

                    if (SimulationManager.Get().addRigidBodyCollider == true) this.SeekSimpleForRB(this.gameObject.GetComponent<Agent>(), target, 3.5f, 3.5f);


                    // Debug.DrawLine(this.gameObject.transform.position, target.transform.position, Color.red);








                }///end if target is not null conditions

            }// END SIGNAL RECEIVER CONDITIONS


        }


        /// <summary>
        /// 
        /// </summary>
        public void ISignalReceiverPassiveBehaviorConditions()
        {

          

            if (this.gameObject.tag == "SignalReceiverPassive")

            {
                base.signalReceiverPassiveCounter++;
                base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
                base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());

             //   StartCoroutine(ApplyWindForce(SimulationManager.Get().windForce, SimulationManager.Get().windForceIntervalSeconds));

                //////////////////// set display////////////////////////////////

                base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false);

                //////////////////// set display////////////////////////////////

                if (base.signalReceiverPassiveCounter > 100) base.ChangeStateToSignalEmmiter(this.gameObject.GetComponent<Agent>(), 0);

            } /// END CONDITIONS SIGNAL RECEIVER PASSIVE

        }


        #endregion


  







    } // END CLASS


} // END NAMESPACE
