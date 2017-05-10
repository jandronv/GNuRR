using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorManager : MonoBehaviour {

    public Image fondo;
    public Sprite ima1, ima2, ima3;

    // Use this for initialization
    void Start () {
  
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //_animations.SetBool("TriggerIzq", true);
        //Debug.LogWarning("Entra");
        fondo.sprite = ima3;
       
    }

    private void OnTriggerExit(Collider other)
    {
        fondo.sprite = ima2;
    }
}
