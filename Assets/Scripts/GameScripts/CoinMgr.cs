using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMgr : MonoBehaviour {

    private GameObject coincogida;
    private int coinscogidas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        coincogida = GameObject.FindGameObjectWithTag("CoinCogida");
        if (coincogida != null)
        {
            coinscogidas = coinscogidas + 1;
            Destroy(coincogida);
            //Debug.Log("Has cogido " + coinscogidas + " coins.");
        }

	}
}
