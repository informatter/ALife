using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpMatter.SharpMath;
using System.Threading;

using OrganizationalModel.Managers;
public class Pixel:MonoBehaviour {
    
   
    private List<string> signalReceiverHistoryNames = new List<string>();
    private List<string> clusterHistoryAgentNames = new List<string>();
   private int countClusterAgent;
    private int countMobileAgent;
    private Vector3 position;
    private string pixelName;
    private float densityValue;

    private float x;
    private float y;
    private float z;





  

    public Pixel(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
       


    }


    public Pixel(float x, float y, float z, float densityValue)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.densityValue = densityValue;



    }


    public List<string> SignalReceiverHistoryNames
    {
        get { return this.signalReceiverHistoryNames; }

    }

    /// <summary>
    /// returns the history of the agentCluster in the current pixel as a list. 
    /// This  is intended for the agent to query how many times it has been in the same voxel
    /// </summary>
    public List<string> ClusterHistoryrAgentNames
    {
        get { return this.clusterHistoryAgentNames; }
    }

    /// <summary>
    /// returns the total number of times the agent has been static in its current voxel
    /// every generation a value of +1 is added
    /// </summary>
    public int CountClusterAgents
    {

        get
        {
            // if (GenerationManager.generationCount==1 && this.countClusterAgent >100 || this.countClusterAgent<100 && this.countClusterAgent!=0) this.countClusterAgent = 100;
            //if (GenerationManager.generationCount == 1  && this.countClusterAgent != 0 && this.countClusterAgent!=1) this.countClusterAgent = 1;

            for (int i = 1; i <= GenerationManager.generationCount; i++)
            {
                if (this.countClusterAgent != 0 && this.countClusterAgent != i) this.countClusterAgent = i;
            }
            return this.countClusterAgent;
        }
    }

    /// <summary>
    /// returns total number of times the current pixel has been crossed by a mobile agent
    /// during the length of the simulation
    /// </summary>
    public int CountMobileAgents
    {
        get
        {

            return this.countMobileAgent;

        }
    }

    public float DensityValue
    {
        get
        {

            return this.densityValue;

        }

        set
        {
            this.densityValue = value;
        }
    }


    public Vector3 Position
    {
        get
        {

            return new Vector3(this.x, this.y, this.z);

        }

    }

    /// <summary>
    /// Get the current pixel name
    /// </summary>
    public string PixelName
    {
        get { return this.pixelName; }
        set { this.pixelName = value; }
    }


    public float X
    {
        get { return this.x; }
    }

    public float Y
    {
        get { return this.y; }
    }

    public float Z
    {
        get { return this.z; }
    }


    /// <summary>
    /// Add the mobile agents name to the history list
    /// </summary>
    /// <param name="agentName"></param>
    public void AddSignalreceiverName(string agentName)
    {
        signalReceiverHistoryNames.Add(agentName);
    }

    /// <summary>
    /// / Adds the cluster agents name to the history list
    /// </summary>
    /// <param name="agentName"></param>
    public void AddClusterAgentNames(string agentName)
    {
        clusterHistoryAgentNames.Add(agentName);
    }

    /// <summary>
    /// adds a value of 1  per generation change
    /// when a cluster agent is in the current pixel
    /// </summary>
    public void ClusterAgentCounter()
    {
        countClusterAgent++;
        
    }

    public void ClusterAgentCounter(int num)
    {
        countClusterAgent+=num;

    }

    /// <summary>
    /// adds a value of 1 to the current pixel when a mobile agent has gone through it
    /// </summary>
    public void MobileAgentCounter()
    {

        countMobileAgent++;
    }




}
