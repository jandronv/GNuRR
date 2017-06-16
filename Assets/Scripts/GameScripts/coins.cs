using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coins : MonoBehaviour {

    public GameObject coin;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            coin.gameObject.tag = "CoinCogida";
        }
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
