using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivaPlanear : MonoBehaviour {

	public PlayerController pc;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			//GameMgr.GetInstance().GetServer<InputMgr>().RegisterPlanear = Planear;
			pc.ActivaPlanear();
		}
	}

}
