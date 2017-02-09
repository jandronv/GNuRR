﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Componente que permite cargar el siguiente nivel desde una pantalla de carga estática. Se presupone que el nivel que ha puesto la pantalla de carga debe
/// haber establecido previamente la variable SCENE_SECTION/NEXT_SCENE con el nombre de la escena a cargar.
/// </summary>
public class StaticLoader : AComponent {

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
		SceneMgr sceneMgr = GameMgr.GetInstance().GetServer<SceneMgr>();
		sceneMgr.ChangeScene(GameMgr.GetInstance().GetStorageMgr().GetVolatile<string>(SceneMgr.SCENE_SECTION,SceneMgr.NEXT_SCENE));
	}
}
