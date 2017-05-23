using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInGame : MonoBehaviour {

    public string _menuSceneName = "";
    public bool _push;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_push)
                GameMgr.GetInstance().GetServer<SceneMgr>().PushScene(_menuSceneName);
            else 
                GameMgr.GetInstance().GetServer<SceneMgr>().ReturnScene(false);
        }

    }

}
