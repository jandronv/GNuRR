using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseBackButton : MonoBehaviour {

    public GameObject thismenu_BG;
    public GameObject thismenu_panel;
    public GameObject options_BG;
    public GameObject options_panel;
    private bool enter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        enter = Input.GetButtonDown("Jump");
        if (enter == true)
        {
            options_BG.SetActive(true);
            options_panel.SetActive(true);
            thismenu_BG.SetActive(false);
            thismenu_panel.SetActive(false);
        }
		
	}
}
