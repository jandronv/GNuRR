using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hologram_Lvl_1A : MonoBehaviour {

    public GameObject y_button;
    private bool enter = false;
    public GameObject txt_hologram_1;
    public GameObject holograma;

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
                holograma.SetActive(true);
                txt_hologram_1.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            holograma.SetActive(false);
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
