using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterTubo : MonoBehaviour {
    private bool enter;
    public string Selector = "Lvl_VC_Selector";

    void OnTriggerStay (Collider _player)
    {
        enter = Input.GetButton("Fire");
        if (_player.gameObject.tag == "Player")
        {
            if (enter == true)
            {
                SceneManager.LoadScene(Selector);
            }
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
