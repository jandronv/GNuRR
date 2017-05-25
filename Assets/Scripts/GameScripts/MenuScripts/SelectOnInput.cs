using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour {

    public EventSystem eventSystem;
    private float vertical;
    public GameObject selectedObject;
    private bool buttonSelected;

	void Start () {
		
	}
	
	void Update () {

        vertical = Input.GetAxisRaw("Vertical");
 
        if (vertical != 0 && buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
	}

    private void OnDisable()
    {
        buttonSelected = false;
    }
}
