using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMgrs : MonoBehaviour {

    private GameObject _player;


    private Transform currentPoint;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Funcion que se llama cuando el player pasa por una zona de "guardado"
    /// Si muere antes de pasar 
    /// </summary>
    /// <param name="point"></param>
    public void SetSpawnPoint(Transform point)
    {
        currentPoint = point;

    }

    public Transform GetSpawPoint() {

        return currentPoint;
    }
}
