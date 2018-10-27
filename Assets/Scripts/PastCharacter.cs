using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Class for controlling a past iteration and replaying the list of commands passed to it.
 */
public class PastCharacter : MonoBehaviour
{
    
    //Set speed to that for original player
    private float verSpeed = Character.verSpeed;
    private float horSpeed = Character.horSpeed;

    //Set mouse speed to that for original player.
    private float sensitivity = Character.sensitivity;
    private float smoothing = Character.smoothing;

    //Declare variables 
    private Vector2 mouseLook; //value of mouse's x and y rotation, must be reset to reset rotation 
    private Vector2 smoothV; //velocity of mouse
    private Vector3 originalPos; //starting position for this iteration
    public Iteration inputList; //This iteration; Our list of commands to execute
    public int iterNum; //Iteration Number - MAY BE USEFUL FOR JUMPING BACK ITERATIONS IN CASE OF PARADOX

    private int tick; //current Command to execute

    GameObject character;

    void Start()
    {
        character = this.transform.parent.gameObject; //Player Object is parent of Camera this is attached to.
        originalPos = new Vector3(character.transform.position.x, character.transform.position.y, character.transform.position.z); //set original position
        tick = 0; //set initial tick;
    }

    void Update()
    {
        if (tick < inputList.Count()) //As long as we've not completed all the commands done, complete next command.
        {
            Command fullMove = inputList[tick]; //current move

            //Keyboard movement
            Vector2 mvMent = fullMove.move; //input
            float translation = mvMent[1]; //y
            float straffe = mvMent[0]; //x
            translation *= Time.deltaTime; //calculate distance
            straffe *= Time.deltaTime;
            translation = Mathf.Clamp(translation, translation / 2, translation);
            character.transform.Translate(straffe, 0, translation);

            //Mouse movement
            Vector2 md = fullMove.mouseMv; //input
            md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * 2 / 3 * smoothing));
            smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
            smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
            mouseLook += smoothV;
            mouseLook.y = Mathf.Clamp(mouseLook.y, -25, 25); //limit how far up and down the player can look

            //Rotate view
            transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
            character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);

            //iterate
            tick++;
        }

        //If next iteration is begun
        if (Input.GetKeyDown("space"))
        {
            character.transform.position = originalPos; //reset position
            character.GetComponent<MeshRenderer>().enabled = true;
            mouseLook.y = mouseLook.x = 0; //reset mouse's rotation
            tick = 0; //reset the playback
        }
    }
}
