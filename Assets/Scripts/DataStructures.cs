using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Necessary data structures for keeping the code for future levels organized and legible
 */


/*
 * Command class. Consists of two vector2's recording the keyboard and mouse inputs respectively
 */ 
public class Command
{
    public Vector2 move; //Keyboard input

    public Vector2 mouseMv; //Mouse input

    //Constructor
    public Command(Vector2 move, Vector2 mouseMv)
    {
        this.move = move; //Keyboard
        this.mouseMv = mouseMv; //Mouse
    }
}

/*
 * Iteration class. Consists of a list of commands to be executed and a GameObject to execute those commands.
 */
public class Iteration
{
    public GameObject avatar; //GameObject to carry out commands

    public List<Command> commands; //list of commands to be carried out

    //Constructor 1
    public Iteration() { this.commands = new List<Command>(); }

    //Constructor 2
    public Iteration(List<Command> commands) { this.commands = commands; }

    //Method for adding to list
    public void Add(Command next) { commands.Add(next); }

    //Overload for Count, treating like list
    public int Count() { return commands.Count; }

    //Overload for [], allowing for Iteration[x]
    public Command this[int key] {
        get { return GetValue(key); }
        set { SetValue(key, value);  }
    }

    //Private functions for overloading []
    private Command GetValue(int index) { return commands[index]; }
    private void SetValue(int index, Command value) { commands[index] = value; }
}


/*
 * Iterations class. Consists of a list of iterations, likely all past iterations so far.
 */
public class Iterations
{
    public List<Iteration> iterations = new List<Iteration>();

    //Method for adding to list
    public void Add(Iteration iteration) { iterations.Add(iteration); }

    //Overload for Count, treating like list
    public int Count() { return iterations.Count; }

    //Overload for [], allowing for Iterations[x]
    public Iteration this[int key]
    {
        get { return GetValue(key); }
        set { SetValue(key, value); }
    }

    //Private functions for overloading []
    private Iteration GetValue(int index) { return iterations[index]; }
    private void SetValue(int index, Iteration value) { iterations[index] = value; }
}
