using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartObject : MonoBehaviour {
    Command[] _commands;


    private void OnTriggerEnter(Collider other)
    {
        //if(other.tag == "Player")
            //Player.TomaMiMierda()
    }

    public Command[] Getcommand()
    {
        return _commands;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
