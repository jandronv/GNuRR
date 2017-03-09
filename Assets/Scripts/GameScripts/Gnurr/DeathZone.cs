﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

    public GameObject InitialZone;
    private int deatZone = 20;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = InitialZone.transform.position;
        other.GetComponent<Player>().RestaVidaEnemigo(deatZone);
    }
}
