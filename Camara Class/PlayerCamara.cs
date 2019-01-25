using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamara : MonoBehaviour {

    public float speed = 2f;
    public float sensitivity = 2f;

    CharacterController player;

    public GameObject eyes;

    float moveFb;
    float moveLR;
    float rotX;
    float rotY;
	// Use this for initialization
	void Start () {
        player = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {


        moveFb = Input.GetAxis("Vertical") * speed;
        moveLR = Input.GetAxis("Horizontal") * speed;

        rotX = Input.GetAxis("Mouse X") * sensitivity;
        rotY = Input.GetAxis("Mouse Y") * sensitivity;

        Vector3 movement = new Vector3(moveLR, 0, moveFb);
        transform.Rotate(0, rotX, 0);
        eyes.transform.Rotate(rotY, 0, 0);

        movement = transform.rotation * movement;
        player.Move(movement * Time.deltaTime);

    }
}
