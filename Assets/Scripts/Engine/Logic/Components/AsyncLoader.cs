using UnityEngine;
using System.Collections;

/// <summary>
/// Componente que permite cargar el siguiente nivel desde una pantalla de carga dinámica de forma asíncrona. Se presupone que el nivel que ha puesto la pantalla de carga debe
/// haber establecido previamente la variable SCENE_SECTION/NEXT_SCENE con el nombre de la escena a cargar.
/// </summary>
public class AsyncLoader : AComponent
{
    private bool loadingInit = false;

    protected override void Update () {

        if (!loadingInit)
        {
            loadingInit = true;
            
            Load();
        }
    }
    
    protected void Load()
    {
        string sceneToLoad = GameMgr.GetInstance().GetStorageMgr().GetVolatile<string>(SceneMgr.SCENE_SECTION, SceneMgr.NEXT_SCENE);
        SceneMgr sceneMgr = GameMgr.GetInstance().GetServer<SceneMgr>();
        sceneMgr.ChangeAsyncScene(sceneToLoad);
    }

}
