using UnityEngine;
using System.Collections;

//Cargador de subescenas... Podemos utilizarlo para componer una escena por piezas.
public class SceneLoader : AComponent {

	public string[] m_subScenesInitialLoaded;
	
	protected override void Start()
	{
		base.Start();
		foreach(string ssinfo in m_subScenesInitialLoaded)
		{
			GameMgr.GetInstance().GetServer<SceneMgr>().LoadSubScene(ssinfo,false);
		}
	}
}
