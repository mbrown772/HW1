//Author: Matthew Brown
//Date: 9/6/24
//Purpose: This file is to control player movement. It also controls game over state and 
//         restart state. If any obstacle hits the player withing 60 seconds the game over screen is shown.
//         Game Over: No collisons with obstacals for 60s is a win otherwise it is a loss.
//         Restart: Restart button is show after a game over state is triggered.
//         Intangible: For 20 seconds total your character will become intangible when clicking Q. Toggelable



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float speed;          //Public variable so speed can be edited in unity
    public Text timerText;       //Public Variable to link timer on screen to gameTimer
    public Text winText;         //Public variable to link win text on screen
    public Text intangibleTextCorner;  //Public variable to link intagible timer to screen in top left to intangibleTimer
    public Text intangibleTextPlayer;  //Public variable to link intagibleTimer on top of player
    public Button restartButton; //Publice variable to link reset button on screen
    private float gameTimer;     //float for timer text counts down from 60
    private float intangibleTimer;     //Timer for total intangible time left
    private bool intangibleFlag;       //Flag to tell when we are currently intangible
    private bool run;            //Flag for telling the reset button to appear


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        run = true;
        gameTimer = 60.0f;
        intangibleTimer = 20.0f;
        intangibleFlag = false;
        timerText.text = "Time Left: " + gameTimer.ToString();
        winText.text = "";
        intangibleTextCorner.text = "(Hotkey Q) Intangible Time Left: " + intangibleTimer;
        intangibleTextPlayer.text = "";
        restartButton.gameObject.SetActive(false);
    }

    // FixedUpdate is in sync with physics engine
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, 10) * Time.deltaTime); //Slowly rotate player
        
        //Control player movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb2d.velocity = movement * speed; //Apply the movement to the rigidbody with speed unity variable
    }

    void Update()
    {
        //Game Time Logice control block
        if (gameTimer > 0 && run) //If we have time left
        {
            gameTimer -= Time.deltaTime; //Subtract time change from the timer
            int seconds = (int)gameTimer % 60; //Convert to int, makes it only update every second
            timerText.text = "Time Left: " + seconds.ToString();
        } else if (run == false && gameTimer > 0) { //If we hit an object and have time left
            winText.text = "You Lose!";
            restartButton.gameObject.SetActive(true); //Shows restart button on game over
        } else {
            winText.text = "You Win!";
            restartButton.gameObject.SetActive(true);
        }


        //Intangible logic activated with Q on keyboard
        if (Input.GetKeyDown(KeyCode.Q)) //If q was pressed
        {
            if (intangibleFlag == false && intangibleTimer > 0) //If we are currently not intangible and have time left
            {
                intangibleFlag = true;
                //Makes player half transperent
                this.GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, .5f);
                Physics2D.IgnoreLayerCollision(6, 7, true); //Turn off collision between obstacles and player
            }
            else
            {
                this.GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                Physics2D.IgnoreLayerCollision(6, 7, false); //Turn on collision between obstacles and player
                intangibleFlag = false;
                intangibleTextPlayer.text = "";
            }
        }
        if(intangibleFlag) //If we are currently intangible
        {
            intangibleTimer -= Time.deltaTime; //Count down are timer
            int seconds = (int)intangibleTimer % 60;
            intangibleTextCorner.text = "(Hotkey Q) Intangible Time Left: " + seconds.ToString();
            intangibleTextPlayer.text = seconds.ToString();
            if(intangibleTimer <= 0) //To catch if intangibility was never turned off and ran out of time
            {
                this.GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                Physics2D.IgnoreLayerCollision(6, 7, false);
                intangibleFlag = false;
                intangibleTextPlayer.text = "";
            }
        }
    }

    //To determine when there is a collision with an obstacle
    void OnCollisionEnter2D(Collision2D col) 
    {
        if (col.gameObject.CompareTag("Obstacle")) //If the other object is an obstacle
        {
            run = false;
        }
    }

    public void onRestartButtonPress()
    {
        SceneManager.LoadScene("SampleScene"); //Restart the game
    }

}

