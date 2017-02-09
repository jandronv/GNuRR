using UnityEngine;
using System.Collections;

public class StaticLoading : AComponent {

	public float m_timeToInitLoad = 0f;
	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
		m_timeToInitLoad -= Time.deltaTime;
		if(m_timeToInitLoad < 0f)
		{
			string levelToLoad = GameMgr.GetInstance().GetStorageMgr().GetVolatile<string>("scene","next_scene");
			GameMgr.GetInstance().GetServer<SceneMgr>().ChangeScene(levelToLoad);
		}
	}
}
