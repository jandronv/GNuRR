using UnityEngine;
using System.Collections;

/// <summary>
/// Prebuild game objects. Clase que podemos colocar en la raiz de la escena y que informa al SpawnerMgr de la lista
/// de objetos precargados que necesita la escena.
/// </summary>
public class PrebuildGameObjects : AComponent {
	
	[System.Serializable]
	public class CacheData
	{
		public GameObject prefab;
		public int cacheSize;
	}

	public CacheData[] m_objectCache;
    protected bool m_isInstanciate = false;


    protected override void Start()
    {
        base.Start();
        //IsLoadingFinish
        SceneMgr sceneMaganer = GameMgr.GetInstance().GetServer<SceneMgr>();
        if (sceneMaganer.IsLoadingFinish())
        {
            GameMgr.GetInstance().GetSpawnerMgr().InstanciateInitialObjects(this);
            m_isInstanciate = true;
        }
        
	}

    protected override void Update()
    {
        base.Update();
        if(!m_isInstanciate)
        {
            GameMgr.GetInstance().GetSpawnerMgr().InstanciateInitialObjects(this);
            m_isInstanciate = true;
        }
    }

}
