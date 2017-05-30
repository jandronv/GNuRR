using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cargador : MonoBehaviour {

    public string _nombreEscena;

    public void OnStartPressed()
    {
        GameMgr.GetInstance().GetServer<SceneMgr>().ChangeScene(_nombreEscena);
    }

    public void VolverNivel()
    {
        GameMgr.GetInstance().GetServer<SceneMgr>().ReturnScene(false);
    }
}
