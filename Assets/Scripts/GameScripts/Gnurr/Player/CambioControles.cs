using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambioControles : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<PlayerController>().PlayerInSlope(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			//Debug.Log("Sale");
			other.GetComponent<PlayerController>().PlayerInSlope(false);
		}
	}
}
