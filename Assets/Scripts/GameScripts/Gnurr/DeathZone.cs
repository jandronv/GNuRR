using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

    private SpawnMgrs mSpawManager;
    public int damageZone = 5;
	public int dañoCritter = 7;
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
		} else if (other.tag == "EnemyCritter")
		{
			other.GetComponent<EnemyCritter>().RestaVida(dañoCritter);
		}


    }
}
