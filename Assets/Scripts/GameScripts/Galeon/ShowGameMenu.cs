using UnityEngine;
using System.Collections;

public class ShowGameMenu : AComponent {
	
	public string m_menuSceneName;
	public enum TPushSceneType { PUSH_SCENE, POP_SCENE};
	public TPushSceneType m_type = TPushSceneType.PUSH_SCENE;
	public bool m_clearReturnScene = false;
	
	protected override void Awake()
	{
		base.Awake();
		GameMgr.GetInstance().GetServer<InputMgr>().RegisterReturn(this,OnReturnPressed);
	}
	
	protected void OnReturnPressed()
	{
		//muestro el menu...
		if(m_type == TPushSceneType.PUSH_SCENE)
			GameMgr.GetInstance().GetServer<SceneMgr>().PushScene(m_menuSceneName);
		else
			GameMgr.GetInstance().GetServer<SceneMgr>().ReturnScene(m_clearReturnScene);
	}
	
	protected override void OnDestroy() 
	{
		base.OnDestroy();
		InputMgr input = GameMgr.GetInstance().GetServer<InputMgr>();
		if(input != null)
			input.UnRegisterReturn(this,OnReturnPressed);
	}
	
	/*protected virtual void Tick(float deltaTime){}
	protected virtual void Init(){}
	
	protected virtual void End() {}*/
}
