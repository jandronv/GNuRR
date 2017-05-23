﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl0VC : MonoBehaviour {

    public string _name = "";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMngr p = GameMgr.GetInstance().GetCustomMgrs().GetPlayerMgr();
            Debug.Log("Vida Actual: "+p.Vida+" Puede Planear:"+p.Planear);
            p.Vida = other.GetComponent<Player>().Vida;
            GameMgr.GetInstance().GetServer<SceneMgr>().ChangeScene(_name);
        }
    }
}
