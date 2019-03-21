﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public enum PanType {MOUSE, KEY};
}


public class PlayerMovement : MonoBehaviour
{

    public Rigidbody playerRB;
    public Camera cam;
    public float camSpeed;
    public float max_velocity;
    public float moveSpeed;
    public int max_jumps;
    public int num_jumps;

    private Abilities abilities;
    private hellSceneManager hsm;
    private bool leftMove;
    private bool rightMove;
    private bool forwardMove;
    private bool backMove;
    private bool panRight;
    private bool panLeft;
    private bool ab1;
    private bool ab2;
    private bool ab3;
    private bool ab4;
    private bool ab5;
    private bool jump;
    public bool wallInPlay;
    private float xangle;
    private float yangle;
    private bool panKey;
    public Constants.PanType pan_type;
    private Vector3 offsetAngle;
    public Transform cameraTransform;
    private FollowPlayer fp;
    

    void Start()
    {
        playerRB = this.GetComponent<Rigidbody>();
        pan_type = Constants.PanType.MOUSE;
        playerRB.AddForce(0, 200, 0);
        abilities = this.gameObject.GetComponent<Abilities>();
        num_jumps = max_jumps;
        hsm = GameObject.Find("GameManager").GetComponent<hellSceneManager>();
        fp = cam.GetComponent<FollowPlayer>();

        GameObject tempObj = new GameObject();
        tempObj.transform.position = transform.position;
        tempObj.transform.eulerAngles = transform.eulerAngles;
        cameraTransform = tempObj.transform;
        wallInPlay = false;
    }

    private void Update()
    {
        panLeft = Input.GetKey("q");
        panRight = Input.GetKey("e");
        if(Input.GetKeyDown("p")) pan_type = pan_type == Constants.PanType.MOUSE ? Constants.PanType.KEY : Constants.PanType.MOUSE;
        leftMove = Input.GetKey("a");
        rightMove = Input.GetKey("d");
        forwardMove = Input.GetKey("w");
        backMove = Input.GetKey("s");
        jump = Input.GetKeyDown(KeyCode.Space);
        ab1 = Input.GetKeyDown(KeyCode.Alpha1);
        ab2 = Input.GetKey(KeyCode.Alpha2) || hsm.fireballDown;
        ab3 = Input.GetKeyDown(KeyCode.Alpha3);
        ab4 = Input.GetKeyDown(KeyCode.Alpha4);
        ab5 = Input.GetKeyDown(KeyCode.Alpha5);


        //Rotates the player's facing direction based on Mouse X and Y axis movement.
        //if (pan_type == Constants.PanType.MOUSE)
        //{
        //    xangle += camSpeed * Input.GetAxis("Mouse X");
        //    yangle += camSpeed * Input.GetAxis("Mouse Y");
        //    offsetAngle = new Vector3(yangle, xangle, 0);
        //    cameraTransform.eulerAngles = offsetAngle;
        //    transform.eulerAngles = new Vector3(0, xangle, 0);
        //} else 
        //{
        //    float axis = 0; ;
        //    if (panRight) axis = 1;
        //    if (panLeft) axis = -1;
        //    xangle += camSpeed * axis;
        //    offsetAngle = new Vector3(0, xangle, 0);
        //    cameraTransform.eulerAngles = offsetAngle;
        //    transform.eulerAngles = offsetAngle;
        //    //if (panRight) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90, 0), camSpeed * Time.deltaTime);
        //    //else if (panLeft) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90, 0), camSpeed * Time.deltaTime);
        //}

        if (pan_type == Constants.PanType.MOUSE) {
            //if(Input.mousePosition.x > 0 && Input.mousePosition.x < Screen.width)
            xangle += camSpeed * Input.GetAxis("Mouse X");
            //if(Input.mousePosition.y > 0 && Input.mousePosition.y < Screen.height)
            //yangle -= camSpeed * Input.GetAxis("Mouse Y") * 550/Screen.height;
            yangle = (Mathf.Max(Mathf.Min(Input.mousePosition.y, Screen.height), 0) / Screen.height - .5f) * -180;
        }
        xangle += camSpeed * ((panRight ? 1 : 0) + (panLeft ? -1 : 0));
        cameraTransform.eulerAngles = new Vector3(yangle, xangle, 0);
        transform.eulerAngles = new Vector3(0, xangle, 0);


        if (Input.GetKey("z")) fp.positionBack -= 10 * Time.deltaTime;
        if (Input.GetKey("x")) fp.positionBack += 10 * Time.deltaTime;

    }


    // Update is called once per frame
    void FixedUpdate() 
    {
        Vector3 delta_velocity = new Vector3(0, 0, 0);

        //Example from on Unity Website https://docs.unity3d.com/ScriptReference/Transform-forward.html
        if (forwardMove)
        {
            //Move the Rigidbody forwards constantly at speed you define (the blue arrow axis in Scene view)
            delta_velocity += transform.forward * moveSpeed;
        }

        if (backMove)
        {
            //Move the Rigidbody backwards constantly at the speed you define (the blue arrow axis in Scene view)
            delta_velocity += -transform.forward * moveSpeed;
        }

        if (rightMove)
        {
            //Rotate the sprite about the Y axis in the positive direction
            delta_velocity += transform.right * moveSpeed;
        }

        if (leftMove)
        {
            //Rotate the sprite about the Y axis in the negative direction
            delta_velocity += -transform.right * moveSpeed;
        }
        
        playerRB.velocity += delta_velocity;
        //playerRB.AddForce(100* delta_velocity * Time.deltaTime, ForceMode.VelocityChange);

        // impose maximum on non-vertical velocity
        Vector3 horizontalVelocity = new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z);
        if (horizontalVelocity.magnitude > max_velocity)
        {
            horizontalVelocity = horizontalVelocity.normalized * max_velocity;
            playerRB.velocity = new Vector3(horizontalVelocity.x, playerRB.velocity.y, horizontalVelocity.z);
        }


        if (jump && num_jumps > 0)
        {
            num_jumps--;
            playerRB.AddForce(0, 15, 0, ForceMode.VelocityChange);
        }

        if (ab1) abilities.SpawnLure();
        
        if (ab2)   {
            abilities.FireballKey();
        }  else if (abilities.isGrowingFireball)   {
            abilities.FireballRelease();
        }

        if (ab3) abilities.spawnSheep();

        if(ab4 && !wallInPlay)
        {
            abilities.trumpWall();
            wallInPlay = true;
        }

        if (ab5) {
            abilities.Lightning();
        }

    }
}
