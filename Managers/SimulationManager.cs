using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrganizationalModel.BaseClass;

namespace OrganizationalModel.Managers
{
    
    public class SimulationManager : MonoBehaviour
    {
        

        // singleton
        private static SimulationManager m_Instance = null;
        public static SimulationManager Get()
        {
            if (m_Instance == null)
                m_Instance = (SimulationManager)FindObjectOfType(typeof(SimulationManager));
            return m_Instance;
        }

        
        [HideInInspector]
        public bool randomInitialConditions;
        public bool runSimulation;
        [HideInInspector]
        public bool oneGeneration;
        [HideInInspector]
        public bool multipleGenerations;
        public bool addaptToEnvironmentalForces;
        public bool exportTrailPaths;
        public bool addWindForce;
        public float windForce;
        public int windForceIntervalSeconds;
        public bool windDisplacementAnalysis;
        public bool updateFieldRealTime;
        public GameObject placeHolderPrefab;
        public GameObject agentPrefab;

        public string individualOrganizationFilePath;
        public string CommunicationTypeFilePath;
        public string DirectionalityRuleFilePath;
        public string NeighbourCountFilePath;
        public string EnergyLevelsFilePath;
        public string ClusterOrganizationFilePath;
        public string DensityThresholdFilePath;
        public string ScalarFieldValuesFilePath;
        public string ScalarFieldHumanEmotionFilePath;
        public string PeopleFilePath;
        public string PixelArrayFilePath;
        public string PixelDataClusterAgentsPath;
        public string PixelDataMobileAgentsPath;
        public string WindAnalysisFilePath;
        public string ImportToUnityInterpolatedFieldFilePath;
        public string ImportToUnityClusterStartingPositionsFilePath;
        public string ImportToUnityDomainsFilePath;

        public bool drawPlaceHolders2D;
        [HideInInspector]
        public bool displayColorByState;
        [HideInInspector]
        public bool displayColorbyEnergy;
        [HideInInspector]
        public bool displayColorbyDisplacement;
        [HideInInspector]
        public bool displayColorMonochrome;
        [HideInInspector]
        public bool displayColorByNeighbours;
        [HideInInspector]
        public bool displayColorByComunication;
        [HideInInspector]
        public bool displayColorByEmotion;
        [HideInInspector]
        public bool GPUInstancing;
        [HideInInspector]
        public bool displayTopology;

        public bool displayByNeighbourLightIntensity;

        public bool displayColorByDistanceToNeighbours;

        public bool displayColorByRule;



        public bool is2D;
        public bool is3D;
        public bool ScalarField3d;
        public bool ScalarField2d;

        public bool gridDistribution_6Neighbours;
        public bool _12Neighbours;

        public bool addFixedJoints;
        public bool addRigidBodyCollider;

        public bool radialScalarField;
        public bool interpolatedScalarField;
        public bool singleRule;
        public bool multipleRules;


        public Color startColorDisplacement;
        public Color endColorDisplacement;

        public Color monochromeColor;

        public Color startColorNeighbours;
        public Color endColorNeighbours;

        public Color colorLine;
        public Color colorBranch;
        public Color colorStar;
        public Color colorCore;

        public Color packProxStart;
        public Color packProxEnd;






    }
}
