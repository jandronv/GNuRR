using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class txt_last_ending : MonoBehaviour {

    public GameObject this_txt;
    private bool enter = false;
    public GameObject Jangah;
    public GameObject trigger_jangah;
    public GameObject block_salida;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        enter = Input.GetButtonDown("Interact");

        if (enter == true)
        {
            GameMgr.GetInstance().GetServer<InputMgr>().BloqueControles = true;
            Jangah.GetComponent<Animation>().Play();
            Destroy(trigger_jangah);
            Destroy(block_salida);
            Destroy(this_txt);

        }
        
		
	}
}
