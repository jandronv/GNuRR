using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeMgr : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Llamamos al playermngr que tiene la vida actual del pj, al crease la escena el player carga su vida correspondiente
        PlayerMngr p = GameMgr.GetInstance().GetCustomMgrs().GetPlayerMgr();
        Player pl = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        pl.Vida = p.Vida;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
