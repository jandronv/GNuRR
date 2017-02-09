using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public delegate void OnSceneDestroy();

public delegate void OnAsyncLoadingProgress(float progress, bool finish);

/// <summary>
/// Scene mgr: Es el encargado de manejar la carga y descarga de escenas...
/// </summary>
public class SceneMgr : AComponent
{
    //NEXT_SCENE y SCENE_SECTION alamcenan en la memoria volatil entre escenas
    //la inormación de la siguiente escena a cargar. Esto es util si queremos construir un
    //una pantalla de carga.
    public const string NEXT_SCENE = "next_scene";
	public const string SCENE_SECTION = "scene";
	
	protected struct TSceneInfo
	{
		public string name;
		public Dictionary<string,SubSceneInfo> subScenes;
	}

    //Delegados para gestión de eventos dle SceneMgr. OnSceneDestroy y OnAsyncLoadingProgress

    public void RegisterDestroyScene(OnSceneDestroy destroy)
	{
		m_sceneDestroyCallbacks += destroy;
	}
	
	public void UnRegisterDestroyScene(OnSceneDestroy destroy)
	{
		m_sceneDestroyCallbacks -= destroy;
	}

    public void RegisterLoadingSceneProgressCallback(OnAsyncLoadingProgress callback)
    {
        m_AsyncLoadingSceneProgress += callback;
    }

    public void UnRegisterLoadingSceneProgressCallback(OnAsyncLoadingProgress callback)
    {
        m_AsyncLoadingSceneProgress -= callback;
    }

    public void RegisterLoadingAdditiveProgressCallback(string subScene, OnAsyncLoadingProgress callback)
    {
        if (!m_AsyncLoadingAditiveProgress.ContainsKey(subScene))
        {
            m_AsyncLoadingAditiveProgress.Add(subScene, callback);
        }
        else
        {
            m_AsyncLoadingAditiveProgress[subScene] += callback;
        }
    }

    public void UnRegisterLoadingAdditiveProgressCallback(string subScene, OnAsyncLoadingProgress callback)
    {
        if (m_AsyncLoadingAditiveProgress.ContainsKey(subScene))
        {
            OnAsyncLoadingProgress progress = m_AsyncLoadingAditiveProgress[subScene] ;
            progress -= callback;
            if (progress == null)
            {
                m_AsyncLoadingAditiveProgress.Remove(subScene);
            }
        }
    }

    //Método para cambiar la escena de foram asincrona.
    public void ChangeAsyncScene(string sceneToLoad)
    {
        if (m_numSubSceneLoading == 0)
        {
            m_justAsyncLoader = true;
            StartCoroutine("LoadingAsync", sceneToLoad);
        }
        else
            m_deferredSceneChange = sceneToLoad;
    }

    /// <summary>
    /// Changes the scene: Cambia la escena actual por otra nueva. La escena anterior se elimina.
    /// </summary>
    /// <param name='scene'>
    /// SceneName: El nombre de la escena a cargar
    /// </param>
    /// <param name='next'>
    /// Next. Nombre de la siguiente escena a cargar.... Si queremos hacer una pantalla de carga, por ejemplo
    /// </param>
    public void ChangeScene(string sceneName, string next = "")
	{
		//Eliminamos la pila de escenas ya que vamos a renovar completamente toda la escena.
		m_stackScenes.Clear();

		//TODO 4 Si next es distinto de "" lo guardamos en el amacenamiento volatil para que la siguiente escena pueda leerla.
		if(next != "" || next == null)
		{
			GameMgr.GetInstance().GetStorageMgr().SetVolatile(SCENE_SECTION,NEXT_SCENE,next);
		}
		//Avisamos de que vamso a destruir la escena...
        //TODO 5 avisamos a los delegados de destrucción de escena.
        if(m_sceneDestroyCallbacks != null)
			m_sceneDestroyCallbacks();
		GameMgr.GetInstance().GetServer<MemoryMgr>().GarbageRecolect(true);
        //Cargamso la escena

		//TODO 6: cargamso la escena y la almacenamos en la cima de la pila usando StoreLevelInfoInStack
        SceneManager.LoadScene(sceneName);
		StoreLevelInfoInStack(sceneName);
	}
	
	/// <summary>
	/// Pushs the scene: Permite apilar escenas unas sobre otras. Es util para mantener todo el contenido en una misma escena y poder
	/// volver rapido a la escena anterior sin necesidad de volver a cargarla.
	/// </summary>
	/// <returns>
	/// La AsyncOperation que nos permite saber si dicha escena esta siendo cargada de forma asincrona
	/// </returns>
	/// <param name='sceneName'>
	/// Scene name: Nombre de la escena que queremos apilar.
	/// </param>
	/// <param name='asyn'>
	/// Asyn: parametro que determina si la carga sera sincrona o asincrona...
	/// </param>
	public void PushScene(string sceneName,bool asyn = false)
	{
		//Suspendemos la escena actual.
		// Si no hay escena que suspender (es decir apilamos la primera escena, no la suspendemos.
		if(m_stackScenes.Count > 0)
		{
			TSceneInfo current = m_stackScenes.Peek();
			SuspendScene(current);
		}
		
		//Si ya tenemos la escena cargada de antes, pero esta desactivada, no la cargamos, simplemente la activamos...
		if(m_desactiveGameObject.ContainsKey(sceneName))
		{
            //TODO 1: leemos del diccionario m_desactiveGameObject el nombre de la escena  para obtenerla y apilarla de nuevo usando StoreLevelInfoInStack
            //Activamos la escena y eliminamos la escena de las escenas desactivadas m_desactiveGameObject
			GameObject goScene = m_desactiveGameObject[sceneName];
			StoreLevelInfoInStack(sceneName);
			goScene.SetActive(true);
			m_desactiveGameObject.Remove(sceneName);
		}
		else
		{
			//Carga de la escena
			if(asyn)
			{
                m_numSubSceneLoading++;
				//TODO 2: lanzamos la corrutina LoadingAdditiveAsync pasándole como parámetro LoadingAdditiveAsyncParam
                StartCoroutine("LoadingAdditiveAsync", new LoadingAdditiveAsyncParam( sceneName, true));
            }
			else
			{
                SceneManager.LoadScene(sceneName,LoadSceneMode.Additive);
				StoreLevelInfoInStack(sceneName);
			}
		}
	}

    protected struct LoadingAdditiveAsyncParam
    {
        public string sceneName;
        public bool inStack;
        public LoadingAdditiveAsyncParam(string a_s, bool a_i)
        {
            sceneName = a_s;
            inStack = a_i;
        }
    }

    protected IEnumerator LoadingAsync(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        //Como vamso a hacer un cambio de escena y las escenas actuales va na ser destruidas
        //no hace falta esperar a cambiar de escena para reescribir nuestra estructura de datos
        //de la escena. Así evitamos que se inicie antes el start de la siguiente escena que 
        //la actualización de los métodos de las corrutinas. "Recordad el flujo de llamadas!!!!!"
        m_justAsyncLoader = true;
        m_stackScenes.Clear();
        StoreLevelInfoInStack(sceneName);
        do
        {
            if (m_AsyncLoadingSceneProgress != null)
            {
                m_AsyncLoadingSceneProgress(op.progress, op.isDone);
            }
            yield return new WaitForEndOfFrame();
        } while (!op.isDone);
        m_justAsyncLoader = false;

    }

    protected IEnumerator LoadingAdditiveAsync(LoadingAdditiveAsyncParam param)
    {
    	//TODO 3.a hacemos la carga aditiva sobre la variale op. (solución 005.a)
        AsyncOperation op = SceneManager.LoadSceneAsync(param.sceneName,LoadSceneMode.Additive);
        do
        {
            if (m_AsyncLoadingAditiveProgress.ContainsKey(param.sceneName))
            {
                OnAsyncLoadingProgress progress = m_AsyncLoadingAditiveProgress[param.sceneName];
                if (progress != null)
                    progress(op.progress, op.isDone);
            }
            //TODO 3.b yield return WaitForEndOfFrame bloquea la corrutina hasta el siguiente frame (solución 005.b)
            yield return new WaitForEndOfFrame();
        } while (!op.isDone);

        if (param.inStack)
            StoreLevelInfoInStack(param.sceneName);
        else
            StoreSubSceneInCurrentScene(param.sceneName);
        m_numSubSceneLoading--;
        yield return null;
    }

    /// <summary>
    /// Returns the scene. Vuelve a la escena anterior.
    /// </summary>
    /// <param name='clearCurrentScene'>
    /// Clear current scene.
    /// </param>
    public void ReturnScene(bool clearCurrentScene)
	{
		Assert.AbortIfNot(m_stackScenes.Count > 1,"Error, No hay escena a la cual volver");
		
		TSceneInfo current = m_stackScenes.Pop();
		
		//NOTA: Puede querer destruirla? en principio si...
		if(clearCurrentScene)
			DestroyScene(current);
		else
			DesactiveScene(current);
		
		TSceneInfo previousScene = m_stackScenes.Peek();
		BackToLifeScene(previousScene);
	}
	
	/// <summary>
	/// Loads the sub scene. Cargo un fragmento de la escena.
	/// </summary>
	/// <param name='subScene'>
	/// Sub scene. El nombre de la subscena que deseamos cargar
	/// </param>
	/// <param name='asyn'>
	/// Asyn. Parametro que determina si la carga de la escena es sincrona o asincrona.
	/// </param>
	public void LoadSubScene(string subScene, bool asyn = false)
	{
        //si voy a cambiar de escena no cargo subescena porque la escena va a ser destruida.
        if (!m_justAsyncLoader)
        {
            if (!m_stackScenes.Peek().subScenes.ContainsKey(subScene))
            {
                if (m_desactiveGameObject.ContainsKey(subScene))
                {
                	//TODO 7: leemos de las escenas de m_desactiveGameObject y la guardamos en StoreSubSceneInCurrentScene, activamos la escena
                    GameObject goSubScene = m_desactiveGameObject[subScene];
                    StoreSubSceneInCurrentScene(subScene);
                    goSubScene.SetActive(true);
                    m_desactiveGameObject.Remove(subScene);
                }
                else
                {
                    //Carga de la subscena
                    if (asyn)
                    {
                        m_numSubSceneLoading++;
                        //TODO 8 lanzar la corrutina LoadingAdditiveAsync
                        StartCoroutine("LoadingAdditiveAsync", new LoadingAdditiveAsyncParam( subScene, false));
                    }
                    else
                    {
                        SceneManager.LoadScene(subScene, LoadSceneMode.Additive);
                        StoreSubSceneInCurrentScene(subScene);
                    }
                }
            }
            else
            {
                Debug.LogWarning("La subscena " + subScene + " Ya esta activa en la actual escene: " + m_stackScenes.Peek().name);
            }
        }
	}
	
	/// <summary>
	/// Unloads the sub scene. Descarga una subscena.
	/// </summary>
	/// <param name='subScene'>
	/// Sub scene. La subscena a descargar
	/// </param>
	/// <param name='clearSubScene'>
	/// Clear sub scene. Si deseo eliminar la subscena por completo o simplemente desactivarla.
	/// </param>
	public void UnloadSubScene(string subScene, bool clearSubScene)
	{
        //si voy a cambiar de escena no cargo subescena porque la escena va a ser destruida.
        if (!m_justAsyncLoader)
        {
            Assert.AbortIfNot(m_stackScenes.Count > 0, "Error no hay ninguna escena cargada");
            //Obtengo la escena actual.
            TSceneInfo current = m_stackScenes.Peek();
            //Busco la subscena
            Assert.AbortIfNot(current.subScenes.ContainsKey(subScene), "Error: la subscena a eliminar/desactivar no existe: " + subScene);
            if (current.subScenes.ContainsKey(subScene))
            {
            	//TODO 10: buscamos en las subscenas la subscena a descargar,
            	//buscamos el gameobject, si eliminamos la escena clearSubScene, la destruimos y si no simplemente la desactivamos y la metemos en la lista
            	//de escenas desactivadas.
                SubSceneInfo subSceneInfo = current.subScenes[subScene];
                GameObject subSceneRoot = GameObject.Find(subSceneInfo.m_name);
                if (clearSubScene)
                {
                    GameObject.Destroy(subSceneRoot);
                }
                else
                {
                    subSceneRoot.SetActive(false);
                    m_desactiveGameObject.Add(subScene, subSceneRoot);
                }
                current.subScenes.Remove(subScene);
            }
            else
            {
                Debug.LogWarning("No se puede eliminar la subscena " + subScene + " que no esta activa ");
            }
        }
	}
	
	/// <summary>
	/// Gets the name of the current scene.
	/// </summary>
	/// <returns>
	/// The current scene name.
	/// </returns>
	
	public string GetCurrentSceneName()
	{
		return m_stackScenes.Peek().name;
	}
	
	public GameObject GetCurrentSceneRoot()
	{
		string rootName = m_stackScenes.Peek().name;
		if(m_cacheSceneRoot == null)
		{
			m_cacheSceneRoot = GameObject.Find(rootName);
		}
		else if(m_cacheSceneRoot.name != rootName)
		{
			m_cacheSceneRoot = GameObject.Find(rootName);
		}
		return m_cacheSceneRoot;
	}
	
	/// <summary>
	/// Gets the number scenes stacked.
	/// </summary>
	/// <returns>
	/// The number scenes stacked.
	/// </returns>
	public int GetNumScenesStacked()
	{
		return m_stackScenes.Count;
	}
	
	public bool IsLoadingFinish()
	{
		return !m_justAsyncLoader;
	}
	
	
	protected override void Awake()
	{
		base.Awake();
		//Para evitar que se destruya entre escenas.
		DontDestroyOnLoad(this);
		StoreLevelInfoInStack(SceneManager.GetActiveScene().name);
	}
	
	
	protected void DestroyScene(TSceneInfo sceneInfo)
	{
		foreach( SubSceneInfo ssinfo in sceneInfo.subScenes.Values)
		{
			GameObject subSceneRoot = GameObject.Find(ssinfo.m_name);
			Destroy(subSceneRoot);
		}
		GameObject root = GameObject.Find(sceneInfo.name);
        m_desactiveGameObject.Clear();
        if(m_sceneDestroyCallbacks != null)
		    m_sceneDestroyCallbacks();
		Destroy(root);
	}
	
	protected void BackToLifeScene(TSceneInfo sceneInfo)
	{
		foreach( SubSceneInfo ssinfo in sceneInfo.subScenes.Values)
		{
			if(!ssinfo.m_active)
			{
				GameObject subSceneRoot = m_desactiveGameObject[ssinfo.m_name];
				subSceneRoot.SetActive(true);
				m_desactiveGameObject.Remove(ssinfo.m_name);
				ssinfo.m_active = true;
			}
		}
		GameObject root = m_desactiveGameObject[sceneInfo.name];
		m_desactiveGameObject.Remove(sceneInfo.name);
		root.SetActive(true);
	}
	
	protected void SuspendScene(TSceneInfo sceneInfo)
	{
		foreach( SubSceneInfo ssinfo in sceneInfo.subScenes.Values)
		{
			if(ssinfo.m_active)
			{
				GameObject subSceneRoot = GameObject.Find(ssinfo.m_name);
				subSceneRoot.SetActive(false);
				m_desactiveGameObject.Add(ssinfo.m_name,subSceneRoot);
				ssinfo.m_active = false;
			}
		}
		GameObject root = GameObject.Find(sceneInfo.name);
		if(!m_desactiveGameObject.ContainsKey(sceneInfo.name))
			m_desactiveGameObject.Add(sceneInfo.name,root);
		else
			m_desactiveGameObject[sceneInfo.name]=root;
		root.SetActive(false);
	}
	
	protected void DesactiveScene(TSceneInfo sceneInfo)
	{
		foreach( SubSceneInfo ssinfo in sceneInfo.subScenes.Values)
		{
			GameObject subSceneRoot = GameObject.Find(ssinfo.m_name);
			Destroy(subSceneRoot);
		}
		sceneInfo.subScenes.Clear();
		GameObject root = GameObject.Find(sceneInfo.name);
		m_desactiveGameObject.Add(sceneInfo.name,root);
		root.SetActive(false);
	}
	
	protected SubSceneInfo StoreSubSceneInCurrentScene(string subSceneName)
	{
		TSceneInfo info = m_stackScenes.Peek();
		SubSceneInfo subSceneInfo = new SubSceneInfo();
		subSceneInfo.m_name = subSceneName;
		subSceneInfo.m_active = true;
		//TODO 9: añadir a la scena que está en la cima m_stackScenes.Peek(); la subscena subSceneName (solución 007)
		if(info.subScenes.ContainsKey(subSceneInfo.m_name))
			info.subScenes[subSceneInfo.m_name] = subSceneInfo;
		else
			info.subScenes.Add(subSceneInfo.m_name,subSceneInfo);
		return subSceneInfo;
	}
	
	protected TSceneInfo StoreLevelInfoInStack(string levelName)
	{
		TSceneInfo info = new TSceneInfo();
		info.name = levelName;
		info.subScenes = new Dictionary<string, SubSceneInfo>();
		m_stackScenes.Push(info);
		return info;
	}
	
	
	// Update is called once per frame
	protected override void Update()
	{
		base.Update();

        if(m_numSubSceneLoading == 0)
        {
            if (m_deferredSceneChange != null && m_deferredSceneChange != "")
            {
                StartCoroutine("LoadingAsync", m_deferredSceneChange);
                m_deferredSceneChange = "";
            }
        }
    }
	
	private GameObject m_cacheSceneRoot = null;
	private Stack<TSceneInfo> m_stackScenes = new Stack<TSceneInfo>();
	private Dictionary<string,GameObject> m_desactiveGameObject = new Dictionary<string, GameObject>();
	
	private OnSceneDestroy m_sceneDestroyCallbacks;

    private bool m_justAsyncLoader = false;
    private OnAsyncLoadingProgress m_AsyncLoadingSceneProgress = null;
    private int m_numSubSceneLoading = 0;
    private string m_deferredSceneChange = "";
    private Dictionary<string, OnAsyncLoadingProgress> m_AsyncLoadingAditiveProgress = new Dictionary<string, OnAsyncLoadingProgress>();


}
