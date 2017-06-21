using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextText : MonoBehaviour {

    private bool enter = false;
    public GameObject Next_txt;
    public GameObject This_txt;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        enter = Input.GetButtonDown("Interact");
        if (enter == true)
        {
            Next_txt.SetActive(true);
            Destroy(This_txt);
        }

		
	}
}
