using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrganizationalModel.Population;
using OrganizationalModel.Behaviors;
using OrganizationalModel.BaseClass;
using OrganizationalModel.Managers;
public class CamaraRotate : MonoBehaviour {




    /// ARRAY SPACING POPULATION = 3.7!!!!!!!!!!!!!!!!!!!!!!!!!

    ///// PARAMETERS FOR CAMARA STARTING POSITION DEPENDING ON POPULATION 

    ///// POPULATION 3000:  23.08  24.77628   -14.82
    ///
    ///// POPULATION 2000:   13.09  27.32  -26.84

    ///POPULATION 700:       19.12   17.82   0.55

    ///POPULATION 400:       19.71   17.36   2.32

    ///// POPULATION 100:   12.8  12.9   -10.5


    /////CONSTRAINTS FOR CAMARA MOVEMENT DEPENDING ON POPUALTION
    ///
    /// POPULATION 3000:  Y MAX = 45    distMax = 75

    ///// POPULATION 2000:  Y MAX = 35    
    /// pop 1000: distMax = 45f;
    ///POPULATION 700: Y MAX = 28        distMax = 50
    ///POPULATION 600: Y MAX = 28        distMax = 40
    ///POPULATION 400: Y MAX = 25        distMax = 40


    ///// POPULATION 100:  Y MAX = 15   distMax = 25f;
    ///POPULATION 27: distMax=10F


    float yMax = 15f;
    float distMax = 10f;// 10f;//60f; // 15 //30 //150; 


   public float distFromTarget = 0;
    public Vector3 target = Vector3.zero;

    float CamaraSpeedMoveAway = .1f;//.1 // 0.2 - 0.5f ---> larger populations

    GameObject generationManager;
    private GenerationManager GM;
    // Use this for initialization

    private Transform _myTransform;
    private float damping = 0.7f;
    private int rotateSpeed =1; //8
    void Awake()
    {
        _myTransform = transform;
    }

    void Start () {


        generationManager = GameObject.Find("GenerationManager");
        GM = generationManager.GetComponent<GenerationManager>();

    }

    // Update is called once per frame



    private void LateUpdate()
    {

        // transform.localPosition += new Vector3(0f, Time.deltaTime * 0.02f, 0.0f);
        //if (transform.localPosition.y > yMax) transform.localPosition = new Vector3(transform.localPosition.x, yMax, transform.localPosition.z);

       
      
            if (GenerationManager.generationCount == 0)
            {
                if(SimulationManager.Get().oneGeneration && AgentPopulation.deActivatedAgentList.Count ==0 && AgentPopulation.signalReceiverAgentList.Count==0 && SimulationManager.Get().displayColorByEmotion==false)
                {
                    rotateSpeed = 10;

                  
                }

                if (AgentPopulation.freezedAgentList.Count > 10 )
                {
                    for (int i = 0; i < AgentPopulation.freezedAgentList.Count; i++)
                    {
                        target = target + AgentPopulation.freezedAgentList[i].transform.position;
                    }

                    target = target / AgentPopulation.freezedAgentList.Count;





                    transform.RotateAround(target, Vector3.up, Time.deltaTime * rotateSpeed); //0.8f


               //     Debug.DrawLine(transform.position, target, Color.grey);



                    Vector3 moveAway = transform.position - target;

                    distFromTarget = moveAway.magnitude;

                    moveAway.Normalize();

                    if (distFromTarget < distMax) transform.position += moveAway * Time.deltaTime * CamaraSpeedMoveAway;



                    Quaternion rotation = Quaternion.LookRotation(target - _myTransform.position);
                    _myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, rotation, Time.deltaTime * damping);




                }

                else
                {





                    if (AgentPopulation.indexActivatedAgentsList.Count != 0)
                    {


                        Agent Target = AgentPopulation.populationList[AgentPopulation.indexActivatedAgentsList[0]];

                        transform.RotateAround(Target.transform.position, Vector3.up, Time.deltaTime * rotateSpeed);

                        


                   //     Debug.DrawLine(transform.position, Target.transform.position, Color.grey);
                        Vector3 moveAway = transform.position - Target.transform.position;

                        distFromTarget = moveAway.magnitude;

                        moveAway.Normalize();

                        if (distFromTarget < distMax) transform.position += moveAway * Time.deltaTime * CamaraSpeedMoveAway;

                        Quaternion rotation = Quaternion.LookRotation(Target.transform.position - _myTransform.position);
                        _myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, rotation, Time.deltaTime * damping);
                    }





                }


            }


            if (GenerationManager.generationCount > 0)
            {

                if (AgentPopulation.populationList.Count > 0)
                {
                    for (int i = 0; i < AgentPopulation.populationList.Count; i++)
                    {
                        target = target + AgentPopulation.populationList[i].transform.position;
                    }

                    target = target / AgentPopulation.populationList.Count;


                    transform.RotateAround(target, Vector3.up, Time.deltaTime * 1f); //0.8f


                  //  Debug.DrawLine(transform.position, target, Color.grey);

                    Vector3 moveAway = transform.position - target;

                    distFromTarget = moveAway.magnitude;

                    moveAway.Normalize();

                    if (distFromTarget < distMax) transform.position += moveAway * Time.deltaTime * CamaraSpeedMoveAway;


                    Quaternion rotation = Quaternion.LookRotation(target - _myTransform.position);
                    _myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, rotation, Time.deltaTime * damping);




                }



            }
        }


    

}
