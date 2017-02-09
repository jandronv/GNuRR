using UnityEngine;
using System.Collections;
using System;
using System.Runtime.CompilerServices;
/// <summary>
/// Game mgr. Singleton que gestiona todos los managers del juego.
/// </summary>
/// 
public class GameMgr 
{
    public const string CONFIGURATION_FOLDER = "Config";
    public const string CONFIGURATION_FILE = "BaseConfig";
    /// <summary>
    /// Clase de la que hay que heredar para añadir managers specíficos del juego que desarrollemos.
    /// </summary>
    public abstract class ProjectSpecificMgrs
    {
        public ProjectSpecificMgrs(GameMgr gameMgr)
        {
            m_gameMgrRef = gameMgr;
        }

        protected void AddServerInGameMgr<T>() where T : Component
        {
            m_gameMgrRef.AddServer<T>();
        }

        protected GameMgr GetGameMgr() { return m_gameMgrRef; }
        private GameMgr m_gameMgrRef;
    }

    public static object Deserializer(string type, string data)
    {

        object obj = null;
        if (type == typeof(bool).ToString())
        {
            obj = System.Convert.ToBoolean(data);
        }
        else if (type == typeof(int).ToString())
        {
            obj = System.Convert.ToInt32(data);
        }
        else if (type == typeof(string).ToString())
        {
            obj = data;
        }
        else if (type == typeof(float).ToString())
        {
            obj = System.Convert.ToSingle(data);
        }
        else if ((type == typeof(Vector2).ToString()) || (type == typeof(Vector3).ToString()) || (type == typeof(Quaternion).ToString()))
        {
            //procesamos la cadena...
            string vector2Str = data.Substring(1);
            vector2Str = vector2Str.Substring(0, vector2Str.Length - 1);
            string[] vectorComponent = vector2Str.Split(',');
            Assert.AbortIfNot(vectorComponent.Length >= 2 && vectorComponent.Length <= 4, "Incorrect Format");
            string xStr = vectorComponent[0].Trim();
            string yStr = vectorComponent[1].Trim();
            if (type == typeof(Vector2).ToString())
            {
                obj = new Vector2(System.Convert.ToSingle(xStr), System.Convert.ToSingle(yStr));
            }
            else
            {
                string zStr = vectorComponent[2].Trim();
                if (type == typeof(Vector3).ToString())
                {
                    obj = new Vector3(System.Convert.ToSingle(xStr), System.Convert.ToSingle(yStr), System.Convert.ToSingle(zStr));
                }
                else
                {
                    string wStr = vectorComponent[3].Trim();
                    obj = new Quaternion(System.Convert.ToSingle(xStr), System.Convert.ToSingle(yStr), System.Convert.ToSingle(zStr), System.Convert.ToSingle(wStr));
                }
            }

        }
        else
        {
            Assert.AbortIfNot(false, "Incompatible Format: " + type);
        }
        return obj;
    }



    //Implementacion del storageMng que nos permite declarar los tipos de datos que permitimos almacenar en nuestros objetos.
    //TODO AllowedType
    [AllowedTypeToStorage(typeof(bool))]
    [AllowedTypeToStorage(typeof(int))]
    [AllowedTypeToStorage(typeof(string))]
    [AllowedTypeToStorage(typeof(float))]
    [AllowedTypeToStorage(typeof(Vector2))]
    [AllowedTypeToStorage(typeof(Vector3))]
    [AllowedTypeToStorage(typeof(Vector4))]
    [AllowedTypeToStorage(typeof(Quaternion))]
	private class StorageMgrImp : StorageMgr
	{
        public StorageMgrImp(string fileName) : base(fileName, GameMgr.Deserializer) { }
	}
	
	/// <summary>
	/// Gets the instance of the GameMgr.
	/// </summary>
	/// <returns>
	/// The instance.
	/// </returns>
    
	public static GameMgr GetInstance()
	{
		if(m_instance == null)
		{
			m_instance = new GameMgr();
		}
		return m_instance;
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="GameMgr"/> class.
	/// </summary>
    private GameMgr()
	{
		ProcessBaseConfiguration();
		m_storageMgr = new StorageMgrImp(m_storageFileName);
		
		//Inicializamos el servidorPrincipal y le registramos todos los servidores...
		
		if(m_servers == null)
		{
			m_servers = GameObject.Find("Servers");
			if(m_servers == null)
			{
				m_servers = new GameObject("Servers");
			}
			//Registramos todos los servidores...
			//InputServer: servidor de entrada.
			//TimeServer: Gestion de llamadas a eventos de tiempo. Metodos que se llaman cada cierto tiempo (periodoco) o alarmas de un solo uso.
			//SceneMgr: Gestiona la carga de escenas... 
            AddServer<SceneMgr>();
			/*SceneMgr sm = m_servers.GetComponent<SceneMgr>();
			if(sm != null)
				Component.DestroyImmediate(sm);
			m_servers.AddComponent<SceneMgr>();*/
			
            InputMgr inputMgr = AddServer<InputMgr>();
            inputMgr.Configure(m_IM_buttonId,m_IM_PAndCActive);
			
            AddServer<TimeMgr>();
			
			MemoryMgr memoryMgr = AddServer<MemoryMgr>();
            memoryMgr.Configure(m_MM_Active,m_MM_maxFramerateToRecolect,m_MM_maxTimeToRecoect,m_MM_RecolectUnityAssets);
		}
		
		SceneMgr smAux = m_servers.GetComponent<SceneMgr>();
		m_spawnerMgr = new SpawnerMgr(smAux);

	}
	
	
	public StorageMgr GetStorageMgr()
	{
		return m_storageMgr;
	}
	
	public SpawnerMgr GetSpawnerMgr()
	{
		return m_spawnerMgr;
	}

	
	public bool ExistServer(string name)
	{
		return m_servers.GetComponent(name) != null;
	}
	
	public bool ExistServer<T>() where T : Component
	{
		return m_servers.GetComponent<T>() != null;
	}
	
	public AComponent GetServer(string name)
	{
		return m_servers.GetComponent(name) as AComponent;
	}
	
	public T GetServer<T>() where T : Component
	{
		if(m_servers)
			return m_servers.GetComponent<T>();
		else
			return null;
	}
	
	public void Register<T>() where T : Component
	{
		m_servers.AddComponent<T>();
	}
	
	public void UnRegister<T>() where T : Component
	{
		Component.Destroy(m_servers.GetComponent<T>());
	}

    // --------------------- CustomMgrs ---------------------------------
    public bool IsCustomMgrInit() { return m_customMgrs != null; }
    public ProjectSpecificMgrs CustomMgr
    {
        get { return m_customMgrs; }
        set { m_customMgrs = value; }
    }
    // ------------------------------------------------------------------


    protected T AddServer<T>() where T : Component
    {
        T t = m_servers.GetComponent<T>();
        if (t != null)
            Component.DestroyImmediate(t);
        t = m_servers.AddComponent<T>();
        return t;
    }

	protected void ProcessBaseConfiguration()
	{
        GameMgrConfig gameMgrConfig = ScriptableObjectMgr.Load<GameMgrConfig>(CONFIGURATION_FOLDER+"/"+CONFIGURATION_FILE);

        m_storageFileName = gameMgrConfig.m_storageMgrConfig.StorageFileName;

		m_MM_Active = gameMgrConfig.m_memoryMgrConfig.ActiveAutoRecolect;
        m_MM_maxFramerateToRecolect = gameMgrConfig.m_memoryMgrConfig.MaxFrameRateToRecolect;
        m_MM_maxTimeToRecoect = gameMgrConfig.m_memoryMgrConfig.TimeSiceLastGarbage;
        m_MM_RecolectUnityAssets = gameMgrConfig.m_memoryMgrConfig.RecollectUnityAssets;
		
        m_IM_buttonId = gameMgrConfig.m_inputMgrConfig.ButtonIdToPointAndClick;
        m_IM_PAndCActive = gameMgrConfig.m_inputMgrConfig.PointAndClickActive;

	}

    //Statics
    private static GameMgr m_instance = null;
	
    //Managers
	private StorageMgr m_storageMgr =   null;
	private SpawnerMgr m_spawnerMgr =   null;
	private GameObject m_servers    =   null;
    private ProjectSpecificMgrs m_customMgrs =   null;
    

    //Configuration fields...
	private string m_storageFileName;
	
	private bool m_MM_Active;
	private float m_MM_maxFramerateToRecolect;
	private float m_MM_maxTimeToRecoect;
	private bool m_MM_RecolectUnityAssets;
	
	private InputMgr.TMouseButtonID m_IM_buttonId;
	private bool m_IM_PAndCActive;
}
