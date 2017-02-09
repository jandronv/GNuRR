using UnityEngine;
using System.Collections;
/// <summary>
/// Configura los settings iniciales del nivel como la luz
/// </summary>
public class GaleonLevelConfiguration : AComponent {
 
	 // Call when component is create (only once)
	 /*protected override void Awake() {
	 	base.Awake();
	 }*/
 
 	// Use this for initialization
 	protected override void Start() {
 		base.Start();
		if(!GameMgr.GetInstance().GetStorageMgr().ExistFile())
		{
			GameMgr.GetInstance().GetStorageMgr().Store();
		}
		
		GameMgr.GetInstance().GetStorageMgr().Load();
		if(!GameMgr.GetInstance().GetStorageMgr().Contains("settings","light_active"))
		{
			GameMgr.GetInstance().GetStorageMgr().Set("settings","light_active",false);
		}
		GameMgr.GetInstance().GetStorageMgr().Store();
 	}
 
	 // Tick is called once per frame
	 protected override void Update() {
		 base.Update();
	 }
	 // End is called when component is destroy
	 /*protected override void OnDestroy() {
	 base.OnDestroy();
	 }*/
}
