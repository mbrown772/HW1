//Author: Matthew Brown
//Date: 9/6/24
//Purpose: Controls movement for the obstacles on screen. Generates a random vector to apply force
//         and launches obstacles at a fixed speed of public variable speed.
//         Has two variables for unity so x and y can be sent away from player at start. 


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float speed;         //Edit object speed in unity
    public bool positiveYaway;  //Determine if we should move in the positive Y direction on game start 
    public bool positiveXaway;  //Determine if we should move in the positive X direction on game start 
    
    // Start is called before the first frame update
    void Start()
    {
        //Generate random vector for direction of obstacle at startup
        float moveHorizontal = Random.Range(0, 0.75f);
        float moveVertical = 1 - moveHorizontal; //So we have a constant speed for all obstacles

        //Flags to be set in unity so obstacles move away from player in the beginning
        if(!positiveXaway) moveHorizontal *= -1; //If -x direction is away from player 
        if(!positiveYaway) moveVertical *= -1;   //If -y direction is away from player

        rb2d = GetComponent<Rigidbody2D>();
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb2d.AddForce(movement * speed); //Set direction and speed of rigidbody
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime); //Rotate 45 degrees every second
    }
}

