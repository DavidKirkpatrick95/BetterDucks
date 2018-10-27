using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour {
    private List<Command> executingCommands;

    private int gameTick;

    private List<Iteration> iterations;

    private int level;

    private Vector2 spawnPosition;

    private List<GameObject> entities;

	// Use this for initialization
	void Start () {
        this.spawnPosition = new Vector3(0, 0, 5);

        var player = new GameObject();
        this.entities.Add(player);

        this.level = 0;
        this.iterations = new List<Iteration> { new Iteration() };		
	}
	
	// Update is called once per frame
	void Update () {
        this.gameTick++;
	}
}
