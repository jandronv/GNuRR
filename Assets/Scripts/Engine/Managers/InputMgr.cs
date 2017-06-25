using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Input mgr. Gestion del input generica. Inplementa ciertas funcionalidades que asumimos tendran todos los juegos como el caso de volver para atras
/// lanzar un rallo desde la camara para implementar botones en 3d (la GUI de Unity es un poco tediosa de utilizar)
/// </summary>
public class InputMgr : AComponent {
    public enum TMouseButtonID { LEFT = 0, MIDDLE, RIGHT };
	//Eventos a los que quiero avisar. El raton o cualquier dispositivo de puntero a presionado sobre alguna parte de la pantalla.
	public delegate void PointAndClickEvent(GameObject onCollision,Vector3 point, float distance);
	public delegate void ReturnDel();
    public delegate void Move(float directionX, bool jump, bool dobleJump,bool blockControl);
    public delegate void Fire();
    public delegate void RecargaPelusas();
    public delegate void CargaPelusas(int numPelusas);
    public delegate void PlanearEvento(bool planear);
	public delegate void SonidoRecarga(bool play);


	private float _contadorRecarga = 0f;
    private float _contadorJump = 0f;
    private float _contadorCarga = 0;
    private int _numPelusasCarga = 0;
    private bool _planeo = false;

    public bool Planear = true;
    public int TamPelusaRecarga = 5;
    public float _coolDawnDisparo = 0.3f;
    public float _coolDawnCarga = 0.3f;
    public float _coolDawnRecarga = 1f;


	public string _menuSceneName = "Menu_Pausa";
	public bool _push = true;
	public bool inPause = false;
	private bool _BloquerMandos = true;

	//Enumerado de diferentes eventos de un boton.
	protected enum TButtonEvent { BEGIN, PRESSED, END, BEGIN_OVER, END_OVER };
	private Vector3 m_axis = new Vector3(0.0f,0.0f,0.0f);
	
    /// <summary>
    /// Clase interna con información del evento de retorno de una pantalla al pulsar el boton return.
    /// </summary>
	protected class TReturnData
	{
		public AComponent m_component;
		public ReturnDel m_returnDel;
		public int m_numCallbacks;
	}
	
	//configuramos el InputMgr con el boton de pointAndClick del raton (0 normalmente) y si queremos activar o no
	//el point and click (por si no lo necesitaramos....)
    public void Configure(TMouseButtonID pointAndClickButton, bool pointAndClickActive = false)
	{
		m_pointAndClickButton = pointAndClickButton;
		m_pointAndClickActive = pointAndClickActive;
	}

    public Move RegisterMove
    {
        //get { return m_DelegateMove; }
        set { m_DelegateMove += value; }
    }

    public Move UnRegisterMove
    {
        //get { return m_DelegateMove; }
        set { m_DelegateMove -= value; }
    }

    public Fire RegisterFire
    {
        set { m_DelegateFire += value; }
    }

    public Fire UnRegisterFire
    {
        set { m_DelegateFire -= value; }
    }

    public PlanearEvento RegisterPlanear
    {
        set { m_planear += value; }
    }

    public PlanearEvento UnregisterPlanear
	{
        set { m_planear -= value; }
    }

	public SonidoRecarga RegisterSonidoRecarga
	{
		set { m_soundRecarga += value; }
	}

	public SonidoRecarga UnRegisterSonidoRecarga
	{
		set { m_soundRecarga -= value; }
	}


	public RecargaPelusas RegisterRecargarPelusas
    {
        set { m_DelegateRecargarPelusas += value; }
    }

    public RecargaPelusas UnRegisterRecargarPelusas
    {
        set { m_DelegateRecargarPelusas -= value; }
    }

    public CargaPelusas RegisterCargaPelusas
    {
        set { m_DelegateCargaPelusas += value; }
    }

    public CargaPelusas UnRegisterCargaPelusas
    {
        set { m_DelegateCargaPelusas -= value; }
    }

    //registramos al evento Return
    public void RegisterReturn(AComponent component, ReturnDel ret)
	{
		//TODO 1: Registramos los delegados usando la calse TReturnData 
		if ( m_returnDelegate.ContainsKey(component.GetID()))
		{
			m_returnDelegate[component.GetID()].m_component = component;
			m_returnDelegate[component.GetID()].m_returnDel += ret;
			m_returnDelegate[component.GetID()].m_numCallbacks++;
		}
		else
		{
			TReturnData data = new TReturnData();
			data.m_component = component;
			data.m_returnDel += ret;
			data.m_numCallbacks++;
			m_returnDelegate.Add(component.GetID(),data);
		}
	}
	//nos desregistramos al evento Return
	public void UnRegisterReturn(AComponent component, ReturnDel ret)
	{
		if ( m_returnDelegate.ContainsKey(component.GetID()))
		{
			m_returnDelegate[component.GetID()].m_component = component;
			m_returnDelegate[component.GetID()].m_returnDel -= ret;
			m_returnDelegate[component.GetID()].m_numCallbacks--;
			//TODO 2 eliminamos el callback si ya no hay registrados.
			if(m_returnDelegate[component.GetID()].m_numCallbacks <= 0)
			{
				m_returnDelegate.Remove(component.GetID());
			}
		}
	}
	//llamamos al evento Return.
	public void ReturnCallback()
	{
		ReturnDel callback = null;
		foreach(TReturnData data in m_returnDelegate.Values)
		{
			if(data.m_returnDel != null && data.m_component.gameObject.activeInHierarchy )
				callback += data.m_returnDel;
		}

        // Antes de llamar a un delegado siempre hay que comprobar si es nulo.
        if(callback != null) callback();
	}
	//registramos un boton. Tenemos dos posibles eventos. pulsado y fin de pulsado.

	public bool BloqueControles {
		get { return _BloquerMandos; }
		set { _BloquerMandos = value; }
	}


	//Registramos los eventos de pointAndClick. Begin es cuando se inicia el pulsado. End cuando termina. pressed mientras se esta presionando
	public void RegisterPointAndClickEvent(PointAndClickEvent begin,PointAndClickEvent end, PointAndClickEvent pressed)
	{
		if(begin != null)
		{
			m_pcBegin += begin;
		}
		
		if (end != null)
		{
			m_pcEnd += end;
		}
		
		if(pressed != null)
		{
			m_pcPressed += pressed;
		}
	}
	//Desregistro de point and click.
	public void UnRegisterPointAndClickEvent(PointAndClickEvent begin,PointAndClickEvent end, PointAndClickEvent pressed)
	{
		if(begin != null)
		{
			m_pcBegin -= begin;
		}
		
		if (end != null)
		{
			m_pcEnd -= end;
		}
		
		if(pressed != null)
		{
			m_pcPressed -= pressed;
		}
	}
	
	//Activar o desactivar el pointAndclick.
	//Nota: una ventaja de tener centralizado todo el manejo es que podemos desactivar el point and click de una 
	//forma rapida y eficaz.
	public void ActivePointAndClick(bool active)
	{
		m_pointAndClickActive = active;
	}
	
	//Obtiene el inputController definido.
	public T GetInput<T>() where T : InputController
	{
		return (T) m_currentController;
	}
	
	//Cambia le input controller
	public void SetInput(InputController input)
	{
		m_currentController = input;
	}
	//Pone el inputController a NULL.
	public void ClearInput()
	{
		m_currentController = null;
	}
	
	//Comprueba si existe algun InputController asignado.
	public bool IsSetAnyInput()
	{
		return m_currentController != null;
	}

    void FixedUpdate()
    {

    }

    // Use this for initialization
    protected override void Update()
    {
        base.Update();
		if (_BloquerMandos)
		{
			if (Input.GetButtonDown("Cancel") && !inPause)
			{
				inPause = true;

				GameMgr.GetInstance().GetServer<SceneMgr>().PushScene(_menuSceneName);

			}
			else
			if (Input.GetButtonDown("Cancel") && inPause)
			{
				inPause = false;
				GameMgr.GetInstance().GetServer<SceneMgr>().ReturnScene(false);


			}

			if (Input.GetButton("Fire") && _contadorCarga > _coolDawnCarga && _numPelusasCarga < TamPelusaRecarga && !_blockControl)
			{

				//_numPelusasCarga++;
				//TODO se ha dejado el disparo siempre a 1
				_numPelusasCarga = 1;
				_contadorCarga = 0;
				//Debug.Log("Aumentando Pelusas: " + _numPelusasCarga);

			}
			else if (Input.GetButtonUp("Fire") && _numPelusasCarga > 0 && !_blockControl)
			{

				m_DelegateCargaPelusas(_numPelusasCarga);
				_numPelusasCarga = 0;

			}


			_contadorCarga += Time.deltaTime;


			if (Input.GetButton("Recargar"))
			{
				//LE damos al play
				m_soundRecarga(true);
				_contadorRecarga += Time.deltaTime;

				if (_contadorRecarga > _coolDawnRecarga)
				{
					_blockControl = true;
					m_DelegateRecargarPelusas();
				}

			}
			else
			{
				_blockControl = false;
			}

			if (Input.GetButtonUp("Recargar"))
			{
				//paramos el sonido
				m_soundRecarga(false);
				_contadorRecarga = 0f;
			}




			jump = Input.GetButtonDown("Jump");
			//Debug.Log("Bloqueo de control: " + _blockControl);


			if (Input.GetAxis("Planear") > 0)
			{
				//Debug.Log("Planear pulsado ");
				_planeo = true;
			}
			else
				_planeo = false;

			if (m_planear != null)
				m_planear(_planeo);

			//De momento no se va a utilizar el doble salto
			if (m_DelegateMove != null)
				m_DelegateMove(Input.GetAxis("Horizontal"), jump, false, _blockControl);
		}
		else {

			m_DelegateMove(0.0f, false, false, false);
		}

        
    }

    //Comprobamos si se ha pulsado el Return.
    protected void OnReturn()
	{
		#if UNITY_IPHONE || UNITY_ANDROID
			//TODO
			if(Input.GetKey(KeyCode.Escape))
			{
				ReturnCallback();
			}
		#else
		try
		{
			if(Input.GetButtonDown("Return"))
			{
				ReturnCallback();
			}
		} catch (UnityException)
		{
			if(!m_alertShow)
			{
				m_alertShow = true;
				Debug.LogWarning("El boton de return no ha sido establecido");
			}
		}
		#endif
	}
	
	//On click de moviles.
	protected void OnTouch()
	{
		
		if(m_pressed && Input.touchCount == 0)
		{
			if(m_pcEnd != null)
				m_pcEnd(m_objectTouch,m_targetPoint,m_distanceTotouch);
            m_pressed = false;
		}
		
		if(Input.touchCount > 0)
		{
			Touch touch = Input.touches[0];
			m_axis.x = touch.position.x;
			m_axis.y = touch.position.y;
			if(!m_pressed)
			{
				m_pressed = true;
                CheckTouch(m_axis, true);	
			}
			else
                CheckTouch(m_axis, false);	
		}

	}
	
	
	//Onclick puede ser producido de muchas formas dependiendo de la plataforma.
	// 
	protected void OnClick()
	{
		#if UNITY_IPHONE || UNITY_ANDROID
			//TODO
			OnTouch();
		#else
        Vector3 mousePosition = Input.mousePosition;
		if(Input.GetMouseButtonDown((int)m_pointAndClickButton))
		{
			//begin
            CheckTouch(mousePosition,true);
			m_pressed = true;
		}
        else if (Input.GetMouseButton((int)m_pointAndClickButton))
		{
			//pressed
            CheckTouch(mousePosition,false);
			m_pressed = true;
		}
		else if(m_pressed)
		{
			//end
            if (m_pcEnd != null)
            {
                m_pcEnd(m_objectTouch, m_targetPoint, m_distanceTotouch);
            }
			m_pressed = false;
		}
		#endif
	}

    public GameObject CheckOver(Vector3 mousePosition)
    {
        GameObject button = null;
        bool collision = false ;
        Camera[] cameras = Camera.allCameras;
        for (int i = cameras.Length - 1; !collision && i >= 0; --i)
        {
            Ray ray = cameras[i].ScreenPointToRay(mousePosition);

            RaycastHit hit;

            collision = Physics.Raycast(ray, out hit); 
            if(collision)
            {
                button = hit.collider.gameObject;
            }
        }
        return button;
    }

	protected void CheckTouch(Vector3 mousePosition, bool begin)
	{
        bool isCollision = false;
        //Lanzamos un rayo desde todas las cámaras en orden de profundidad de la cámara... De esta forma podemos hacer point and click desde cualquier camara
        // ¿Porque? podemos tener en una cámara la GUI y en otra el pintado del juego y mezclarlas al final.
        Camera[] cameras = Camera.allCameras;
        for(int i = cameras.Length -  1; !isCollision && i >= 0; --i)
        {
            isCollision = ThrowRay(cameras[i],mousePosition, begin);
            if(isCollision)
                m_cameraUsedToTouch = null;
        }
		
	}

    public Camera GetCameraUsedToTouch() { return m_cameraUsedToTouch; }
	//Lanzamos un rayo desde las camaras hasta la posicion y le indicamos
	//si ese rallo se ha producido al iniciar el Click/touch o no.
    protected bool ThrowRay(Camera camera, Vector3 position, bool begin)
	{
        bool eventTrow = false;
         //TODO 3 lanzamos el rayo.
        Ray ray = camera.ScreenPointToRay(position);
		RaycastHit hit;
		bool collision = Physics.Raycast(ray,out hit); 

		if (collision) 
		{
			m_targetPoint = hit.point;
			m_objectTouch = hit.collider.gameObject;
			m_distanceTotouch = hit.distance;
			if(begin)
			{
				//TODO 4 avisamos a Begin
                if (m_pcBegin != null)
                {
                    m_pcBegin(m_objectTouch, m_targetPoint, m_distanceTotouch);
                    eventTrow = true;
                }
			}	
			else
			{
				//TODO 5 avisamos a End.
                if (m_pcPressed != null)
                {
                    m_pcPressed(m_objectTouch, m_targetPoint, m_distanceTotouch);
                    eventTrow = true;
                }
			}
		}
        return eventTrow;
	}
	
	protected override void Start()
	{
		base.Start();
		DontDestroyOnLoad(this);
	}

   

	
	private InputController m_currentController = null;


    protected Dictionary<int,TReturnData> m_returnDelegate = new Dictionary<int,TReturnData>();
	protected Dictionary<int,AComponent> m_returnDelegateGameObj = new Dictionary<int, AComponent>();
    //Eventos de Point And click
	protected PointAndClickEvent m_pcBegin;
	protected PointAndClickEvent m_pcEnd;
	protected PointAndClickEvent m_pcPressed;
	//Eventos de boton pulsado y soltado.	
	private Vector3 m_targetPoint;
  
    private bool jump;
    private bool _blockControl = false;
    private float m_distanceTotouch;
   
    private bool m_pressed = false;
	private bool m_alertShow = false;
   
    private GameObject m_objectTouch;

    private Camera m_cameraUsedToTouch = null;
    private PlanearEvento m_planear;

    private Move m_DelegateMove;
    private Fire m_DelegateFire;
    private RecargaPelusas m_DelegateRecargarPelusas;
    private CargaPelusas m_DelegateCargaPelusas;
	private SonidoRecarga m_soundRecarga;

	protected bool m_pointAndClickActive = false;
    protected TMouseButtonID m_pointAndClickButton = TMouseButtonID.LEFT;
}
