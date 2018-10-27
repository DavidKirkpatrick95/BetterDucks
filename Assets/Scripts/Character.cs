using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Class for player control of current iteration aswell as recording commands for past iterations,
 * instantiating and placing current and past iterations, and keeping track of all said iterations.
 */
public class Character : MonoBehaviour {

    //Set speed of player
    public static float verSpeed = 10.0f;
    public static float horSpeed = 8.0f;

    //Set mouse speed of player
    public static float sensitivity = 5.0f;
    public static float smoothing = 2.0f;

    //Declare variables
    private Vector2 mouseLook; //value of mouse's x and y rotation, must be reset to reset rotation 
    private Vector2 smoothV; //velocity of mouse
    private int iterNum = 0; //current iteration the player is playing as

    //Declare data structures
    public static Iterations allRec; // Every iteration fully recorded so far. SHOULD BE USEFUL FOR JUMPING BACK ITERATIONS IN CASE OF PARADOX.
    public static Iteration currRec; // The iteration we are currently recording for

    GameObject character;
    public GameObject PastSelf; //Past character prefab passed to this script in editor

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked; // Hide mouse

        allRec = new Iterations(); //Instantiate data structures
        currRec = new Iteration();

        character = this.transform.parent.gameObject; //Player Object is parent of Camera this is attached to.
    }

    // Update is called once per frame
    void Update () {
        //Keyboard movement
        float translation = Input.GetAxis("Vertical") * verSpeed; //y input
        float straffe = Input.GetAxis("Horizontal") * horSpeed; //x input
        Vector2 mvMent = new Vector2(straffe, translation); //Save inputs for recording
        translation *= Time.deltaTime; //calculate distance
        straffe *= Time.deltaTime;
        translation = Mathf.Clamp(translation, translation / 2, translation);
        character.transform.Translate(straffe, 0, translation);

        //Mouse Movement
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")); //input
        Vector2 mseMvment = md; //Save input for recording
        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * 2/3 * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        mouseLook += smoothV;
        mouseLook.y = Mathf.Clamp(mouseLook.y, -25, 25); //limit how far up and down the player can look

        //Rotate view
        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);

        //record full command
        currRec.Add(next: new Command(mvMent, mseMvment));

        //If next iteration is begun
        if (Input.GetKeyDown("space"))
        {
            //instantiate PastSelf for recorded iteration and save iteration to allRec
            Iteration temp = currRec; //create duplicate
            temp.avatar = Instantiate(PastSelf, new Vector3(0, 2, -10 * (iterNum)), new Quaternion(0,0,0,1)); //instantiate
            temp.avatar.GetComponentInChildren<Camera>().enabled = false; //disable camera to avoid conflicts
            temp.avatar.GetComponentInChildren<PastCharacter>().iterNum = iterNum++; //keep track of and iterate iteration number; MAY BE USEFUL FOR JUMPING BACK ITERATIONS IN CASE OF PARADOX
            temp.avatar.GetComponentInChildren<PastCharacter>().inputList = temp; //apply recorded commands to PastSelf
            allRec.Add(temp); //Add iteration to list of iterations

            //Reset recording iteration
            currRec = new Iteration();
                
            //place player for next iteration
            character.transform.position = new Vector3(0, 2, -10 * iterNum);
            character.transform.localRotation = Quaternion.Euler(0, 0, 0);
            mouseLook.y = mouseLook.x = 0;
        }

        //Control of mouse visibility
        if (Input.GetKeyDown("escape")) //free mouse if esc is pressed
            Cursor.lockState = CursorLockMode.None;
        if (Input.GetMouseButtonDown(0)) //hide mouse if game is clicked
            Cursor.lockState = CursorLockMode.Locked;
    }
}
