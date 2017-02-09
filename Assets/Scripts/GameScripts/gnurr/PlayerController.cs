using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	// Use this for initialization
	void Start ()
    {

        GameMgr.GetInstance().GetServer<InputMgr>().RegisterMove = Move;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Move(Vector3 direction)
    {

    }
}
