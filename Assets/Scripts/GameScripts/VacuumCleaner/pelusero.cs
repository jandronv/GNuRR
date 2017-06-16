using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pelusero : MonoBehaviour {

    public GameObject y_button;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            y_button.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            y_button.SetActive(false);
        }
    }

    // Use this for initialization
    void Start () {
        y_button.SetActive(false);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
