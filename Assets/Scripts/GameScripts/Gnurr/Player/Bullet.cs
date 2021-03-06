﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public int TamBullet;
    public ParticleSystem PSbullet;


    public void SetTamBullet(int TamBullet) {

        this.TamBullet = TamBullet;
    }

    void OnTriggerEnter(Collider other)
    {
		
		if (other.tag == "Platforms" || other.tag == "EnemyCritter" || other.tag == "EnemyBat" || other.tag == "Enemies")
        {
			GetComponent<Rigidbody>().Sleep();
			GetComponent<SpriteRenderer>().enabled = false;
			PSbullet.Play();
			Debug.Log("Entra..");
            if (other.tag == "EnemyCritter")
            {
                other.GetComponent<EnemyCritter>().RestaVida(TamBullet);
            }
            if (other.tag == "EnemyBat")
            {
                other.GetComponent<EnemyBat>().RestaVida(TamBullet);
            }
			
            
            //aqui debería haber algo plan que tarde 0.2 s o algo así
            //en destruirse, pq no da tiempo a verse la animacion
			Destroy(gameObject, 0.4f);
		}
		
	}


}
