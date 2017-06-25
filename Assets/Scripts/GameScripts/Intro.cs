using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour {

	public AudioSource audio;
	public AudioClip clip;
	 
	// Use this for initialization
	void Start () {
		StartCoroutine("Wait");
	}
	
	// Update is called once per frame
	void Update () {

		
		
	}


	IEnumerator Wait()
	{
		yield return new WaitForSeconds(47);

		Debug.Log("Cambia de escena");
		//Application.LoadLevel("SurveillanceModeSelectScreen");
		GameMgr.GetInstance().GetServer<SceneMgr>().ChangeScene("Loading");
	}

}
