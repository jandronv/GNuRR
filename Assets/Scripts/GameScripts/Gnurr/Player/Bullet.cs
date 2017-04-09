using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entra");
        if (other.tag == "Platforms")
        {
            Destroy(gameObject);
        }
    }
}
