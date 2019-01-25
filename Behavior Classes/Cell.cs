using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    private float scalarValueRules;
    private float scalarValueProximity;
    private float scalarValueHumanData;
    private string cellName;
    private float pheromone; 

    private float x;
    private float y;
    private float z;

    private Vector3 position;

    public Cell(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
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

    public Vector3 Position
    {
        get
        {

            return new Vector3(this.x, this.y, this.z);

        }

    }

    public float Pheromone
    {

        get { return pheromone; }

        set { pheromone += value; }
    }

    public float ScalarValueRules
    {
        get { return scalarValueRules; }

        set { scalarValueRules = value; }
    }


    public float ScalarValueProximity
    {
        get { return scalarValueProximity; }

        set { scalarValueProximity = value; }
    }

    public float ScalarValueHumanData
    {
        get { return scalarValueHumanData; }

        set { scalarValueHumanData = value; }
    }


    /// <summary>
    /// Get the current pixel name
    /// </summary>
    public string CellName
    {
        get { return this.cellName; }
        set { this.cellName = value; }
    }
}
