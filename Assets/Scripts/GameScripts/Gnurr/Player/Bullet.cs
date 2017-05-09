using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public int TamBullet;


    public void SetTamBullet(int TamBullet) {

        this.TamBullet = TamBullet;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Platforms" || other.tag == "EnemyCritter" || other.tag == "EnemyBat")
        {
            if (other.tag == "EnemyCritter")
            {
                other.GetComponent<EnemyCritter>().RestaVida(TamBullet);
            }
            if (other.tag == "EnemyBat")
            {
                other.GetComponent<EnemyBat>().RestaVida(TamBullet);
            }
            Destroy(gameObject);
        }
    }


}
