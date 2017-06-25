using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class txt_fake_behaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameMgr.GetInstance().GetServer<InputMgr>().BloqueControles = true;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
