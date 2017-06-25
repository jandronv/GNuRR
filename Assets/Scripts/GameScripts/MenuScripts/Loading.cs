using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour {

	public string nivel;
	public bool primera = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (primera)
		{
			GameMgr.GetInstance().GetServer<SceneMgr>().ChangeAsyncScene(nivel);
			primera = false;
		}
	}
}
