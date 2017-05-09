using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

    private SpawnMgrs mSpawManager;
	// Use this for initialization
	void Start () {
        mSpawManager = GetComponentInParent<SpawnMgrs>();
        if (mSpawManager == null)
        {
            Debug.LogError("No se ha podido inicializar el SpawnManager!!");
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
           
            mSpawManager.SetSpawnPoint(this.transform);
        }
    }
}
