using UnityEngine;
using System.Collections;


/// <summary>
/// Gestiona el cambio de escena desde la UI de Unity (sólo disponible a partir de la 4.6)
/// </summary>
public class ChangeSceneUI : MonoBehaviour {

    public string m_nextSceneAfterTransitionScene;
    public bool m_return;


    /// <summary>
    /// El método es llamado por el sistema de eventos de la UI y se le pasa por parámetro la escena que quiere cargar. Adicionalmente peude tener establecida como variable pública
    /// en el inspector una siguiente escena a cargar, en caso de que la escena de carga sea una pantalla de carga.
    /// </summary>
    /// <param name="sceneToGo"></param>
    public void OnChangeScene(string sceneToGo)
    {
        if (m_return)
            GameMgr.GetInstance().GetServer<SceneMgr>().ReturnScene(false);
        else
            GameMgr.GetInstance().GetServer<SceneMgr>().ChangeScene(sceneToGo, m_nextSceneAfterTransitionScene);
    }

    /// <summary>
    /// El método es llamado por el sistema de eventos de la UI para cerrar la aplciación.
    /// </summary>
    /// <param name="quit"></param>
    public void OnQuitScene(bool quit)
    {
        if(quit)
            Application.Quit();
    }
}
