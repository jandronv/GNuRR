using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pelusero : MonoBehaviour {

    public GameObject y_button;
    private bool enter = false;
    public GameObject txt_pelusero_1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            y_button.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        enter = Input.GetButtonDown("Interact");
        if (other.gameObject.tag == "Player")
        {
            if (enter == true)
            {
                y_button.SetActive(false);
                txt_pelusero_1.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //y_button.SetActive(false);
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
