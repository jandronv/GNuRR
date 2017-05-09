using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

    private SpawnMgrs mSpawManager;
    private int damageZone = 20;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
	
        if (other.tag == "Player") {
            other.GetComponent<Player>().FallInDeathZone(damageZone);
        }
    }
}
