using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour {

	public string nivel;
	public bool primera = true;
	public Animation anim;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (primera)
		{
			GameMgr.GetInstance().GetServer<SceneMgr>().ChangeAsyncScene(GameMgr.GetInstance().GetCustomMgrs().GetPlayerMgr().UltimaEscena);
			primera = false;
		}

		if (!anim.IsPlaying("Loading_animation"))
		{
			anim.Play();
		}
			
	}



}
