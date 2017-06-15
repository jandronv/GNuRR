﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger_jangah : MonoBehaviour {

    public GameObject Y_button;
    private bool enter = false;
    public GameObject txt_jangah;

	void Start () {
        Y_button.SetActive(false);
        txt_jangah.SetActive(false);
	}


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Y_button.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        enter = Input.GetButton("Interact");


        if (other.gameObject.tag == "Player")
        {
            if (enter == true)
            {
                Y_button.SetActive(false);
                txt_jangah.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Y_button.SetActive(true);
            txt_jangah.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}